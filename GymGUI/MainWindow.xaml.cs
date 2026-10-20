using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gym_interface;

namespace GymGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Gym gym;

        public MainWindow()
        {
            InitializeComponent();
            gym = new Gym();

            for (int hour = 8; hour <= 20; hour++)
            {
                CbReservationTime.Items.Add(hour);
            }
            CbReservationTime.SelectedIndex = 0;

            RefreshGrid();
        }

        private void CbReservationType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PanelTeamMembers == null) return;

            if (CbReservationType.SelectedIndex == 1)
            {
                PanelTeamMembers.Visibility = Visibility.Visible;
            }
            else
            {
                PanelTeamMembers.Visibility = Visibility.Collapsed;
            }
        }
        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(TxtContactName.Text.Trim()))
            {
                MessageBox.Show("Kérlek, add meg a nevet / kapcsolattartót!", "Validációs hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (CbReservationTime.SelectedItem == null)
            {
                MessageBox.Show("Kérlek, válassz ki egy idősávot!", "Validációs hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void BtnBook_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                string name = TxtContactName.Text.Trim();
                int time = (int)CbReservationTime.SelectedItem;
                bool success = false;

                if (CbReservationType.SelectedIndex == 1)
                {
                    var membersInput = TxtTeamMembers.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> members = new List<string>();
                    foreach (var m in membersInput)
                    {
                        members.Add(m.Trim());
                    }

                    TeamReservation teamRes = new TeamReservation(name, members, time);
                    success = gym.addReservation(teamRes);
                }
                else
                {
                    SingleReservation singleRes = new SingleReservation(name, time);
                    success = gym.addReservation(singleRes);
                }

                if (success)
                {
                    MessageBox.Show("A foglalás sikeresen rögzítve!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                    TxtContactName.Clear();
                    TxtTeamMembers.Clear();
                    RefreshGrid();
                }
                else
                {
                    MessageBox.Show("Erre az idősávra már van csoportos foglalás, amely blokkolja a többit!", "Foglalás megtagadva", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RefreshGrid()
        {
            var slots = new List<TimeSlotViewModel>();

            for (int hour = 8; hour <= 20; hour++)
            {
                string status = "Szabad";
                Brush bgColor = Brushes.LightGray;
                bool hasTeam = false;
                int singleCount = 0;

                foreach (var res in gym.Reservations)
                {
                    if (res.getReservationTime == hour)
                    {
                        if (res is TeamReservation)
                        {
                            hasTeam = true;
                            status = $"[CSOPORTOS]\nKapcsolattartó: {res.getContactName}";
                            bgColor = Brushes.LightCoral;
                        }
                        else
                        {
                            singleCount++;
                        }
                    }
                }

                if (!hasTeam && singleCount > 0)
                {
                    status = $"[EGYÉNI]\nFoglalások: {singleCount} fő";
                    bgColor = Brushes.LightGreen; 
                }

                slots.Add(new TimeSlotViewModel
                {
                    TimeText = $"{hour}:00",
                    StatusText = status,
                    BackgroundColor = bgColor
                });
            }

            IcTimeSlots.ItemsSource = slots;
        }
    }

    public class TimeSlotViewModel
    {
        public string TimeText { get; set; } = string.Empty;
        public string StatusText { get; set; } = string.Empty;
        public Brush BackgroundColor { get; set; } = Brushes.White;
    }
}