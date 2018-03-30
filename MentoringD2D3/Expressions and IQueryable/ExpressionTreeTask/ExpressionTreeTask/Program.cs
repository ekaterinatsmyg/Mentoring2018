using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ExpressionTreeTask.Mapper;
using ExpressionTreeTask.Visitor;

namespace ExpressionTreeTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var replacingParams = new Dictionary<string, int>() { { "a", 3 }, { "b", 5 } };
            Expression<Func<int, int, int>> expr1 = (a, b) => a + 4 + 5 + b;
            var res = new LambdaExpressionTreeTransformer<int>().VisitLambda(expr1, replacingParams);

            Expression<Func<int, int, int, int>> expr2 = (a, b, c) => a + 4 + 5 + b + c;
            Expression<Func<string, bool>> expr = s => s != null;

            //var mapGenerator = new MappingGenerator();
            //var mapper = mapGenerator.Generate<Foo, Bar>();

            //var res = mapper.Map(new Foo(){Id =1, Name = "name"});

        }

        public class Foo
        {
            public Foo()
            {
            }
            public Foo(string name, int id)
            {
                Name = name;
                Id = id;
            }
            public int Id { get; set; }
            public string Name { get; set; }
            
        }

        public class Bar
        {
            public Bar()
            {
            }

            public int Id { get; set; }
            public string Name { get; set; }

            public Bar(string name, int id)
            {
                Name = name;
                Id = id;
            }
        }

    }
}
