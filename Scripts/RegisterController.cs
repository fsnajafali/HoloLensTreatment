using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RegisterController : MonoBehaviour
{
    //Our registers will take things off the shelves at 
    //(product in queue) * ( rand(0.0f, 1.0f) aka DequeueQuantity ) / RegisterRate

    public int RegisterRate;

    internal int itemsAtCheckout;
    Queue<Food> BaseQueue = new Queue<Food>();
    // Each ArrayList represents a customer
    Queue<ArrayList> Reg1Queue = new Queue<ArrayList>();
    Queue<ArrayList> Reg2Queue = new Queue<ArrayList>();
    Queue<ArrayList> Reg3Queue = new Queue<ArrayList>();
    Simulation sim;

    public Text RegisterText;
    
    int total_food_sold_per_hour;
    decimal total_money_made_per_hour;

    int register1;
    int register2;
    int register3;

    float DequeueQuantity;

    //mayhaps this?
    int[] register = new int[3];

    int PercentageOfItemsTakenFromQueue;
    int checkoutsPossible;
    int checkoutsPerformed;
    public float registerUtilization;


    // Start is called before the first frame update
    void Start()
    {
       sim = GameObject.Find("Simulation").GetComponent<Simulation>();
        checkoutsPossible = 0;
        checkoutsPerformed = 0;
    }
    
    public void bringFoodToRegisters(Food food)
    {
        BaseQueue.Enqueue(food);
        itemsAtCheckout++;
        /*
        // Semi-random distribution of items
        int numRegisters = sim.NumberEmployeesRegisters;
        int[] lineLengths = new int[3];
        for (int i = 0; i < sim.NumberEmployeesRegisters; i++)
        {

        }
        */
    }

    // Base queue is a queue of foods, distribute collections of foods as 'customers' to 
    // the three registers
    // called from PullItemsOfShelves() in Simulation
    public void distributeBaseQueue(int numRegisters)
    {
        // mimicks Poisson Distrubition
        int[] customerItemCountDistribution = new int[19] {3, 6, 9, 12, 13, 14, 15, 15, 16, 16,
                                                            16, 17, 17, 18, 20, 22, 25, 28, 32};
        // max is exclusive here because we passed in integers (generate int 0-18)
        int distIndex, numItems;

        // store number of items in each register
        int[] registerItemCount = new int[3] { 0, 0, 0 };
        
        // Tally current item count in line 1
        if(Reg1Queue.Count != 0)
            foreach (ArrayList al in Reg1Queue)
                registerItemCount[0] += al.Count;

        // Tally current item count in line 2
        if (Reg2Queue.Count != 0)
            foreach (ArrayList al in Reg2Queue)
                registerItemCount[1] += al.Count;

        // Tally current item count in line 3
        if (Reg3Queue.Count != 0)
            foreach (ArrayList al in Reg3Queue)
                registerItemCount[2] += al.Count;


        // distribute the base queue (contains foods) until empty
        while (BaseQueue.Count > 0)
        {
            // max is exclusive here because we passed in integers (generate int 0-18)
            distIndex = UnityEngine.Random.Range(0, 19);
            numItems = customerItemCountDistribution[distIndex];

            if (numItems > BaseQueue.Count)
                numItems = BaseQueue.Count;

            // create 'customer'
            ArrayList customer = new ArrayList();
            for (int i = 0; i < numItems; i++)
                customer.Add(BaseQueue.Dequeue());

            // Determines which register to send the customer to
            if (sim.NumberEmployeesRegisters == 3)
            {
                // determine register to send it to, follows: ~40% of items going to reg1
                //                                            ~35% of items going to reg2
                //                                            ~25 % of items going to reg3
                if ((registerItemCount[0] / .4) <= (registerItemCount[1] + registerItemCount[2]) / .6) // send to reg1
                {
                    Reg1Queue.Enqueue(customer);
                    registerItemCount[0] += customer.Count;
                }
                else if (registerItemCount[1] / .35 <= registerItemCount[2] / .25) // send to reg2
                {
                    Reg2Queue.Enqueue(customer);
                    registerItemCount[1] += customer.Count;
                }
                else // send to reg3
                {
                    Reg3Queue.Enqueue(customer);
                    registerItemCount[2] += customer.Count;
                }
            }
            else if(sim.NumberEmployeesRegisters == 2)
            {
                // determine register to send it to, follows: ~60% of items going to reg1
                //                                            ~40% of items going to reg2
                if (registerItemCount[0] / .6 <= registerItemCount[1] / .4)
                {
                    Reg1Queue.Enqueue(customer);
                    registerItemCount[0] += customer.Count;
                }
                else
                {
                    Reg2Queue.Enqueue(customer);
                    registerItemCount[1] += customer.Count;
                }
            }
            else // only one register to send it to (100% to reg1)
            {
                Reg1Queue.Enqueue(customer);
                registerItemCount[0] += customer.Count;
            }
        }
    }

    // Called from MakeAction when a register is closed
    // Must distribute customers in the closed register's queue
    public void closeRegister()
    {
        int numRegisters = sim.NumberEmployeesRegisters;

        // if register 2 was closed, put all of register 2's customers in
        // register 1 line
        if (numRegisters == 1)
        { 
            int size = Reg2Queue.Count;
            for (int i = 0; i < size; i++)
                Reg1Queue.Enqueue(Reg2Queue.Dequeue());
        }
        else // distribute register 3 customers to 1 and 2 (50/50 disbursement)
        {
            int flip = 0; 

            while (Reg3Queue.Count > 0)
            {
                if (flip == 0)
                    Reg1Queue.Enqueue(Reg3Queue.Dequeue());
                else
                    Reg2Queue.Enqueue(Reg3Queue.Dequeue());

                flip = (flip == 0) ? 1 : 0;
            }

        }

    }

    // gets called every hour to push foods through the open registers
    // number of foods processed = number of register * rate
    //we need a variable register rate...
    //the rate can be a random number calculated at checkout time
    public void CalculateItemsToBeTakenFromQueue() 
    {
        DequeueQuantity = UnityEngine.Random.Range(0.0f, 1.0f);
            
    }

    public void checkoutFoods()
    {
        //so for each employee on the register (sim.NumberEmployeesRegisters), I will calculate a new value (Dequeue Quantity) and apply it to the formula
        //Then I'll capture the rate
        total_food_sold_per_hour = 0;
        total_money_made_per_hour = 0;
        //calculateRegisterRate();

        
        
        // for each open register
        for (int i = 0; i < sim.NumberEmployeesRegisters; i++)
        {
            Queue<ArrayList> currentRegisterQueue;

            if (i == 0)
            {
                currentRegisterQueue = Reg1Queue;
            }
            else if (i == 1)
            {
                currentRegisterQueue = Reg2Queue;
            }
            else
            {
                currentRegisterQueue = Reg3Queue;
            }



            // if this line is empty, we have no customers to check out
            if (currentRegisterQueue.Count == 0)
                continue;


            // only push through a certain rate of foods per hour (i defined it as 8)
            for (int j = 0; j < RegisterRate; j++)
            {
                checkoutsPossible++;
                ArrayList al = currentRegisterQueue.Peek();

                // if this customer is checked out, dequeue it and go to next one
                if (al.Count == 0)
                {
                    currentRegisterQueue.Dequeue();
                    //Debug.Log("Checked out a customer from line " + (i + 1));

                    if (currentRegisterQueue.Count != 0)
                        al = currentRegisterQueue.Peek();
                    else
                        break;
                }


                // REGISTER UTILIZATION: HOW MANY TIMES DO WE REACH HERE / RegisterRate
                checkoutsPerformed++;

                // ArrayLists are weird in c#!! remove methods don't return object removed
                Food food = (Food) al[0];
                al.RemoveAt(0);
                Simulation.Cash += (float) food.getUnitPriceCustomer(); 

                //keep track of a food item sold
                total_food_sold_per_hour += 1;
                total_money_made_per_hour += ((Food) food).getUnitPriceCustomer();

                itemsAtCheckout--;

                  
                // TODO: make cool animation of foods walking out the door and send me a video of it lmao
                // food.cyaBitches();
                
            }

            registerUtilization = ((float)checkoutsPerformed) / checkoutsPossible;
        }


        PostDayManager.total_food_sold += total_food_sold_per_hour;
        
        RegisterText.text = "Register Text:\n" + "Total Food Sold:" + PostDayManager.total_food_sold + "\nRegister Rate: " 
            + RegisterRate + "\nFood sold per hour: " + total_food_sold_per_hour + "\nMoney made per hour: " + total_money_made_per_hour;
        
        sim.updateCheckoutItems();
        // print("queue size after = " + queue.Count);
    }

    // gets called at the end of the day, whatever is waiting at the registers gets returned to shelves
    // this is used to simulate people getting angry for waiting too long idk
    // need to have a reason not to have too litte registers open
    public void returnFoodsToShelves()
    {
        // EMPTY EACH QUEUE
        while(BaseQueue.Count > 0)
        {
            Food food = BaseQueue.Dequeue();
            itemsAtCheckout--;
            // add the item back to shelf
            Simulation.totalOnShelves[food.getItemDescription()] += 1;
        }
        while (Reg1Queue.Count > 0)
        {
            ArrayList al = Reg1Queue.Dequeue();

            foreach (Food foodItem in al)
            {
                itemsAtCheckout--;
                // add the item back to shelf
                Simulation.totalOnShelves[foodItem.getItemDescription()] += 1;
            }
        }
        while (Reg2Queue.Count > 0)
        {
            ArrayList al = Reg2Queue.Dequeue();

            foreach (Food foodItem in al)
            {
                itemsAtCheckout--;
                // add the item back to shelf
                Simulation.totalOnShelves[foodItem.getItemDescription()] += 1;
            }
        }
        while (Reg3Queue.Count > 0)
        {
            ArrayList al = Reg3Queue.Dequeue();

            foreach (Food foodItem in al)
            {
                itemsAtCheckout--;
                // add the item back to shelf
                Simulation.totalOnShelves[foodItem.getItemDescription()] += 1;
            }
        }
    }
}
