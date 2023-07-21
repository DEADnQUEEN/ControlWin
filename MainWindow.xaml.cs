using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Threading.Tasks;

namespace TEST1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum StateUI : int
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
        private StateUI BlackUI()
        {
            Min.Source = new BitmapImage(new Uri(@"/Icons/MinBlack.png", UriKind.Relative));
            Cls.Source = new BitmapImage(new Uri(@"/Icons/CrosBlack.png", UriKind.Relative));
            MoonIco.Source = new BitmapImage(new Uri(@"/Icons/moon.png", UriKind.Relative));


            App.Current.Resources.MergedDictionaries[1].Source = new Uri("Resourses\\BlackColorsUI.xaml", UriKind.Relative);

            return StateUI.Black;
        }
        private StateUI WhiteUI()
        {
            Min.Source = new BitmapImage(new Uri(@"/Icons/Min.png", UriKind.Relative));
            Cls.Source = new BitmapImage(new Uri(@"/Icons/Cros.png", UriKind.Relative));
            MoonIco.Source = new BitmapImage(new Uri(@"/Icons/sun.png", UriKind.Relative));

            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Resourses\\StructValues.xaml", UriKind.Relative) });
            App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Resourses\\WhiteColorsUI.xaml", UriKind.Relative) });

            return StateUI.White;
        }
        async private static void Work()
        {
            await Task.Run(() => { _ = new Sharper(); });
        }
        public MainWindow()
        {
            InitializeComponent();
            Work();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
            this.DragMove();
        }
    }
}
