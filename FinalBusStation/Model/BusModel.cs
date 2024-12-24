using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalBusStation.Model
{
    public class BusModel
    {
        private readonly Model1 _context;

        public BusModel()
        {
            _context = new Model1();
        }

        // Получить все автобусы
        public List<buses> GetAllBuses()
        {
            return _context.buses.ToList();
        }

        // Добавить новый автобус
        public void AddBus(buses bus)
        {
            _context.buses.Add(bus);
            _context.SaveChanges();
        }

        // Обновить информацию об автобусе
        public void UpdateBus(buses bus)
        {
            var existingBus = _context.buses.Find(bus.bus_id);
            if (existingBus != null)
            {
                existingBus.model = bus.model;
                existingBus.license_plate = bus.license_plate;
                existingBus.capacity = bus.capacity;

                _context.SaveChanges();
            }
        }

        // Удалить автобус
        public void DeleteBus(buses bus)
        {
            _context.buses.Remove(bus);
            _context.SaveChanges();
        }
    }
}
