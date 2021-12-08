using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        IDictionary<string, object> _keyValuePairs;

        public Expression Transform(Expression expression)
        {
            return this.Transform(expression, null);
        }

        public Expression Transform(Expression expression, IDictionary<string, object> keyValuePairs)
        {
            this._keyValuePairs = null;
            var updatedExpression = Visit(expression);
            return updatedExpression;

        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Expression result = node;
            switch (node.NodeType)
            {
                case ExpressionType.Add:
                    if (node.Left.NodeType == ExpressionType.Parameter && node.Right.NodeType == ExpressionType.Constant)
                    {
                        var constant = node.Right as ConstantExpression;
                        if (constant.Value is int value && value == 1)
                        {
                            result = Expression.PostIncrementAssign(node.Left);
                        }
                    }

                    break;
                case ExpressionType.Subtract:
                    if (node.Left.NodeType == ExpressionType.Parameter && node.Right.NodeType == ExpressionType.Constant)
                    {
                        var constant = node.Right as ConstantExpression;
                        if (constant.Value is int value && value == 1)
                        {
                            result = Expression.PostDecrementAssign(node.Left);
                        }
                    }

                    break;
                default:
                    result = node;
                    break;
            }

            return result;
        }
    }
}
