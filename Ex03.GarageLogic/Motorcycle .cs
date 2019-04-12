using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{

    public enum eLicenseType
    {
        A,
        A1,
        B1,
        B2
    }

    internal class Motorcycle : Vehicle, ISupportedByGarage
    {
        private eLicenseType m_LicenseType;
        private int m_EngineVolume;
        private static readonly string[] sr_InformationRequests = 
{
@"Please choose your motorcycle license type:
(1) A
(2) A1
(3) B1
(4) B2
",
@"Please enter your motorcycle engine Volume
"};
    
        public Motorcycle(string i_OwnerName, string i_OwnerPhoneNumber, string i_ModelName, string i_LicenseNumber)
            : base(i_OwnerName, i_OwnerPhoneNumber, i_ModelName, i_LicenseNumber)
        {
            for (int i = 0; i < 2; i++)
            {
                Wheel wheel = new Wheel(30);
                WheelsList.Add(wheel);
            }
        }

        public string[] InformationRequests
        {
            get { return sr_InformationRequests; }
        }

        public void SetMaxAmountOfEnergyAndOptionalFuelType()
        {
            IEnergySystem energySystem = this.EnergySystem;
            FuelEnergySystem fuelEnergySystem = energySystem as FuelEnergySystem;

            if (fuelEnergySystem != null)
            {
                fuelEnergySystem.FuelType = eFuelType.Octan96;
                fuelEnergySystem.MaxAmountOfEnergy = 6;
            }
            else
            {
                energySystem.MaxAmountOfEnergy = 1.8f;
            }

        }

        public void SetInformationByRequestIndex(string i_InformationToSubmmit, int i_RequestIndex)
        {
            bool theAnswerIsNumber;

            if (i_RequestIndex == 0)
            {
                theAnswerIsNumber = ushort.TryParse(i_InformationToSubmmit, out ushort answerNumberQ1);
                if (!theAnswerIsNumber || answerNumberQ1 < 1 || answerNumberQ1 > 4)
                {
                    throw new FormatException("invalid input - please enter the number from 1 to 4 that represent the correct answer");
                }

                switch (answerNumberQ1)
                {
                    case 1:
                        m_LicenseType = eLicenseType.A;
                        break;
                    case 2:
                        m_LicenseType = eLicenseType.A1;
                        break;
                    case 3:
                        m_LicenseType = eLicenseType.B1;
                        break;
                    default:
                        m_LicenseType = eLicenseType.B2;
                        break;
                }
            }
            else
            {
                theAnswerIsNumber = int.TryParse(i_InformationToSubmmit, out int answerValueQ2);
                if (!theAnswerIsNumber || answerValueQ2 < 0)
                {
                    throw new ArgumentException("invalid input - engine volume can't be negetive number, please enter the correct engine volume");
                }

                m_EngineVolume = answerValueQ2;
            }
        }

        public override string ToString()
        {
            string energySystemTypeDecleration = "Fuel system information";
            if (this.EnergySystem is ElectricEnergySystem)
            {
                energySystemTypeDecleration = "Battery information";
            }

            string vehicleInformation = string.Format(@"Vehicle type: Motorcycle
{0}
License type: {1}, engine Volume: {2}
wheels information: {3}
{4}: {5}", base.ToString(), m_LicenseType, m_EngineVolume, this.WheelsList.First().ToString(), energySystemTypeDecleration
, this.EnergySystem.ToString());

            return vehicleInformation;
        }

    }
}
