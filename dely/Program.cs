using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;

namespace dely
{
    class Food
    {
        public string Name { get; set; }
        public double Cooktime_left { get; set; }
        public string Type { get; set; }
        public int Num_places { get; set; }

        public Food(string name, double cook, string t, int num)
        {
            Name = name;
            Cooktime_left = cook;
            Type = t;
            Num_places = num;
        }
    }
    class Order
    {
        public List<Food> Contents { get; set; }

        public Order(List<Food> list)
        {
            Contents = list;
        }

        public void Add_item(Food item)
        {
            Contents.Add(item);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Poisson order_num = new Poisson(9.375);
            Bernoulli corn_bread = new Bernoulli(0.56667);
            Bernoulli orange_cake = new Bernoulli(0.26667);
            Bernoulli choco_cake = new Bernoulli(0.46667);
            Bernoulli three_milks = new Bernoulli(0.43333);
            Bernoulli turkey = new Bernoulli(0.73333);
            Bernoulli pork_leg = new Bernoulli(0.8);

            int corn_total = 0;
            int orange_total = 0;
            int choco_total = 0;
            int milk_total = 0;
            int turkey_total = 0;
            int pork_total = 0;

            List<Order> orders = new List<Order>();

            Console.WriteLine("Order List:\n-------------------------");
            for(int i = 0; i <= order_num.Sample(); i++)
            {
                Console.WriteLine("Order #" + (i + 1) + ":");
                orders.Add(new Order(new List<Food>()));
                if (Convert.ToBoolean(corn_bread.Sample()))
                {
                    Console.WriteLine("* Corn Bread");
                    orders[i].Add_item(new Food("Corn Bread", 50, "salt", 1));
                    corn_total++;
                }
                if (Convert.ToBoolean(orange_cake.Sample()))
                {
                    Console.WriteLine("* Orange Cake");
                    orders[i].Add_item(new Food("Orange Cake", 60, "sweet", 1));
                    orange_total++;
                }
                if (Convert.ToBoolean(choco_cake.Sample()))
                {
                    Console.WriteLine("* Chocolate Cake");
                    orders[i].Add_item(new Food("Chocolate Cake", 60, "sweet", 1));
                    choco_total++;

                }
                if (Convert.ToBoolean(three_milks.Sample()))
                {
                    Console.WriteLine("* Tres Leches");
                    orders[i].Add_item(new Food("Tres Leches", 40, "sweet", 1));
                    milk_total++;
                }
                if (Convert.ToBoolean(turkey.Sample()))
                {
                    Console.WriteLine("* Turkey");
                    orders[i].Add_item(new Food("Turkey", 270, "salt", 2));
                    turkey_total++;
                }
                if (Convert.ToBoolean(pork_leg.Sample()))
                {
                    Console.WriteLine("* Pork Leg");
                    orders[i].Add_item(new Food("Pork Leg", 270, "salt", 2));
                    pork_total++;
                }
                Console.WriteLine();
            }

            Console.WriteLine("\nTotals:\n-------------------------");
            Console.WriteLine("* Corn Bread: " + corn_total);
            Console.WriteLine("* Orange Cake: " + orange_total);
            Console.WriteLine("* Chocolate Cake: " + choco_total);
            Console.WriteLine("* Tres Leches: " + milk_total);
            Console.WriteLine("* Turkey: " + turkey_total);
            Console.WriteLine("* Pork Leg: " + pork_total);
            Console.WriteLine("\n\n3-slot Oven Test:\n-------------------------");

            //3-spot oven test
            Food slot_1 = new Food("", 0, "", 1);
            Food slot_2 = new Food("", 0, "", 1);
            Food slot_3 = new Food("", 0, "", 1);
            double option_a_time = 0;
            int next_step = 0;
            while (turkey_total > 0 || pork_total > 0 || corn_total > 0 || orange_total > 0 || choco_total > 0 || milk_total > 0 || next_step != 0)
            {
                if (slot_1.Cooktime_left == 0 && slot_1.Type != "")
                {
                    Console.WriteLine(slot_1.Name + " comes out of Slot 1");
                    slot_1 = new Food("", 0, "", 1);
                }
                if (slot_2.Cooktime_left == 0 && slot_2.Type != "")
                {
                    Console.WriteLine(slot_2.Name + " comes out of Slot 2");
                    slot_2 = new Food("", 0, "", 1);
                }
                if (slot_3.Cooktime_left == 0 && slot_3.Type != "")
                {
                    Console.WriteLine(slot_3.Name + " comes out of Slot 3");
                    slot_3 = new Food("", 0, "", 1);
                }
                if (turkey_total > 0)
                {
                    if(turkey_total > 0 && slot_1.Type == "" && slot_2.Type == "" && slot_3.Type != "sweet")
                    {
                        turkey_total--;
                        slot_1 = new Food("Turkey A", 270, "salt", 2);
                        Console.WriteLine(slot_1.Name + " goes into Slot 1. " + turkey_total + " turkeys still left to cook...");
                        slot_2 = new Food("Turkey B", 270, "salt", 2);
                        Console.WriteLine(slot_2.Name + " goes into Slot 2. " + turkey_total + " turkeys still left to cook...");
                    }
                    if (turkey_total > 0 && slot_1.Type != "sweet" && slot_2.Type == "" && slot_3.Type == "")
                    {
                        turkey_total--;
                        slot_2 = new Food("Turkey A", 270, "salt", 2);
                        Console.WriteLine(slot_2.Name + " goes into Slot 2. " + turkey_total + " turkeys still left to cook...");
                        slot_3 = new Food("Turkey B", 270, "salt", 2);
                        Console.WriteLine(slot_3.Name + " goes into Slot 3. " + turkey_total + " turkeys still left to cook...");
                    }
                    if (turkey_total > 0 && slot_1.Type == "" && slot_2.Type == "salt" && slot_3.Type == "")
                    {
                        slot_3 = slot_2;
                        slot_2 = new Food("", 0, "", 1);
                        Console.WriteLine(slot_3.Name + " was swapped over from slot 2 to slot 3");
                        turkey_total--;
                        slot_1 = new Food("Turkey A", 270, "salt", 2);
                        Console.WriteLine(slot_1.Name + " goes into Slot 1. " + turkey_total + " turkeys still left to cook...");
                        slot_2 = new Food("Turkey B", 270, "salt", 2);
                        Console.WriteLine(slot_1.Name + " goes into Slot 2. " + turkey_total + " turkeys still left to cook...");
                    }
                }
                if (pork_total > 0)
                {
                    if (pork_total > 0 && slot_1.Type == "" && slot_2.Type == "" && slot_3.Type != "sweet")
                    {
                        pork_total--;
                        slot_1 = new Food("Pork Leg A", 270, "salt", 2);
                        Console.WriteLine(slot_1.Name + " goes into Slot 1. " + pork_total + " pork legs still left to cook...");
                        slot_2 = new Food("Pork Leg B", 270, "salt", 2);
                        Console.WriteLine(slot_2.Name + " goes into Slot 2. " + pork_total + " pork legs still left to cook...");
                    }
                    if (pork_total > 0 && slot_1.Type != "sweet" && slot_2.Type == "" && slot_3.Type == "")
                    {
                        pork_total--;
                        slot_2 = new Food("Pork Leg A", 270, "salt", 2);
                        Console.WriteLine(slot_2.Name + " goes into Slot 2. " + pork_total + " pork legs still left to cook...");
                        slot_3 = new Food("Pork Leg B", 270, "salt", 2);
                        Console.WriteLine(slot_3.Name + " goes into Slot 3. " + pork_total + " pork legs still left to cook...");
                    }
                    if (pork_total > 0 && slot_1.Type == "" && slot_2.Type == "salt" && slot_3.Type == "")
                    {
                        slot_3 = slot_2;
                        slot_2 = new Food("", 0, "", 1);
                        Console.WriteLine(slot_3.Name + " was swapped over from slot 2 to slot 3");
                        pork_total--;
                        slot_1 = new Food("Pork Leg A", 270, "salt", 2);
                        Console.WriteLine(slot_1.Name + " goes into Slot 1. " + pork_total + " pork legs still left to cook...");
                        slot_2 = new Food("Pork Leg B", 270, "salt", 2);
                        Console.WriteLine(slot_1.Name + " goes into Slot 2. " + pork_total + " pork legs still left to cook...");
                    }
                }
                if(corn_total > 0)
                {
                    if(corn_total > 0 && slot_1.Type == "" && slot_2.Type != "sweet" && slot_3.Type != "sweet")
                    {
                        corn_total--;
                        slot_1 = new Food("Corn Bread", 50, "salt", 1);
                        Console.WriteLine("Corn Bread goes into Slot 1. " + corn_total + " corn breads still left to cook...");
                    }
                    if(corn_total > 0 && slot_1.Type != "sweet" && slot_2.Type == "" && slot_3.Type != "sweet")
                    {
                        corn_total--;
                        slot_2 = new Food("Corn Bread", 50, "salt", 1);
                        Console.WriteLine("Corn Bread goes into Slot 2. " + corn_total + " corn breads still left to cook...");
                    }
                    if(corn_total > 0 && slot_1.Type != "sweet" && slot_2.Type != "sweet" && slot_3.Type == "")
                    {
                        corn_total--;
                        slot_3 = new Food("Corn Bread", 50, "salt", 1);
                        Console.WriteLine("Corn Bread goes into Slot 3. " + corn_total + " corn breads still left to cook...");
                    }
                }
                if(orange_total > 0)
                {
                    if (orange_total > 0 && slot_1.Type == "" && slot_2.Type != "salt" && slot_3.Type != "salt")
                    {
                        orange_total--;
                        slot_1 = new Food("Orange Cake", 60, "sweet", 1);
                        Console.WriteLine("Orange Cake goes into Slot 1. " + orange_total + " orange cakes still left to cook...");
                    }
                    if (orange_total > 0 && slot_1.Type != "salt" && slot_2.Type == "" && slot_3.Type != "salt")
                    {
                        orange_total--;
                        slot_2 = new Food("Orange Cake", 60, "sweet", 1);
                        Console.WriteLine("Orange Cake goes into Slot 2. " + orange_total + " orange cakes still left to cook...");
                    }
                    if (orange_total > 0 && slot_1.Type != "salt" && slot_2.Type != "salt" && slot_3.Type == "")
                    {
                        orange_total--;
                        slot_3 = new Food("Orange Cake", 60, "sweet", 1);
                        Console.WriteLine("Orange Cake goes into Slot 3. " + orange_total + " orange cakes still left to cook...");
                    }
                }
                if (choco_total > 0)
                {
                    if (choco_total > 0 && slot_1.Type == "" && slot_2.Type != "salt" && slot_3.Type != "salt")
                    {
                        choco_total--;
                        slot_1 = new Food("Chocolate Cake", 60, "sweet", 1);
                        Console.WriteLine("Chocolate Cake goes into Slot 1. " + choco_total + " chocolate cakes still left to cook...");
                    }
                    if (choco_total > 0 && slot_1.Type != "salt" && slot_2.Type == "" && slot_3.Type != "salt")
                    {
                        choco_total--;
                        slot_2 = new Food("Chocolate Cake", 60, "sweet", 1);
                        Console.WriteLine("Chocolate Cake goes into Slot 2. " + choco_total + " chocolate cakes still left to cook...");
                    }
                    if (choco_total > 0 && slot_1.Type != "salt" && slot_2.Type != "salt" && slot_3.Type == "")
                    {
                        choco_total--;
                        slot_3 = new Food("Chocolate Cake", 60, "sweet", 1);
                        Console.WriteLine("Chocolate Cake goes into Slot 3. " + choco_total + " chocolate cakes still left to cook...");
                    }
                }
                if (milk_total > 0)
                {
                    if (milk_total > 0 && slot_1.Type == "" && slot_2.Type != "salt" && slot_3.Type != "salt")
                    {
                        milk_total--;
                        slot_1 = new Food("Tres Leches", 40, "sweet", 1);
                        Console.WriteLine("Tres Leches goes into Slot 1. " + milk_total + " tres leches still left to cook...");
                    }
                    if (milk_total > 0 && slot_1.Type != "salt" && slot_2.Type == "" && slot_3.Type != "salt")
                    {
                        milk_total--;
                        slot_2 = new Food("Tres Leches", 40, "sweet", 1);
                        Console.WriteLine("Tres Leches goes into Slot 2. " + milk_total + " tres leches still left to cook...");
                    }
                    if (milk_total > 0 && slot_1.Type != "salt" && slot_2.Type != "salt" && slot_3.Type == "")
                    {
                        milk_total--;
                        slot_3 = new Food("Tres Leches", 40, "sweet", 1);
                        Console.WriteLine("Tres Leches goes into Slot 3. " + milk_total + " tres leches still left to cook...");
                    }
                }
                next_step = Convert.ToInt16(slot_1.Cooktime_left);
                if (next_step > slot_2.Cooktime_left && slot_2.Cooktime_left != 0) next_step = Convert.ToInt16(slot_2.Cooktime_left);
                if (next_step > slot_3.Cooktime_left && slot_3.Cooktime_left != 0) next_step = Convert.ToInt16(slot_3.Cooktime_left);
                if (slot_1.Cooktime_left > 0) slot_1.Cooktime_left = slot_1.Cooktime_left - next_step;
                if (slot_2.Cooktime_left > 0) slot_2.Cooktime_left = slot_2.Cooktime_left - next_step;
                if (slot_3.Cooktime_left > 0) slot_3.Cooktime_left = slot_3.Cooktime_left - next_step;
                option_a_time = option_a_time + next_step;
                Console.WriteLine(next_step + " minutes pass...");

            }
            Console.WriteLine("Total Cook-time using 3-slot Oven: " + option_a_time + " Minutes (" + (option_a_time/60) + " Hours or " + (option_a_time/60/24) + " Days)");

            for(int i = 0; i<orders.Count; i++)
            {
                for(int j = 0; j<orders[i].Contents.Count; j++)
                {
                    switch (orders[i].Contents[j].Name)
                    {
                        case "Turkey":
                            turkey_total++;
                            break;
                        case "Pork Leg":
                            pork_total++;
                            break;
                        case "Corn Bread":
                            corn_total++;
                            break;
                        case "Orange Cake":
                            orange_total++;
                            break;
                        case "Chocolate Cake":
                            choco_total++;
                            break;
                        case "Tres Leches":
                            milk_total++;
                            break;
                        default:
                            break;
                    }
                }
            }

            //Secondhand Oven Test
            Console.WriteLine("\n\nSecondhand Oven Test:\n-------------------------");

            Food slot_4 = new Food("", 0, "", 1);

            double option_b_time = 0;

            while (turkey_total > 0 || pork_total > 0 || corn_total > 0 || orange_total > 0 || choco_total > 0 || milk_total > 0 || next_step != 0)
            {
                if (slot_1.Cooktime_left == 0 && slot_1.Type != "")
                {
                    Console.WriteLine(slot_1.Name + " comes out of Regular Oven Slot 1");
                    slot_1 = new Food("", 0, "", 1);
                }
                if (slot_2.Cooktime_left == 0 && slot_2.Type != "")
                {
                    Console.WriteLine(slot_2.Name + " comes out of Regular Oven Slot 2");
                    slot_2 = new Food("", 0, "", 1);
                }
                if (slot_3.Cooktime_left == 0 && slot_3.Type != "")
                {
                    Console.WriteLine(slot_3.Name + " comes out of Secondhand Oven Slot 1");
                    slot_3 = new Food("", 0, "", 1);
                }
                if (slot_4.Cooktime_left == 0 && slot_4.Type != "")
                {
                    Console.WriteLine(slot_4.Name + " comes out of Secondhand Oven Slot 2");
                    slot_4 = new Food("", 0, "", 1);
                }
                if (turkey_total > 0)
                {
                    if (turkey_total > 0 && ((slot_1.Type == "" &&  slot_2.Type != "sweet") || (slot_2.Type == "" && slot_1.Type != "sweet")))
                    {
                        turkey_total--;
                        slot_1 = new Food("Turkey A", 270, "salt", 2);
                        Console.WriteLine(slot_1.Name + " goes into Regular Oven Slot 1. " + turkey_total + " turkeys still left to cook...");
                        slot_2 = new Food("Turkey B", 270, "salt", 2);
                        Console.WriteLine(slot_2.Name + " goes into Regular Oven Slot 2. " + turkey_total + " turkeys still left to cook...");
                    }
                    if (turkey_total > 0 && ((slot_3.Type != "sweet" && slot_4.Type == "") || (slot_4.Type != "sweet" && slot_3.Type == "")))
                    {
                        turkey_total--;
                        slot_3 = new Food("Turkey A", 290, "salt", 2);
                        Console.WriteLine(slot_3.Name + " goes into Secondhand Oven Slot 1. " + turkey_total + " turkeys still left to cook...");
                        slot_4 = new Food("Turkey B", 290, "salt", 2);
                        Console.WriteLine(slot_4.Name + " goes into Secondhand Oven Slot 2. " + turkey_total + " turkeys still left to cook...");
                    }
                }
                if (pork_total > 0)
                {
                    if (pork_total > 0 && ((slot_1.Type == "" && slot_2.Type != "sweet") || (slot_2.Type == "" && slot_1.Type != "sweet")))
                    {
                        pork_total--;
                        slot_1 = new Food("Pork Leg A", 270, "salt", 2);
                        Console.WriteLine(slot_1.Name + " goes into Regular Oven Slot 1. " + pork_total + " pork legs still left to cook...");
                        slot_2 = new Food("Pork Leg B", 270, "salt", 2);
                        Console.WriteLine(slot_2.Name + " goes into Regular Oven Slot 2. " + pork_total + " pork legs still left to cook...");
                    }
                    if (pork_total > 0 && ((slot_3.Type != "sweet" && slot_4.Type == "") || (slot_4.Type != "sweet" && slot_3.Type == "")))
                    {
                        pork_total--;
                        slot_3 = new Food("Pork Leg A", 290, "salt", 2);
                        Console.WriteLine(slot_3.Name + " goes into Secondhand Oven Slot 1. " + pork_total + " pork legs still left to cook...");
                        slot_4 = new Food("Pork Leg B", 290, "salt", 2);
                        Console.WriteLine(slot_4.Name + " goes into Secondhand Oven Slot 2. " + pork_total + " pork legs still left to cook...");
                    }
                }
                if (corn_total > 0)
                {
                    if (corn_total > 0 && slot_1.Type == "" && slot_2.Type != "sweet")
                    {
                        corn_total--;
                        slot_1 = new Food("Corn Bread", 50, "salt", 1);
                        Console.WriteLine("Corn Bread goes into Regular Oven Slot 1. " + corn_total + " corn breads still left to cook...");
                    }
                    if (corn_total > 0 && slot_1.Type != "sweet" && slot_2.Type == "")
                    {
                        corn_total--;
                        slot_2 = new Food("Corn Bread", 50, "salt", 1);
                        Console.WriteLine("Corn Bread goes into Regular Oven Slot 2. " + corn_total + " corn breads still left to cook...");
                    }
                    if (corn_total > 0 && slot_3.Type == "" && slot_4.Type != "sweet")
                    {
                        corn_total--;
                        slot_3 = new Food("Corn Bread", 70, "salt", 1);
                        Console.WriteLine("Corn Bread goes into Secondhand Oven Slot 1. " + corn_total + " corn breads still left to cook...");
                    }
                    if (corn_total > 0 && slot_3.Type != "sweet" && slot_4.Type == "")
                    {
                        corn_total--;
                        slot_3 = new Food("Corn Bread", 70, "salt", 1);
                        Console.WriteLine("Corn Bread goes into Secondhand Oven Slot 2. " + corn_total + " corn breads still left to cook...");
                    }
                }
                if (orange_total > 0)
                {
                    if (orange_total > 0 && slot_1.Type == "" && slot_2.Type != "salt")
                    {
                        orange_total--;
                        slot_1 = new Food("Orange Cake", 60, "sweet", 1);
                        Console.WriteLine("Orange Cake goes into Regular Oven Slot 1. " + orange_total + " orange cakes still left to cook...");
                    }
                    if (orange_total > 0 && slot_1.Type != "salt" && slot_2.Type == "")
                    {
                        orange_total--;
                        slot_2 = new Food("Orange Cake", 60, "sweet", 1);
                        Console.WriteLine("Orange Cake goes into Regular Oven Slot 2. " + orange_total + " orange cakes still left to cook...");
                    }
                    if (orange_total > 0 && slot_3.Type == "" && slot_4.Type != "salt")
                    {
                        orange_total--;
                        slot_3 = new Food("Orange Cake", 80, "sweet", 1);
                        Console.WriteLine("Orange Cake goes into Secondhand Oven Slot 1. " + orange_total + " orange cakes still left to cook...");
                    }
                    if (orange_total > 0 && slot_4.Type == "" && slot_3.Type != "salt")
                    {
                        orange_total--;
                        slot_4 = new Food("Orange Cake", 80, "sweet", 1);
                        Console.WriteLine("Orange Cake goes into Secondhand Oven Slot 2. " + orange_total + " orange cakes still left to cook...");
                    }
                }
                if (choco_total > 0)
                {
                    if (choco_total > 0 && slot_1.Type == "" && slot_2.Type != "salt")
                    {
                        choco_total--;
                        slot_1 = new Food("Chocolate Cake", 60, "sweet", 1);
                        Console.WriteLine("Chocolate Cake goes into Regular Oven Slot 1. " + choco_total + " chocolate cakes still left to cook...");
                    }
                    if (choco_total > 0 && slot_1.Type != "salt" && slot_2.Type == "")
                    {
                        choco_total--;
                        slot_2 = new Food("Chocolate Cake", 60, "sweet", 1);
                        Console.WriteLine("Chocolate Cake goes into Regular Oven Slot 2. " + choco_total + " chocolate cakes still left to cook...");
                    }
                    if (choco_total > 0 && slot_3.Type == "" && slot_4.Type != "salt")
                    {
                        choco_total--;
                        slot_3 = new Food("Chocolate Cake", 80, "sweet", 1);
                        Console.WriteLine("Chocolate Cake goes into Secondhand Oven Slot 1. " + choco_total + " chocolate cakes still left to cook...");
                    }
                    if (choco_total > 0 && slot_4.Type == "" && slot_3.Type != "salt")
                    {
                        choco_total--;
                        slot_4 = new Food("Chocolate Cake", 80, "sweet", 1);
                        Console.WriteLine("Chocolate Cake goes into Secondhand Oven Slot 2. " + choco_total + " chocolate cakes still left to cook...");
                    }
                }
                if (milk_total > 0)
                {
                    if (milk_total > 0 && slot_1.Type == "" && slot_2.Type != "salt")
                    {
                        milk_total--;
                        slot_1 = new Food("Tres Leches", 40, "sweet", 1);
                        Console.WriteLine("Tres Leches goes into Regular Oven Slot 1. " + milk_total + " tres leches still left to cook...");
                    }
                    if (milk_total > 0 && slot_1.Type != "salt" && slot_2.Type == "")
                    {
                        milk_total--;
                        slot_2 = new Food("Tres Leches", 40, "sweet", 1);
                        Console.WriteLine("Tres Leches goes into Regular Oven Slot 2. " + milk_total + " tres leches still left to cook...");
                    }
                    if (milk_total > 0 && slot_3.Type == "" && slot_4.Type != "salt")
                    {
                        milk_total--;
                        slot_3 = new Food("Tres Leches", 60, "sweet", 1);
                        Console.WriteLine("Tres Leches goes into Secondhand Oven Slot 1. " + milk_total + " tres leches still left to cook...");
                    }
                    if (milk_total > 0 && slot_4.Type == "" && slot_3.Type != "salt")
                    {
                        milk_total--;
                        slot_4 = new Food("Tres Leches", 60, "sweet", 1);
                        Console.WriteLine("Tres Leches goes into Secondhand Oven Slot 2. " + milk_total + " tres leches still left to cook...");
                    }
                }
                next_step = Convert.ToInt16(slot_1.Cooktime_left);
                if (next_step > slot_2.Cooktime_left && slot_2.Cooktime_left != 0) next_step = Convert.ToInt16(slot_2.Cooktime_left);
                if (next_step > slot_3.Cooktime_left && slot_3.Cooktime_left != 0) next_step = Convert.ToInt16(slot_3.Cooktime_left);
                if (slot_1.Cooktime_left > 0) slot_1.Cooktime_left = slot_1.Cooktime_left - next_step;
                if (slot_2.Cooktime_left > 0) slot_2.Cooktime_left = slot_2.Cooktime_left - next_step;
                if (slot_3.Cooktime_left > 0) slot_3.Cooktime_left = slot_3.Cooktime_left - next_step;
                if (slot_4.Cooktime_left > 0) slot_4.Cooktime_left = slot_4.Cooktime_left - next_step;
                option_b_time = option_b_time + next_step;
                Console.WriteLine(next_step + " minutes pass...");
            }
            Console.WriteLine("Total Cook-time using 2 2-slot Ovens: " + option_b_time + " Minutes (" + (option_b_time / 60) + " Hours or " + (option_b_time / 60 / 24) + " Days)\n\n");
            Console.WriteLine("Conclusions:\n-------------------------");
            Console.WriteLine("Time using 3-slot Oven:       " + option_a_time + " Minutes");
            Console.WriteLine("Time using a Secondhand Oven: " + option_b_time + " Minutes");
            double difference = Math.Abs(option_a_time - option_b_time);
            Console.WriteLine("Difference:                   " + difference + " Minutes");
            Console.Read();
        }
    }
}