using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTreeTask.Visitor
{
    /// <summary>Represents a visitor or rewriter for expression trees.</summary>
    public class LambdaExpressionTreeTransformer<TParametr> : ExpressionVisitor
    {
        private readonly Dictionary<string, TParametr> replacedParametrs = new Dictionary<string, TParametr>();

        /// <summary>Visits the children of the <see cref="T:System.Linq.Expressions.Expression`1" />.</summary>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        /// <param name="lambdaExpression">The expression to visit.</param>
        /// <param name="replacedParametrs">the parameters of the lambda that should be replaced.</param>
        /// <typeparam name="T">The type of the delegate.</typeparam>
        public Expression VisitLambda<TFunc>(Expression<TFunc> lambdaExpression, Dictionary<string, TParametr> replacedParametrs)
        {
            foreach (var parametr in replacedParametrs)
            {
                this.replacedParametrs.Add(parametr.Key, parametr.Value);
            }

            return this.VisitLambda(lambdaExpression);
        }

        /// <summary>Visits the children of the <see cref="T:System.Linq.Expressions.Expression`1" />.</summary>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        /// <param name="expression">The expression to visit.</param>
        /// <typeparam name="T">The type of the delegate.</typeparam>
        protected override Expression VisitLambda<T>(Expression<T> expression)
        {
            return Expression.Lambda(Visit(expression.Body), expression.Parameters);
        }


        /// <summary>Visits the <see cref="T:System.Linq.Expressions.ParameterExpression" />.</summary>
        /// <returns>The modified expression, if it should be replaced and is primitive type; otherwise, returns the original expression.</returns>
        /// <param name="parameter">The expression to visit.</param>
        protected override Expression VisitParameter(ParameterExpression parameter)
        {
             if ((parameter.Type.IsPrimitive || parameter.Type.Name == "String") && replacedParametrs.TryGetValue(parameter.Name, out var parametrValue))
                {
                    return Expression.Constant(parametrValue);
                }

            return base.VisitParameter(parameter);
        }


    }
}
