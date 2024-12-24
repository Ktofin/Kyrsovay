using FinalBusStation.ViewModel;
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
using System.Windows.Shapes;

namespace FinalBusStation.View
{
    /// <summary>
    /// Логика взаимодействия для PassengerView.xaml
    /// </summary>
    public partial class PassengerView : Window
    {
        public PassengerView()
        {
            InitializeComponent();
            //DataContext = new PassengerViewModel(username);
        }
    }
}
