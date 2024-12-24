using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalBusStation.Model
{
    public class ScheduleModel
    {
        private readonly Model1 _context;

        public ScheduleModel()
        {
            _context = new Model1();
        }

        // Получение всех расписаний с загрузкой связанных данных
        public List<schedules> GetSchedules()
        {
            return _context.schedules
                           .Include("buses")
                           .Include("routes")
                           .Include("drivers")
                           .ToList();
        }

        // Получение всех маршрутов
        public List<routes> GetRoutes()
        {
            return _context.routes.ToList();
        }

        // Получение всех автобусов
        public List<buses> GetBuses()
        {
            return _context.buses.ToList();
        }

        // Получение всех водителей
        public List<drivers> GetDrivers()
        {
            return _context.drivers.ToList();
        }

        // Добавление нового расписания
        public void AddSchedule(schedules schedule)
        {
            schedule.schedule_id = 0;
            _context.schedules.Add(schedule);
            _context.SaveChanges();
        }

        // Удаление расписания
        public void DeleteSchedule(schedules schedule)
        {
            _context.schedules.Remove(schedule);
            _context.SaveChanges();
        }
    }
}
