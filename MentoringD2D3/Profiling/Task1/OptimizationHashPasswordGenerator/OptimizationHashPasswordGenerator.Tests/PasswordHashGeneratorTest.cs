using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OptimizationHashPasswordGeneratorTask;

namespace OptimizationHashPasswordGenerator.Tests
{
    [TestClass]
    public class PasswordHashGeneratorTest
    {
        private readonly string password = "password123";
        private readonly byte[] salt = { 12, 23, 34, 45, 56, 67, 78, 89, 90, 01, 12, 23, 34, 45, 56, 67 };
        private PasswordHashGenerator  passwordHashGenerator;

        [TestInitialize]
        public void Setup()
        {
            passwordHashGenerator = new PasswordHashGenerator();
        }

        [TestMethod]
        public void GeneratePasswordHashUsingSaltTest()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            var stopwatch = Stopwatch.StartNew();

            passwordHashGenerator.GeneratePasswordHashUsingSalt(password, salt);

            stopwatch.Stop();

            Debug.WriteLine($"original version: {stopwatch.ElapsedMilliseconds} ms");
        }

        [TestMethod]
        public void GeneratePasswordHashUsingSaltOptimized_v1Test()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            var stopwatch = Stopwatch.StartNew();

            passwordHashGenerator.GeneratePasswordHashUsingSaltOptimized_v1(password, salt);

            stopwatch.Stop();

            Debug.WriteLine($"original version: {stopwatch.ElapsedMilliseconds} ms");
        }

        [TestMethod]
        public void GeneratePasswordHashUsingSaltOptimized_v2Test()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            var stopwatch = Stopwatch.StartNew();

            passwordHashGenerator.GeneratePasswordHashUsingSaltOptimized_v2(password, salt);

            stopwatch.Stop();

            Debug.WriteLine($"original version: {stopwatch.ElapsedMilliseconds} ms");
        }

        [TestMethod]
        public void GeneratePasswordHashUsingSaltOptimized_v3Test()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            var stopwatch = Stopwatch.StartNew();

            passwordHashGenerator.GeneratePasswordHashUsingSaltOptimized_v3(password, salt);

            stopwatch.Stop();

            Debug.WriteLine($"original version: {stopwatch.ElapsedMilliseconds} ms");
        }

        [TestMethod]
        public void GeneratePasswordHashUsingSaltOptimized_v3_1Test()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            var stopwatch = Stopwatch.StartNew();

            passwordHashGenerator.GeneratePasswordHashUsingSaltOptimized_v3_1(password, salt);

            stopwatch.Stop();

            Debug.WriteLine($"original version: {stopwatch.ElapsedMilliseconds} ms");
        }


        [TestMethod]
        public void GeneratePasswordHashUsingSaltOptimized_v3_2Test()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            var stopwatch = Stopwatch.StartNew();

            passwordHashGenerator.GeneratePasswordHashUsingSaltOptimized_v3_2(password, salt);

            stopwatch.Stop();

            Debug.WriteLine($"original version: {stopwatch.ElapsedMilliseconds} ms");
        }

        [TestMethod]
        public void GeneratePasswordHashUsingSaltOptimized_v4Test()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            var stopwatch = Stopwatch.StartNew();

            passwordHashGenerator.GeneratePasswordHashUsingSaltOptimized_v4(password, salt);

            stopwatch.Stop();

            Debug.WriteLine($"original version: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
