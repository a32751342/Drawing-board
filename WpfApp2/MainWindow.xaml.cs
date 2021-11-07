using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RadioButton = System.Windows.Controls.RadioButton;

namespace WpfApp2
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        Point start, dest;//起點&終點
        Color currentFillColor;//填滿顏色
        Color currentStrokeColor;//筆刷顏色
        Brush currentFillBrush = new SolidColorBrush(Colors.Black);//填滿顏色 預設黑色
        Brush currentStrokeBrush = new SolidColorBrush(Colors.Black);//筆刷類型 預設黑色
        int currentStrokeThickness=1;//筆刷粗細
        string currentShape = "Line";//預設畫線模式
        string currentAction = "Draw";//預設為畫圖模式
        public MainWindow()
        {
            InitializeComponent();
        }

        /*取得終點*/
        private void myCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            switch(currentAction)
            {
                case "Draw":
                    //畫圖模式,滑鼠左鍵被按住時,持續取得滑鼠座標
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        MyCanvas.Cursor = System.Windows.Input.Cursors.Pen;//更改游標為畫筆
                        dest = e.GetPosition(MyCanvas);//終點座標
                        MyLabel.Content = $"座標點:({start})-({dest})";
                    }
                    break;
                case "Erase"://清除模式
                    var selectedShape = e.OriginalSource as Shape;//取得滑鼠碰到的物件
                    MyCanvas.Children.Remove(selectedShape);//清除MyCanvas子項目
                    break;
                default:
                    return;
            }
        }

        /*繪圖*/
        private void MyCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (currentShape)
            {
                case "Line":
                    //畫一條直線
                    DrawLine();
                    break;
                case "Rectangle":
                    //畫一個矩形
                    DrawRectangle();
                    break;
                case "Ellipse":
                    //畫一個橢圓形
                    DrawEllipse();
                    break;
                case "Polygon":
                    //畫一個多邊形
                    DrawPolygon();
                    break;
            }
            MyCanvas.Cursor = System.Windows.Input.Cursors.Arrow;//游標維持箭頭
        }

        /*畫多邊形    !!!!!!!!!!不會!!!!!!!!!!*/
        private void DrawPolygon()
        {
            
        }

        /*畫橢圓形*/
        private void DrawEllipse()
        {
            AdjustPoint();
            double width = dest.X - start.X;
            double height = dest.Y - start.Y;
            Ellipse newEllipse = new Ellipse()
            {
                Stroke = currentStrokeBrush,//筆刷顏色
                StrokeThickness = currentStrokeThickness,//粗細
                Fill = currentFillBrush,//填滿顏色
                Width=width,
                Height=height,
            };
            newEllipse.SetValue(Canvas.LeftProperty, start.X);
            newEllipse.SetValue(Canvas.TopProperty, start.Y);
            MyCanvas.Children.Add(newEllipse);//加入為MyCanvas子項目
        }

        /*畫矩形*/
        private void DrawRectangle()
        {
            AdjustPoint();//不受限繪圖方向(重設XY)
            double width = dest.X - start.X;//寬
            double height = dest.Y - start.Y;//高
            Rectangle newRectangle = new Rectangle();
            {
                newRectangle.Stroke = currentStrokeBrush;//筆刷顏色
                newRectangle.StrokeThickness = currentStrokeThickness;//粗細
                newRectangle.Fill = currentFillBrush;//填滿顏色
                newRectangle.Width = width;
                newRectangle.Height = height;
            };
            newRectangle.SetValue(Canvas.LeftProperty, start.X);//左上角起點
            newRectangle.SetValue(Canvas.TopProperty, start.Y);
            MyCanvas.Children.Add(newRectangle);//加入為MyCanvas子項目
        }

        /*重設XY 以不受限特定繪圖方向*/
        private void AdjustPoint()
        {
            double X_min, Y_min, X_max, Y_max;

            X_min = Math.Min(start.X, dest.X);
            Y_min = Math.Min(start.Y, dest.Y);
            X_max = Math.Max(start.X, dest.X);
            Y_max = Math.Max(start.Y, dest.Y);

            start.X = X_min;
            start.Y = Y_min;
            dest.X = X_max;
            dest.Y = Y_max;
        }

        /*畫直線*/
        private void DrawLine()
        {
            Line newLine = new Line();
            newLine.Stroke = currentStrokeBrush;//筆刷顏色
            newLine.StrokeThickness = currentStrokeThickness;//粗細
            newLine.X1 = start.X;
            newLine.Y1 = start.Y;
            newLine.X2 = dest.X;
            newLine.Y2 = dest.Y;
            MyCanvas.Children.Add(newLine);//加入為MyCanvas子項目
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
            ColorButton.Background = currentStrokeBrush;//更改按鍵背景為所選顏色
        }

        /*選擇顏色*/
        private Color GetDialogColor()
        {
            ColorDialog dlg = new ColorDialog();
            dlg.ShowDialog();

            System.Drawing.Color dlgColor = dlg.Color;
            return Color.FromArgb(dlgColor.A, dlgColor.R, dlgColor.G, dlgColor.B);
        }

        /*畫圖模式共用*/
        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton btn = sender as RadioButton;//轉型成button
            currentShape = btn.Content.ToString();//currentShape讀取存字串按鍵類型
            currentAction = "Draw";//畫圖模式
        }

        /*填滿色彩*/
        private void FillButton_Click(object sender, RoutedEventArgs e)
        {
            currentFillColor = GetDialogColor();
            currentFillBrush = new SolidColorBrush(currentFillColor);
            FillButton.Background = currentFillBrush;
        }

        /*橡皮擦功能*/
        private void EraseButton_Click(object sender, RoutedEventArgs e)
        {
            currentAction = "Erase";//清除模式
        }

        /*隱藏顯示工具列*/
        private void menuCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (menuCheckBox.IsChecked == true)
            {
                MyToolBarTray.Visibility = Visibility.Visible;//顯示工具列
                MyCanvas.Height -= MyToolBarTray.Height;
            }
            else
            {
                MyToolBarTray.Visibility = Visibility.Collapsed;//隱藏工具列,並且不保留空位
                MyCanvas.Height += MyToolBarTray.Height;
            }
        }

        /*清空畫布*/
        private void ClearCanvasButton_Click(object sender, RoutedEventArgs e)
        {
            MyCanvas.Children.Clear();//清除所有子項目
            MyCanvas.Cursor = System.Windows.Input.Cursors.Arrow;//游標改回箭頭
            MyLabel.Content = "Ready";
        }

        /*儲存畫布*/
        private void SaveCanvasMenuItem_Click(object sender, RoutedEventArgs e)
        {
            int w = Convert.ToInt32(MyCanvas.RenderSize.Width);//取得畫布寬度
            int h = Convert.ToInt32(MyCanvas.RenderSize.Height);//取得畫布高度

            //將MyCanvas轉存成bitmp
            RenderTargetBitmap rtb = new RenderTargetBitmap(w, h, 64d, 64d, PixelFormats.Default);
            rtb.Render(MyCanvas);

            //將bitmap編碼成png jpg格式
            PngBitmapEncoder png = new PngBitmapEncoder();
            PngBitmapEncoder jpg = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(rtb));
            jpg.Frames.Add(BitmapFrame.Create(rtb));

            //開啟存檔對話方塊
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "儲存小畫家檔案";
            saveFileDialog.DefaultExt = "*.png";
            saveFileDialog.DefaultExt = "*.jpg";
            saveFileDialog.Filter = "PNG檔案(*.png)|*.png|JPG檔案(*.jpg)|*.jpg|全部檔案|*.*";
            saveFileDialog.ShowDialog();
            string path = saveFileDialog.FileName;

            //儲存png jpg檔案
            using (var fs = File.Create(path))
            {
                png.Save(fs);
                jpg.Save(fs);
            }
        }
        /*點擊滑鼠右鍵出現選單*/
        private void MenuItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

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
