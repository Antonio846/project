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
        Semaphore semaphore;

        public Elevator()
        { 
            semaphore = new Semaphore(1, 1);
        }
        
        public bool isElevatorAvailable(Agent agent)
        {
            if (semaphore.WaitOne())
            {               
                return true;             
            }
            else
            {
                return false;
            }
                
        }
        
        public void LeavingElevator()
        {
            semaphore.Release();
        }
    }
}
