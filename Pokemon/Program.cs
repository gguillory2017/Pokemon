// See https://aka.ms/new-console-template for more information

namespace Pokemon
{
    /*
    Main class for the project
    */
    public class Program
    {
        public static async Task Main(string[] args)
        {

            HashSet<string> COMMANDS = new HashSet<string>() { "t", "c", "q" };
            Console.WriteLine("Welcome to the pokemon app!");
            string command = "c";
            // While loop keeps the application running until q command is selected
            while (command != "q")
            {
                command = getCommand();
                if (command is null || !COMMANDS.Contains(command))
                {
                    Console.WriteLine("Couldn't recognize command! Try again.");
                }
                else
                {
                    Console.WriteLine($"Handling command: {command}");
                    if (command == "c" || command == "t")
                    {
                        Console.Write("Enter pokemon name: ");
                        // Name of pokemon is accepted from user and tested
                        string? pokemonName = Console.ReadLine();
                        if (pokemonName is null)
                        {
                            Console.WriteLine("Pokemon name was null. Try again!");
                            continue;
                        }
                        // 
                        // processor = await processor.BuildCreatureProcessorAsync(pokemonName);
                        CreatureProcessor processor = new CreatureProcessor();
                        Boolean processed = await processor.BuildCreatureProcessorAsync(pokemonName);
                        if (!processed)
                        {
                            Console.WriteLine($"Error occured while processing pokemon {pokemonName}. Try again!");
                            continue;
                        }
                        Creature? pokemon = processor.Pokemon;
                        if (pokemon is null)
                        {
                            Console.WriteLine($"Error occured while processing pokemon {pokemonName}. Try again!");
                            continue;
                        }
                        Console.WriteLine($"{pokemonName}, I choose you!");
                        if (command == "t")
                        {
                            pokemon.printTypeSummaries();
                        }
                    }
                    else if (command == "q")
                    {
                        Console.WriteLine("Bye!");
                        Environment.Exit(0);
                    }
                }
            }
        }

        //Method responsible for handling user commands
        static string getCommand()
        {
            Console.WriteLine("Choose a command: ");
            Console.WriteLine("c: check pokemon name");
            Console.WriteLine("t: check pokemon damage types");
            Console.WriteLine("q: quit");
            Console.Write("Choice: ");
            string? command = Console.ReadLine();
            if (command is null)
            {
                return "";
            }
            return command;
        }
    }
}
