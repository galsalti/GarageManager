using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    internal class ElectricEnergySystem : IEnergySystem
    {
        float m_MotorTimeRemaining;
        float m_MaxMotorTime;

        public float CurrentAmountOfEnergy
        {
            get { return m_MotorTimeRemaining; }
            set { m_MotorTimeRemaining = value; }
        }

        public float MaxAmountOfEnergy
        {
            get { return m_MaxMotorTime; }
            set { m_MaxMotorTime = value; }
        }

        public void EnergyFill(EnergyFillInformationInput i_DataOnEnergy)
        {
            checkValidityOfData(i_DataOnEnergy);
            m_MotorTimeRemaining += i_DataOnEnergy.AmountToFill;
        }

        private void checkValidityOfData(EnergyFillInformationInput i_DataOnEnergy)
        {
            if (i_DataOnEnergy.AmountToFill + m_MotorTimeRemaining > m_MaxMotorTime)
            {
                throw new ValueOutOfRangeException(0, m_MaxMotorTime - m_MotorTimeRemaining);
            }
        }

        public override string ToString()
        {
            return string.Format("Time remaning in battery: {0} hours", m_MotorTimeRemaining);
        }
    }
}
