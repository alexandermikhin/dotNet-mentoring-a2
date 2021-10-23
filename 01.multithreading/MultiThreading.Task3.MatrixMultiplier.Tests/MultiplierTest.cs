using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    [TestClass]
    public class MultiplierTest
    {
        [TestMethod]
        public void MultiplyMatrix3On3Test()
        {
            TestMatrix3On3(new MatricesMultiplier());
            TestMatrix3On3(new MatricesMultiplierParallel());
        }

        [TestMethod]
        public void ParallelEfficiencyTest()
        {
            const int maxSize = 1000;
            var multiplier = new MatricesMultiplier();
            var parallelMultiplier = new MatricesMultiplierParallel();
            var stopWatch = new Stopwatch();
            var isFaster = false;
            for (int i = 1; i <= maxSize; i++)
            {
                var firstMatrix = CreateMatrix(i);
                var secondMatrix = CreateMatrix(i);

                stopWatch.Start();
                multiplier.Multiply(firstMatrix, secondMatrix);
                var ellapsedSecondsLinear = stopWatch.ElapsedMilliseconds;
                stopWatch.Restart();
                parallelMultiplier.Multiply(firstMatrix, secondMatrix);
                var ellapsedSecondsParallel = stopWatch.ElapsedMilliseconds;
                stopWatch.Reset();
                if (ellapsedSecondsParallel < ellapsedSecondsLinear)
                {
                    isFaster = true;
                    Console.WriteLine("Parallel execution became faster then linear execution when matrix size is " + i);
                    break;
                }
            }

            Assert.IsTrue(isFaster, "Linear execution is faster for matrices up to " + maxSize);
        }

        #region private methods

        void TestMatrix3On3(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(3, 3);
            m1.SetElement(0, 0, 34);
            m1.SetElement(0, 1, 2);
            m1.SetElement(0, 2, 6);

            m1.SetElement(1, 0, 5);
            m1.SetElement(1, 1, 4);
            m1.SetElement(1, 2, 54);

            m1.SetElement(2, 0, 2);
            m1.SetElement(2, 1, 9);
            m1.SetElement(2, 2, 8);

            var m2 = new Matrix(3, 3);
            m2.SetElement(0, 0, 12);
            m2.SetElement(0, 1, 52);
            m2.SetElement(0, 2, 85);

            m2.SetElement(1, 0, 5);
            m2.SetElement(1, 1, 5);
            m2.SetElement(1, 2, 54);

            m2.SetElement(2, 0, 5);
            m2.SetElement(2, 1, 8);
            m2.SetElement(2, 2, 9);

            var multiplied = matrixMultiplier.Multiply(m1, m2);
            Assert.AreEqual(448, multiplied.GetElement(0, 0));
            Assert.AreEqual(1826, multiplied.GetElement(0, 1));
            Assert.AreEqual(3052, multiplied.GetElement(0, 2));

            Assert.AreEqual(350, multiplied.GetElement(1, 0));
            Assert.AreEqual(712, multiplied.GetElement(1, 1));
            Assert.AreEqual(1127, multiplied.GetElement(1, 2));

            Assert.AreEqual(109, multiplied.GetElement(2, 0));
            Assert.AreEqual(213, multiplied.GetElement(2, 1));
            Assert.AreEqual(728, multiplied.GetElement(2, 2));
        }

        private IMatrix CreateMatrix(long size)
        {
            var resultMatrix = new Matrix(size, size);
            for (long i = 0; i < size; i++)
            {
                for (long j = 0; j < size; j++)
                {
                    resultMatrix.SetElement(i, j, (i + 1) * (j + 1));
                }
            }

            return resultMatrix;
        }

        private void compareMatrices(IMatrix a, IMatrix b)
        {
            for (long i = 0; i < a.RowCount; i++)
            {
                for (long j = 0; j < a.ColCount; j++)
                {
                    Assert.AreEqual(a.GetElement(i, j), b.GetElement(i, j));
                }
            }
        }

        #endregion
    }
}
