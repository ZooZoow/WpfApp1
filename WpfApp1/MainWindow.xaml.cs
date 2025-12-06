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

namespace PhoneCallCalculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtDuration.Text) ||
                string.IsNullOrWhiteSpace(TxtPrice.Text))
            {
                MessageBox.Show(
                    "Пожалуйста, заполните все поля с числовыми данными.",
                    "Ошибка ввода",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (!IsAnyDaySelected())
            {
                MessageBox.Show(
                    "Выберите день недели.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            double duration, pricePerMinute;
            if (!double.TryParse(TxtDuration.Text, out duration) ||
                !double.TryParse(TxtPrice.Text, out pricePerMinute))
            {
                MessageBox.Show(
                    "В поля «Длительность» и «Цена за минуту» можно вводить только числа.",
                    "Ошибка ввода",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (duration <= 0 || pricePerMinute <= 0)
            {
                MessageBox.Show(
                    "Длительность и цена за минуту должны быть положительными числами.",
                    "Ошибка ввода",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            double totalCost = CalculateCost(duration, pricePerMinute);

            TxtCost.Text = $"{totalCost:F2}";
        }

        private bool IsAnyDaySelected()
        {
            return RbMonday.IsChecked == true ||
                   RbTuesday.IsChecked == true ||
                   RbWednesday.IsChecked == true ||
                   RbThursday.IsChecked == true ||
                   RbFriday.IsChecked == true ||
                   RbSaturday.IsChecked == true ||
                   RbSunday.IsChecked == true;
        }

        private double CalculateCost(double duration, double pricePerMinute)
        {
            double cost = 0;

            double first30Minutes = Math.Min(duration, 30);
            cost += first30Minutes * pricePerMinute;

            if (duration > 30)
            {
                double extraMinutes = duration - 30;
                cost += extraMinutes * pricePerMinute * 0.7; 
            }

            if (IsWeekendSelected())
            {
                cost *= 0.85; 
            }

            return cost;
        }

        private bool IsWeekendSelected()
        {
            return RbSaturday.IsChecked == true || RbSunday.IsChecked == true;
        }
    }
}

