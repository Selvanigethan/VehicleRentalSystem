using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OopCw.DriverDirectory
{
    class Driver
    {
        private string name;
        private string surName;
        private DateOnly dateOfBirth;
        private int licenseNumber;

        public Driver(string name, string surName, DateOnly dateOfBirth, int licenseNumber)
        {
            this.name = name;
            this.surName = surName;
            this.dateOfBirth = dateOfBirth;
            this.licenseNumber = licenseNumber;
        }

        public string GetName () { return name; }  
        public string GetSurName () { return surName; }
        public DateOnly GetDateOfBirth () { return dateOfBirth;}
        public int GetLicenseNumber () {  return licenseNumber; }

        public void SetName (string name) { this.name = name;}
        public void SetSurName (string surName) { this.surName = surName; }
        public void SetDateOfBirth (DateOnly dateOfBirth) {  this.dateOfBirth = dateOfBirth;}
        public void SetLicenseNumber (int licenseNumber) { this.licenseNumber = licenseNumber; }
    }
}
