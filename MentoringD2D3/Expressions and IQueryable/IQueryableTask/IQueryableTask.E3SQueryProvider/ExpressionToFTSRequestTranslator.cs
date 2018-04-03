using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace IQueryableTask.E3SQueryProvider
{
    public class ExpressionToFTSRequestTranslator : ExpressionVisitor
    {
        StringBuilder resultString;
        private const string QUERY_SEPORATOR = ",";

        public string Translate(Expression exp)
        {
            resultString = new StringBuilder();
            Visit(exp);

            return resultString.ToString();
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);
                return node;
            }

            switch (node.Method.Name)
            {
                case "StartsWith":
                    VisitNodes(node.Object, node.Arguments[0], "(", "*)");
                    return node;
                case "EndsWith":
                    VisitNodes(node.Object, node.Arguments[0], "(*", ")");
                    return node;
                case "Contains":
                    VisitNodes(node.Object, node.Arguments[0], "(*", "*)");
                    return node;
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:

                    if (node.Left.NodeType == ExpressionType.MemberAccess &&
                        node.Right.NodeType == ExpressionType.Constant)
                    {
                        VisitNodes(node.Left, node.Right, "(", ")");
                        break;
                    }
                    else if (node.Left.NodeType == ExpressionType.Constant &&
                        node.Right.NodeType == ExpressionType.MemberAccess)
                    {
                        VisitNodes(node.Right, node.Left, "(", ")");
                        break;
                    }
                    else
                        throw new NotSupportedException(
                            $"Left and right operands should be property or field: Left node type: {node.Left.NodeType}, Right node type:  {node.Right.NodeType}");

                case ExpressionType.AndAlso:

                    VisitNodes(node.Left, node.Right, QUERY_SEPORATOR, string.Empty);
                    break;

                default:
                    throw new NotSupportedException($"Operation {node.NodeType} is not supported");
            }

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            resultString.Append(node.Member.Name).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            resultString.Append(node.Value);

            return node;
        }

        private void VisitNodes(Expression left, Expression right, string openQuery, string closeQuery)
        {
            Visit(left);
            resultString.Append(openQuery);
            Visit(right);
            resultString.Append(closeQuery);
        }
    }
}
