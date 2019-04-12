using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        float m_MinValue;
        float m_MaxValue;

        public ValueOutOfRangeException(float i_MinValue, float i_MaxValue) :
            base(string.Format("The value that you entered is out of range, it should be between {0} and {1}",
                i_MinValue, i_MaxValue))
        {
            m_MinValue = i_MinValue;
            m_MaxValue = i_MaxValue;
        }

        internal float MinValue
        {
            get { return m_MinValue; }
            set { m_MinValue = value; }
        } 

        internal float MaxValue
        {
            get { return m_MaxValue; }
            set { m_MaxValue = value; }
        }
    }
}
