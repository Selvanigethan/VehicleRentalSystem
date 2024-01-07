using OopCw.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OopCw.Comparer
{
    public class VehicleMakeComparer : IComparer<Vehicle>
    {

        int IComparer<Vehicle>.Compare(Vehicle? x, Vehicle? y)
        {
            return string.Compare(x.GetMake(), y.GetMake(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
