using System.Diagnostics;
using System.Linq.Expressions;

namespace YourRest.Producer.Infrastructure.ExpressionHelper
{
    internal static class LambdaExpressionExtension
    {
        internal static Expression<Func<TDestination, bool>> ToEntityExpression<TSource, TDestination>(this Expression<Func<TSource, bool>> expression)
        {
            if (expression.NodeType == ExpressionType.Lambda)
            {
                Debug.WriteLine("Поступившее выражение: {0}", expression.Body);
                var expressionParameter = Expression.Parameter(typeof(TDestination), typeof(TDestination).Name.ToLower());
                var newExp = getBE((BinaryExpression)expression.Body, expressionParameter);
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
            for(int i = 0; i < expressions.Length; i++)
            {
                result[i] = expressions[i].ToEntityExpression<TSource, TDestination>();
            }
            return result;
        }

        internal static Expression<Func<TDestination, object>> ToEntityExpression<TSource, TDestination>(this Expression<Func<TSource, object>> expression)
        {
            throw new NotImplementedException();
        }

        private static BinaryExpression getBE(BinaryExpression expression, ParameterExpression parameterExpression)
        {
            //Expression left, right;

            //left = getBEPar<T>(expression.Left, argument);
            //right = getBEPar<T>(expression.Right, argument);

            return expression.NodeType switch
            {
                ExpressionType.AndAlso => Expression.AndAlso(
                    left: getBE((BinaryExpression)expression.Left, parameterExpression),
                    right: getBE((BinaryExpression)expression.Right, parameterExpression)),
                ExpressionType.Equal => Expression.Equal(
                    left: getBEPar(expression.Left, parameterExpression),
                    right: getBEPar(expression.Right, parameterExpression)),
                ExpressionType.GreaterThan => Expression.GreaterThan(
                    left: getBEPar(expression.Left, parameterExpression),
                    right: getBEPar(expression.Right, parameterExpression)),
                ExpressionType.GreaterThanOrEqual => Expression.GreaterThanOrEqual(
                    left: getBEPar(expression.Left, parameterExpression),
                    right: getBEPar(expression.Right, parameterExpression)),
                ExpressionType.LessThan => Expression.LessThan(
                    left: getBEPar(expression.Left, parameterExpression),
                    right: getBEPar(expression.Right, parameterExpression)),
                ExpressionType.LessThanOrEqual => Expression.LessThanOrEqual(
                    left: getBEPar(expression.Left, parameterExpression),
                    right: getBEPar(expression.Right, parameterExpression)),
                _ => Expression.LessThan(
                    left: getBEPar(expression.Left, parameterExpression),
                    right: getBEPar(expression.Right, parameterExpression))
            };
        }

        private static Expression getBEPar(Expression expression, ParameterExpression parameterExpression)
        {
            if (expression.NodeType == ExpressionType.Constant)
            {
                return expression;
            }
            else if (expression.NodeType == ExpressionType.MemberAccess)
            {
                var propertyInfo = parameterExpression.Type.GetProperty(((MemberExpression)expression).Member.Name);

                return Expression.Property(parameterExpression, propertyInfo);
            }
            throw new NotSupportedException($"ExpressionType: \"{expression.NodeType}\" пока не поддерживатеся.");
        }
    }
}
