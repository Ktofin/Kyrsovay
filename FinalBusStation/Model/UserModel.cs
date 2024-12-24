using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalBusStation.Model
{
    public class UserModel
    {
        private readonly Model1 _context;

        public UserModel()
        {
            _context = new Model1();
        }

        // Получить всех пользователей
        public List<users> GetAllUsers()
        {
            return _context.users.ToList();
        }

        // Добавить нового пользователя
        public void AddUser(users user)
        {
            _context.users.Add(user);
            _context.SaveChanges();
        }

        // Обновить информацию о пользователе
        public void UpdateUser(users user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }



        public void SaveUser(users user, bool isNewUser)
        {
            if (isNewUser)
            {
                _context.users.Add(user);
            }
            else
            {
                var existingUser = _context.users.Find(user.user_id);
                if (existingUser != null)
                {
                    existingUser.first_name = user.first_name;
                    existingUser.last_name = user.last_name;
                    existingUser.middle_name = user.middle_name;
                    existingUser.username = user.username;
                    existingUser.email = user.email;
                    existingUser.phone_number = user.phone_number;
                    existingUser.passport_number = user.passport_number;
                    existingUser.role = user.role;
                    existingUser.password_hash = user.password_hash;
                   
                }
            }

            _context.SaveChanges();
        }

        public void DeleteUser(users user)
        {
            var userToDelete = _context.users.Find(user.user_id);
            if (userToDelete != null)
            {
                _context.users.Remove(userToDelete);
                _context.SaveChanges();
            }
        }

    }
}
