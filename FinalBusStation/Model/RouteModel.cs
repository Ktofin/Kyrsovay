using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalBusStation.Model
{
    public class RouteModel
    {
        private readonly Model1 _context;

        public RouteModel()
        {
            _context = new Model1();
        }

        public List<routes> GetRoutes()
        {
            return _context.routes.ToList();
        }

        public void AddRoute(routes route)
        {

            _context.routes.Add(route);
            _context.SaveChanges();
        }

        public void DeleteRoute(routes route)
        {
            var routeToDelete = _context.routes.Find(route.route_id);

            if (routeToDelete != null)
            {
                _context.routes.Remove(routeToDelete);
                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Маршрут не найден в базе данных.");
            }
        }
    }
}
