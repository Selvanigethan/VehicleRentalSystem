using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OopCw.Vehicles
{
    class Motorbike : Vehicle
    {
        public Motorbike(string number, string make, string model, double dailyRentalPrice) : base(number, make, model, dailyRentalPrice)
        {
        }
    }
}
