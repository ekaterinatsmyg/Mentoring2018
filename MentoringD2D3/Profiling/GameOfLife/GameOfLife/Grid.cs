using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife
{
    class Grid
    {

        private int SizeX;
        private int SizeY;
        private Cell[,] cells;
        private Cell[,] nextGenerationCells;
        private Canvas drawCanvas;
        private Ellipse[,] cellsVisuals;
        private static Random rnd;


        public Grid(Canvas c)
        {
            drawCanvas = c;
            rnd = new Random();
            SizeX = (int) (c.Width / 5);
            SizeY = (int)(c.Height / 5);
            cells = new Cell[SizeX, SizeY];
            nextGenerationCells = new Cell[SizeX, SizeY];
            cellsVisuals = new Ellipse[SizeX, SizeY];


            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    cells[i, j] = new Cell(i, j, 0, GetRandomPattern());
                    nextGenerationCells[i, j] = new Cell(i, j, 0, false);
                    cellsVisuals[i, j] = GetCellVisual(cells[i, j]);
                    drawCanvas.Children.Add(cellsVisuals[i, j]);
                }
            }

            drawCanvas.MouseMove += MouseMove;
        }


        public void Clear()
        {
            for (int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                {
                    cells[i, j] = new Cell(i, j, 0, false);
                    nextGenerationCells[i, j] = new Cell(i, j, 0, false);
                    cellsVisuals[i, j].Fill = Brushes.Gray;
                }
        }


        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            if (e.OriginalSource is Ellipse cellVisual)
            {
                int i = (int)cellVisual.Margin.Left / 5;
                int j = (int)cellVisual.Margin.Top / 5;

                if (!cells[i, j].IsAlive)
                {
                    cells[i, j].IsAlive = true;
                    cells[i, j].Age = 0;
                    cellVisual.Fill = Brushes.White;
                }
            }
        }

        public void UpdateGraphics()
        {
            for (int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                    cellsVisuals[i, j].Fill = GetCellVisualColor(cells[i, j]);
        }

        private Ellipse GetCellVisual(Cell cell)
        {
            return new Ellipse
            {
                Width = 5,
                Height = 5,
                Margin = new Thickness(cell.PositionX, cell.PositionY, 0, 0),
                Fill = GetCellVisualColor(cell)
            };
        }

        private Brush GetCellVisualColor(Cell cell)
        {
            return cell.IsAlive
                ? (cell.Age < 2 ? Brushes.White : Brushes.DarkGray)
                : Brushes.Gray;
        }

        public static bool GetRandomPattern()
        {
            return rnd.NextDouble() > 0.8;
        }
        

        public void UpdateToNextGeneration()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    var currentCell = cells[i, j];
                    var nextGenerationCell = nextGenerationCells[i, j];

                    currentCell.IsAlive = nextGenerationCell.IsAlive;
                    currentCell.Age = nextGenerationCell.Age;

                    UpdateCellVisualInterior(currentCell, cellsVisuals[i, j]);
                }
            }
        }

        private void UpdateCellVisualInterior(Cell cell, Ellipse cellVisual)
        {
            var cellBrush = GetCellVisualColor(cell);

            if (cellBrush != cellVisual.Fill)
            {
                cellVisual.Fill = cellBrush;
            }
        }


        public void Update()
        {
            CalculateNextGeneration();
            UpdateToNextGeneration();
        }

        public void CalculateNextGeneration()    
        {
            bool alive = false;
            int age = 0;

            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    CalculateNextGeneration(i, j, ref alive, ref age);   // OPTIMIZED
                    nextGenerationCells[i, j].IsAlive = alive;  // OPTIMIZED
                    nextGenerationCells[i, j].Age = age;  // OPTIMIZED
                }
            }
        }

        public void CalculateNextGeneration(int row, int column, ref bool isAlive, ref int age)     // OPTIMIZED
        {
            isAlive = cells[row, column].IsAlive;
            age = cells[row, column].Age;

            int count = CountNeighbors(row, column);

            if (isAlive && count < 2)
            {
                isAlive = false;
                age = 0;
            }

            if (isAlive && (count == 2 || count == 3))
            {
                cells[row, column].Age++;
                isAlive = true;
                age = cells[row, column].Age;
            }

            if (isAlive && count > 3)
            {
                isAlive = false;
                age = 0;
            }

            if (!isAlive && count == 3)
            {
                isAlive = true;
                age = 0;
            }
        }

        public int CountNeighbors(int i, int j)
        {
            int count = 0;

            if (i != SizeX - 1 && cells[i + 1, j].IsAlive) count++;
            if (i != SizeX - 1 && j != SizeY - 1 && cells[i + 1, j + 1].IsAlive) count++;
            if (j != SizeY - 1 && cells[i, j + 1].IsAlive) count++;
            if (i != 0 && j != SizeY - 1 && cells[i - 1, j + 1].IsAlive) count++;
            if (i != 0 && cells[i - 1, j].IsAlive) count++;
            if (i != 0 && j != 0 && cells[i - 1, j - 1].IsAlive) count++;
            if (j != 0 && cells[i, j - 1].IsAlive) count++;
            if (i != SizeX - 1 && j != 0 && cells[i + 1, j - 1].IsAlive) count++;

            return count;
        }
    }
}