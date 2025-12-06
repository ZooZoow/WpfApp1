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
            // 1. Проверка заполнения полей
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

            // 2. Проверка выбора дня недели
            if (!IsAnyDaySelected())
            {
                MessageBox.Show(
                    "Выберите день недели.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // 3. Проверка на числовые значения
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

            // 4. Проверка на положительные значения
            if (duration <= 0 || pricePerMinute <= 0)
            {
                MessageBox.Show(
                    "Длительность и цена за минуту должны быть положительными числами.",
                    "Ошибка ввода",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            // 5. Расчёт стоимости
            double totalCost = CalculateCost(duration, pricePerMinute);

            // 6. Вывод результата в TxtCost
            TxtCost.Text = $"{totalCost:F2}";
        }

        // Проверка, выбран ли хотя бы один день недели
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

        // Расчёт стоимости разговора
        private double CalculateCost(double duration, double pricePerMinute)
        {
            double cost = 0;

            // Стоимость первых 30 минут
            double first30Minutes = Math.Min(duration, 30);
            cost += first30Minutes * pricePerMinute;

            // Стоимость минут после 30-й (скидка 30%)
            if (duration > 30)
            {
                double extraMinutes = duration - 30;
                cost += extraMinutes * pricePerMinute * 0.7; // 30% скидка
            }

            // Скидка 15% в выходные (суббота, воскресенье)
            if (IsWeekendSelected())
            {
                cost *= 0.85; // 15% скидка
            }

            return cost;
        }

        // Проверка, выбран ли выходной день
        private bool IsWeekendSelected()
        {
            return RbSaturday.IsChecked == true || RbSunday.IsChecked == true;
        }
    }
}

