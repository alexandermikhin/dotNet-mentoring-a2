using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        IDictionary<string, object> _keyValuePairs;

        public Expression Transform(Expression expression)
        {
            return Transform(expression, null);
        }

        public Expression Transform(Expression expression, IDictionary<string, object> keyValuePairs)
        {
            _keyValuePairs = keyValuePairs;
            var updatedExpression = Visit(expression);
            return updatedExpression;

        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            var incrementExpression = CreateIncDecExpression(node);
            if (incrementExpression != null)
            {
                return incrementExpression;
            }

            return base.VisitBinary(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> expression)
        {
            var updatedExpressionBody = Visit(expression.Body);
            return Expression.Lambda(updatedExpressionBody, expression.Parameters);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (ParameterHasValue(node))
            {
                return Expression.Constant(GetParameterValue(node));
            }

            return base.VisitParameter(node);
        }

        private bool ParameterHasValue(ParameterExpression parameterExpression)
        {
            if (_keyValuePairs == null)
            {
                return false;
            }

            return _keyValuePairs.ContainsKey(parameterExpression.Name);
        }

        private object GetParameterValue(ParameterExpression parameterExpression)
        {
            if (!ParameterHasValue(parameterExpression))
            {
                throw new ArgumentException($"Provided parameter name ${parameterExpression.Name} does not exist.");
            }

            return _keyValuePairs[parameterExpression.Name];
        }

        private Expression CreateIncDecExpression(BinaryExpression node)
        {
            if (node.Left.NodeType == ExpressionType.Parameter && node.Right.NodeType == ExpressionType.Constant)
            {
                var parameter = node.Left as ParameterExpression;
                var constant = node.Right as ConstantExpression;
                if (!ParameterHasValue(parameter) && constant.Value is int value && value == 1)
                {
                    if (node.NodeType == ExpressionType.Add)
                    {
                        return Expression.PostIncrementAssign(node.Left);
                    }

                    if (node.NodeType == ExpressionType.Subtract)
                    {
                        return Expression.PostDecrementAssign(node.Left);
                    }
                }
            }

            return null;
        }
    }
}
