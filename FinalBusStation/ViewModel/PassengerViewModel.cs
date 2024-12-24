using DAL.Entity;
using FinalBusStation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using FinalBusStation.View;
using System.Windows.Controls;
using System.Windows.Documents;

namespace FinalBusStation.ViewModel
{
    public class PassengerViewModel : INotifyPropertyChanged
    {
        private readonly TripModel _tripModel;
        private readonly AuthModel _authModel;
        private readonly TicketModel _ticketModel;
        private readonly ScheduleModel _scheduleModel;
        public ObservableCollection<trips> AvailableTrips { get; set; }
        public ObservableCollection<tickets> UserTickets { get; set; }
        public ObservableCollection<string> StartLocations { get; set; }
        public ObservableCollection<string> EndLocations { get; set; }
        public ObservableCollection<schedules> Schedules { get; set; }

        public ObservableCollection<TicketViewModel> UserTickets1 { get; set; }

        public ICommand PurchaseTicketCommand { get; }
        public ICommand RefreshTripsCommand { get; }

        public string FilterStartLocation { get; set; }
        public string FilterEndLocation { get; set; }
        public DateTime? FilterDepartureDate { get; set; }

        public ICommand ApplyFilterCommand { get; }
        public ICommand UpdateProfileCommand { get; }
        
        public ICommand PrintTicketCommand { get; }

        private users _currentUser;
        public users CurrentUser
        {
            get => _currentUser;
            set { _currentUser = value; OnPropertyChanged(); }
        }

        private bool _isBuyingForOther;
        public bool IsBuyingForOther
        {
            get => _isBuyingForOther;
            set
            {
                _isBuyingForOther = value;
                OnPropertyChanged();
            }
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }


        public string OtherPassengerFirstName { get; set; }
        public string OtherPassengerLastName { get; set; }
        public string OtherPassengerMiddleName { get; set; }
        public string OtherPassengerPassportNumber { get; set; }

        public PassengerViewModel()
        {
            _authModel = new AuthModel();
            _tripModel = new TripModel();
            _scheduleModel = new ScheduleModel();

            // Здесь можно инициализировать CurrentUser с пустыми значениями
            CurrentUser = _authModel.GetUserById(UserSession.UserId);

            AvailableTrips = new ObservableCollection<trips>(_tripModel.GetAvailableTrips());
            UserTickets = new ObservableCollection<tickets>(_tripModel.GetUserTickets(UserSession.UserId));
            StartLocations = new ObservableCollection<string>(_tripModel.GetUniqueStartLocations());
            EndLocations = new ObservableCollection<string>(_tripModel.GetUniqueEndLocations());
            PrintTicketCommand = new RelayCommand(ExecutePrintTicket, CanExecutePrintTicket);
            AddCardCommand = new RelayCommand(ExecuteAddCard);
            UpdateProfileCommand = new RelayCommand(ExecuteUpdateProfile);
            _ticketModel = new TicketModel();
            LoadUserTickets();

            PurchaseTicketCommand = new RelayCommand(ExecutePurchaseTicket);
            RefreshTripsCommand = new RelayCommand(ExecuteRefreshTrips);
            ApplyFilterCommand = new RelayCommand(ExecuteApplyFilter);
            LoadSchedules();
        }

        private void LoadSchedules()
        {
            var schedulesList = _scheduleModel.GetSchedules();
            Schedules = new ObservableCollection<schedules>(schedulesList);
            OnPropertyChanged(nameof(Schedules));
        }

        private void ExecuteUpdateProfile(object parameter)
        {
            var newPasswordBox = parameter as PasswordBox;
            string newPassword = newPasswordBox?.Password;

            try
            {
                _authModel.UpdateUser(CurrentUser, newPassword);
                MessageBox.Show("Профиль успешно обновлён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении профиля: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanExecutePrintTicket(object parameter)
        {
            return parameter is TicketViewModel;
        }

        private void ExecutePrintTicket(object parameter)
        {
            if (parameter is TicketViewModel ticket)
            {
                PrintTicket(ticket);
            }
        }

        private void PrintTicket(TicketViewModel ticket)
        {
            string ticketContent = GenerateTicketContent(ticket);

            // Создаём FlowDocument для печати
            FlowDocument doc = new FlowDocument(new Paragraph(new Run(ticketContent)));
            doc.Name = "TicketDocument";

            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                doc.PageHeight = printDialog.PrintableAreaHeight;
                doc.PageWidth = printDialog.PrintableAreaWidth;
                doc.PagePadding = new Thickness(50);
                doc.ColumnWidth = doc.PageWidth;

                printDialog.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "Печать билета");
            }
        }

        private string GenerateTicketContent(TicketViewModel ticket)
        {
            return $"Билет\n\n" +
                   $"Пассажир: {ticket.PassengerName}\n" +
                   $"Автобус: {ticket.BusModel} (Номер: {ticket.BusLicensePlate})\n" +
                   $"Водитель: {ticket.DriverName}\n" +
                   $"Откуда: {ticket.StartLocation}\n" +
                   $"Куда: {ticket.EndLocation}\n" +
                   $"Дата отправления: {ticket.Departure:dd.MM.yyyy HH:mm}\n" +
                   $"Дата прибытия: {ticket.Arrival:dd.MM.yyyy HH:mm}\n" +
                   $"Паспортные данные: {ticket.PassengerPassportNumber}\n" +
                   $"Дата покупки: {ticket.PurchaseDate:dd.MM.yyyy}\n" +
                   $"Цена: {ticket.Price} руб.\n" +
                   $"Дистанция: {ticket.Distance} км\n";
        }

        private void ExecuteApplyFilter(object parameter)
        {
            var filteredTrips = _tripModel.GetAvailableTrips();

            Console.WriteLine("Total Trips before filtering: " + filteredTrips.Count);

            if (!string.IsNullOrWhiteSpace(FilterStartLocation))
            {
                filteredTrips = filteredTrips
                    .Where(t => t.schedules.routes.start_location
                        .ToLower()
                        .Contains(FilterStartLocation.ToLower()))
                    .ToList();
                Console.WriteLine("Trips after start location filter: " + filteredTrips.Count);
            }

            if (!string.IsNullOrWhiteSpace(FilterEndLocation))
            {
                filteredTrips = filteredTrips
                    .Where(t => t.schedules.routes.end_location
                        .ToLower()
                        .Contains(FilterEndLocation.ToLower()))
                    .ToList();
                Console.WriteLine("Trips after end location filter: " + filteredTrips.Count);
            }

            if (FilterDepartureDate.HasValue)
            {
                string filterDateString = FilterDepartureDate.Value.Date.ToString("yyyy-MM-dd");

                filteredTrips = filteredTrips
                    .Where(t => t.departure_datetime.Date.ToString("yyyy-MM-dd") == filterDateString)
                    .ToList();
            }

            AvailableTrips.Clear();
            foreach (var trip in filteredTrips)
            {
                AvailableTrips.Add(trip);
            }

            Console.WriteLine("Final filtered trips count: " + AvailableTrips.Count);
        }

        public ICommand AddCardCommand { get; }

        private void LoadUserTickets()
        {
            var tickets = _ticketModel.GetUserTickets(UserSession.UserId);
            UserTickets1 = new ObservableCollection<TicketViewModel>(tickets);
            OnPropertyChanged(nameof(UserTickets));
        }
        private void ExecuteAddCard(object parameter)
        {
            var addCardWindow = new AddCardView
            {
                DataContext = new AddCardViewModel()
            };
            addCardWindow.ShowDialog();
        }

        private void ExecutePurchaseTicket(object parameter)
        {
            var selectedTrip = parameter as trips;
            if (!_tripModel.UserHasCard(CurrentUser.user_id))
            {
                MessageBox.Show("У вас нет привязанной карты. Пожалуйста, добавьте карту для покупки билета.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (selectedTrip != null)
            {
                string firstName = IsBuyingForOther ? OtherPassengerFirstName : CurrentUser.first_name;
                string lastName = IsBuyingForOther ? OtherPassengerLastName : CurrentUser.last_name;
                string middleName = IsBuyingForOther ? OtherPassengerMiddleName : CurrentUser.middle_name;
                string passportNumber = IsBuyingForOther ? OtherPassengerPassportNumber : CurrentUser.passport_number;

                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(passportNumber))
                {
                    MessageBox.Show("Заполните все данные пассажира.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                bool success = _tripModel.PurchaseTicket(
                    selectedTrip.trip_id,
                    CurrentUser.user_id,
                    firstName,
                    lastName,
                    middleName,
                    passportNumber,
                    selectedTrip.price
                );

                if (success)
                {
                    MessageBox.Show("Билет успешно приобретён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Обновляем список доступных рейсов и билетов
                    RefreshAvailableTrips();
                    RefreshUserTickets();
                }
                else
                {
                    MessageBox.Show("Ошибка при покупке билета.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Метод для обновления списка доступных рейсов
        private void RefreshAvailableTrips()
        {
            AvailableTrips.Clear();
            var trips = _tripModel.GetAvailableTrips();
            foreach (var trip in trips)
            {
                AvailableTrips.Add(trip);
            }
        }

        private void ExecuteRefreshTrips(object parameter)
        {
            AvailableTrips.Clear();
            foreach (var trip in _tripModel.GetAvailableTrips())
            {
                AvailableTrips.Add(trip);
            }
        }

        private void RefreshUserTickets()
        {
            UserTickets.Clear();
            var tickets = _tripModel.GetUserTickets(CurrentUser.user_id);
            foreach (var ticket in tickets)
            {
                UserTickets.Add(ticket);
            }
            OnPropertyChanged(nameof(UserTickets));
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
