using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeTask.Mapper
{
    /// <summary>
    /// Generates a mapper.
    /// </summary>
    public class MappingGenerator
    {
        /// <summary>
        /// Returns an instance of mapper to map object of <see cref="TSource"/> type to object of <see cref="TDestination"/> type.
        /// </summary>
        /// <typeparam name="TSource">The type of the incoming object.</typeparam>
        /// <typeparam name="TDestination">The type that should be as a result of mapping.</typeparam>
        /// <returns></returns>
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>() where TSource : new() where TDestination: new()
        {
            var sourceParam = Expression.Parameter(typeof(TSource));

            var mappingProperties = GetMappingProperties<TSource, TDestination>();

            var memberBindings = mappingProperties.Select(property => Expression.Bind(property, Expression.Call(sourceParam, sourceParam.Type.GetProperty(property.Name).GetGetMethod()))).ToList();

            var newDestinationObject = Expression.New(typeof(TDestination));
            var memberInitExpression = Expression.MemberInit(newDestinationObject, memberBindings);
            var mapFunction =
                Expression.Lambda<Func<TSource, TDestination>>(
                    memberInitExpression,
                    sourceParam
                );

            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }

        /// <summary>
        /// Gets properties that should be mepped.
        /// </summary>
        /// <typeparam name="TSource">The type of the incoming object.</typeparam>
        /// <typeparam name="TDestination">The type that should be as a result of mapping.</typeparam>
        /// <returns></returns>
        private  IEnumerable<PropertyInfo> GetMappingProperties<TSource, TDestination>()
        {
            var sourceTypeProperties = typeof(TSource).GetProperties();
            var destTypeProperties = typeof(TDestination).GetProperties();
            return destTypeProperties.Intersect(sourceTypeProperties, new PropertyInfoComparer());
        }
    }
}
