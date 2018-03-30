using System.Linq.Expressions;

namespace ExpressionTreeTask.Visitor
{
    /// <summary>Represents a visitor or rewriter for expression trees.</summary>
    public class BinaryExpressionTreeTransformer : ExpressionVisitor
    {
        /// <summary>Visits the children of the <see cref="T:System.Linq.Expressions.BinaryExpression" />.</summary>
        /// <returns>The modified expression, if it was expression <code>param + 1</code>; or <code>param - 1</code>; otherwise, returns the original expression.</returns>
        /// <param name="node">The expression to visit.</param>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.Right.NodeType == ExpressionType.Constant && node.Right.Type == typeof(int) &&
                node.Right.ToString() == "1" && node.NodeType == ExpressionType.Add)
            {
                var incrementExpression = Expression.Increment(node.Left);
                return base.VisitUnary(incrementExpression);
            }
            if (node.Right.NodeType == ExpressionType.Constant && node.Right.Type == typeof(int) &&
                node.Right.ToString() == "1" && node.NodeType == ExpressionType.Subtract)
            {
                var decrementExpression = Expression.Decrement(node.Left);
                return base.VisitUnary(decrementExpression);
            }
            return base.VisitBinary(node);
        }
        
    }
}
