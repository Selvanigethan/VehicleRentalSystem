using OopCw.Reservation;
using OopCw.Vehicles.OverlapChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OopCw.Vehicles
{
    class Vehicle : IOverlappable
    {
        private string number;
        private string make;
        private string model;
        private double dailyRentalPrice;

        private List<Schedule> reservations = new List<Schedule>();


        public Vehicle(string number, string make, string model, double dailyRentalPrice)
        {
            this.number = number;
            this.make = make;
            this.model = model;
            this.dailyRentalPrice = dailyRentalPrice;
        } 

        public string GetNumber() { return number; }
        public string GetMake() { return make; }
        public double GetDailyRentalPrice() { return dailyRentalPrice; }
        public string GetModel() { return model; }
        public void SetNumber(string number) { this.number = number; }
        public void SetMake(string make) { this.make = make; }
        public void SetModel(string model) { this.model = model; }
        public void SetDailyRentalPrice(double dailyRentalPrice) { this.dailyRentalPrice = dailyRentalPrice; }
        public void SetReservation(Schedule reservation) { reservations.Add(reservation); }

        public void SetReservation(int index, Schedule reservation) { reservations[index] = reservation; }

        public List<Schedule> GetReservations() { return reservations; }
         //x---y
        public bool Overlaps(Schedule other)
        {
            foreach (Schedule schedule in reservations)
            {
                if (schedule==null) {  continue; }

                if (!(other.GetPickUp() < schedule.GetPickUp() && other.GetDropOff() < schedule.GetPickUp() || other.GetPickUp() > schedule.GetDropOff() && other.GetDropOff() > schedule.GetDropOff()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
