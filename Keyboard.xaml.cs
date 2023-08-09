using SharpDX.XInput;
using System;
using System.Windows;
using TEST1;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace ControlWin
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private readonly Mouse _m = new();
        public Window1()
        {
            InitializeComponent();
            Hide();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
        private void Minimise_Click(object sender, RoutedEventArgs e)
        {
            w.WindowState = WindowState.Minimized;
        }
        private void StatBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            var v = ((Button)sender).Content.ToString() ?? throw new("Content of sender isn't a string");

            await Task.Delay(1000);

            if (_m.Shift)
            {
                Mouse.LowerStringEmulate(v);
                Shift_Button_Click(sender, e);
            }
            else
            {
                Mouse.UpperStringEmulate(v);
            }
        }

        private void Shift_Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < KeyBoard.Children.Count - 1; i++)
            {
                if (KeyBoard.Children[i] is StackPanel val)
                {
                    for (int j = 1; j < val.Children.Count - 1; j++)
                    {
                        string content = ((Button)val.Children[j]).Content.ToString() ?? throw new("Content of children isn't a string");

                        if (!_m.Shift)
                            ((Button)val.Children[j]).Content = content.ToUpper();
                        else
                            ((Button)val.Children[j]).Content = content.ToLower();
                    }
                }
            }
            _m.Shift = !_m.Shift;
        }
    }
}
