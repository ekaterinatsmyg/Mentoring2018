﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IQueryableTask.E3SQueryProvider.E3SClient
{
    public class FTSRequestGenerator
    {
        private readonly UriTemplate FTSSearchTemplate = new UriTemplate(@"data/searchFts?metaType={metaType}&query={query}&fields={fields}");
        private readonly Uri BaseAddress;

        public FTSRequestGenerator(string baseAddres) : this(new Uri(baseAddres))
        {
        }

        public FTSRequestGenerator(Uri baseAddress)
        {
            BaseAddress = baseAddress;
        }

        public Uri GenerateRequestUrl<T>(string[] queries, int start = 0, int limit = 10)
        {
            return GenerateRequestUrl(typeof(T), queries, start, limit);
        }

        public Uri GenerateRequestUrl(Type type, string[] queries, int start = 0, int limit = 10)
        {
            string metaTypeName = GetMetaTypeName(type);

            var ftsQueryRequest = new FTSQueryRequest
            {
                Statements = new List<Statement>(),
                Start = start,
                Limit = limit
            };

            foreach (var query in queries)
            {
                ftsQueryRequest.Statements.Add(new Statement(){Query = query});
            }

            var ftsQueryRequestString = JsonConvert.SerializeObject(ftsQueryRequest);

            var uri = FTSSearchTemplate.BindByName(BaseAddress,
                new Dictionary<string, string>()
                {
                    { "metaType", metaTypeName },
                    { "query", ftsQueryRequestString }
                });

            return uri;
        }

        private string GetMetaTypeName(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(E3SMetaTypeAttribute), false);

            if (attributes.Length == 0)
                throw new Exception($"Entity {type.FullName} do not have attribute E3SMetaType");

            return ((E3SMetaTypeAttribute)attributes[0]).Name;
        }
    }
}
