using System;
using System.IO;

namespace BrainfuCS
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            const bool parseInputAsNumber = true;           //Allows the user to enter a number instead of an ASCII character at a , prompt
            const bool outputAsNumber = false;              //Outputs a number instead of an ASCII character
            const int arrayCapacity = int.MaxValue / 64;    //Capacity of the internal array

            //Read a program from a file if it is passed as an argument, otherwise run a hardcoded application.
            string program;
            if (args.Length > 0)
                program = File.ReadAllText(args[0]);
            else
                program = "++++++++[>++++[>++>+++>+++>+<<<<-]>+>+>->>+[<]<-]>>.>---.+++++++..+++.>>.<-.<.+++.------.--------.>>+.>++.";

            //Goes through the program and assigns each [ its corresponding ]
            Stack<int> openBrackets = new Stack<int>();
            Dictionary<int, int> bracketPairs = new Dictionary<int, int>();
            for (int i = 0; i < program.Length; i++)
            {
                if (program[i] == '[')
                    openBrackets.Push(i);
                else if (program[i] == ']')
                    bracketPairs.Add(openBrackets.Pop(), i);
            }

            byte[] array = new byte[arrayCapacity];
            int pointer = 0;

            for (int i = 0; i < program.Length; i++)
            {
                switch (program[i])
                {
                    case '>':
                        pointer++;
                        break;
                    case '<':
                        pointer--;
                        break;

                    case '+':
                        array[pointer % arrayCapacity]++;
                        break;
                    case '-':
                        array[pointer % arrayCapacity]--;
                        break;

                    case '.':
                        if (outputAsNumber)
                            Console.Write(array[pointer % arrayCapacity]);
                        else
                            Console.Write((char)array[pointer % arrayCapacity]);
                        break;
                    case ',':
                        if (parseInputAsNumber)
                        {
                            Console.Write("\nInput (as number): ");
                            array[pointer % arrayCapacity] = byte.Parse(Console.ReadLine());
                        }
                        else
                        {
                            Console.Write("\nInput: ");
                            array[pointer % arrayCapacity] = (byte)Console.ReadKey().KeyChar;
                            Console.WriteLine();
                        }
                        break;

                    case '[':
                        if (array[pointer % arrayCapacity] != 0)
                            break;

                        i = bracketPairs[i];
                        break;

                    case ']':
                        if (array[pointer % arrayCapacity] == 0)
                            break;

                        i = (bracketPairs.First((x) => { return x.Value == i; })).Key;
                        break;

                    default:
                        break;
                }
            }

            Console.WriteLine("\nProgram end. Press any key to exit.");
            Console.ReadKey();
        }
    }
}