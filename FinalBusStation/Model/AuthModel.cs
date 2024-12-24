using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinalBusStation.Model
{
    public class AuthModel
    {
        private readonly Model1 _context;

        public AuthModel()
        {
            _context = new Model1();
        }

        public users Authenticate(string username, string password)
        {
            // Проверка логина и пароля
            return _context.users
                           .FirstOrDefault(u => u.username == username && u.password_hash == password);
        }

        public bool Register(string username, string password, string firstName, string lastName, string middleName, string phoneNumber, string passportData, string email)
        {
            try
            {
                if (_context.users.Any(u => u.username == username))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует.");
                    return false;
                }

                if (_context.users.Any(u => u.email == email))
                {
                    MessageBox.Show("Пользователь с таким email уже существует.");
                    return false;
                }

                var user = new users
                {
                    username = username,
                    password_hash = password, // В реальном приложении используйте хеширование пароля
                    first_name = firstName,
                    last_name = lastName,
                    middle_name = middleName,
                    phone_number = phoneNumber,
                    passport_number = passportData,
                    email = email,
                    role = "Passenger"
                };

                _context.users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }
        }

        public void UpdateUser(users user, string newPassword)
        {
            var existingUser = _context.users.Find(user.user_id);
            if (existingUser != null)
            {
                existingUser.first_name = user.first_name;
                existingUser.last_name = user.last_name;
                existingUser.middle_name = user.middle_name;
                existingUser.phone_number = user.phone_number;
                existingUser.email = user.email;
                existingUser.username = user.username;
                existingUser.passport_number = user.passport_number;

                // Обновляем пароль, если новый пароль указан
                if (!string.IsNullOrEmpty(newPassword))
                {
                    existingUser.password_hash = newPassword;
                }

                _context.SaveChanges();
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }


        public users GetUserById(int userId)
        {
            return _context.users.Find(userId);
        }

        public void AddCard(int userId, string cardNumber, DateTime expirationDate, string cvvCode)
        {
            try
            {
                var newCard = new user_cards
                {
                    user_id = userId,
                    card_number = cardNumber,
                    expiration_date = expirationDate,
                    cvv_code = cvvCode
                };

                _context.user_cards.Add(newCard);
                _context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        MessageBox.Show($"Ошибка: {validationError.PropertyName} - {validationError.ErrorMessage}", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении карты: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}

