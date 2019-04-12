using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    internal class Wheel
    {
        private string m_ManufacturerName;
        private float m_CurrentAirPressure;
        private readonly float r_MaxAirPressure;

        public Wheel(float i_MaxAirPressure)
        {
            r_MaxAirPressure = i_MaxAirPressure;
        }

        internal void InflateAirPressure(float i_AmountOfAirToAdd)
        {
            if (i_AmountOfAirToAdd < 0 || m_CurrentAirPressure + i_AmountOfAirToAdd > r_MaxAirPressure)
            {
                throw new ValueOutOfRangeException(0, r_MaxAirPressure - m_CurrentAirPressure);
            }

            m_CurrentAirPressure += i_AmountOfAirToAdd;
        }

        internal void InflateAirPressureToMaximum()
        {
            m_CurrentAirPressure = r_MaxAirPressure;
        }

        internal string ManufacturerName
        {
            get { return m_ManufacturerName; }
            set { m_ManufacturerName = value; }
        }

        internal float CurrentAirPressure
        {
            get { return m_CurrentAirPressure; }
            set { m_CurrentAirPressure = value; }
        }

        internal float MaxAirPressure
        {
            get { return r_MaxAirPressure; }
        }

        public override string ToString()
        {
            string vehicleInformation = string.Format("Manufacturer name: {0}, Current air pressure: {1}"
, m_ManufacturerName, m_CurrentAirPressure);

            return vehicleInformation;
        }
    }
}
