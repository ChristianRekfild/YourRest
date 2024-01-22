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
            if (expression.NodeType == ExpressionType.Lambda)
            {
                Debug.WriteLine("Поступившее выражение: {0}", expression.Body);
                //var expressionParameter = Expression.Parameter(typeof(TDestination), typeof(TDestination).Name.ToLower());
                var expressionParameter = Expression.Parameter(typeof(TDestination), expression.Parameters[0].Name);
                var newExp = getBE<TSource>((BinaryExpression)expression.Body, expressionParameter);
                Debug.WriteLine("TDestination определен как {0}", typeof(TDestination));

                var result = Expression.Lambda<Func<TDestination, bool>>(newExp, expressionParameter);
                Debug.WriteLine("Полученное выражение: {0}", result.Body);
                return result;
            }
            throw new NotSupportedException($"ExpressionType: \"{expression.NodeType}\" пока не поддерживатеся.");
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

        private static BinaryExpression getBE<TSource>(BinaryExpression expression, ParameterExpression parameterExpression)
        {
            //Expression left, right;

            //left = getBEPar<T>(expression.Left, argument);
            //right = getBEPar<T>(expression.Right, argument);

            return expression.NodeType switch
            {
                ExpressionType.AndAlso => Expression.AndAlso(
                    left: getBE<TSource>((BinaryExpression)expression.Left, parameterExpression),
                    right: getBE<TSource>((BinaryExpression)expression.Right, parameterExpression)),
                ExpressionType.Equal => Expression.Equal(
                    left: getBEPar<TSource>(expression.Left, parameterExpression),
                    right: getBEPar<TSource>(expression.Right, parameterExpression)),
                ExpressionType.GreaterThan => Expression.GreaterThan(
                    left: getBEPar<TSource>(expression.Left, parameterExpression),
                    right: getBEPar<TSource>(expression.Right, parameterExpression)),
                ExpressionType.GreaterThanOrEqual => Expression.GreaterThanOrEqual(
                    left: getBEPar<TSource>(expression.Left, parameterExpression),
                    right: getBEPar<TSource>(expression.Right, parameterExpression)),
                ExpressionType.LessThan => Expression.LessThan(
                    left: getBEPar<TSource>(expression.Left, parameterExpression),
                    right: getBEPar<TSource>(expression.Right, parameterExpression)),
                ExpressionType.LessThanOrEqual => Expression.LessThanOrEqual(
                    left: getBEPar<TSource>(expression.Left, parameterExpression),
                    right: getBEPar<TSource>(expression.Right, parameterExpression)),
                _ => Expression.LessThan(
                    left: getBEPar<TSource>(expression.Left, parameterExpression),
                    right: getBEPar<TSource>(expression.Right, parameterExpression))
            };
        }

        private static Expression getBEPar<TSource>(Expression expression, ParameterExpression parameterExpression)
        {
            if (expression.NodeType == ExpressionType.Constant)
            {
                return expression;
            }
            else if (expression.NodeType == ExpressionType.MemberAccess)
            {
                Debug.WriteLine(expression.ToString());
                if (((MemberExpression)expression).Expression == null ||
                    ((MemberExpression)expression).Expression.Type.FullName != typeof(TSource).FullName/*
                    !((MemberExpression)expression).Expression.Type .Member.DeclaringType?.FullName.Contains("BaseEntityDto") ?? false ||
                    ((MemberExpression)expression).Member.DeclaringType?.FullName != typeof(TSource).FullName*/)
                {
                    if (expression.CanReduce)
                    {
                        expression = expression.ReduceExtensions();
                    }
                    return expression;
                }
                var propertyInfo = parameterExpression.Type.GetProperty(((MemberExpression)expression).Member.Name);

                return Expression.Property(parameterExpression, propertyInfo);
            }
            throw new NotSupportedException($"ExpressionType: \"{expression.NodeType}\" пока не поддерживатеся.");
        }
    }
}
