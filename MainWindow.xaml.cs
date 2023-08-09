using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Threading.Tasks;
using ControlWin;
using System.Windows.Data;

namespace ControlWin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Window1 KeyboardWin;
        public enum StateUI : int
        {
            Black,
            White, 
        }
        private StateUI _state = StateUI.White;
        private void ChangeUI()
        {
            _state = _state switch
            {
                StateUI.White => BlackUI(),
                StateUI.Black => WhiteUI(),
                _ => throw new NotImplementedException(),
            };
        }
        private static StateUI BlackUI()
        {
            Application.Current.Resources.MergedDictionaries[1].Source = new Uri("Resourses\\BlackColorsUI.xaml", UriKind.Relative);

            return StateUI.Black;
        }
        private static StateUI WhiteUI()
        {
            Application.Current.Resources.MergedDictionaries[1].Source = new Uri("Resourses\\WhiteColorsUI.xaml", UriKind.Relative);

            return StateUI.White;
        }
        public MainWindow()
        {
            InitializeComponent();
            KeyboardWin = new Window1();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            KeyboardWin.Close();
            Close();
        }
        private void Minimise_Click(object sender, RoutedEventArgs e)
        {
            w.WindowState = WindowState.Minimized;
        }
        private void Moon_Click(object sender, RoutedEventArgs e)
        {
            ChangeUI();
        }
        private void StatBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            KeyboardWin.Visibility = Visibility.Visible;
        }

        async private void w_Initialized(object sender, EventArgs e)
        {
            await Task.Run(() => { _ = new Sharper(this); });
        }
    }
}
