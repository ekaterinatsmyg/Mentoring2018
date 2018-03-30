using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExpressionTreeTask.Visitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTreeTask.Tests
{
    [TestClass]
    public class LambdaExpressionTreeTransformerTest
    {
        private LambdaExpressionTreeTransformer<String> lambdaExpressionTransformer;
        private  Dictionary<string, string> replacedConstants;
        

        [TestInitialize]
        public void TestInit()
        {
            lambdaExpressionTransformer = new LambdaExpressionTreeTransformer<String>();
            replacedConstants = new Dictionary<string, string>() {{ "s1", "sample1"}, {"s2", "sample2"}};
        }

        [TestMethod]
        public void VisitLambda()
        {
            string expectedResultStr = "(s1, s2) => (\"sample1\" + \"sample2\").Length";
            Expression<Func<string, string, int>> sourceExpr = (s1, s2) => (s1 + s2).Length;

            var resultexpression = lambdaExpressionTransformer.VisitLambda(sourceExpr, replacedConstants);

            StringAssert.Contains(expectedResultStr, resultexpression?.ToString());
        }
    }
}
