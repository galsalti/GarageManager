using System;
using Ex03.GarageLogic;

namespace Ex03.ConsoleUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
           GarageManager myGarage = new GarageManager();
           UserInputManager garageInputManager = new UserInputManager(myGarage);

           garageInputManager.StartUserService();
        }
    }
}
