using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI;
using System.Numerics;
using System.Collections.Generic;
using Windows.Foundation;
using Microsoft.UI;
using System.IO;
using System;

namespace App8
{
    public sealed partial class MainWindow : Window
    {
        private List<Vector2> currentStroke;
        private List<List<Vector2>> strokes;
        private Color currentColor;
        private float strokeWidth;
        private bool isDrawing;

        public MainWindow()
        {
            this.InitializeComponent();
            strokes = new List<List<Vector2>>();
            currentColor = Colors.Black;
            strokeWidth = 2;
            isDrawing = false;
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            // New File 처리
        }

        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            string filePath = @"C:\Users\Desktop\1.txt";

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    for (int i = 0; i < strokes.Count; i++)
                    {
                        foreach (var point in strokes[i])
                        {
                            writer.WriteLine($"{point.X} {point.Y} {currentColor.R} {currentColor.G} {currentColor.B} {currentColor.A} {strokeWidth}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 에러 처리
                Console.WriteLine($"파일 저장 중 오류 발생: {ex.Message}");
            }
        }

        private void MenuFlyoutItem_Click_2(object sender, RoutedEventArgs e)
        {
        }

        private void MenuFlyoutItem_Click_3(object sender, RoutedEventArgs e)
        {
            // Exit 처리
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            strokeWidth = (float)e.NewValue;
        }

        private void myWrite_Click(object sender, RoutedEventArgs e)
        {
            // Write 처리
        }

        private void myRead_Click(object sender, RoutedEventArgs e)
        {
            // Read 처리
        }

        private void myClear_Click(object sender, RoutedEventArgs e)
        {
            strokes.Clear();
            canvas.Invalidate(); // canvas는 CanvasControl의 이름입니다. XAML에서 이름을 확인하세요.
        }

        private void CanvasControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            isDrawing = true;
            currentStroke = new List<Vector2>();
            Windows.Foundation.Point position = e.GetCurrentPoint(sender as CanvasControl).Position;
            currentStroke.Add(new Vector2((float)position.X, (float)position.Y));
        }

        private void CanvasControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            isDrawing = false;
            strokes.Add(currentStroke);
        }

        private void CanvasControl_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (isDrawing)
            {
                Windows.Foundation.Point position = e.GetCurrentPoint(sender as CanvasControl).Position;
                currentStroke.Add(new Vector2((float)position.X, (float)position.Y));
                (sender as CanvasControl).Invalidate();
            }
        }

        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            foreach (var stroke in strokes)
            {
                DrawStroke(args, stroke);
            }

            if (currentStroke != null && currentStroke.Count > 1)
            {
                DrawStroke(args, currentStroke);
            }
        }

        private void DrawStroke(CanvasDrawEventArgs args, List<Vector2> stroke)
        {
            for (int i = 1; i < stroke.Count; i++)
            {
                Vector2 start = stroke[i - 1];
                Vector2 end = stroke[i];

                // 선의 중점 계산
                Vector2 center = (start + end) / 2;

                // 선의 길이 계산
                float length = Vector2.Distance(start, end);

                // FillEllipse 메서드 사용
                args.DrawingSession.FillEllipse(
                    center,  // 타원의 중심
                    length / 2,  // x 반지름 (선의 길이의 절반)
                    strokeWidth / 2,  // y 반지름 (선 두께의 절반)
                    currentColor);
            }
        }

        private void ColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            currentColor = args.NewColor;
        }
    }
}
