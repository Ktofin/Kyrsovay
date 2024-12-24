using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinalBusStation.Model
{
    public class DriverModel
    {
        private readonly Model1 _context;

        public DriverModel()
        {
            _context = new Model1();
        }

        public List<drivers> GetAllDrivers()
        {
            return _context.drivers.ToList();
        }

        public void SaveDriver(drivers driver, bool isNewDriver)
        {
            try
            {
                if (isNewDriver)
                {
                    driver.driver_id = 0; // Сброс идентификатора для автоинкремента
                    _context.drivers.Add(driver);
                }
                else
                {
                    _context.Entry(driver).State = EntityState.Modified;
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении водителя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void DeleteDriver(drivers driver)
        {
            _context.drivers.Remove(driver);
            _context.SaveChanges();
        }
    }
}
