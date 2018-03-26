using System;
using System.Collections.Generic;
using AsyncTasks.Task3.DBModels;
using AutoMapper;
using AutoMapper.Configuration;

namespace AsyncTasks.Task3.Mappers
{
    public static class Mapper
    {
        private const string InvalidOperationMessage = "Mapper is not initialized. Call Initialize.";
        private static readonly MapperConfigurationExpression Configuration = new MapperConfigurationExpression();
        private static MapperConfiguration _mapperConfig;
        private static IMapper _mapper;

        private static IMapper Instance
        {
            get
            {
                if (_mapper == null)
                {
                    throw new InvalidOperationException(InvalidOperationMessage);
                }

                return _mapper;
            }

            set { _mapper = value; }
        }
        public static void Initialize()
        {
            _mapperConfig = new MapperConfiguration(Configuration);
            Instance = _mapperConfig.CreateMapper();
        }

        public static IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
        {
            return Configuration.CreateMap<TSource, TDestination>();
        }

        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            return Instance.Map<TSource, TDestination>(source);
        }

        public static IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> sources)
        {
            return Instance.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(sources);
        }
    }
}