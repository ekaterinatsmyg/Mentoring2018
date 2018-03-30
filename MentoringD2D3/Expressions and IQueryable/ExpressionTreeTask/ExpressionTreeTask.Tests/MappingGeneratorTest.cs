using ExpressionTreeTask.Mapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTreeTask.Tests
{
    [TestClass]
    public class MappingGeneratorTest
    {
        [TestMethod]
        public void MapSourceToDestination()
        {
            SourceType srcTypeInstance = new SourceType("Ivan", 21, true, new SomeType());
            MappingGenerator mappingGenerator = new MappingGenerator();
            Mapper<SourceType, DestinationType> mapper = mappingGenerator.Generate<SourceType, DestinationType>();

            DestinationType destTypeInstance = mapper.Map(srcTypeInstance);
           
            Assert.IsNotNull(destTypeInstance);
            Assert.AreEqual(srcTypeInstance.Name, destTypeInstance.Name);
            Assert.AreNotEqual(srcTypeInstance.IsActive, destTypeInstance.IsNotActive);
            Assert.AreNotEqual(srcTypeInstance.Id, destTypeInstance.Id);
            Assert.AreEqual(srcTypeInstance.Title, destTypeInstance.Title);
        }

        [TestMethod]
        public void MapDestinationToSource()
        {
            
            DestinationType destTypeInstance = new DestinationType("John", 1, true, new SomeType());
            MappingGenerator mappingGenerator = new MappingGenerator();
            Mapper<DestinationType, SourceType> mapper = mappingGenerator.Generate<DestinationType, SourceType>();
            
            SourceType srcTypeInstance = mapper.Map(destTypeInstance);
            
            Assert.IsNotNull(srcTypeInstance);
            Assert.AreEqual(destTypeInstance.Name, srcTypeInstance.Name);
            Assert.AreNotEqual(destTypeInstance.IsNotActive, srcTypeInstance.IsActive);
            Assert.AreNotEqual(destTypeInstance.Id, srcTypeInstance.Id);
            Assert.AreEqual(destTypeInstance.Title, srcTypeInstance.Title);
        }

        private class SourceType
        {
            public SourceType()
            {
            }

            public SourceType(string name, int id, bool isActive, SomeType title)
            {
                Name = name;
                Id = id;
                Title = title;
                IsActive = isActive;
            }

            public string Name { get; set; }
            public int Id { get; set; }
            public bool IsActive { get; set; }
            public SomeType Title { get; set; }
        }

        private class DestinationType
        {
            public DestinationType()
            {
            }
            public DestinationType(string name, long id, bool isNotActive, SomeType title)
            {
                Name = name;
                Id = id;
                Title = title;
                IsNotActive = isNotActive;
            }

            public string Name { get; set; }
            public long Id { get; set; }
            public SomeType Title { get; set; }
            public bool IsNotActive { get; set; }
        }
        private class SomeType
        {
        }
    }
}
