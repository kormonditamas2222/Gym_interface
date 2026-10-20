using System;
using System.Collections.Generic;
using System.Text;

namespace Gym_interface
{
    public class Gym
    {
        List<IGymReservation> reservations;
        public Gym()
        {
            reservations = [];
        }

        public IReadOnlyList<IGymReservation> Reservations => reservations.AsReadOnly();

        public bool addReservation(IGymReservation reservation)
        {
            if (reservation is TeamReservation tr)
            {
                foreach (IGymReservation r in reservations)
                {
                    if (r is TeamReservation current && current.getReservationTime == tr.getReservationTime)
                    {
                        return false;
                    }
                }
                reservations.Add(tr);
                return true;
            }
            reservations.Add(reservation);
            return true;
        }
    }
}
