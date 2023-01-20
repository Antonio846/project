using Base_Area_51;
using System.Collections.Concurrent;
using System.Reflection;
using System.Security.Cryptography;

class Programm
{
    static void Main()
    {
        Elevator elevator= new Elevator();
        Random randomNum = new Random();
        int valueSecLevel;
        string secLevel;
        int numOfAgents;
        string? userInput;

        Console.WriteLine("Enter the number of agents");
        userInput = Console.ReadLine();
        while (true)
        {
            if (int.TryParse(userInput, out numOfAgents))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid number! Try again");
                userInput = Console.ReadLine();
            }
                       
        }
        for (int i = 0; i < numOfAgents; i++)
        {
            valueSecLevel = randomNum.Next(30);

            if (valueSecLevel < 10)
            {
                secLevel = "Confidential";
            }
            else if(valueSecLevel < 20)
            {
                secLevel = "Secret";
            }
            else
            {
                secLevel = "TopSecret"; 
            }

            Agent agent = new Agent(secLevel, elevator);
            agent.Id = i;

            var thread = new Thread(agent.DecideBehaviour);
            thread.Start();
        }


        
    }

}
