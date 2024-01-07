using OopCw.Comparer;
using OopCw.DriverDirectory;
using OopCw.Reservation;
using OopCw.UserRoles;
using OopCw.Vehicles;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OopCw
{
    class WestminsterRentalVehicle : IRentalCustomer, IRentalManager
    {
        private int availableParkingSlots = 50;
        private List<Vehicle> vehicles = new List<Vehicle>(50);

        public void StartApplication()
        {
            bool firstTime = true;
            bool beenInCustomer = false;
            bool beenInManager = false;

            while (true)
            {
                string userInput = null;
                if (firstTime)
                {
                    Console.WriteLine("Hello, Please Select your Role : ");
                    Console.WriteLine("1 - Manager\n" + "2 - Customer\n");
                    userInput = Console.ReadLine();

                    if (userInput != "1" && userInput != "2")
                    {
                        Console.WriteLine("Please Enter a valid input!");
                        Console.WriteLine();
                        continue;
                    }
                }

                firstTime = false;

                if (userInput == "1" || beenInCustomer)
                {
                    beenInManager = true;
                    while (true)
                    {
                        Console.WriteLine();
                        Console.WriteLine("You are a manager. Please choose what you want to do: ");
                        Console.WriteLine("1 - AddVehicle\n" + "2 - DeleteVehicle\n" + "3 - ListVehicles\n" + "4 - ListOrderedVehicles\n" + "5 - GenerateReport\n" + "6 - Go to Customer menu\n");
                        string managerInput = Console.ReadLine();

                        if (managerInput == "1")
                        {
                            CreateVehicleAndAdd();
                        }
                        else if (managerInput == "2")
                        {
                            Console.WriteLine("Enter the vehicle number you want to delete : ");
                            string deleteNumber = Console.ReadLine();
                            // Attempt to parse the string to an int
                            DeleteVehicle(deleteNumber);
                        }
                        else if (managerInput == "3")
                        {
                            ListVehicles();
                        }
                        else if (managerInput == "4")
                        {
                            ListOrderedVehicles();
                        } else if (managerInput == "5")
                        {
                            Console.WriteLine("Enter the file name for generating the report : ");
                            string fileName = Console.ReadLine();
                            if (ContinueIfNull(fileName)) { continue; }

                            GenerateReport(fileName);
                        }
                        else if (managerInput == "6")
                        {
                            break;
                        }
                    }
                }


                if (userInput == "2" || beenInManager)
                {
                    while (true)
                    { 
                        beenInCustomer = true;
                        Console.WriteLine();
                        Console.WriteLine("You are a customer. Please choose what you want to do: ");
                        Console.WriteLine("1 - ListAvailableVehicles\n" + "2 - AddReservation\n" + "3 - ChangeReservation\n" + "4 - DeleteReservation\n" + "5 - Go to Manager menu\n");
                        string customerInput = Console.ReadLine();

                        if (customerInput == "1")
                        {
                            Console.Write("Enter the vehicle type (1 - Car, 2 - Bike, 3 - Van, 4 - Electric Car): ");
                            string vehicleTypeInput = Console.ReadLine();

                            // Get the Type object for the specified type
                            if (vehicleTypeInput == "1")
                            {
                                vehicleTypeInput = "Car";
                            }
                            else if (vehicleTypeInput == "2")
                            {
                                vehicleTypeInput = "Bike";
                            }
                            else if (vehicleTypeInput == "3")
                            {
                                vehicleTypeInput = "Van";
                            }
                            else if (vehicleTypeInput == "4")
                            {
                                vehicleTypeInput = "ElectricCar";
                            }
                            else
                            {
                                Console.WriteLine("Wrong input");
                                continue;
                            }

                            Type specifiedType = Type.GetType(vehicleTypeInput);
                            if (specifiedType == null)
                            {
                                Console.WriteLine("Type is null value...");
                                continue;
                            }
                            //The below line has to be modified to pass the correct schedules from user input
                            ListAvailableVehicles(new Schedule(new DateOnly(), new DateOnly()), specifiedType);
                        }

                        if (customerInput == "5")
                        {
                            break;
                        }
                        else if (customerInput == "2")
                        {
                            Console.WriteLine("Please Enter the vehicle number you want to book: ");
                            string vehicleNumber = Console.ReadLine();
                            if (ContinueIfNull(vehicleNumber)) { continue; }

                            if (!FindVehicle(vehicleNumber))
                            {
                                Console.WriteLine("The Vehicle number you entered is not available in the system!");
                                continue;
                            }

                            Schedule schedule = CreateSchedule();
                            if (schedule == null)
                            {
                                continue;
                            }

                            if (AddReservation(vehicleNumber, schedule))
                            {
                                Console.WriteLine("Successfully added the reservation!");
                            }
                        }
                        else if (customerInput == "3")  // change reservation
                        {
                            Console.WriteLine("Please Enter the vehicle number you want to change reservation: ");
                            string vehicleNumber = Console.ReadLine();
                            if (ContinueIfNull(vehicleNumber)) { continue; }

                            if (!FindVehicle(vehicleNumber))
                            {
                                Console.WriteLine("The Vehicle number you entered is not available in the system!");
                                continue;
                            }

                            Console.WriteLine("Now you are going to enter the details of the schedule which you want to change...");

                            Schedule oldSchedule = CreateSchedule();

                            if (oldSchedule == null)
                            {
                                continue;
                            }

                            Console.WriteLine("Now you are going to enter the details of the new schedule...");

                            Schedule newSchedule = CreateSchedule();

                            if (newSchedule == null)
                            {
                                continue;
                            }
                            ChangeReservation(vehicleNumber, oldSchedule, newSchedule);
                        }
                        else if (customerInput == "4")
                        {
                            Console.WriteLine("Please Enter the vehicle number you want to Delete reservation: ");
                            string vehicleNumber = Console.ReadLine();
                            if (ContinueIfNull(vehicleNumber)) { continue; }

                            if (!FindVehicle(vehicleNumber))
                            {
                                Console.WriteLine("The Vehicle number you entered is not available in the system!");
                                continue;
                            }

                            Console.WriteLine("Now you are going to enter the details of the schedule which you want to Delete...");

                            Schedule schedule = CreateSchedule();

                            if (schedule == null)
                            {
                                continue;
                            }
                            DeleteReservation(vehicleNumber, schedule);
                        }
                    }

                }

            }

        }

        public void ListAvailableVehicles(Schedule wantedSchedule, Type type)
        {
            var filteredVehicles = vehicles.Where(v => type.IsInstanceOfType(v));

            foreach (Vehicle filteredVehicle in filteredVehicles)
            {
                /*foreach (Schedule reservation in vehicle.GetReservations())
                {
                    //you have to use the interface overlap

                }*/
                Console.WriteLine(filteredVehicle.GetNumber());
            }
        }

        public bool AddReservation(string number, Schedule wantedSchedule)
        { 
            foreach (Vehicle vehicle in vehicles)
            {
                if (vehicle.GetNumber() == number)
                {
                    if (!vehicle.Overlaps(wantedSchedule))
                    {

                        DateTime dateTime1 = wantedSchedule.GetPickUp().ToDateTime(TimeOnly.MinValue);
                        DateTime dateTime2 = wantedSchedule.GetDropOff().ToDateTime(TimeOnly.MinValue);
                        int daysDifference = (dateTime2 - dateTime1).Days;

                        wantedSchedule.SetTotalPrice(daysDifference * vehicle.GetDailyRentalPrice());

                        vehicle.SetReservation(wantedSchedule);
                        //vehicle.SetReservation(index, wantedSchedule);
                        Console.WriteLine("Successfully added the reservation!");
                        return true;
                    }
                    Console.WriteLine("Schedule overlaps!");
                    return false;
                }
            }
            Console.WriteLine("Vehicle not found in the registered list!");
            return false;
        }

        public bool ChangeReservation(string number, Schedule oldSchedule, Schedule newSchedule)
        {
            foreach (Vehicle vehicle in vehicles)
            {
                if (vehicle.GetNumber() == number)
                {
                    for (int i = 0; i < vehicle.GetReservations().Count; i++)
                    {
                        if (oldSchedule.Equals(vehicle.GetReservations()[i]))
                        {
                            if (!vehicle.Overlaps(newSchedule))
                            {
                                vehicle.SetReservation(i, newSchedule);
                                Console.WriteLine("Successfully changed the Schedule!");
                                return true;
                            }
                            return false;
                        }
                    }
                    Console.WriteLine("The schedule you want to replace is not existing in the list of schedules!");
                    return false;
                }
                Console.WriteLine("Vehicle not found in the registered list!");
                return false;
            }
            Console.WriteLine("Vehicle List is empty!");
            return false;
        }

        public bool DeleteReservation(string number, Schedule schedule)
        {
            foreach (Vehicle vehicle in vehicles)
            {
                if (vehicle.GetNumber() == number)
                {
                    for (int i = 0; i < vehicle.GetReservations().Count; i++)
                    {
                        if (schedule.Equals(vehicle.GetReservations()[i]))
                        {
                            vehicle.GetReservations().Remove(vehicle.GetReservations()[i]);
                            Console.WriteLine("Successfully Deleted the reservation!");
                            return true;
                        }
                    }
                    Console.WriteLine("The schedule you want to Delete is not existing in the list of schedules!");
                    return false;
                }
                Console.WriteLine("Vehicle not found in the registered list!");
                return false;
            }
            Console.WriteLine("Vehicle List is empty!");
            return false;
        }

        public bool DeleteVehicle(string number)
        {
            foreach (Vehicle vehicle in vehicles)
            {
                if (vehicle.GetNumber() == number)
                {
                    vehicles.Remove(vehicle);
                    availableParkingSlots += 1;
                    Console.WriteLine("Successfully deleted the vehicle from the system : ");
                    Console.WriteLine(vehicle.GetNumber() + "   " + vehicle.GetType() + "   " + vehicle.GetMake() + "   " + vehicle.GetModel());
                    Console.WriteLine("Available Parking Slots : " + availableParkingSlots);
                    return true;
                }
            }
            Console.WriteLine("The vehicle is not found in the system");
            return false;
        }

        public void GenerateReport(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (Vehicle vehicle in vehicles)
                {
                    writer.WriteLine($"Vehicle Number: {vehicle.GetNumber()}, Type: {vehicle.GetType().Name}");
                    foreach (var reservation in vehicle.GetReservations().OrderBy(r => r.GetPickUp()))
                    {
                        writer.WriteLine($"Pickup Date: {reservation.GetPickUp()}, Drop-off Date: {reservation.GetDropOff()}");
                    }
                }
            }
            Console.WriteLine($"Report generated and saved to {fileName}.");
        }

        public void ListOrderedVehicles()
        {
            var sortedVehicles = new List<Vehicle>(vehicles);
            sortedVehicles.Sort(new VehicleMakeComparer());
            Console.WriteLine("List of vehicle details - ordered alphabetically : ");
            Console.WriteLine();
            foreach (Vehicle vehicle in sortedVehicles)
            {
                Console.WriteLine($"{vehicle.GetNumber()}  {vehicle.GetType().Name}  {vehicle.GetMake()}");
                Console.WriteLine("Reservations:");
                foreach (Schedule schedule in vehicle.GetReservations())
                {
                    Console.WriteLine($"   {schedule.GetPickUp()} - {schedule.GetDropOff()}");
                }
                Console.WriteLine();
            }
        }

        public void ListVehicles()
        {
            Console.WriteLine("List of vehicle details : ");
            Console.WriteLine();
            foreach (Vehicle vehicle in vehicles)
            {
                Console.WriteLine($"{vehicle.GetNumber()}  {vehicle.GetType().Name}  {vehicle.GetMake()}");
                Console.WriteLine("Reservations:");
                foreach (Schedule schedule in vehicle.GetReservations())
                {
                    Console.WriteLine($"   {schedule.GetPickUp()} - {schedule.GetDropOff()}");
                }
                Console.WriteLine();
            }
        }

        public bool AddVehicle(Vehicle v)
        {
            if (vehicles.Contains(v))
            {
                Console.WriteLine("Vehicle already in the list");
                return false;
            }
            else if (!(vehicles.Count < 50))
            {
                Console.WriteLine("Vehicles list capacity has been reached. Cannot add more vehicles");
                return false;
            }
            vehicles.Add(v);
            availableParkingSlots -= 1;
            Console.WriteLine("Successfully added Vehicle!");
            Console.WriteLine("Available Parking slots : " + availableParkingSlots);
            return true;
        }

        private bool CreateVehicleAndAdd()
        {
            Console.WriteLine("Enter the vehicle Type: ");
            Console.WriteLine("1 - Car, 2 - Van, 3 - Motorbike, 4 - Electric car");
            string type = Console.ReadLine();

            Console.WriteLine("Enter the number: ");
            string number = Console.ReadLine();
            Console.WriteLine("Enter the make: ");
            string make = Console.ReadLine();
            Console.WriteLine("Enter the model: ");
            string model = Console.ReadLine();
            Console.WriteLine("Enter the daily rental price: ");
            string dailyRentalPrice = Console.ReadLine();

            double.TryParse(dailyRentalPrice, out double doubleValue);

            Vehicle vehicle = null;

            if (type == "1")
            {
                vehicle = new Car(number, make, model, doubleValue);
            }
            else if (type == "2")
            {
                vehicle = new Van(number, make, model, doubleValue);
            }
            else if (type == "3")
            {
                vehicle = new Motorbike(number, make, model, doubleValue);
            }
            else if (type == "4")
            {
                vehicle = new ElectricCar(number, make, model, doubleValue);
            }

            if (vehicle != null)
            {
                AddVehicle(vehicle);
                return true;
            }
            Console.WriteLine("Something went wrong when creating and adding vehicle");
            return false;
        }

        private bool FindVehicle(string vehicleNumber)
        {
            foreach (Vehicle vehicle in vehicles)
            {
                if (vehicle.GetNumber() == vehicleNumber)
                {
                    return true;
                }
            }
            return false;
        }

        private Schedule CreateSchedule()
        {
            Console.WriteLine("Please enter below details for the pick up date...");
            Console.WriteLine("Year: ");
            string yearForpickUp = Console.ReadLine();
            if (ContinueIfNull(yearForpickUp)) { return null; }
            int.TryParse(yearForpickUp, out int yearForpickUpInt);
            Console.WriteLine("Month: ");
            string monthForpickUp = Console.ReadLine();
            if (ContinueIfNull(monthForpickUp)) { return null; }
            int.TryParse(monthForpickUp, out int monthForpickUpInt);
            Console.WriteLine("Date: ");
            string dateForpickUp = Console.ReadLine();
            if (ContinueIfNull(dateForpickUp)) { return null; }
            int.TryParse(dateForpickUp, out int dateForpickUpInt);

            Console.WriteLine("Please enter below details for the drop off date...");
            Console.WriteLine("Year: ");
            string yearForDropOff = Console.ReadLine();
            if (ContinueIfNull(yearForDropOff)) { return null; }
            int.TryParse(yearForDropOff, out int yearForDropOffInt);
            Console.WriteLine("Month: ");
            string monthForDropOff = Console.ReadLine();
            if (ContinueIfNull(monthForDropOff)) { return null; }
            int.TryParse(monthForDropOff, out int monthForDropOffInt);
            Console.WriteLine("Date: ");
            string dateForDropOff = Console.ReadLine();
            if (ContinueIfNull(dateForDropOff)) { return null; }
            int.TryParse(dateForDropOff, out int dateForDropOffInt);

            var pickUpDate = new DateOnly(yearForpickUpInt, monthForpickUpInt, dateForpickUpInt);
            var dropOffDate = new DateOnly(yearForDropOffInt, monthForDropOffInt, dateForDropOffInt);

            if (!(pickUpDate < dropOffDate))
            {
                Console.WriteLine("Drop off date should be later than pick up date!");
                return null;
            }

            Schedule schedule = new Schedule(pickUpDate, dropOffDate);
            return schedule;
        }

        private bool ContinueIfNull(string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue))
            {
                Console.WriteLine("You have entered a null or empty value!");
                return true;
            }
            return false;
        }
    }
}
