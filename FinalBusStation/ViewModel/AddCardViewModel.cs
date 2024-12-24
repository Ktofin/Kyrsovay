using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using FinalBusStation.Model;

namespace FinalBusStation.ViewModel
{
    public class AddCardViewModel : BaseViewModel
    {
        private readonly AuthModel _authModel;
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string CVVCode { get; set; }

        public ICommand AddCardCommand { get; }

        public AddCardViewModel()
        {
            _authModel = new AuthModel();
            AddCardCommand = new RelayCommand(ExecuteAddCard);
        }

        private void ExecuteAddCard(object parameter)
        {
            try
            {
                if (string.IsNullOrEmpty(CardNumber) || string.IsNullOrEmpty(ExpirationDate) || string.IsNullOrEmpty(CVVCode))
                {
                    MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Преобразование даты окончания карты
                DateTime expiration;
                if (!DateTime.TryParseExact(ExpirationDate, "MM/yy", null, System.Globalization.DateTimeStyles.None, out expiration))
                {
                    MessageBox.Show("Неверный формат даты окончания. Используйте формат MM/YY.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _authModel.AddCard(UserSession.UserId, CardNumber, expiration, CVVCode);
                MessageBox.Show("Карта успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении карты: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
