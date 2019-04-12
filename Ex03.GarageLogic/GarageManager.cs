using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    
    public class GarageManager
    {
        private readonly GarageVehicles r_GarageVehicles = new GarageVehicles();

        public List<eTypeOfVehicle> GetSupportedVehiclesInformation()
        {
            return r_GarageVehicles.SupportedVehiclesInformation;
        }

        public Vehicle ConstructVehicle(eTypeOfVehicle i_VehicleType, string i_OwnerName, string i_OwnerPhoneNumber,
            string i_ModelName,string i_LicenseNumber, out List<eEnergySystemType> o_SupportedEnergyTypesList)
        {
            Vehicle vehicleToReturn = r_GarageVehicles.ConstructVehicle(i_VehicleType, i_OwnerName, i_OwnerPhoneNumber, i_ModelName, i_LicenseNumber, out o_SupportedEnergyTypesList);

            return vehicleToReturn;
        }

        public void ConstructEnergySystemInVehicle(Vehicle i_Vehicle, eTypeOfVehicle i_VehicleType, eEnergySystemType i_EnergySystemType, 
            float i_CurrentAmountOfEnergy)
        {
            r_GarageVehicles.ConstructEnergySystemInVehicle(i_Vehicle, i_VehicleType, i_EnergySystemType, i_CurrentAmountOfEnergy);
        }
        
        public string[] GetVehicleInformationRequests(Vehicle i_Vehicle, eTypeOfVehicle i_VehicleType)
        {
            Vehicle hasInformationRequests = i_Vehicle as Vehicle;

            return hasInformationRequests.InformationRequests;
        }

        public void SetVehicleInformationByRequestIndex(Vehicle i_Vehicle, eTypeOfVehicle i_VehicleType, string i_InformationToSubmmit, int i_RequestIndex)
        {
            ISupportedByGarage vehicleWitheInformationRequests = i_Vehicle as ISupportedByGarage;
            vehicleWitheInformationRequests.SetInformationByRequestIndex(i_InformationToSubmmit, i_RequestIndex);
        }

        public void RemoveVehicle(string i_VehicleLicenseNumberToRemove)
        {
            r_GarageVehicles.Remove(i_VehicleLicenseNumberToRemove);
        }

        public List<string> GetVehiclesLicenseNumberList()
        {
            List<string> vehiclesLicenseNumbersList = new List<string>();

            foreach (KeyValuePair<string, Vehicle> vehiclesDictionaryItem in r_GarageVehicles.VehiclesDictionary)
            {
                   vehiclesLicenseNumbersList.Add(vehiclesDictionaryItem.Key);
            }

            return vehiclesLicenseNumbersList;
        }

        public List<string> GetVehiclesListFilteredByState(eStateOfVehicle i_RequestedVehicleState)
        {
            List<string> vehiclesLicenseNumbersList = new List<string>();

            foreach (KeyValuePair<string,Vehicle> vehiclesDictionaryItem in r_GarageVehicles.VehiclesDictionary)
            {
                if (vehiclesDictionaryItem.Value.StateOfVehicle == i_RequestedVehicleState)
                {
                    vehiclesLicenseNumbersList.Add(vehiclesDictionaryItem.Key);
                }
            }

            return vehiclesLicenseNumbersList;
        }

        public bool TryChangeVehicleState(string i_VehicleLicenseNumber, eStateOfVehicle i_VehiclesNewState)
        {
            bool theVehicleExistsInGarage = r_GarageVehicles.VehiclesDictionary.TryGetValue(i_VehicleLicenseNumber, out Vehicle vehicleToUpdate);

            if (theVehicleExistsInGarage)
            {
                vehicleToUpdate.StateOfVehicle = i_VehiclesNewState;
            }

            return theVehicleExistsInGarage;
        }

        public bool TryInflateVehiclesWheelsToMaximum(string i_VehicleLicenseNumber)
        {
            bool theVehicleExistsInGarage = r_GarageVehicles.VehiclesDictionary.TryGetValue(i_VehicleLicenseNumber, out Vehicle vehicleToUpdate);
            List<Wheel> vehicleWheelsList;

            if (theVehicleExistsInGarage)
            {
                vehicleWheelsList = vehicleToUpdate.WheelsList;
                foreach (Wheel wheel in vehicleWheelsList)
                {
                    wheel.InflateAirPressureToMaximum();
                }
            }

            return theVehicleExistsInGarage;
        }

        public bool TryFillEnergyInVehicle(string i_VehicleLicenseNumber, float i_AmountToFill, eFuelType? i_FuelType)
        {
            bool theVehicleExistsInGarage = r_GarageVehicles.VehiclesDictionary.TryGetValue(i_VehicleLicenseNumber, out Vehicle vehicleToFill);
            bool vehicleMatchesEnergyFiller;
            EnergyFillInformationInput fillInformation = new EnergyFillInformationInput(i_AmountToFill, i_FuelType);

            if (theVehicleExistsInGarage)
            {
                vehicleMatchesEnergyFiller = ((i_FuelType == null) && (vehicleToFill.EnergySystem is ElectricEnergySystem))
                   || ((i_FuelType != null) && (vehicleToFill.EnergySystem is FuelEnergySystem));
                if (!vehicleMatchesEnergyFiller)
                {
                    throw new ArgumentException("Illegal filling operation! Your energy system does not match!");
                }

                vehicleToFill.EnergySystem.EnergyFill(fillInformation);
            }

            return theVehicleExistsInGarage;
        }

        public bool TryGetVehicleInformation(string i_VehicleLicenseNumber, out string o_InformationString)
        {
            bool theVehicleExistsInGarage = r_GarageVehicles.VehiclesDictionary.TryGetValue(i_VehicleLicenseNumber, out Vehicle vehicle);

            if (theVehicleExistsInGarage)
            {
                o_InformationString = vehicle.ToString();
            }
            else
            {
                o_InformationString = null;
            }

            return theVehicleExistsInGarage;
        }

    }
}
