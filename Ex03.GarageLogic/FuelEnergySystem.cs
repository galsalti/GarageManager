using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public enum eFuelType
    {
        Soler,
        Octan95,
        Octan96,
        Octan98
    }

    internal class FuelEnergySystem : IEnergySystem
    {
        eFuelType m_FuelType;
        float m_CurrentAmountOfFuel;
        float m_MaxAmountOfFuel;

        public eFuelType FuelType
        {
            get { return m_FuelType; }
            set { m_FuelType = value; }
        }

        public float CurrentAmountOfEnergy
        {
            get { return m_CurrentAmountOfFuel; }
            set { m_CurrentAmountOfFuel = value; }
        }

        public float MaxAmountOfEnergy { 
           get { return m_MaxAmountOfFuel; }
           set { m_MaxAmountOfFuel = value; }
        }

        public void EnergyFill(EnergyFillInformationInput i_DataOnEnergy)
        {
            checkValidityOfData(i_DataOnEnergy);
            m_CurrentAmountOfFuel += i_DataOnEnergy.AmountToFill;
        }

        private void checkValidityOfData(EnergyFillInformationInput i_DataOnEnergy)
        {
            if (i_DataOnEnergy.GasolineType != m_FuelType)
            {
                throw new ArgumentException("Illegal type of gasoline!");
            }

            if (i_DataOnEnergy.AmountToFill + m_CurrentAmountOfFuel > m_MaxAmountOfFuel)
            {
                throw new ValueOutOfRangeException(0, m_MaxAmountOfFuel - m_CurrentAmountOfFuel);
            }
        }

        public override string ToString()
        {
            return string.Format("Fuel type: {0}, fuel remains: {1} liters", m_FuelType, m_CurrentAmountOfFuel);
        }
    }
}
