using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OopCw.Reservation
{
   public class Schedule
    {
        private DateOnly PickUp;
        private DateOnly DropOff;
        private double totalPrice;

        public Schedule(DateOnly pickUp, DateOnly dropOff)
        {
            PickUp = pickUp;
            DropOff = dropOff;
        }

        public DateOnly GetPickUp()
        {
            return PickUp;
        } 
        public DateOnly GetDropOff() {
            return DropOff;
        }

        public void SetPickUp(DateOnly pickUp) {
            this.PickUp = pickUp;
        }

        public void setDropOff (DateOnly dropOff)
        {
            this.DropOff = dropOff;
        }

        public double GetTotalPrice() { return totalPrice; }

        public void SetTotalPrice (double totalPrice) {  this.totalPrice = totalPrice; }


        // Override Equals to check for equality based on all fields
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Schedule otherSchedule = (Schedule)obj;

            return PickUp == otherSchedule.GetPickUp() &&
                   DropOff == otherSchedule.GetDropOff();
        }


    }
}
