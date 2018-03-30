using System;
using System.Linq.Expressions;
using ExpressionTreeTask.Visitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTreeTask.Tests
{
    [TestClass]
    public class BinaryExpressionTreeTransformerTest
    {
        private BinaryExpressionTreeTransformer binaryExprTransform;

        [TestInitialize]
        public void TestInit()
        {
            binaryExprTransform = new BinaryExpressionTreeTransformer();
        }

        [TestMethod]
        public void VisitBinary()
        {
            string expectedResultStr = "i => (Increment(i) + Decrement(i))";
            Expression<Func<int, int>> sourceExpr = i => (i + 1) + (i - 1);
            
            var resultExpr = binaryExprTransform.VisitAndConvert(sourceExpr, "");
            
            StringAssert.Contains(expectedResultStr, resultExpr?.ToString());
        }
    }
}
