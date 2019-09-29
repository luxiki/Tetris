using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Tetris
{
    class VM:DependencyObject
    {

        private Image image { get; }
        private static readonly Uri uri = new Uri(System.IO.Directory.GetCurrentDirectory() + @"\Images\Kvadrato.png");
        private BitmapImage bitmap = new BitmapImage(uri);

        public ObservableCollection<Image> blocks { get; } = new ObservableCollection<Image>();
        private Figure figure;



        public int Score
        {
            get { return (int)GetValue(ScoreProperty); }
            set { SetValue(ScoreProperty, value); }
        }
        public static readonly DependencyProperty ScoreProperty =
            DependencyProperty.Register("Score", typeof(int), typeof(VM), new PropertyMetadata(0));



        public DelegateCommand FigureRotateCommand { get; }
        public DelegateCommand FigureDownCommand { get; }
        public DelegateCommand FigureLeftCommand { get; }
        public DelegateCommand FigureRightCommand { get; }

        Timer timer = new Timer(300);

        public VM()
        {
            figure = new Figure();
            FigureRotateCommand = new DelegateCommand(obj => figure.Rotate());
            FigureDownCommand = new DelegateCommand(obj => figure.Down());
            FigureLeftCommand = new DelegateCommand(obj => figure.Left());
            FigureRightCommand = new DelegateCommand(obj => figure.Right());
            figure.Paint += Paint;

            for (int y = 0; y < Figure.VERTICAL; y++)
            {
                for (int x = 0; x < Figure.HORIZONTAL; x++)
                {
                    if (x == 0 || x == Figure.HORIZONTAL - 1 || y == Figure.VERTICAL - 1)
                    {
                        // TODO Можно нарисовать стены другим блоком!
                    }

                    image = new Image();
                    image.Source = bitmap;
                    blocks.Add(image);
                    Canvas.SetLeft(blocks.Last(), x * 20);
                    Canvas.SetTop(blocks.Last(), y * 20);
                }
            }
 
            timer.Elapsed += GameTick;
            timer.AutoReset = true;
            timer.Enabled = true;
            
            //RoutedEvent Ke

            //Paint();
        }

        private void Paint()
        {
            Dispatcher.BeginInvoke(new Action(()=>
            {
                Score = figure.Score;
                int x, y;
                for (int i = 0; i < Figure.HORIZONTAL * Figure.VERTICAL; i++)
                {
                    y = i / Figure.HORIZONTAL;
                    x = i - (y * Figure.HORIZONTAL);
                    if (figure.GetPanel(x,y))
                    {
                        blocks[i].Visibility = Visibility.Visible;
                    }
                    else
                    {
                        blocks[i].Visibility = Visibility.Hidden;
                    }
                }
            }));
        }


        private void GameTick( object obj, ElapsedEventArgs elapsed)
        {
            if (!figure.StepDown())
            {
                Dispatcher.Invoke(() =>
                {
                    timer.Stop();
                    MessageBox.Show("Game over");
                    Application.Current.Shutdown(0);
                });
            }

            
            // Dispatcher.Invoke(Paint);  
        }


    }
}
