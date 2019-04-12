using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public interface IEnergySystem
    {
        void EnergyFill(EnergyFillInformationInput i_DataOnEnergy);
        float CurrentAmountOfEnergy { get; set; }
        float MaxAmountOfEnergy { get; set; }
        string ToString();
    }
}
