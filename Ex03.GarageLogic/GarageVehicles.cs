using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    internal class GarageVehicles
    {
        private Dictionary<string, Vehicle> m_VehiclesDictionary = new Dictionary<string, Vehicle>();
        private static readonly List<eTypeOfVehicle> sr_SupportedVehicles =
            new List<eTypeOfVehicle> { eTypeOfVehicle.Car, eTypeOfVehicle.Motorcycle, eTypeOfVehicle.Truck };
        internal List<eTypeOfVehicle> SupportedVehiclesInformation
        {
            get { return sr_SupportedVehicles; }
        }

        internal Dictionary<string, Vehicle> VehiclesDictionary
        {
            get { return m_VehiclesDictionary; }
            set { m_VehiclesDictionary = value; }
        }
        internal Vehicle ConstructVehicle(eTypeOfVehicle i_VehicleType, string i_OwnerName, string i_OwnerPhoneNumber, string i_ModelName, string i_LicenseNumber, out List<eEnergySystemType> o_SupportedEnergyTypesList)
        {
            Vehicle vehicleToReturn = null;
            o_SupportedEnergyTypesList = new List<eEnergySystemType>();

            bool theVehicleIsInTheGarage = m_VehiclesDictionary.TryGetValue(i_LicenseNumber, out Vehicle theExistedVehicle);
            if (theVehicleIsInTheGarage)
            {
                theExistedVehicle.StateOfVehicle = eStateOfVehicle.InRepair;
                throw new ArgumentException("A vehicle with this license number is already in the garage");
            }
     
            switch (i_VehicleType)
            {
                case eTypeOfVehicle.Car:
                    vehicleToReturn = new Car(i_OwnerName, i_OwnerPhoneNumber, i_ModelName, i_LicenseNumber);
                    o_SupportedEnergyTypesList.Add(eEnergySystemType.ElectricEnergySystem);
                    o_SupportedEnergyTypesList.Add(eEnergySystemType.FueldEnergySystem);
                    break;

                case eTypeOfVehicle.Motorcycle:
                    vehicleToReturn = new Motorcycle(i_OwnerName, i_OwnerPhoneNumber, i_ModelName, i_LicenseNumber);
                    o_SupportedEnergyTypesList.Add(eEnergySystemType.ElectricEnergySystem);
                    o_SupportedEnergyTypesList.Add(eEnergySystemType.FueldEnergySystem);
                    break;
                case eTypeOfVehicle.Truck:
                    vehicleToReturn = new Truck(i_OwnerName, i_OwnerPhoneNumber, i_ModelName, i_LicenseNumber);
                    o_SupportedEnergyTypesList.Add(eEnergySystemType.FueldEnergySystem);
                    break;
            }
            m_VehiclesDictionary.Add(i_LicenseNumber, vehicleToReturn);

            return vehicleToReturn;
        }

        internal void ConstructEnergySystemInVehicle(Vehicle i_Vehicle, eTypeOfVehicle i_VehicleType, eEnergySystemType i_EnergySystemType, float i_CurrentAmountOfEnergy)
        {
            i_Vehicle.ConstructEnergySystem(i_EnergySystemType);
            ISupportedByGarage vehicleToConstruct = i_Vehicle as ISupportedByGarage;
            vehicleToConstruct.SetMaxAmountOfEnergyAndOptionalFuelType();
            i_Vehicle.SetCurrentAmountOfEnergy(i_CurrentAmountOfEnergy);
        }

        internal void Remove(string i_VehicleLicenseNumberToRemove)
        {
            m_VehiclesDictionary.Remove(i_VehicleLicenseNumberToRemove);
        }
    }
}
