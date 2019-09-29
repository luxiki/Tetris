using System;
using System.Diagnostics;

namespace Tetris
{
    public static class FigureRandomMatrix
    {
        private static Random random = new Random();

        private static readonly bool[,] I = new bool[,]
        {
            {false,false,true,false },
            {false,false,true,false },
            {false,false,true,false },
            {false,false,true,false }
        };

        private static readonly bool[,] L = new bool[,]
        {
            {false,false,true,false },
            {true,true,true,false },
            {false,false,false,false },
            {false,false,false,false }
        };

        private static readonly bool[,] J = new bool[,]
        {
            {false,true,false,false },
            {false,true,true,true },
            {false,false,false,false },
            {false,false,false,false }
        };

        private static readonly bool[,] O = new bool[,]
        {
            {false,false,false,false },
            {false,true,true,false },
            {false,true,true,false },
            {false,false,false,false }
        };

        private static readonly bool[,] S = new bool[,]
        {
            {false,false,false,false },
            {false,false,true,true },
            {false,true,true,false },
            {false,false,false,false }
        };

        private static readonly bool[,] T = new bool[,]
        {
            {false,false,false,false },
            {false,false,true,false },
            {false,true,true,true },
            {false,false,false,false }
        };

        private static readonly bool[,] Z = new bool[,]
        {
            {false,false,false,false },
            {true,true,false,false },
            {false,true,true,false },
            {false,false,false,false }
        };

        public static bool[,] GetRandomMatrix()
        {
            
            switch (random.Next(6))
            {
                case 0: return I;
                case 1: return J;
                case 2: return L;
                case 3: return O;
                case 4: return S;
                case 5: return T;
                case 6: return Z;
            }

            return null;
        }

    }
}