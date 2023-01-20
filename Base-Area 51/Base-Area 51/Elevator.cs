using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Area_51
{
    internal class Elevator
    {
        public string[] floorNames = new [] {"G", "S", "T1", "T2" };

        public int currentFloor;
        public ManualResetEvent elevatorStatus;
        Semaphore semaphore;


        public Elevator()
        { 
            semaphore = new Semaphore(1, 1);
            elevatorStatus = new ManualResetEvent(true);
        }
        
        public bool IsElevatorAvailable()
        {
            if (elevatorStatus.WaitOne(0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UseElevator()
        {
            semaphore.WaitOne();
        }
        
        public void LeavingElevator()
        {
            semaphore.Release();
        }
    }
}
