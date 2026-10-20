using System;
using System.Collections.Generic;
using System.Text;

namespace Gym_interface
{
    internal interface IGymReservation
    {
        string getContactName();
        int getReservationTime();
        int calculatePriceToPay();
    }
}
