using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using FinalBusStation.Model;
using System.Windows.Documents;

namespace FinalBusStation.ViewModel
{
    public class AdminViewModel : BaseViewModel
    {
        private readonly BusModel _busModel;


        public ObservableCollection<buses> Buses { get; set; }

        public string NewBusModel { get; set; }
        public string NewBusLicensePlate { get; set; }
        public string NewBusCapacity { get; set; }

        public ICommand AddBusCommand { get; }
        public ICommand EditBusCommand { get; }
        public ICommand DeleteBusCommand { get; }

        private readonly UserModel _userModel;

        public users SelectedUser { get; set; } = new users();
        public ObservableCollection<users> Users { get; set; }
        public bool IsNewUser { get; set; } = false; // Флаг для нового пользователя

        public ICommand SaveUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand SaveBusCommand { get; }
        public ICommand CreateUserCommand { get; }

        private readonly DriverModel _driverModel;

        public ObservableCollection<schedules> Schedules { get; set; }
        public ObservableCollection<routes> Routes { get; set; }

        private readonly ScheduleModel _scheduleModel;

        public schedules SelectedSchedule { get; set; }

        public ICommand AddScheduleCommand { get; }
        public ICommand DeleteScheduleCommand { get; }


        private readonly RouteModel _routeModel;

        private readonly TripModel _tripModel;


        public ICommand GenerateUserReportCommand { get; }
        public ICommand GenerateDriverReportCommand { get; }
        public ICommand GenerateRouteReportCommand { get; }
        public ICommand GenerateTripReportCommand { get; }

        private string _reportText;
        public string ReportText
        {
            get => _reportText;
            set
            {
                _reportText = value;
                OnPropertyChanged(nameof(ReportText));
            }
        }

        public ICommand GenerateMonthlyTicketReportCommand { get; }
        public ICommand PrintReportCommand { get; }

        public AdminViewModel()
        {
            _busModel = new BusModel();
            LoadBuses();

            AddBusCommand = new RelayCommand(ExecuteAddBus);
            EditBusCommand = new RelayCommand(ExecuteEditBus);
            DeleteBusCommand = new RelayCommand(ExecuteDeleteBus);
            SaveBusCommand = new RelayCommand(ExecuteSaveBus);

            _userModel = new UserModel();
            LoadUsers();

            SaveUserCommand = new RelayCommand(ExecuteSaveUser);
            EditUserCommand = new RelayCommand(ExecuteEditUser);
            DeleteUserCommand = new RelayCommand(ExecuteDeleteUser);
            CreateUserCommand = new RelayCommand(ExecuteCreateUser);

            _driverModel = new DriverModel();
            LoadDrivers();

            _scheduleModel = new ScheduleModel();


            SaveDriverCommand = new RelayCommand(ExecuteSaveDriver);
            EditDriverCommand = new RelayCommand(ExecuteEditDriver);
            DeleteDriverCommand = new RelayCommand(ExecuteDeleteDriver);
            CreateNewDriverCommand = new RelayCommand(ExecuteCreateNewDriver);

            Schedules = new ObservableCollection<schedules>(_scheduleModel.GetSchedules());
            Routes = new ObservableCollection<routes>(_scheduleModel.GetRoutes());

            SelectedSchedule = new schedules();

            AddScheduleCommand = new RelayCommand(ExecuteAddSchedule);
            DeleteScheduleCommand = new RelayCommand(param => ExecuteDeleteSchedule((schedules)param));


            _routeModel = new RouteModel();
            Routes1 = new ObservableCollection<routes>(_routeModel.GetRoutes());
            NewRoute = new routes();
            AddRouteCommand = new RelayCommand(ExecuteAddRoute);
            DeleteRouteCommand = new RelayCommand(param => ExecuteDeleteRoute((routes)param));

            _tripModel = new TripModel();
            LoadSchedules1();
            LoadTrips();

            AddFullDayTripsCommand = new RelayCommand(ExecuteAddFullDayTrips);
            DeleteTripCommand = new RelayCommand(ExecuteDeleteTrip);


            GenerateUserReportCommand = new RelayCommand(ExecuteGenerateUserReport);
            GenerateDriverReportCommand = new RelayCommand(ExecuteGenerateDriverReport);
            GenerateRouteReportCommand = new RelayCommand(ExecuteGenerateRouteReport);
            GenerateTripReportCommand = new RelayCommand(ExecuteGenerateTripReport);

            GenerateMonthlyTicketReportCommand = new RelayCommand(ExecuteGenerateMonthlyTicketReport);
            PrintReportCommand = new RelayCommand(ExecutePrintReport);
        }

        private void LoadBuses()
        {
            Buses = new ObservableCollection<buses>(_busModel.GetAllBuses());
            OnPropertyChanged(nameof(Buses));
        }
        public void UpdateBus(buses bus)
        {
            try
            {
                _busModel.UpdateBus(bus); // Предполагается, что в BusModel есть метод UpdateBus
                MessageBox.Show("Данные автобуса обновлены успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadBuses(); // Обновляем список автобусов после изменений
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении автобуса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExecuteSaveBus(object parameter)
        {
            if (parameter is buses bus)
            {
                try
                {
                    _busModel.UpdateBus(bus);
                    MessageBox.Show("Данные автобуса обновлены успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadBuses();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении автобуса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExecuteAddBus(object parameter)
        {
            if (string.IsNullOrWhiteSpace(NewBusModel) || string.IsNullOrWhiteSpace(NewBusLicensePlate) || !int.TryParse(NewBusCapacity, out int capacity))
            {
                MessageBox.Show("Заполните все поля корректно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newBus = new buses
            {
                model = NewBusModel,
                license_plate = NewBusLicensePlate,
                capacity = capacity
            };

            try
            {
                Console.WriteLine($"Model: {newBus.model}, License Plate: {newBus.license_plate}, Capacity: {newBus.capacity}");
                _busModel.AddBus(newBus);
                LoadBuses();

                // Очистка полей после добавления
                NewBusModel = string.Empty;
                NewBusLicensePlate = string.Empty;
                NewBusCapacity = string.Empty;
                OnPropertyChanged(nameof(NewBusModel));
                OnPropertyChanged(nameof(NewBusLicensePlate));
                OnPropertyChanged(nameof(NewBusCapacity));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении автобуса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteEditBus(object parameter)
        {
            if (parameter is buses bus)
            {
                
                _busModel.UpdateBus(bus);
                LoadBuses();
            }
        }

        private void ExecuteDeleteBus(object parameter)
        {
            if (parameter is buses bus)
            {
                var result = MessageBox.Show($"Удалить автобус: {bus.model} ({bus.license_plate})?", "Удалить автобус", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _busModel.DeleteBus(bus);
                    LoadBuses();
                }
            }
        }

        private void LoadUsers()
        {
            Users = new ObservableCollection<users>(_userModel.GetAllUsers());
            OnPropertyChanged(nameof(Users));
        }

        private void ExecuteSaveUser(object parameter)
        {
            try
            {
                if (string.IsNullOrEmpty(SelectedUser.password_hash))
                {
                    MessageBox.Show("Поле пароля не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!string.IsNullOrEmpty(SelectedUser.role))
                {
                    SelectedUser.role = SelectedUser.role.Trim();
                }

                if (IsNewUser)
                {
                    SelectedUser.user_id = 0;
                }

                _userModel.SaveUser(SelectedUser, IsNewUser);
                LoadUsers();
                SelectedUser = new users();
                IsNewUser = false;
                OnPropertyChanged(nameof(SelectedUser));
                MessageBox.Show("Пользователь сохранён успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
                MessageBox.Show($"Ошибка при сохранении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ExecuteEditUser(object parameter)
        {
            if (parameter is users user)
            {
                SelectedUser = user;
                IsNewUser = false; // Редактирование существующего пользователя
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        private void ExecuteCreateUser(object parameter)
        {
            SelectedUser = new users(); // Создаём новый экземпляр пользователя
            IsNewUser = true;           // Устанавливаем флаг для нового пользователя
            OnPropertyChanged(nameof(SelectedUser));
        }

        private void ExecuteDeleteUser(object parameter)
        {
            if (parameter is users user)
            {
                var result = MessageBox.Show($"Удалить пользователя {user.username}?", "Удалить пользователя", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _userModel.DeleteUser(user);
                    LoadUsers();
                }
            }
        }

        // Свойства для работы с водителями
        public ObservableCollection<drivers> Drivers { get; set; } = new ObservableCollection<drivers>();
        public drivers SelectedDriver { get; set; } = new drivers();
        public bool IsNewDriver { get; set; }

        // Команды
        public ICommand SaveDriverCommand { get; }
        public ICommand EditDriverCommand { get; }
        public ICommand DeleteDriverCommand { get; }
        public ICommand CreateNewDriverCommand { get; }

        private void LoadDrivers()
        {
            Drivers.Clear();
            foreach (var driver in _driverModel.GetAllDrivers())
            {
                Drivers.Add(driver);
            }
        }

        private void ExecuteSaveDriver(object parameter)
        {
            _driverModel.SaveDriver(SelectedDriver, IsNewDriver);
            LoadDrivers();
            SelectedDriver = new drivers();
            IsNewDriver = true;
            OnPropertyChanged(nameof(SelectedDriver));
        }

        private void ExecuteEditDriver(object parameter)
        {
            if (parameter is drivers driver)
            {
                SelectedDriver = driver;
                IsNewDriver = false;
                OnPropertyChanged(nameof(SelectedDriver));
            }
        }

        private void ExecuteDeleteDriver(object parameter)
        {
            if (parameter is drivers driver)
            {
                _driverModel.DeleteDriver(driver);
                LoadDrivers();
            }
        }

        private void ExecuteCreateNewDriver(object parameter)
        {
            SelectedDriver = new drivers();
            IsNewDriver = true;
            OnPropertyChanged(nameof(SelectedDriver));
        }

        private void ExecuteAddSchedule(object parameter)
        {
            _scheduleModel.AddSchedule(SelectedSchedule);
            Schedules.Add(SelectedSchedule);
            SelectedSchedule = new schedules();
            OnPropertyChanged(nameof(SelectedSchedule));
        }

        private void ExecuteDeleteSchedule(schedules schedule)
        {
            if (schedule != null)
            {
                _scheduleModel.DeleteSchedule(schedule);
                Schedules.Remove(schedule);
            }
        }


        // Коллекция маршрутов, переименованная на Routes1
        public ObservableCollection<routes> Routes1 { get; set; }

        // Новый маршрут для добавления
        public routes NewRoute { get; set; }

        // Команда для добавления маршрута
        public ICommand AddRouteCommand { get; }

        private void ExecuteAddRoute(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(NewRoute.start_location) &&
                !string.IsNullOrWhiteSpace(NewRoute.end_location) &&
                NewRoute.distance.HasValue)
            {
                _routeModel.AddRoute(NewRoute);
                Routes1.Add(new routes
                {
                    route_id =0,
                    start_location = NewRoute.start_location,
                    end_location = NewRoute.end_location,
                    distance = NewRoute.distance
                });

                NewRoute = new routes();
                OnPropertyChanged(nameof(NewRoute));
            }
        }

        // Команда для удаления маршрута
        public ICommand DeleteRouteCommand { get; }

        private void ExecuteDeleteRoute(routes route)
        {
            if (route != null)
            {
                _routeModel.DeleteRoute(route);
                Routes1.Remove(route);
            }
        }


        // Коллекция расписаний для ComboBox
        public ObservableCollection<schedules> Schedules1 { get; set; } = new ObservableCollection<schedules>();

        // Коллекция рейсов для DataGrid
        public ObservableCollection<trips> Trips { get; set; } = new ObservableCollection<trips>();

        private schedules _selectedSchedule1;
        public schedules SelectedSchedule1
        {
            get => _selectedSchedule1;
            set
            {
                _selectedSchedule1 = value;
                OnPropertyChanged();
            }
        }

        private trips _newTrip = new trips();
        public trips NewTrip
        {
            get => _newTrip;
            set
            {
                _newTrip = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddFullDayTripsCommand { get; }
        public ICommand DeleteTripCommand { get; }

        private void LoadSchedules1()
        {
            Schedules1.Clear();
            var schedules = _scheduleModel.GetSchedules();
            foreach (var schedule in schedules)
            {
                Schedules1.Add(schedule);
            }
            OnPropertyChanged(nameof(Schedules1));
        }

        private void LoadTrips()
        {
            Trips.Clear();
            var trips = _tripModel.GetAllTrips();
            foreach (var trip in trips)
            {
                Trips.Add(trip);
            }
        }

        private void ExecuteAddFullDayTrips(object parameter)
        {
            Console.WriteLine($"Selected Schedule ID: {SelectedSchedule1?.schedule_id}");
            Console.WriteLine($"Price: {NewTrip.price}");

            if (SelectedSchedule1 != null && NewTrip.price > 0)
            {
                try
                {
                    _tripModel.AddFullDayTrips(SelectedSchedule1.schedule_id, NewTrip.price);
                    MessageBox.Show("Рейсы успешно созданы.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadTrips();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании рейсов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите расписание и укажите цену.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void ExecuteDeleteTrip(object parameter)
        {
            if (parameter is trips trip)
            {
                try
                {
                    _tripModel.DeleteTrip(trip);
                    MessageBox.Show("Рейс успешно удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadTrips();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении рейса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void ExecuteGenerateUserReport(object parameter)
        {
            var users = _userModel.GetAllUsers();
            var report = new StringBuilder("Отчёт по пользователям:\n\n");

            foreach (var user in users)
            {
                report.AppendLine($"ID: {user.user_id}, Имя: {user.first_name} {user.last_name}, Логин: {user.username}, Email: {user.email}, Роль: {user.role}");
            }

            ReportText = report.ToString();
        }

        private void ExecuteGenerateDriverReport(object parameter)
        {
            var drivers = _driverModel.GetAllDrivers();
            var report = new StringBuilder("Отчёт по водителям:\n\n");

            foreach (var driver in drivers)
            {
                report.AppendLine($"ID: {driver.driver_id}, Имя: {driver.first_name} {driver.last_name}, Лицензия: {driver.license_number}, Телефон: {driver.phone_number}");
            }

            ReportText = report.ToString();
        }

        private void ExecuteGenerateRouteReport(object parameter)
        {
            var routes = _routeModel.GetRoutes();
            var report = new StringBuilder("Отчёт по маршрутам:\n\n");

            foreach (var route in routes)
            {
                report.AppendLine($"ID: {route.route_id}, Откуда: {route.start_location}, Куда: {route.end_location}, Дистанция: {route.distance} км");
            }

            ReportText = report.ToString();
        }

        private void ExecuteGenerateTripReport(object parameter)
        {
            var trips = _tripModel.GetAllTrips();
            var report = new StringBuilder("Отчёт по рейсам:\n\n");

            foreach (var trip in trips)
            {
                report.AppendLine($"ID рейса: {trip.trip_id}, Расписание ID: {trip.schedule_id}, Отправление: {trip.departure_datetime}, Прибытие: {trip.arrival_datetime}, Цена: {trip.price}");
            }

            ReportText = report.ToString();
        }

        private void ExecuteGenerateMonthlyTicketReport(object parameter)
        {
            var tickets = _tripModel.GetMonthlyTickets();
            var report = new StringBuilder($"Отчёт по купленным билетам за {DateTime.Now:MMMM yyyy}:\n\n");

            foreach (var ticket in tickets)
            {
                report.AppendLine($"ID билета: {ticket.ticket_id}, Пассажир: {ticket.passenger_last_name} {ticket.passenger_first_name}, Дата покупки: {ticket.purchase_date}, Цена: {ticket.price}, Рейс: {ticket.trip_id}");
            }

            ReportText = report.ToString();
        }

        private void ExecutePrintReport(object parameter)
        {
            var printDialog = new System.Windows.Controls.PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                var document = new FlowDocument(new Paragraph(new Run(ReportText)))
                {
                    FontSize = 12,
                    PagePadding = new Thickness(50)
                };

                document.ColumnWidth = printDialog.PrintableAreaWidth;
                printDialog.PrintDocument(((IDocumentPaginatorSource)document).DocumentPaginator, "Печать отчёта");
            }
        }

    }
}
