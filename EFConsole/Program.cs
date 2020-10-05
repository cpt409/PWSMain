using System;
using EFConsole.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace EFConsole
{
    class Program
    {
        static Random rand = new Random();
        static List<Names> LoadNames()
        {
            using (var context = new PWS_NamesContext())
            {
                return context.Names.ToList();
            }
        }

        static void PrintNameList(List<Names> names)
        {
            Console.WriteLine();
            foreach (Names n in names)
            {
                Console.WriteLine($"{n.NameId,4}  {n.Name,-22}{((n.TopBool == false) ? 0 : 1),3} " +
                    $"{n.TopCount,3} {n.Wins,3}");
            }

        }

        static void TestForFalse(List<Names> names)
        {
            var count = names.Count(x => x.TopBool == false);
            if (count == 0)
            {
                names.ForEach(x => x.TopBool = false);
            }
        }

        static List<Names> CreateTrueList(List<Names> names) => names.Where(x => x.TopBool == true).ToList();
        static List<Names> CreateFalseList(List<Names> names) => names.Where(x => x.TopBool == false).ToList();

        static void ShuffleList(List<Names> names)
        {
            for (int i = 0; i < names.Count; i++)
            {
                int k = rand.Next(0, i);
                var val = names[k];
                names[k] = names[i];
                names[i] = val;
            }
        }

        static List<Names> GetTopFour(List<Names> names)
        {
            var temp = names.GetRange(0, 4);
            foreach (Names n in temp)
            {
                n.TopBool = true;
                n.TopCount++;
                UpdateDBTopBool(n.NameId);
            }

            names.RemoveRange(0, 4);
            return temp;
        }

        static private void UpdateDBTopBool(int id)
        {
            using (var context = new PWS_NamesContext())
            {
                var item = context.Names.First(f => f.NameId == id);
                item.TopBool = true;
                item.TopCount++;
                context.SaveChanges();
            }
        }

        static int GetInputInt()
        {
            int value = 0;
            string input = string.Empty;
            do
            {
                Console.Write("Please enter a selection: ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out value));

            return value;
        }

        static void SetTournamentWinner(List<Names> names)
        {
            Console.Clear();
            Console.WriteLine($"\nFull Name List");
            PrintNameList(names);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n\nUse PlayerID of the record that you want to update.\n\n");
            Console.ResetColor();

            bool validId = false;
            int value = 0;
            do
            {
                value = GetInputInt();

                if (value >= 1 && value <= 60)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\n\nUpdating the database...");
                    Console.ResetColor();
                    validId = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\nInvalid PlayerID. Please try again.\n\n");
                    Console.ResetColor();
                    validId = false;
                }

            } while (!validId);


            using (var context = new PWS_NamesContext())
            {
                var item = context.Names.First(f => f.NameId == value);
                item.Wins++;
                context.SaveChanges();

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"\n{item.Name}'s win count has been updated!");
                Thread.Sleep(1000);

                PrintPlayerWins(item);
                Console.ResetColor();
            }

        }

        static void ViewWinners(List<Names> names)
        {

            var winnerList = names.FindAll(x => x.Wins > 0);

            Console.WriteLine($"\n\n{"Name",8} {"Wins",16}");
            foreach (Names n in winnerList)
            {
                Console.WriteLine($"{n.Name,-22} {n.Wins}");
            }

        }

        static void SetTournamentOrder(List<Names> names)
        {
            TestForFalse(names);

            var falseList = CreateFalseList(names);
            var trueList = CreateTrueList(names);

            ShuffleList(falseList);
            var topFourList = GetTopFour(falseList);

            var NewList = falseList.GetRange(0, falseList.Count);
            NewList.AddRange(trueList);

            ShuffleList(NewList);
            topFourList.AddRange(NewList);

            Console.WriteLine();
            PrintNameList(topFourList);
            Console.WriteLine($"count: {topFourList.Count}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n\nPress any key to go back to the Main Menu");
            Console.ResetColor();
            Console.ReadLine();

        }

        static void PrintMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n\nWelcome to the PWS Utility\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Main Menu");
            Console.ResetColor();
            Console.WriteLine($"1) Add Tournament Winner");
            Console.WriteLine($"2) Get Tournament Name List");
            Console.WriteLine($"3) View Winners");
            Console.WriteLine($"8) View All Player Stats");
            Console.WriteLine($"9) Exit App");
            Console.WriteLine();

        }

        static void PrintPlayerWins(Names n)
        {
            int count = n.Wins;
            if (count > 1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n{n.Name} now has {n.Wins} wins!\n\n");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n{n.Name} now has {n.Wins} win!\n\n");
                Console.ResetColor();
            }
        }

        static void Main(string[] args)
        {

            int menuSelection;

            do
            {
                PrintMenu();
                menuSelection = GetInputInt();

                var names = LoadNames();
                switch (menuSelection)
                {
                    case 1:
                        SetTournamentWinner(names);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("\n\nPress enter to go back to the main menu");
                        Console.ResetColor();
                        Console.ReadLine();
                        break;
                    case 2:
                        SetTournamentOrder(names);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("\n\nPress enter to go back to the main menu");
                        Console.ResetColor();
                        Console.ReadLine();
                        break;
                    case 3:
                        ViewWinners(names);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("\n\nPress enter to go back to the main menu");
                        Console.ResetColor();
                        Console.ReadLine();
                        break;
                    case 8:
                        PrintNameList(names);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("\n\nPress enter to go back to the main menu");
                        Console.ResetColor();
                        Console.ReadLine();
                        break;
                    case 9:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n\nThank you for using the PWS Utility");
                        Console.ResetColor();
                        Thread.Sleep(1000);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\n\nInvalid selection.  Please try again\n\n");
                        Console.ResetColor();
                        Thread.Sleep(1000);
                        break;
                }

            } while (menuSelection != 9);



        }

    }
}




//    if (int.TryParse(selection, out value))
//    {
//        switch (value)
//        {
//            case 1:
//            case 2:
//                validSelection = true;
//                break;
//            default:
//                Console.WriteLine("Bad Selection. Please try a different value.");
//                validSelection = false;
//                break;
//        }
//    }
//    else
//    {
//        Console.WriteLine($"{selection} is not readable.  Please try a different value.");
//        validSelection = false;
//    }
//} while (!validSelection);


//return value;