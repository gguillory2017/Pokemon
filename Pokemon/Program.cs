// See https://aka.ms/new-console-template for more information
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using static System.Net.HttpStatusCode;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;


namespace Pokemon
{
    /*
    Main class for the project
    */
    public class Program
    {
        static string BASE_POKEMON_ADDRESS = "https://pokeapi.co/api/v2/pokemon";
        static string BASE_TYPE_ADDRESS = "https://pokeapi.co/api/v2/type";
        static HttpClient client = new HttpClient();
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
                            break;
                        }
                        /*
                        Try to get a response from the PokeAPI. 
                        The JSON response will have the pokemons name and type(s)
                        */
                        HttpResponseMessage? pokemonResponse;
                        try
                        {
                            pokemonResponse = await getPokemonResponse(pokemonName);
                        }
                        catch (Exception e)
                        {
                            //Log any exceptions and then ask for a new command
                            Console.WriteLine($"Exception occured while getting pokemon {pokemonName}");
                            Console.Write(e.StackTrace);
                            continue;
                        }
                        if (!pokemonResponse.IsSuccessStatusCode)
                        {
                            //Log unsuccessful API calls and ask for a new command
                            Console.WriteLine($"Could not find pokemon with name: {pokemonName}. Try again!");
                            continue;
                        }
                        else
                        {
                            /*
                            The PokeAPI call for the pokemon information was successfull, but now we need to make a 
                            separate API call for the type relationship information
                            */

                            try
                            {
                                /*
                                Serialize the JSON response from the first PokeAPI call into a Creature object.
                                It doesn't contain any type relationship information yet
                                */
                                Creature? pokemon = await pokemonResponse.Content.ReadFromJsonAsync<Creature>();
                                if (pokemon is null)
                                {
                                    Console.WriteLine("Could not derive Creature object from JSON response below:");
                                    Console.Write(pokemonResponse.Content);
                                    continue;

                                }
                                //If the user only wants to check the name of the pokemon, stop here
                                Console.WriteLine($"{pokemon.Name}, I choose you!");
                                if (command == "t")
                                {
                                    //Print the pokemon type(s)
                                    Console.WriteLine($"{pokemon.Name} has type(s) {pokemon.getTypeString()}");
                                    Dictionary<PokemonType.Type, DamageRelations> typeRelations = new Dictionary<PokemonType.Type, DamageRelations>();
                                    //For each pokemon type present, make another PokeAPI call for the type relationship data
                                    foreach (PokemonType pokemonType in pokemon.Types)
                                    {

                                        string typeName = pokemonType.type.Name;

                                        HttpResponseMessage? damageResponse;
                                        try
                                        {
                                            damageResponse = await getDamageRelationsResponse(typeName);
                                            if (!damageResponse.IsSuccessStatusCode)
                                            {
                                                Console.WriteLine($"Could not find damage type: {typeName}. Try again!");
                                                continue;

                                            }
                                            //Serialize the PokeAPI type response into an object 
                                            TypeResponse? response = await damageResponse.Content.ReadFromJsonAsync<TypeResponse>();
                                            if (response is null)
                                            {
                                                Console.WriteLine("Could not derive TypeResponse object from JSON response below:");
                                                Console.Write(pokemonResponse.Content);
                                                continue;
                                            }


                                            typeRelations.Add(pokemonType.type, response.damage_relations);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine($"Exception occured while getting pokemon {typeName}");
                                            Console.Write(e.StackTrace);
                                            continue;
                                        }

                                    }
                                    //Set the damage relations map on the Creature object, print the type summaries
                                    pokemon.setDamageRelationsMap(typeRelations);
                                    pokemon.printTypeSummaries();
                                }
                            }
                            catch (OperationCanceledException e)
                            {
                                Console.Write(e.StackTrace);
                                Console.WriteLine("Operation cancelled while trying to derive creature object. Try again!");


                            }
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
        //Method responsible for making the PokeAPI request for pokemon information
        public static async Task<HttpResponseMessage> getDamageRelationsResponse(string p_Name)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
            string url = $"{BASE_TYPE_ADDRESS}/{p_Name}/";
            return await client.GetAsync(url);
        }
        //Method responsible for making PokeAPI request for type information
        public static async Task<HttpResponseMessage> getPokemonResponse(string p_Name)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
            string url = $"{BASE_POKEMON_ADDRESS}/{p_Name}/";
            return await client.GetAsync(url);
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
