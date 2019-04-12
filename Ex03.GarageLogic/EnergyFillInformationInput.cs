using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ex03.GarageLogic.FuelEnergySystem;

namespace Ex03.GarageLogic
{
    public struct EnergyFillInformationInput
    {
        private float m_AmountToFill;
        private eFuelType? m_GasolineType;

        public EnergyFillInformationInput(float i_AmountToFill, eFuelType? i_GasolineType)
        {
            m_AmountToFill = i_AmountToFill;
            m_GasolineType = i_GasolineType;
        }

        public float AmountToFill
        {
            get { return m_AmountToFill; }
            set { m_AmountToFill = value; }
        }

        public eFuelType? GasolineType
        {
            get { return m_GasolineType; }
            set { m_GasolineType = value; }
        }
    }
}
