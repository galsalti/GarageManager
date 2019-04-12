using System;
using System.Collections.Generic;
using System.Text;
using Ex03.GarageLogic;

namespace Ex03.ConsoleUI
{
    public enum eUserActionType
    {
        Illegal,
        AddVehicle,
        DisplayAllVehicleLicenseNumbers,
        ChangeVehicleState,
        InflateVehicleWheelsToMaximum,
        RefuelVehicle,
        RechargeVehicle,
        DisplayFullVehicleData,
        Exit
    }

    public class UserInputManager
    {
        private readonly GarageManager r_Garage;
        private const bool v_FillingOperation = true;
        private const bool v_WordOfLettersOnly = true;
        private const bool v_ChangeStateOperation = true;

        public UserInputManager(GarageManager i_Garage)
        {
            r_Garage = i_Garage;
        }
        internal void StartUserService()
        {
            bool userActionIsValid, serviceHasBegun = false;
            ushort numOfUserAction = 0;
            eUserActionType userActionType = eUserActionType.Illegal;

            while (userActionType != eUserActionType.Exit)
            {
                if (serviceHasBegun)
                {
                    Console.WriteLine(@"
====================================
Press ENTER to return to menu....
====================================
");
                    Console.ReadLine();
                    Console.Clear();
                }

                serviceHasBegun = true;
                userActionIsValid = false;
                while (!userActionIsValid)
                {
                    displayAvailableUserActions();
                    userActionIsValid = (ushort.TryParse(Console.ReadLine(), out numOfUserAction))
                        && (numOfUserAction <= 8) && (numOfUserAction != 0);
                    if (!userActionIsValid)
                    {
                        Console.Clear();
                        Console.WriteLine(@"====================================
Illegal action mode! Please choose a valid action between 1 to 8.
====================================");
                    }
                }

                Console.Clear();
                userActionType = (eUserActionType)numOfUserAction;
                applyUserAction(userActionType);
            }
        }

        private void applyUserAction(eUserActionType i_UserActionType)
        {
            switch (i_UserActionType)
            {
                case eUserActionType.Illegal:
                    Console.WriteLine("Illegal Mode!");
                    break;
                case eUserActionType.Exit:
                    Console.WriteLine("Have a nice day!");
                    break;
                case eUserActionType.AddVehicle:
                    addVehicle();
                    break;
                case eUserActionType.ChangeVehicleState:
                    changeVehicleState();
                    break;
                case eUserActionType.DisplayAllVehicleLicenseNumbers:
                    displayVehicleLicenseNumbersByState();
                    break;
                case eUserActionType.DisplayFullVehicleData:
                    displayFullVehicleData();
                    break;
                case eUserActionType.InflateVehicleWheelsToMaximum:
                    inflateVehicleWheelsToMaximum();
                    break;
                case eUserActionType.RechargeVehicle:
                    refillVehicle(eEnergySystemType.ElectricEnergySystem);
                    break;
                case eUserActionType.RefuelVehicle:
                    refillVehicle(eEnergySystemType.FueldEnergySystem);
                    break;
            }
        }

        private void refillVehicle(eEnergySystemType i_TypeOfEnergy)
        {
            bool fillSucceeded = false;
            eFuelType? fuelType;
            string licenseNumber = receiveNumber("license");
            string energyFillerType = getStringOfEnergyFiller(i_TypeOfEnergy);
            float amountToFill = receiveFillOrCurrentAmount(energyFillerType, v_FillingOperation);

            if (i_TypeOfEnergy == eEnergySystemType.FueldEnergySystem)
            {
                fuelType = receiveFuelType();
            }
            else
            {
                fuelType = null;
            }

            try
            {
                if (!(fillSucceeded = r_Garage.TryFillEnergyInVehicle(licenseNumber, amountToFill, fuelType)))
                {
                    Console.WriteLine("Illegal license number!");
                }
            }
            catch (ArgumentException ae)
            {
                fillSucceeded = false;
                Console.WriteLine(ae.Message);
            }
            catch (ValueOutOfRangeException voore)
            {
                fillSucceeded = false;
                Console.WriteLine(voore.Message);
            }
            finally
            {
                if (!fillSucceeded)
                {
                    if (checkIfUserWantsAnotherTry())
                    {
                        refillVehicle(i_TypeOfEnergy);
                    }
                }
                else
                {
                    Console.WriteLine("Refilling succeeded!");
                }
            }
        }

        private float receiveFillOrCurrentAmount(string i_TypeToFillAsString, bool i_Fill)
        {
            float currentOrFillAmount;
            bool amountIsLegal = false;
            string messageToUser;

            if (i_Fill)
            {
                messageToUser = string.Format("Please enter the amount of {0} you want to fill", i_TypeToFillAsString);
            }
            else
            {
                messageToUser = string.Format("Please enter the amount of {0} you currently have", i_TypeToFillAsString);
            }

            Console.WriteLine(messageToUser);
            while (!(amountIsLegal = float.TryParse(Console.ReadLine(), out currentOrFillAmount)))
            {
                Console.WriteLine(string.Format(@"Illegal input! {0}", messageToUser));
            }

            return currentOrFillAmount;
        }

        private eFuelType receiveFuelType()
        {
            ushort fuelTypeAsNumber;
            eFuelType fuelType;
            bool fuelTypeIsLegal = false;
            string messageToUser = @"Please enter the type of the fuel:
(0) Soler
(1) Octan95
(2) Octan96
(3) Octan98";

            Console.WriteLine(messageToUser);
            while (!(fuelTypeIsLegal = ushort.TryParse(Console.ReadLine(), out fuelTypeAsNumber))
                || fuelTypeAsNumber > 3)
            {
                Console.WriteLine(string.Format(@"Illegal input! {0}", messageToUser));
            }

            fuelType = (eFuelType)fuelTypeAsNumber;

            return fuelType;
        }

        private string receiveNumber(string i_NumberOf)
        {
            string numberToReceive;
            bool isLegalNumber = false;
            string messageToUser = string.Format("Please enter your {0} number:", i_NumberOf);

            Console.WriteLine(messageToUser);
            numberToReceive = Console.ReadLine();
            while (!isLegalNumber)
            {
                isLegalNumber = !string.IsNullOrWhiteSpace(numberToReceive);
                foreach (char digit in numberToReceive)
                {
                    if (!char.IsDigit(digit))
                    {
                        isLegalNumber = false;
                        break;
                    }
                }

                if (!isLegalNumber)
                {
                    Console.WriteLine(string.Format("Illegal input! {0}", messageToUser));
                    numberToReceive = Console.ReadLine();
                }
            }

            return numberToReceive;
        }

        private void inflateVehicleWheelsToMaximum()
        {
            bool inflateSuceeded;
            string licenseNumber = receiveNumber("license");

            inflateSuceeded = r_Garage.TryInflateVehiclesWheelsToMaximum(licenseNumber);
            if (!inflateSuceeded)
            {
                Console.WriteLine("Illegal license number!");
                if (checkIfUserWantsAnotherTry())
                {
                    inflateVehicleWheelsToMaximum();
                }
            }
            else
            {
                Console.WriteLine("Inflated wheels to maximum.");
            }
        }

        private void displayVehicleLicenseNumbersByState()
        {
            string stateAsString;
            StringBuilder licenseNumbersListAsString = new StringBuilder();
            List<string> licenseNumberOfVehicles;
            eStateOfVehicle? filterState = getStateOfVehicle(!v_ChangeStateOperation);

            if (filterState == null)
            {
                stateAsString = "all";
                licenseNumberOfVehicles = r_Garage.GetVehiclesLicenseNumberList();
            }
            else
            {
                eStateOfVehicle stateChosen = (eStateOfVehicle)filterState;
                stateAsString = Vehicle.ConvertVehicleStateToReadableString(stateChosen);
                licenseNumberOfVehicles = r_Garage.GetVehiclesListFilteredByState((eStateOfVehicle)filterState);
            }

            licenseNumbersListAsString.Append(string.Format(@"List of {0} vehicles in the garage:
", stateAsString));
            foreach (string currentLicenseNumber in licenseNumberOfVehicles)
            {
                licenseNumbersListAsString.Append(string.Format(@"{0}
", currentLicenseNumber));
            }

            Console.WriteLine(licenseNumbersListAsString);
        }

        private void displayFullVehicleData()
        {
            string informationOnVehicle;
            string licenseNumber = receiveNumber("license");
            bool licenseNumberExists = r_Garage.TryGetVehicleInformation(licenseNumber, out informationOnVehicle);

            if (licenseNumberExists)
            {
                Console.WriteLine(informationOnVehicle);
            }
            else
            {
                Console.WriteLine("The license number doesn't exist in our garage!");
                if (checkIfUserWantsAnotherTry())
                {
                    displayFullVehicleData();
                }
            }
        }

        private bool checkIfUserWantsAnotherTry()
        {
            ushort optionChosenInNum;
            string messageToUser = @"Do you want to:
(0) Return to menu
(1) Try again";

            Console.WriteLine(messageToUser);
            while (!(ushort.TryParse(Console.ReadLine(), out optionChosenInNum))
                || optionChosenInNum > 1)
            {
                Console.WriteLine(string.Format("Illegal input! {0}", messageToUser));
            }

            return optionChosenInNum == 1;
        }

        private void changeVehicleState()
        {
            bool userWantsAnotherTry = true;
            bool changeSucceeded;
            string licenseNumber = receiveNumber("license");
            eStateOfVehicle newState = (eStateOfVehicle)getStateOfVehicle(v_ChangeStateOperation);

            while (userWantsAnotherTry)
            {
                userWantsAnotherTry = false;
                if (!(changeSucceeded = r_Garage.TryChangeVehicleState(licenseNumber, newState)))
                {
                    Console.WriteLine("The vehicle doesn't exist!");
                    if ((userWantsAnotherTry = checkIfUserWantsAnotherTry()))
                    {
                        licenseNumber = receiveNumber("license");
                    }
                }
                else
                {
                    Console.WriteLine(string.Format(@"Change vehicle state to: {0}",
                        Vehicle.ConvertVehicleStateToReadableString(newState)));
                }
            }
        }

        private eStateOfVehicle? getStateOfVehicle(bool i_ChangeState)
        {
            eStateOfVehicle? stateChosen;
            bool isLegalState;
            ushort stateInNum, maxAmount;
            string messageToUser = @"Please enter the desired state of the vehicle:
(0) Fixed
(1) In Repair
(2) Paid";
            
            if (!i_ChangeState)
            {
                messageToUser = string.Format(@"{0}
(3) All", messageToUser);
                maxAmount = 3;
            }
            else
            {
                maxAmount = 2;
            }

            Console.WriteLine(messageToUser);
            while (!(isLegalState = ushort.TryParse(Console.ReadLine(), out stateInNum)) ||
                stateInNum > maxAmount)
            {
                Console.WriteLine(string.Format("Illegal Input! {0}", messageToUser));
            }

            if (!i_ChangeState && stateInNum == 3)
            {
                stateChosen = null;
            }
            else
            {
                stateChosen = (eStateOfVehicle)stateInNum;
            }

            return stateChosen;
        }

        private void addVehicle()
        {
            List<eEnergySystemType> energySystemsAvailableInCurrentType;
            List<eTypeOfVehicle> listOfVehicleTypes = r_Garage.GetSupportedVehiclesInformation();
            eTypeOfVehicle vehicleTypeChosen = receiveTypeOfVehicleFromUser(listOfVehicleTypes);
            string[] additionalDataOnSpecificVehicleInRequests;
            string[] dataOnOwnerAndVehicle = { receiveNumber("license"), receiveName("owner", v_WordOfLettersOnly),
                receiveNumber("phone"), receiveName("model", !v_WordOfLettersOnly) };
            string wheelManufacturer;
            string fillByEnergyTypeAsString;
            float currentAirInWheels;
            float amountOfEnergyLeft;
            Vehicle userVehicle = createVehicleByType(vehicleTypeChosen, dataOnOwnerAndVehicle[0],
                dataOnOwnerAndVehicle[1], dataOnOwnerAndVehicle[2], dataOnOwnerAndVehicle[3], out energySystemsAvailableInCurrentType);
            eEnergySystemType energySystem;
            eEnergySystemType? maybeEnergySystem = receiveEnergySystemType(energySystemsAvailableInCurrentType);

            if (maybeEnergySystem == null)
            {
                r_Garage.RemoveVehicle(userVehicle.LicenseNumber);
            }
            else
            {
                energySystem = (eEnergySystemType)maybeEnergySystem;
                fillByEnergyTypeAsString = getStringOfEnergyFiller(energySystem);
                amountOfEnergyLeft = receiveFillOrCurrentAmount(fillByEnergyTypeAsString, !v_FillingOperation);
                insertEnergySystemInVehicle(userVehicle, vehicleTypeChosen, energySystem, amountOfEnergyLeft);
                wheelManufacturer = receiveName("wheel manufacturer", !v_WordOfLettersOnly);
                currentAirInWheels = receiveFillOrCurrentAmount("air", !v_FillingOperation);
                insertWheelInformation(userVehicle, wheelManufacturer, currentAirInWheels);
                additionalDataOnSpecificVehicleInRequests = r_Garage.GetVehicleInformationRequests(userVehicle, vehicleTypeChosen);
                addInformationOnAdditionalRequests(additionalDataOnSpecificVehicleInRequests, userVehicle, vehicleTypeChosen);
            }
        }
        private void insertWheelInformation(Vehicle i_UserVehicle, string i_WheelManufacturer, float i_CurrentAirInWheels)
        {
            bool actionSucceeded = false;

            while (!actionSucceeded)
            {
                try
                {
                    actionSucceeded = true;
                    i_UserVehicle.SetWheelsInformation(i_WheelManufacturer, i_CurrentAirInWheels);
                }
                catch (ValueOutOfRangeException voore)
                {
                    actionSucceeded = false;
                    Console.WriteLine(voore.Message);
                    i_CurrentAirInWheels = receiveFillOrCurrentAmount("air", !v_FillingOperation);
                }
            }
        }

        private void insertEnergySystemInVehicle(Vehicle i_UserVehicle, eTypeOfVehicle i_VehicleTypeChosen, eEnergySystemType i_EnergySystem, float i_AmountOfEnergyLeft)
        {
            bool actionSucceeded = false;

            while (!actionSucceeded)
            {
                try
                {
                    actionSucceeded = true;
                    r_Garage.ConstructEnergySystemInVehicle(i_UserVehicle, i_VehicleTypeChosen, i_EnergySystem, i_AmountOfEnergyLeft);
                }
                catch (ValueOutOfRangeException voore)
                {
                    actionSucceeded = false;
                    Console.WriteLine(voore.Message);
                    i_AmountOfEnergyLeft = receiveFillOrCurrentAmount(getStringOfEnergyFiller(i_EnergySystem), !v_FillingOperation);
                }
            }
        }

        private Vehicle createVehicleByType(eTypeOfVehicle i_VehicleTypeChosen, string i_LicenseNumber, string i_VehicleOwnerName, string i_PhoneNumber, string i_ModelName, out List<eEnergySystemType> i_EnergySystemsAvailable)
        {
            Vehicle userVehicle = null;
            i_EnergySystemsAvailable = null;
            bool actionSucceeded = false;

            while (!actionSucceeded)
            {
                try
                {
                    actionSucceeded = true;
                    userVehicle = r_Garage.ConstructVehicle(i_VehicleTypeChosen, i_VehicleOwnerName, i_PhoneNumber, i_ModelName, i_LicenseNumber, out i_EnergySystemsAvailable);
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine(ae.Message);
                    i_LicenseNumber = receiveNumber("license");
                    actionSucceeded = false;
                }
            }

            return userVehicle;
        }

        private void addInformationOnAdditionalRequests(string[] i_AdditionalDataOnSpecificVehicleInRequests, Vehicle i_UserVehicle, eTypeOfVehicle i_VehicleType)
        {
            bool requestSucceeded = false;
            string inputFromUser;
            int indexOfCurrentRequest = 0;

            foreach (string currentRequest in i_AdditionalDataOnSpecificVehicleInRequests)
            {
                requestSucceeded = false;

                while (!requestSucceeded)
                {
                    try
                    {
                        Console.Write(currentRequest);
                        requestSucceeded = true;
                        inputFromUser = Console.ReadLine();
                        while (string.IsNullOrWhiteSpace(inputFromUser))
                        {
                            Console.Write(string.Format("Illegal input! {0}", currentRequest));
                        }
                        r_Garage.SetVehicleInformationByRequestIndex(i_UserVehicle, i_VehicleType, inputFromUser, indexOfCurrentRequest);
                    }
                    catch (FormatException fe)
                    {
                        requestSucceeded = false;
                        Console.WriteLine(fe.Message);
                    }
                    catch (ValueOutOfRangeException voore)
                    {
                        requestSucceeded = false;
                        Console.WriteLine(voore.Message);
                    }
                    catch (ArgumentException ae)
                    {
                        requestSucceeded = false;
                        Console.WriteLine(ae.Message);
                    }
                }

                indexOfCurrentRequest++;
            }
        }

        private string getStringOfEnergyFiller(eEnergySystemType i_EnergySystem)
        {
            string stringOfEnergyFiller;

            if (i_EnergySystem == eEnergySystemType.ElectricEnergySystem)
            {
                stringOfEnergyFiller = "energy";
            }
            else
            {
                stringOfEnergyFiller = "fuel";
            }

            return stringOfEnergyFiller;
        }

        private string receiveName(string i_NameType, bool i_LetterOnly)
        {
            string nameChosen;
            bool nameIsLegal = false;
            string messageToUser = string.Format("Please enter {0} name:", i_NameType);

            Console.WriteLine(messageToUser);
            nameChosen = Console.ReadLine();
            while (!nameIsLegal)
            {
                nameIsLegal = !string.IsNullOrWhiteSpace(nameChosen);
                foreach (char letter in nameChosen)
                {
                    if (((!(char.IsLetter(letter)) && i_LetterOnly)
                        || (!(char.IsLetterOrDigit(letter)) && !i_LetterOnly))
                        && !char.IsWhiteSpace(letter))
                    {
                        nameIsLegal = false;
                        break;
                    }
                }

                if (!nameIsLegal)
                {
                    Console.WriteLine(string.Format("Illegal input! {0}", messageToUser));
                    nameChosen = Console.ReadLine();
                }
            }

            return nameChosen;
        }

        private eEnergySystemType? receiveEnergySystemType(ICollection<eEnergySystemType> i_EnergySystemsAvailableForVehicle)
        {
            List<ushort> availableAnswers = new List<ushort>();
            eEnergySystemType? energySystemToReturn;
            bool isValidEnergySystem;
            ushort numOfEnergySystemChosen, currentNumOfEnergySystem;
            StringBuilder messageToUser = new StringBuilder(@"For this vehicle, we support the following energy systems:
");

            foreach (eEnergySystemType currentEnergySystem in i_EnergySystemsAvailableForVehicle)
            {
                currentNumOfEnergySystem = (ushort)currentEnergySystem;
                messageToUser.Append(string.Format(@"({0}) {1}
", currentNumOfEnergySystem, currentEnergySystem));
                availableAnswers.Add(currentNumOfEnergySystem);
            }

            availableAnswers.Add(2);
            messageToUser.Append(@"Choose the energy system of your vehicle.
If we don't support it, please press (2) to exit to menu");
            Console.WriteLine(messageToUser);
            while (!(isValidEnergySystem = ushort.TryParse(Console.ReadLine(), out numOfEnergySystemChosen))
                || (!availableAnswers.Contains(numOfEnergySystemChosen)))
            {
                Console.WriteLine(string.Format(@"Illegal input! {0}", messageToUser));
            }

            if (numOfEnergySystemChosen == 2)
            {
                energySystemToReturn = null;
            }
            else
            {
                energySystemToReturn = (eEnergySystemType)numOfEnergySystemChosen;
            }

            return energySystemToReturn;
        }

        private eTypeOfVehicle receiveTypeOfVehicleFromUser(List<eTypeOfVehicle> i_ListOfVehicleTypes)
        {
            ushort numOfVehicleType;
            bool isValidVehicleType;
            StringBuilder messageToUser = new StringBuilder(@"Please enter the type of your vehicle:
");
            ushort currentIndexInList = 0;

            foreach (eTypeOfVehicle currentTypeOfVehicle in i_ListOfVehicleTypes)
            {
                messageToUser.Append(string.Format(@"({0}) {1}
", currentIndexInList, currentTypeOfVehicle));
                currentIndexInList++;
            }

            Console.WriteLine(messageToUser);
            while (!(isValidVehicleType = ushort.TryParse(Console.ReadLine(), out numOfVehicleType))
                || (numOfVehicleType > i_ListOfVehicleTypes.Count))
            {
                Console.WriteLine(string.Format(@"Illegal input! {0}", messageToUser));
            }

            return (eTypeOfVehicle)numOfVehicleType;
        }

        private void displayAvailableUserActions()
        {
            string messageForUser = string.Format(@"Hello! Welcome to Niv&Gal Garage, 
please enter the number of the action you want to do:
(1) Add a vehicle to the garage
(2) Display all vehicle license numbers(option for filtering)
(3) Change a vehicle state
(4) Inflate vehicle's wheels to maximum
(5) Refuel vehicle
(6) Recharge vehicle
(7) Display full data on a vehicle
(8) Exit garage");

            Console.WriteLine(messageForUser);
        }
    }
}
