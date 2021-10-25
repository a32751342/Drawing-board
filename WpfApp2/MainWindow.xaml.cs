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
            //畫一條直線
            DrawLine();
        }

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

        private void StrokeThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)//粗細
        {
            currentStrokeThickness = Convert.ToInt32(StrokeThicknessSlider.Value);
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)//筆刷顏色
        {
            currentStrokeColor = GetDialogColor();
            currentStrokeBrush = new SolidColorBrush(currentStrokeColor);
            MyLabel.Content=$"筆刷色彩: {currentStrokeColor.ToString()}";
        }

        private Color GetDialogColor()//選擇顏色
        {
            ColorDialog dlg = new ColorDialog();
            dlg.ShowDialog();

            System.Drawing.Color dlgColor = dlg.Color;
            return Color.FromArgb(dlgColor.A, dlgColor.R, dlgColor.G, dlgColor.B);
        }

        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)//取得起點
        {
            MyCanvas.Cursor = System.Windows.Input.Cursors.Cross;//按下滑鼠左鍵將游標改成十字
            start = e.GetPosition(MyCanvas);//起點座標點
            MyLabel.Content = $"座標點:({start})-({dest})";
        }
    }
}
