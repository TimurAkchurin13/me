using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace z2
{
    public partial class MainWindow : Window
    {
        private Point? startPoint;
        private Point? endPoint;
        private bool isDrawing = false;
        private bool isEditing = false;
        private bool isDeleting = false;
        private Color brushColor = Colors.Black;
        private int brushSize = 5;

        public MainWindow()
        {
            InitializeComponent();

            DrawingCanvas.MouseDown += DrawingCanvas_MouseDown;
            DrawingCanvas.MouseMove += DrawingCanvas_MouseMove;
            DrawingCanvas.MouseUp += DrawingCanvas_MouseUp;

            ColorPicker.SelectionChanged += ColorPicker_SelectionChanged;
            BrushSizeSlider.ValueChanged += BrushSizeSlider_ValueChanged;
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AboutDeveloperMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Разработчик: Акчурин Тимур \nКонтакт: +79090982695", "О разработчике", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DrawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                startPoint = e.GetPosition(DrawingCanvas);
                isDrawing = DrawingModeRadioButton.IsChecked == true;
                isDeleting = DeletingModeRadioButton.IsChecked == true;
            }
        }

        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && startPoint.HasValue)
            {
                endPoint = e.GetPosition(DrawingCanvas);

                if (isDrawing)
                {
                    DrawLine(startPoint.Value, endPoint.Value);
                    startPoint = endPoint;
                }
                else if (isDeleting)
                {
                    Erase(endPoint.Value);
                }
            }
        }

        private void DrawingCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                startPoint = null;
                endPoint = null;
                isDrawing = false;
                isDeleting = false;
            }
        }

        private void DrawLine(Point start, Point end)
        {
            Line line = new Line
            {
                Stroke = new SolidColorBrush(brushColor),
                StrokeThickness = brushSize,
                X1 = start.X,
                Y1 = start.Y,
                X2 = end.X,
                Y2 = end.Y
            };

            DrawingCanvas.Children.Add(line);
        }

        private void ColorPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)ColorPicker.SelectedItem;
            switch (selectedItem.Content.ToString())
            {
                case "Красный":
                    brushColor = Colors.Red;
                    break;
                case "Зеленый":
                    brushColor = Colors.Green;
                    break;
                case "Синий":
                    brushColor = Colors.Blue;
                    break;
                case "Черный":
                    brushColor = Colors.Black;
                    break;
            }
        }

        private void BrushSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            brushSize = (int)BrushSizeSlider.Value;
        }

        private void Erase(Point position)
        {
            var eraser = new Rectangle
            {
                Fill = Brushes.White,
                Width = brushSize,
                Height = brushSize
            };
            Canvas.SetLeft(eraser, position.X - (brushSize / 2));
            Canvas.SetTop(eraser, position.Y - (brushSize / 2));
            DrawingCanvas.Children.Add(eraser);
        }
    }
}