using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public enum eColor
    {
        Gray,
        Blue,
        White,
        Black
    }

    internal class Car : Vehicle, ISupportedByGarage
    {
        private eColor m_Color;
        private ushort m_DoorsNumber;
        private static readonly string[] sr_InformationRequests =
{ @"Please choose your car's color:
(1) Grey
(2) Blue
(3) White
(4) Black
",
@"Please choose the number of doors in your car:
(1) 2
(2) 3
(3) 4
(4) 5
" };
            
        public Car(string i_OwnerName, string i_OwnerPhoneNumber, string i_ModelName, string i_LicenseNumber)
           : base (i_OwnerName, i_OwnerPhoneNumber, i_ModelName, i_LicenseNumber)
        {
            for (int i = 0; i < 4; i++)
            {
                Wheel wheel = new Wheel(32);
                base.WheelsList.Add(wheel);
            }
        }

        public string[] InformationRequests
        {
            get { return sr_InformationRequests; }
        }

        public void SetMaxAmountOfEnergyAndOptionalFuelType()
        {
            IEnergySystem energySystem = base.EnergySystem;
            FuelEnergySystem fuelEnergySystem = energySystem as FuelEnergySystem;

            if (fuelEnergySystem != null)
            {
                fuelEnergySystem.FuelType = eFuelType.Octan98;
                energySystem.MaxAmountOfEnergy = 45;
            } else if(energySystem is ElectricEnergySystem)
            {
                energySystem.MaxAmountOfEnergy = 3.2f;
            }
            else //occur if base.EnergySystem == null
            {
                throw new InvalidOperationException("The vehicle's energy system didn't initialize yet");
            }
        }

        public void SetInformationByRequestIndex(string i_InformationToSubmmit, int i_RequestIndex)
        {
            bool theAnswerIsNumber;

            theAnswerIsNumber = ushort.TryParse(i_InformationToSubmmit, out ushort answerNumber);
            if (!theAnswerIsNumber || answerNumber < 1 || answerNumber > 4)
            {
                throw new FormatException("invalid input - please enter the number from 1 to 4 that represent the correct answer");
            }

            if (i_RequestIndex == 0)
            {
                switch (answerNumber)
                {
                    case 1:
                        m_Color = eColor.Gray;
                        break;
                    case 2:
                        m_Color = eColor.Blue;
                        break;
                    case 3:
                        m_Color = eColor.White;
                        break;
                    default:
                        m_Color = eColor.Black;
                        break;
                }
            }
            else
            {
                m_DoorsNumber = answerNumber;
            }
        }

        public override string ToString()
        {
            string energySystemTypeDecleration = "Fuel system information";
            if (this.EnergySystem is ElectricEnergySystem)
            {
                energySystemTypeDecleration = "Battery information";
            }

            string vehicleInformation = string.Format(@"Vehicle type: Car
{0}
color: {1}, number of doors: {2}
wheels information: {3}
{4}: {5}", base.ToString(), m_Color, m_DoorsNumber, this.WheelsList.First().ToString(),energySystemTypeDecleration,
this.EnergySystem.ToString());

            return vehicleInformation;
        }
    }
}
