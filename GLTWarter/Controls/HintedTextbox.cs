using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GLTWarter.Controls
{
    public class HintedTextBox : TextBox
    {
        VisualBrush backgroundBrush;

        public HintedTextBox()
            : base()
        {
            this.AddHandler(TextBox.TextChangedEvent, new TextChangedEventHandler(HintedTextBox_TextChanged), true);
            this.AddHandler(TextBox.SizeChangedEvent, new SizeChangedEventHandler(HintedTextBox_SizeChanged), true);
            GenerateBrush();
            UpdateHints();
        }

        void HintedTextBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GenerateBrush();
            UpdateHints();
        }

        void HintedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHints();
        }

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(HintedTextBox), new PropertyMetadata(new PropertyChangedCallback(HintedTextBox.onHintsChanged)));
        public String Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        private static void onHintsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((HintedTextBox)d).GenerateBrush();
            ((HintedTextBox)d).UpdateHints();
        }

        private void GenerateBrush()
        {
            TextBlock tb = new TextBlock();
            tb.Text = this.Hint;
            tb.FontFamily = this.FontFamily;
            tb.FontSize = this.FontSize;
            tb.FontStretch = this.FontStretch;
            tb.FontStyle = this.FontStyle;
            tb.FontWeight = this.FontWeight;
            tb.Foreground = SystemColors.GrayTextBrush;
            tb.Background = SystemColors.WindowBrush;
            tb.Margin = new Thickness(5, 0, 0, 0);

            Border b = new Border();
            b.Child = tb;
            b.BorderBrush = SystemColors.WindowBrush;
            b.BorderThickness = new Thickness(1);
            b.Background = SystemColors.WindowBrush;
            b.Width = this.Width;
            b.Height = this.Height;

            backgroundBrush = new VisualBrush(b);
            backgroundBrush.Stretch = Stretch.None;
            backgroundBrush.TileMode = TileMode.None;
            backgroundBrush.AlignmentX = AlignmentX.Left;
            backgroundBrush.AlignmentY = AlignmentY.Center;
        }

        private void UpdateHints()
        {

            this.Background = string.IsNullOrEmpty(this.Text) ? (Brush)backgroundBrush : (Brush)SystemColors.WindowBrush;
        }
    }

}
