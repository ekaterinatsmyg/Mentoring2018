using System;
using System.Linq;
using System.Linq.Expressions;
using IQueryableTask.E3SQueryProvider.E3SClient;

namespace IQueryableTask.E3SQueryProvider
{
    public class E3SLinqProvider : IQueryProvider
    {
        private E3SQueryClient e3sClient;
        private const char QUERY_SEPORATOR = ',';

        public E3SLinqProvider(E3SQueryClient client)
        {
            e3sClient = client;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new E3SQuery<TElement>(expression, this);
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var itemType = TypeHelper.GetElementType(expression.Type);

            var translator = new ExpressionToFTSRequestTranslator();
            var queryString = translator.Translate(expression).Split(QUERY_SEPORATOR);

            return (TResult)(e3sClient.SearchFTS(itemType, queryString));
        }
    }
}
