using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Gym_interface
{
    public class SingleReservation : IGymReservation
    {
        string contactName;
        int reservationTime;
        bool hasSeasonTicket;

        public SingleReservation(string contactName, int reservationTime)      
        {
            this.contactName = contactName;
            this.reservationTime = reservationTime;
            hasSeasonTicket = false;
        }
        public string getContactName => contactName;

        public int getReservationTime => reservationTime;

        public void buySeasonTicket()
        {
            hasSeasonTicket = true;
        }

        public int calculatePriceToPay()
        {
            return hasSeasonTicket ? 2250 : 4500;
        }
    }
}
