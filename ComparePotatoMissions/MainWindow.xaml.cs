using ComparePotatoMissions.Forms;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComparePotatoMissions
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();



        }

        private void BtnOpenPotatoEvent_Click(object sender, RoutedEventArgs e)
        {
            bool canOpenNewWindow = true;
            foreach (Window window in App.Current.Windows)
            {              
                if (window is WindowPotatoEvent)
                {
                    canOpenNewWindow = false;
                }                    
            }

            if (canOpenNewWindow)
            {
                WindowPotatoEvent newWindowPotatoEvent = new WindowPotatoEvent()
                {
                    Owner = this
                };
                newWindowPotatoEvent.Show();
            }
        }
    }
}
