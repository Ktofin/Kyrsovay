using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace FinalBusStation.Model
{
    public class TicketModel
    {
        private readonly Model1 _context;

        public TicketModel()
        {
            _context = new Model1();
        }

        // Метод для получения билетов пользователя
        public List<TicketViewModel> GetUserTickets(int userId)
        {
            var tickets = _context.tickets
                .Include(t => t.trips)
                .Include(t => t.trips.schedules)
                .Include(t => t.trips.schedules.routes)
                .Include(t => t.trips.schedules.buses)
                .Include(t => t.trips.schedules.drivers)
                .Where(t => t.buyer_id == userId)
                .ToList();

            return tickets.Select(t => new TicketViewModel
            {
                Departure = t.trips.departure_datetime,
                Arrival = t.trips.arrival_datetime,
                StartLocation = t.trips.schedules.routes.start_location,
                EndLocation = t.trips.schedules.routes.end_location,
                PassengerName = $"{t.passenger_last_name} {t.passenger_first_name} {t.passenger_middle_name}",
                PassengerPassportNumber = t.passenger_passport_number,
                BusModel = t.trips.schedules.buses.model,
                BusLicensePlate = t.trips.schedules.buses.license_plate,
                DriverName = $"{t.trips.schedules.drivers.last_name} {t.trips.schedules.drivers.first_name}",
                Price = t.price,
                PurchaseDate = t.purchase_date ?? DateTime.MinValue,
                Distance = t.trips.schedules.routes.distance ?? 0
            }).ToList();
        }
    }

    public class TicketViewModel
    {
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public string PassengerName { get; set; }
        public string PassengerPassportNumber { get; set; }
        public string BusModel { get; set; }
        public string BusLicensePlate { get; set; }
        public string DriverName { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Distance { get; set; }
    }

}

