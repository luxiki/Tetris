using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Tetris
{
    public class Block
    {
        public bool Visible { get; set; }
        //public  int X { get; set; }
        //public int Y { get; set; }
        public Image image { get; private set; }
        private static readonly Uri uri = new Uri(System.IO.Directory.GetCurrentDirectory() + @"\Images\Kvadrato.png");
        private BitmapImage bitmap = new BitmapImage(uri);

        public Block ( int x , int y)
        {
            Visible = true;
            image = new Image();
            image.Source = bitmap;
            Canvas.SetRight(image, x);
            Canvas.SetTop(image, y);
        }



    }
}