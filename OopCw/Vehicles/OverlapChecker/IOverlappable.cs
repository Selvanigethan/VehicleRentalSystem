using OopCw.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OopCw.Vehicles.OverlapChecker
{
    public interface IOverlappable
    {
        public bool Overlaps(Schedule other);
    }
}
