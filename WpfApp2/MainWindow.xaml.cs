using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        Point start, dest;//起點&終點
        Color currentStrokeColor;//筆刷顏色
        Brush currentStrokeBrush = new SolidColorBrush(Colors.Black);//筆刷類型 預設黑色
        int currentStrokeThickness;//筆刷粗細
        string currentShape;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void myCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)//取得終點
        {
            dest = e.GetPosition(MyCanvas);//終點座標
            MyLabel.Content = $"座標點:({start})-({dest})";

        }

        private void MyCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (currentShape)
            {
                case "Line":
                    //畫一條直線
                    DrawLine();
                    break;
                case "Rectangle":
                    //畫一個正方形
                    DrawRectangle();
                    break;
            }
        }

        /*畫正方形*/
        private void DrawRectangle()
        {
            double width = dest.X - start.X;
            double height = dest.Y - start.Y;
            Rectangle newRectangle = new Rectangle();
            {
                newRectangle.Stroke = currentStrokeBrush;
                newRectangle.StrokeThickness = currentStrokeThickness;
                width = width;
                Height = height;
            };
            MyCanvas.Children.Add(newRectangle);
        }

        /*畫直線*/
        private void DrawLine()
        {
            Line newLine = new Line();
            newLine.Stroke = currentStrokeBrush;
            newLine.StrokeThickness = currentStrokeThickness;
            newLine.X1 = start.X;
            newLine.Y1 = start.Y;
            newLine.X2 = dest.X;
            newLine.Y2 = dest.Y;

            MyCanvas.Children.Add(newLine);
        }

        /*筆刷粗細*/
        private void StrokeThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            currentStrokeThickness = Convert.ToInt32(StrokeThicknessSlider.Value);
        }

        /*筆刷顏色*/
        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            currentStrokeColor = GetDialogColor();
            currentStrokeBrush = new SolidColorBrush(currentStrokeColor);
            MyLabel.Content=$"筆刷色彩: {currentStrokeColor.ToString()}";
        }

        /*選擇顏色*/
        private Color GetDialogColor()
        {
            ColorDialog dlg = new ColorDialog();
            dlg.ShowDialog();

            System.Drawing.Color dlgColor = dlg.Color;
            return Color.FromArgb(dlgColor.A, dlgColor.R, dlgColor.G, dlgColor.B);
        }

        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Button;
            currentShape = btn.Content.ToString();
        }

        /*取得起點*/
        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MyCanvas.Cursor = System.Windows.Input.Cursors.Cross;//按下滑鼠左鍵將游標改成十字
            start = e.GetPosition(MyCanvas);//起點座標點
            MyLabel.Content = $"座標點:({start})-({dest})";
        }
    }
}
