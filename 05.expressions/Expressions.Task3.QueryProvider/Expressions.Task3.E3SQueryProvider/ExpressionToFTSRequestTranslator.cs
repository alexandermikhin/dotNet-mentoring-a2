using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Expressions.Task3.E3SQueryProvider.Models.Request;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;
        readonly List<string> _queries;

        public ExpressionToFtsRequestTranslator()
        {
            _resultStringBuilder = new StringBuilder();
            _queries = new List<string>();
        }

        public string Translate(Expression exp)
        {
            ClearBuffer();
            Visit(exp);
            AddLatestQuery();

            return string.Join(string.Empty, _queries);
        }

        public FtsQueryRequest CreateRequest(Expression exp)
        {
            ClearBuffer();
            Visit(exp);
            AddLatestQuery();
            var request = BuildRequest();
            
            return request;
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }

            if (node.Method.DeclaringType == typeof(string))
            {
                ProcessStringMethod(node);
                return node;
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    if (node.Left.NodeType == ExpressionType.MemberAccess)
                    {
                        Visit(node.Left);
                        _resultStringBuilder.Append("(");
                        Visit(node.Right);
                        _resultStringBuilder.Append(")");
                    }

                    if (node.Left.NodeType == ExpressionType.Constant)
                    {
                        Visit(node.Right);
                        _resultStringBuilder.Append("(");
                        Visit(node.Left);
                        _resultStringBuilder.Append(")");
                    }

                    break;
                case ExpressionType.AndAlso:
                    Visit(node.Left);
                    AddLatestQuery();
                    _resultStringBuilder.Clear();
                    Visit(node.Right);
                    break;

                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append(node.Member.Name).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultStringBuilder.Append(node.Value);

            return node;
        }

        #endregion

        #region private methods

        private void ProcessStringMethod(MethodCallExpression node)
        {
            var methodObject = node.Object;
            Visit(methodObject);
            _resultStringBuilder.Append("(");
            if (node.Method.Name == nameof(string.EndsWith) || node.Method.Name == nameof(string.Contains))
            {
                _resultStringBuilder.Append("*");
            }

            var predicate = node.Arguments[0];
            Visit(predicate);

            if (node.Method.Name == nameof(string.StartsWith) || node.Method.Name == nameof(string.Contains))
            {
                _resultStringBuilder.Append("*");
            }

            _resultStringBuilder.Append(")");
        }

        private void ClearBuffer()
        {
            _resultStringBuilder.Clear();
            _queries.Clear();
        }

        private void AddLatestQuery()
        {
            _queries.Add(_resultStringBuilder.ToString());
        }

        private FtsQueryRequest BuildRequest()
        {
            return new FtsQueryRequest()
            {
                Statements = _queries.Select(q => new Statement() { Query = q }).ToList()
            };
        }

        #endregion
    }
}
