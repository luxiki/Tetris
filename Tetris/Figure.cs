using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tetris
{
    class Figure
    {
        public static readonly int HORIZONTAL = 15;
        public static readonly int VERTICAL = 30;
        private bool[,] figureMatrix = new bool[4,4];
        public int Score { get; private  set; }
        private const int xOffset = 5;
        public int X { get; private set; }
        public int Y { get; private set; }
        private bool[,] Panel = new bool[HORIZONTAL, VERTICAL];
        private Mutex mutex = new Mutex();
        public event Action Paint;

        public Figure()
        {
            for (int y = 0; y < Figure.VERTICAL; y++)
            {
                for (int x = 0; x < Figure.HORIZONTAL; x++)
                {
                    if (x == 0 || x == Figure.HORIZONTAL - 1 || y == Figure.VERTICAL - 1)
                    {
                        Panel[x, y] = true;
                    }
                }
            }

            Reset();
        }

        private bool Reset()
        {
            X = xOffset;
            Y = 0;
            Array.Copy( FigureRandomMatrix.GetRandomMatrix() , figureMatrix , 16);
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (Panel[x+X,y] && (Panel[x+X,y] == figureMatrix[x,y]))
                    {
                        return false;
                    }

                    Panel[x+X, y] = figureMatrix[x, y];
                }  
            }
            return true;
        }

        private bool Step(int dx , int dy )
        {
            mutex.WaitOne();
            bool colision = false;
            //Y++;

            for (int y = 0; y < 4; y++) 
            { //стирание фигуры
                for (int x = 0; x < 4; x++)
                {
                    if (figureMatrix[x, y])
                    {
                        Panel[x + X, y + Y] = false;
                    }
                }
            }

            for (int y = 0; y < 4; y++)
            { // проверка на столкновение
                for (int x = 0; x < 4; x++)
                {
                    if (figureMatrix[x, y] && Panel[x + dx, y + dy])
                    {
                       colision = true;
                       break;
                    }
                }
            }

            if (colision)
            {// если было столкновение остаёмся на томже месте
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        if (figureMatrix[x, y])
                        {
                            Panel[x + X, y + Y] = true;
                        }
                    }
                }
            }
            else
            {// иначе движемся в указаном направлении
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        if (figureMatrix[x, y])
                        {
                            Panel[x + dx, y + dy] = true;
                        }
                    }
                }

                Paint?.Invoke();
                Y = dy;
                X = dx;
            }
            mutex.ReleaseMutex();
            return colision;

        }

        private void DeletRow()
        {
            int count, _score;
            _score = 0;
            for (int y = 0; y < VERTICAL - 1; y++)
            {
                count = 0;
                for (int x = 1; x < HORIZONTAL - 1; x++)
                {
                    if (Panel[x, y])
                    {
                        count++;
                    }
                }

                if (count == HORIZONTAL - 2)
                {
                    _score++;
                    while (y > 0)
                    {
                        for (int x = 1; x < HORIZONTAL - 1; x++)
                        {
                            Panel[x, y] = Panel[x, y - 1];
                        }

                        y--;
                    }
                }

            }

            switch (_score)
            {
                case 1: Score += 100; break;
                case 2: Score += 300; break;
                case 3: Score += 700; break;
                case 4: Score += 1500; break;
            }
        }

        public bool StepDown()
        {
            if (Step(X, Y + 1))
            {
                DeletRow();
                return Reset();
            }

            return true;
        }

        public bool GetPanel(int x, int y)
        {
            return Panel[x, y];
        }

        public void Down()
        {
            while (!Step(X, Y + 1)) ;
        }

        public void Rotate()
        {
            mutex.WaitOne();
            bool colision = false;
            //Y++;

            for (int y = 0; y < 4; y++)
            { //стирание фигуры
                for (int x = 0; x < 4; x++)
                {
                    if (figureMatrix[x, y])
                    {
                        Panel[x + X, y + Y] = false;
                    }
                }
            }

            bool[,] matrix = new bool[4,4];

            for (int y = 0; y < 4; y++)
            { //
                for (int x = 0; x < 4; x++)
                {
                    if (figureMatrix[x, y])
                    {
                        matrix[y, 3-x] = figureMatrix[x,y];
                    }
                }
            }

            for (int y = 0; y < 4; y++)
            { // проверка на столкновение
                for (int x = 0; x < 4; x++)
                {
                    if (matrix[x, y] && Panel[x + X, y + Y])
                    {
                        colision = true;
                        break;
                    }
                }
            }

            if (colision)
            {// если было столкновение остаёмся на томже месте
                //for (int y = 0; y < 4; y++)
                //{
                //    for (int x = 0; x < 4; x++)
                //    {
                //        if (figureMatrix[x, y])
                //        {
                //            Panel[x + X, y + Y] = true;
                //        }
                //    }
                //}
            }
            else
            {// иначе движемся в указаном направлении
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    { 
                            figureMatrix[x,y] = matrix[x,y];
                            if (figureMatrix[x, y])
                            {
                                Panel[x + X, y + Y] = true;
                            }
                    }
                }


                Paint?.Invoke();

            }
            mutex.ReleaseMutex();
        }

        public void Left()
        {
            Step(X-1, Y);
        }

        public void Right()
        {
            Step(X+1,Y);
        }
    }
}
