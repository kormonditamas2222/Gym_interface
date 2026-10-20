using System;
using System.Collections.Generic;
using System.Text;

namespace Gym_interface
{
    public class TeamReservation : IGymReservation
    {
        string contactName;
        List<string> teamMembers;
        int reservationTime;

        public TeamReservation (string contactName, List<string> teamMembers, int reservationTime)
        {
            this.contactName = contactName;
            this.teamMembers = teamMembers;
            this.reservationTime = reservationTime;
        }
        public string getContactName => contactName;

        public int getReservationTime => reservationTime;

        public int calculatePriceToPay()
        {
            const int price = 9000;
            return price / (teamMembers.Count + 1);
        }
        
    }
}
