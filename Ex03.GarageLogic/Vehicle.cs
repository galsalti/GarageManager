using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public enum eTypeOfVehicle
    {
        Car,
        Motorcycle,
        Truck
    }
    public enum eStateOfVehicle
    {
        Fixed,
        InRepair,
        Paid
    }

    public enum eEnergySystemType
    {
        ElectricEnergySystem,
        FueldEnergySystem
    }

    public abstract class Vehicle
    {
        private readonly string r_OwnerName;
        private readonly string r_OwnerPhoneNumber;
        private readonly string r_ModelName;
        private readonly string r_LicenseNumber;
        private List<Wheel> m_WheelsList = new List<Wheel>();
        private IEnergySystem m_EnergySystem;
        private float m_RemainingEnergyPercentage;
        private eStateOfVehicle m_VehicleState;

        public Vehicle(string i_OwnerName, string i_OwnerPhoneNumber, string i_ModelName, string i_LicenseNumber)
        {
            m_VehicleState = eStateOfVehicle.InRepair;
            r_OwnerName = i_OwnerName;
            r_OwnerPhoneNumber = i_OwnerPhoneNumber;
            r_ModelName = i_ModelName;
            r_LicenseNumber = i_LicenseNumber;
        }

        internal IEnergySystem EnergySystem
        {
            get { return m_EnergySystem; }
        }

        internal List<Wheel> WheelsList
        {
            get { return m_WheelsList; }
            set { m_WheelsList = value; }
        }

        internal eStateOfVehicle StateOfVehicle
        {
            get { return m_VehicleState; }
            set { m_VehicleState = value; }
        }

        public string LicenseNumber
        {
            get { return r_LicenseNumber; }
        }

        public static string ConvertVehicleStateToReadableString(eStateOfVehicle i_StateOfVehicle)
        {
            string stateAsString;

            switch (i_StateOfVehicle)
            {
                case eStateOfVehicle.Fixed:
                    stateAsString = "fixed";
                    break;
                case eStateOfVehicle.InRepair:
                    stateAsString = "in repair";
                    break;
                default:
                    stateAsString = "paid";
                    break;
            }

            return stateAsString;
        }

        internal void ConstructEnergySystem(eEnergySystemType i_EnergySystemType)
        {
            if (i_EnergySystemType == eEnergySystemType.FueldEnergySystem)
            {
                m_EnergySystem = new FuelEnergySystem();
            } else if (i_EnergySystemType == eEnergySystemType.ElectricEnergySystem)
            {
                m_EnergySystem = new ElectricEnergySystem();
            } else
            {
                throw new ArgumentException("illeagal input please insert input in the requested format: ");
            }
        }

        // The method sets current amount of energy and updates m_RemainingEnergyPercentage
        internal void SetCurrentAmountOfEnergy(float i_CurrentAmountOfEnergy)
        {
            if (i_CurrentAmountOfEnergy < 0 || i_CurrentAmountOfEnergy > m_EnergySystem.MaxAmountOfEnergy)
            {
                throw new ValueOutOfRangeException(0, m_EnergySystem.MaxAmountOfEnergy);
            }

            m_EnergySystem.CurrentAmountOfEnergy = i_CurrentAmountOfEnergy;
            m_RemainingEnergyPercentage = m_EnergySystem.CurrentAmountOfEnergy / m_EnergySystem.MaxAmountOfEnergy;
        }

        public void SetWheelsInformation(string i_ManufacturerName, float i_CurrentAirPressure)
        {
            foreach (Wheel wheel in m_WheelsList)
            {
                wheel.ManufacturerName = i_ManufacturerName;
                if (i_CurrentAirPressure < 0 || i_CurrentAirPressure > wheel.MaxAirPressure)
                {
                    throw new ValueOutOfRangeException(0, wheel.MaxAirPressure);
                }

                wheel.CurrentAirPressure = i_CurrentAirPressure;
            }
        }

        public override string ToString()
        {
            string vehicleInformation = string.Format("License number: {0}, Model: {1}, Owner name: {2}, Vehicle state: {3}"
,r_LicenseNumber, r_ModelName, r_OwnerName, ConvertVehicleStateToReadableString(m_VehicleState));

            return vehicleInformation;
        }
    }

}
