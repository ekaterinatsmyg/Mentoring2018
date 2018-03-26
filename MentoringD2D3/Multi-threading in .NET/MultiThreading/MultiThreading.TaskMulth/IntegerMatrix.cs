using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.TaskMulth
{
    /// <summary>
    /// Represents a matrix of integers.
    /// </summary>
    public class IntegerMatrix
    {
        #region Random
        private const int MIN_RANDOM_NUMBER = -100;
        private const int MAX_RANDOM_NUMBER = 100;
        private readonly Random randomGenerator = new Random();
        #endregion
        
        private int[][] matrix;

        /// <summary>
        /// A row's number of matrix.
        /// </summary>
        public int RowsNumber { get; }

        /// <summary>
        /// A column's number of matrix.
        /// </summary>
        public int ColumnsNumber { get; }

        /// <summary>
        /// Gets or sets element of the metrex by row and coulumn position.
        /// </summary>
        /// <param name="indexRow">The number of row where is the requared element of the matrix.</param>
        /// <param name="indexColumn">The number of column where is the requared element of the matrix.</param>
        /// <returns>The requared element of the matrix.</returns>
        public int this[int indexRow, int indexColumn]
        {
            get => matrix[indexRow][indexColumn];
            set
            {
                if (indexRow < 0 || indexColumn < 0 || indexRow > RowsNumber - 1 || indexColumn > ColumnsNumber - 1)
                    throw new ArgumentOutOfRangeException($"{indexRow} or {indexColumn}");

                matrix[indexRow][indexColumn] = value;
            }
        }

        #region ctor
        public IntegerMatrix(int rowsNumber, int columnsNumber)
        {
            RowsNumber = rowsNumber;
            ColumnsNumber = columnsNumber;

            matrix = new int[RowsNumber][];
            for (int i = 0; i < RowsNumber; ++i)
                matrix[i] = new int[ColumnsNumber];
        }
        #endregion

        /// <summary>
        /// Initializes the instance of matrix by random integers.
        /// </summary>
        public void InitializeMatrixWithRandomIntegers()
        {
            
            for (int i = 0; i < RowsNumber; i++)
            {
                for (int j = 0; j < ColumnsNumber; j++)
                {
                    matrix[i][j] = randomGenerator.Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER);
                }
            }
        }
        
    }
}
