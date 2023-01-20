using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Formats.Asn1;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Base_Area_51
{
    internal class Agent
    {
        Elevator elevator;
        string securityLevel;
        public int Id;
        public int currentFloorNum;
        public string currentFloorName;
        int floorDiff;
        int waitTimeComing;
        int waitTimeGoing;
        int randValue;
        int floorGoingTo;
        string agentCredentials = "";

        public Agent(string securityLevel, Elevator elevator)
        {
            this.securityLevel = securityLevel;
            this.elevator = elevator;

            currentFloorName = elevator.floorNames[0];
            currentFloorNum= 0;
        }

        public enum infoTypes
        {
            ArrivedAtBase,
            EnteringElevator,
            LeavingElevator,
            CallingElevator,
            WaitingElevator,
            Working,
            LeavingBase,
            GoingToFloorG,
            GoingFromAtoB,
            Credentials
        }
        public void DisplayInfo(infoTypes infoType)
        {
            switch (infoType)
            {
                case infoTypes.ArrivedAtBase:
                    Console.WriteLine(agentCredentials + " arrived at the base");
                    break;

                case infoTypes.EnteringElevator:
                    Console.WriteLine(agentCredentials + " is entering the elevator");
                    break;

                case infoTypes.LeavingElevator:
                    Console.WriteLine(agentCredentials + " left the elevator");
                    break;

                case infoTypes.CallingElevator:
                    Console.WriteLine(agentCredentials + " is calling the elevator");
                    break;

                case infoTypes.WaitingElevator:
                    Console.WriteLine(agentCredentials + " is waiting for the elevator to be free");
                    break;

                case infoTypes.Working:
                    Console.WriteLine(agentCredentials + $" is working on floor {currentFloorName}.");
                    break;

                case infoTypes.LeavingBase:
                    Console.WriteLine(agentCredentials + " is leaving the base");
                    break;

                case infoTypes.GoingToFloorG:
                    Console.WriteLine(agentCredentials + $" is going from floor {currentFloorName} to floor G");
                    break;

                case infoTypes.GoingFromAtoB:
                    Console.WriteLine(agentCredentials + $" is going from floor {currentFloorName} to floor {elevator.floorNames[floorGoingTo]}");
                    break;

                case infoTypes.Credentials:
                    Console.Write(agentCredentials);
                    break;

                default:
                    Console.WriteLine("Invalid action");
                    break;
            }
        }

        public void CallingElevator()
        {
            DisplayInfo(infoTypes.CallingElevator);

            floorDiff = Math.Abs(elevator.currentFloor - currentFloorNum);
            waitTimeComing = int.Parse(floorDiff + "000");

            Thread.Sleep(waitTimeComing);
        }
        public void DecideBehaviour()
        {
            switch (securityLevel)
            {
                case "Confidential":
                    agentCredentials = $"|{securityLevel}| Agent-{Id}";
                    DisplayInfo(infoTypes.ArrivedAtBase);
                    BehaviourConfidential();
                    break;

                case "Secret":
                    agentCredentials = $"   |{securityLevel}|    Agent-{Id}";
                    DisplayInfo(infoTypes.ArrivedAtBase);
                    BehaviourSecret();
                    break;

                case "TopSecret":
                    agentCredentials = $"  |{securityLevel}|  Agent-{Id}";
                    DisplayInfo(infoTypes.ArrivedAtBase);
                    BehaviourTopSecret();
                    break;
            }
        }

        public void BehaviourConfidential()
        {
            while (true)
            {
                randValue = Random.Shared.Next(10);
                Thread.Sleep(400);
                //Should I do something on this floor?
                if (randValue < 9)
                {
                    //Staying on the same floor
                    DisplayInfo(infoTypes.Working);
                }
                else
                {
                    DisplayInfo(infoTypes.LeavingBase);
                    break;
                }
            }
        }

        public void BehaviourSecret()
        {   
            while (true)
            {
                randValue = Random.Shared.Next(100);
                Thread.Sleep(400);
                //Should I do something on this floor?
                if (randValue < 40)
                {
                    //Staying on the same floor
                    DisplayInfo(infoTypes.Working);
                }
                else if (randValue < 80)
                {
                    //Going to elevator
                    if (!elevator.IsElevatorAvailable())
                    {
                        DisplayInfo(infoTypes.WaitingElevator);
                    }

                    elevator.elevatorStatus.Reset();
                    elevator.UseElevator();

                    if (elevator.currentFloor != currentFloorNum)
                    {
                        CallingElevator();
                    }
                    
                    DisplayInfo(infoTypes.EnteringElevator);

                    for (int i = 0; i < 2; i++)
                    {
                        if (currentFloorNum == i)
                        {
                            continue;
                        }
                        else
                        {
                            floorGoingTo = i;
                            DisplayInfo(infoTypes.GoingFromAtoB);
                            Thread.Sleep(1000);
                            DisplayInfo(infoTypes.LeavingElevator);
                            elevator.currentFloor = i;

                            currentFloorNum = i;
                            currentFloorName = elevator.floorNames[i];
                            break;
                        }
                    }
                    elevator.LeavingElevator();
                    elevator.elevatorStatus.Set();
                }
                else
                {
                    if (currentFloorNum == 0)
                    {
                        DisplayInfo(infoTypes.LeavingBase);
                        break;
                    }
                    else
                    {
                        if (!elevator.IsElevatorAvailable())
                        {
                            DisplayInfo(infoTypes.WaitingElevator);
                        }

                        elevator.elevatorStatus.Reset();
                        elevator.UseElevator();

                        if (elevator.currentFloor != currentFloorNum)
                        {
                            CallingElevator();
                        }

                        DisplayInfo(infoTypes.EnteringElevator);
                        DisplayInfo(infoTypes.GoingToFloorG);
                        Thread.Sleep(1000);

                        DisplayInfo(infoTypes.LeavingElevator);
                        elevator.currentFloor = 0;
                        elevator.LeavingElevator();
                        elevator.elevatorStatus.Set();

                        DisplayInfo(infoTypes.LeavingBase);
                        break;                                                   
                    }                 
                }
            }
        }

        public void BehaviourTopSecret()
        {
            while (true)
            {
                int randValue = Random.Shared.Next(100);
                Thread.Sleep(400);
                //Should I do something on this floor?
                if (randValue < 40)
                {
                    //Staying on the same floor
                    DisplayInfo(infoTypes.Working);
                }
                else if (randValue < 80)
                {
                    //Going to elevator
                    if (!elevator.IsElevatorAvailable())
                    {
                        DisplayInfo(infoTypes.WaitingElevator);
                    }

                    elevator.elevatorStatus.Reset();
                    elevator.UseElevator();

                    if (elevator.currentFloor != currentFloorNum)
                    {
                        CallingElevator();
                    }

                    DisplayInfo(infoTypes.EnteringElevator);

                    List<int> floorOptions = new List<int>();

                    for (int i = 0; i < 4; i++)
                    {
                        if (currentFloorNum == i)
                        {
                            continue;
                        }
                        else
                        {
                            floorOptions.Add(i);
                        }

                    }

                    //Randomly choosing a floor
                    randValue = Random.Shared.Next(3);
                    if (randValue == 0)
                    {
                        floorGoingTo = floorOptions[0];
                    }
                    else if (randValue == 1)
                    {
                        floorGoingTo = floorOptions[1];
                    }
                    else
                    {
                        floorGoingTo = floorOptions[2];
                    }

                    DisplayInfo(infoTypes.GoingFromAtoB);

                    floorDiff = Math.Abs(currentFloorNum - floorGoingTo);
                    waitTimeGoing = int.Parse(floorDiff + "000");
                    Thread.Sleep(waitTimeGoing);
                    DisplayInfo(infoTypes.LeavingElevator);
                    elevator.currentFloor = floorGoingTo;

                    currentFloorNum = floorGoingTo;
                    currentFloorName = elevator.floorNames[floorGoingTo];

                    elevator.LeavingElevator();
                    elevator.elevatorStatus.Set();
                }
                else
                {
                    if (currentFloorNum == 0)
                    {
                        DisplayInfo(infoTypes.LeavingBase);
                        break;
                    }
                    else
                    {
                        if (!elevator.IsElevatorAvailable())
                        {
                            DisplayInfo(infoTypes.WaitingElevator);
                        }

                        elevator.elevatorStatus.Reset();
                        elevator.UseElevator();

                        if (elevator.currentFloor != currentFloorNum)
                        {
                            CallingElevator();
                        }

                        DisplayInfo(infoTypes.EnteringElevator);
                        DisplayInfo(infoTypes.GoingToFloorG);

                        waitTimeGoing = int.Parse(currentFloorNum + "000");
                        Thread.Sleep(waitTimeGoing);
                        DisplayInfo(infoTypes.LeavingElevator);
                        elevator.currentFloor = 0;

                        DisplayInfo(infoTypes.LeavingBase);

                        elevator.LeavingElevator();
                        elevator.elevatorStatus.Set();
                        break;                                                
                    }
                }
            }
        }
    }
}
