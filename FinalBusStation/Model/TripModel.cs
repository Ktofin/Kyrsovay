using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinalBusStation.Model
{
    public class TripModel
    {
        private readonly Model1 _context;

        public TripModel()
        {
            _context = new Model1();
        }

        // Получение доступных рейсов
        public List<trips> GetAvailableTrips()
        {
            return _context.trips
                .Include("schedules.buses")
                .Include("schedules.routes")
                .Where(t => t.capacity > 0) // Фильтрация рейсов с ненулевой вместимостью
                .ToList();
        }

        // Покупка билета
        public bool PurchaseTicket(int tripId, int buyerId, string passengerFirstName, string passengerLastName, string passengerMiddleName, string passengerPassportNumber, decimal price)
        {
            try
            {
                var trip = _context.trips.Find(tripId);
                if (trip != null && trip.capacity > 0)
                {
                    var ticket = new tickets
                    {
                        trip_id = tripId,
                        buyer_id = buyerId,
                        passenger_first_name = passengerFirstName,
                        passenger_last_name = passengerLastName,
                        passenger_middle_name = passengerMiddleName,
                        passenger_passport_number = passengerPassportNumber,
                        price = price,
                        purchase_date = System.DateTime.Now
                    };

                    // Уменьшаем вместимость на 1, но только если она больше 0
                    if (trip.capacity > 1)
                    {
                        trip.capacity -= 1;
                    }
                    else
                    {
                        trip.capacity = 0; // Устанавливаем 0, если это был последний билет
                    }

                    _context.tickets.Add(ticket);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public List<string> GetUniqueStartLocations()
        {
            return _context.routes.Select(r => r.start_location).Distinct().ToList();
        }

        public List<string> GetUniqueEndLocations()
        {
            return _context.routes.Select(r => r.end_location).Distinct().ToList();
        }

        // Получение билетов пользователя
        public List<tickets> GetUserTickets(int userId)
        {
            return _context.tickets.Where(t => t.buyer_id == userId).ToList();
        }

        public bool UserHasCard(int userId)
        {
            return _context.user_cards.Any(c => c.user_id == userId);
        }
        // Добавить рейсы на весь день
        public void AddFullDayTrips(int scheduleId, decimal price)
        {
            var schedule = _context.schedules.Find(scheduleId);

            if (schedule == null)
                throw new Exception("Расписание не найдено.");

            // Время начала и конца из расписания
            var startTime = schedule.start_time;
            var endTime = schedule.end_time;
            var interval = schedule.interval_minutes;

            // Начальные дата и время отправления и прибытия
            var today = DateTime.Today;
            var departureDateTime = today.Add(startTime);
            var arrivalDateTime = departureDateTime.AddMinutes(interval);

            while (departureDateTime.Date == today && arrivalDateTime.TimeOfDay <= endTime)
            {
                Console.WriteLine($"Создание рейса: Отправление - {departureDateTime}, Прибытие - {arrivalDateTime}, Цена - {price}");

                if (!_context.trips.Any(t => t.schedule_id == scheduleId && t.departure_datetime == departureDateTime))
                {
                    var trip = new trips
                    {
                        trip_id = 0,
                        schedule_id = scheduleId,
                        departure_datetime = departureDateTime,
                        arrival_datetime = arrivalDateTime,
                        price = price,
                        capacity = schedule.buses.capacity
                    };

                    _context.trips.Add(trip);
                }

                // Увеличиваем время отправления и прибытия
                departureDateTime = departureDateTime.AddMinutes(interval);
                arrivalDateTime = departureDateTime.AddMinutes(interval);
            }

            try
            {
                _context.SaveChanges();
                MessageBox.Show("Рейсы успешно созданы.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении рейсов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Удалить рейс
        public void DeleteTrip(trips trip)
        {
            _context.trips.Remove(trip);
            _context.SaveChanges();
        }

        // Получить все рейсы
        public List<trips> GetAllTrips()
        {
            return _context.trips.ToList();
        }


        public List<tickets> GetMonthlyTickets()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            return _context.tickets
                           .Include(nameof(tickets.trips))
                           .Where(tickets => tickets.purchase_date.HasValue &&
                                       tickets.purchase_date.Value.Month == currentMonth &&
                                       tickets.purchase_date.Value.Year == currentYear)
                           .ToList();
        }
    }
}
   
