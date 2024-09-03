using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace laba1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _path = "test.txt";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void textBoxX_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Разрешаем только цифры, точку и минус
            e.Handled = !IsTextAllowedForDouble(e.Text, (sender as TextBox).Text);
        }

        private bool IsTextAllowedForDouble(string newText, string currentText)
        {
            string combinedText = currentText + newText;
            return Regex.IsMatch(combinedText, @"^-?\d*\,?\d*$");
        }

        private async void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(textBoxX.Text, out double x))
            {
                textBoxLog.Clear(); // Очищаем лог перед началом расчёта
                await Task.Run(() => CalculateSeries(x));
                File.WriteAllText(_path, textBoxLog.Text);
                MessageBox.Show("Результат сохранён в " + _path);
            }
            else
            {
                MessageBox.Show("Введите действительное число в поле X.");
            }
        }

        private void CalculateSeries(double x)
        {
            double sum = 0.0;
            double term;
            int k = 1;

            do
            {
                term = Math.Sqrt(Math.Abs(x)) / Math.Pow(k, 3);
                sum += term;

                // Логирование текущего шага
                string logMessage = $"k={k}, term={term:F10}, sum={sum:F10}\n";
                Dispatcher.Invoke(() => textBoxLog.AppendText(logMessage));

                k++;
            } while (Math.Abs(term) > 1e-6);
        }

        private int factorial(int n)
        {
            int result = 1;
            for (int i = 1; i < n; i++)
            {
                result *= i;
            }
            return result;
        }
    }
}