﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using IQueryableTask.E3SQueryProvider.E3SClient;

namespace IQueryableTask.E3SQueryProvider
{
    public class E3SEntitySet<T> : IQueryable<T> where T : E3SEntity
    {
        protected Expression expression;
        protected IQueryProvider provider;

        public E3SEntitySet(string user, string password)
        {
            expression = Expression.Constant(this);

            var client = new E3SQueryClient(user, password);

            provider = new E3SLinqProvider(client);
        }

        public Type ElementType => typeof(T);

        public Expression Expression => expression;

        public IQueryProvider Provider => provider;

        public IEnumerator<T> GetEnumerator()
        {
            return provider.Execute<IEnumerable<T>>(expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return provider.Execute<IEnumerable>(expression).GetEnumerator();
        }
    }
}
