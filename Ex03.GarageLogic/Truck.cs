using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    internal class Truck : Vehicle, ISupportedByGarage
    {
        private bool m_IsCarryingHazardousMaterials;
        private float m_WeightOfCargo;
        private static readonly string[] sr_InformationRequests =
{
@"Is your truck contains hazardous materials:
(1) Yes
(2) No
",
@"Please enter the weight of your truck's cargo:
"
 };

        public Truck(string i_OwnerName, string i_OwnerPhoneNumber, string i_ModelName, string i_LicenseNumber)
             : base (i_OwnerName, i_OwnerPhoneNumber, i_ModelName, i_LicenseNumber)
        {
            for (int i = 0; i < 4; i++)
            {
                Wheel wheel = new Wheel(28);
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
            ((FuelEnergySystem)energySystem).FuelType = eFuelType.Octan96;
            energySystem.MaxAmountOfEnergy = 115;
        }

        public void SetInformationByRequestIndex(string i_InformationToSubmmit, int i_RequestIndex)
        {
            bool theAnswerIsNumber;

            if (i_RequestIndex == 0)
            {
                theAnswerIsNumber = ushort.TryParse(i_InformationToSubmmit, out ushort answerNumberQ1);
                if (!theAnswerIsNumber || answerNumberQ1 < 1 || answerNumberQ1 > 2)
                {
                    throw new FormatException("invalid input - please enter the number from 1 to 2 that represent the correct answer");
                }

                switch (answerNumberQ1)
                {
                    case 1:
                        m_IsCarryingHazardousMaterials = true;
                        break;
                    default:
                        m_IsCarryingHazardousMaterials = false;
                        break;
                }
            }
            else
            {
                theAnswerIsNumber = float.TryParse(i_InformationToSubmmit, out float answerValueQ2);
                if (!theAnswerIsNumber || answerValueQ2 < 0)
                {
                    throw new ArgumentException("invalid input - weight of cargo can't be negetive number, please enter the correct weight of cargo");
                }

                m_WeightOfCargo = answerValueQ2;
            }
        }

        public override string ToString()
        {
            string containsHazardousMaterials = "this truck doesn't contain hazardous materials";

            if (m_IsCarryingHazardousMaterials)
            {
                containsHazardousMaterials = "this truck contains hazardous materials!";
            }

            string vehicleInformation = string.Format(@"Vehicle type: Truck
{0}
{1}, weight of cargo: {2}
wheels information: {3}
Battery information: {4}", base.ToString(), containsHazardousMaterials, m_WeightOfCargo, this.WheelsList.First().ToString()
,this.EnergySystem.ToString());

            return vehicleInformation;
        }
    }
}
