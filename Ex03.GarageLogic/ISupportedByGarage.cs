using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    internal interface ISupportedByGarage
    {
        string[] InformationRequests { get; }
        void SetInformationByRequestIndex(string i_InformationToSubmmit, int i_RequestIndex);
        void SetMaxAmountOfEnergyAndOptionalFuelType();
    }
}
