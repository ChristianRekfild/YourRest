using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.Json;
using YourRest.Infrastructure.Core.Contracts.Models;

[assembly: InternalsVisibleTo("YourRest.Producer.Infrastructure.Tests")]

namespace YourRest.Producer.Infrastructure.ExpressionHelper
{
    internal static class LambdaExpressionExtension
    {
        internal static Expression<Func<TDestination, bool>> ToEntityExpression<TSource, TDestination>(this Expression<Func<TSource, bool>> expression)
        {
            return (Expression<Func<TDestination, bool>>)NodeTypeExpression<TSource, TDestination>(expression);
        }

        internal static Expression<Func<TDestination, object>>[] ToEntityExpression<TSource, TDestination>(this Expression<Func<TSource, object>>[] expressions)
        {
            var result = new Expression<Func<TDestination, object>>[expressions.Length];
            for (int i = 0; i < expressions.Length; i++)
            {
                result[i] = expressions[i].ToEntityExpression<TSource, TDestination>();
            }
            return result;
        }

        internal static Expression<Func<TDestination, object>> ToEntityExpression<TSource, TDestination>(this Expression<Func<TSource, object>> expression)
        {
            if (expression.NodeType == ExpressionType.Lambda && expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                Debug.WriteLine("Поступившее выражение: {0}", expression.Body);
                foreach (var parameter in expression.Parameters)
                {
                    Debug.WriteLine("{0}", parameter);
                    //parameter.Name
                    var parameterExpression = Expression.Parameter(typeof(TDestination), parameter.Name);
                    var propertyInfo = parameterExpression.Type.GetProperty(((MemberExpression)expression.Body).Member.Name);
                    var memberExpression = Expression.Property(parameterExpression, propertyInfo);
                    var result1 = Expression.Lambda<Func<TDestination, object>>(memberExpression, parameterExpression);
                    return result1;
                }
            }
            throw new NotSupportedException($"ExpressionType: \"{expression.NodeType}\" пока не поддерживатеся.");
        }

        private static Expression NodeTypeExpression<TSource, TDestination>(Expression expression, ParameterExpression parameterExpression = null)
        {
            return NodeTypeExpression<TSource, TDestination>(expression, new[] { parameterExpression });
        }
        private static Expression NodeTypeExpression<TSource, TDestination>(Expression expression, IEnumerable<ParameterExpression>? parameters)
        {
            return expression.NodeType switch
            {
                ExpressionType.AndAlso => AndAlsoConvert<TSource, TDestination>((BinaryExpression)expression, parameters),
                ExpressionType.Call => MethodCallExpressionConvert<TSource, TDestination>((MethodCallExpression)expression, parameters),
                ExpressionType.Constant => expression,
                ExpressionType.Equal => EqualConvert<TSource, TDestination>((BinaryExpression)expression, parameters),
                ExpressionType.GreaterThan => GreaterThanConvert<TSource, TDestination>((BinaryExpression)expression, parameters),
                ExpressionType.GreaterThanOrEqual => GreaterThanOrEqualConvert<TSource, TDestination>((BinaryExpression)expression, parameters),
                ExpressionType.Lambda => LambdaExpressionType<TSource, TDestination>(expression),
                ExpressionType.LessThan => LessThanConvert<TSource, TDestination>((BinaryExpression)expression, parameters),
                ExpressionType.LessThanOrEqual => LessThanOrEqualConvert<TSource, TDestination>((BinaryExpression)expression, parameters),
                ExpressionType.MemberAccess => MemberAccessConvert<TSource>((MemberExpression)expression, parameters),
                ExpressionType.OrElse => OrElseConvert<TSource, TDestination>((BinaryExpression)expression, parameters),
                _ => throw new NotSupportedException($"ExpressionType: \"{expression.NodeType}\" пока не поддерживатеся.")
            };
        }

        private static Expression AndAlsoConvert<TSource, TDestination>(BinaryExpression expression, IEnumerable<ParameterExpression>? parameters)
        {
            return Expression.AndAlso(
                    left: ExpressionTypeFactory<TSource, TDestination>((BinaryExpression)expression.Left, parameters),
                    right: ExpressionTypeFactory<TSource, TDestination>((BinaryExpression)expression.Right, parameters));
        }

        private static Expression OrElseConvert<TSource, TDestination>(BinaryExpression expression, IEnumerable<ParameterExpression>? parameters)
        {
            return Expression.OrElse(
                left: ExpressionTypeFactory<TSource, TDestination>((BinaryExpression)expression.Left, parameters),
                right: ExpressionTypeFactory<TSource, TDestination>((BinaryExpression)expression.Right, parameters));
        }

        private static Expression EqualConvert<TSource, TDestination>(BinaryExpression expression, IEnumerable<ParameterExpression>? parameters)
        {
            return Expression.Equal(
                    left: ExpressionTypeFactory<TSource, TDestination>(expression.Left, parameters),
                    right: ExpressionTypeFactory<TSource, TDestination>(expression.Right, parameters));
        }
        private static Expression GreaterThanConvert<TSource, TDestination>(BinaryExpression expression, IEnumerable<ParameterExpression>? parameters)
        {
            return Expression.GreaterThan(
                    left: ExpressionTypeFactory<TSource, TDestination>(expression.Left, parameters),
                    right: ExpressionTypeFactory<TSource, TDestination>(expression.Right, parameters));
        }
        private static Expression GreaterThanOrEqualConvert<TSource, TDestination>(BinaryExpression expression, IEnumerable<ParameterExpression>? parameters)
        {
            return Expression.GreaterThanOrEqual(
                    left: ExpressionTypeFactory<TSource, TDestination>(expression.Left, parameters),
                    right: ExpressionTypeFactory<TSource, TDestination>(expression.Right, parameters));

        }
        private static Expression LessThanConvert<TSource, TDestination>(BinaryExpression expression, IEnumerable<ParameterExpression>? parameters)
        {
            return Expression.LessThan(
                left: ExpressionTypeFactory<TSource, TDestination>(expression.Left, parameters),
                    right: ExpressionTypeFactory<TSource, TDestination>(expression.Right, parameters));
        }
        private static Expression LessThanOrEqualConvert<TSource, TDestination>(BinaryExpression expression, IEnumerable<ParameterExpression>? parameters)
        {
            return Expression.LessThanOrEqual(
                    left: ExpressionTypeFactory<TSource, TDestination>(expression.Left, parameters),
                    right: ExpressionTypeFactory<TSource, TDestination>(expression.Right, parameters));
        }
        private static Expression<Func<TDestination, bool>> LambdaExpressionType<TSource, TDestination>(Expression expression)
        {
            return expression switch
            {
                Expression<Func<TSource, bool>> ex => ex.Body switch
                {
                    BinaryExpression be => LambdaBinaryExpression<TSource, TDestination>(be, ex),
                    MethodCallExpression mce => LambdaMethodCallExpressionConvert<TSource, TDestination>(mce, ex),
                    _ => throw new NotSupportedException($"Expression NodeType: \"{ex.Body.NodeType}\" пока не поддерживатеся.")
                },
                _ => throw new NotSupportedException($"Expression NodeType: \"{expression.Type}\" пока не поддерживатеся.")
            };
        }

        private static Expression ExpressionTypeFactory<TSource, TDestination>(Expression expression, IEnumerable<ParameterExpression>? parameters)
        {
            return expression switch
            {
                BinaryExpression be => BinaryExpressionConvert<TSource, TDestination>(be, parameters),
                ConstantExpression ce => ce,
                MemberExpression me => MemberAccessConvert<TSource>((MemberExpression)expression, parameters),
                MethodCallExpression mce => MethodCallExpressionConvert<TSource, TDestination>(mce),
                _ => throw new NotSupportedException($"Expression NodeType: \"{expression.NodeType}\" пока не поддерживатеся.")
            };
        }
        private static Expression<Func<TDestination, bool>> LambdaBinaryExpression<TSource, TDestination>(BinaryExpression be, Expression<Func<TSource, bool>> expression)
        {
            Debug.WriteLine("Поступившее выражение: {0}", be);
            var expressionParameter = Expression.Parameter(typeof(TDestination), expression.Parameters[0].Name);
            var newExp = NodeTypeExpression<TSource, TDestination>(be, expressionParameter);
            Debug.WriteLine("TDestination определен как {0}", typeof(TDestination));

            var result = Expression.Lambda<Func<TDestination, bool>>(newExp, expressionParameter);
            Debug.WriteLine("Полученное выражение: {0}", result.Body);
            return result;
        }

        private static Expression BinaryExpressionConvert<TSource, TDestination>(BinaryExpression be, IEnumerable<ParameterExpression>? parameters)
        {
            Debug.WriteLine("Поступившее выражение: {0}", be);
            //var parameterExpression = Expression.Parameter(typeof(TDestination), expression.Parameters[0].Name);
            var newExp = NodeTypeExpression<TSource, TDestination>(be, parameters);
            Debug.WriteLine("TDestination определен как {0}", typeof(TDestination));

            //var result = Expression.Lambda<Func<TDestination, bool>>(newExp, parameterExpression);
            var result = newExp;
            //Debug.WriteLine("Полученное выражение: {0}", result.Body);
            Debug.WriteLine("Полученное выражение: {0}", result);
            return result;
        }

        private static Expression<Func<TDestination, bool>> LambdaMethodCallExpressionConvert<TSource, TDestination>(MethodCallExpression mce, Expression<Func<TSource, bool>> expression)
        {
            Debug.WriteLine("Поступившее выражение: {0}", mce);
            //MethodCallExpression newExp = null;
            var expressionParameters = new List<ParameterExpression>();
            foreach (var argument in mce.Arguments)
            {
                //var me = (MemberExpression)argument;
                //var pe = (ParameterExpression)me.Expression;
                expressionParameters.Add(Expression.Parameter(typeof(TDestination), expression.Parameters[0].Name));
            }
            var newExp = MethodCallExpressionConvert<TSource, TDestination>(mce, expressionParameters);
            Debug.WriteLine("TDestination определен как {0}", typeof(TDestination));

            var result = Expression.Lambda<Func<TDestination, bool>>(newExp, expressionParameters);
            Debug.WriteLine("Полученное выражение: {0}", result.Body);
            return result;
        }

        private static Expression MethodCallExpressionConvert<TSource, TDestination>(MethodCallExpression mce)
        {
            Debug.WriteLine("Поступившее выражение: {0}", mce);
            MethodCallExpression newExp = null;
            ParameterExpression expressionParameter = null;
            foreach (var argument in mce.Arguments)
            {
                var me = (MemberExpression)argument;
                var pe = (ParameterExpression)me.Expression;
                expressionParameter = Expression.Parameter(typeof(TDestination), pe.Name);
                newExp = MethodCallExpressionConvert<TSource, TDestination>(mce, expressionParameter);
            }
            Debug.WriteLine("TDestination определен как {0}", typeof(TDestination));

            //var result = Expression.Lambda<Func<TDestination, bool>>(newExp, expressionParameter);
            //Debug.WriteLine("Полученное выражение: {0}", result.Body);
            var result = newExp;
            Debug.WriteLine("Полученное выражение: {0}", result);
            return result;
        }

        private static MethodCallExpression MethodCallExpressionConvert<TSource, TDestination>(MethodCallExpression expression, ParameterExpression parameterExpression)
        { return MethodCallExpressionConvert<TSource, TDestination>(expression, new[] { parameterExpression }); }
        private static MethodCallExpression MethodCallExpressionConvert<TSource, TDestination>(MethodCallExpression expression, IEnumerable<ParameterExpression>? parameters)
        {
            Debug.WriteLine(expression);
            var arguments = new List<Expression>();
            foreach (var argument in expression.Arguments)
            {
                arguments.Add((MemberExpression)NodeTypeExpression<TSource, TDestination>(argument, parameters));
            }
            try
            {
                return Expression.Call((MemberExpression)expression.Object, expression.Method, arguments);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.Source);
                Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        private static Expression MemberAccessConvert<TSource>(MemberExpression expression, IEnumerable<ParameterExpression>? parameters)
        {
            Debug.WriteLine(expression.ToString());
            if (expression.Expression == null ||
                expression.Expression.Type.FullName != typeof(TSource).FullName)
            {
                return expression;
            }
            var propertyInfo = parameters.First().Type.GetProperty(expression.Member.Name);

            return Expression.Property(parameters.First(), propertyInfo);
        }
    }
}
