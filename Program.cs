// See https://aka.ms/new-console-template for more information
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;


string BASE_POKEMON_ADDRESS = "https://pokeapi.co/api/v2/pokemon";
string BASE_TYPE_ADDRESS = "https://pokeapi.co/api/v2/type";
HttpClient client = new HttpClient();
HashSet<string> COMMANDS = new HashSet<string>() { "t", "c", "q" };


Console.WriteLine("Welcome to the pokemon app!");
string command = "c";

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
            string? pokemonName = Console.ReadLine();
            if (pokemonName is null)
            Creature pokemon = await getPokemon(pokemonName);
            if (pokemon is null)
            {
                Console.WriteLine($"No Pokemon with name: {pokemonName}");
            }
            else
            {

                Console.WriteLine($"{pokemon.name}, I choose you!");
                Console.WriteLine($"{pokemon.Name}, I choose you!");
                if (command == "t")
                {
                    Console.WriteLine($"{pokemon.name} has type(s) {pokemon.getTypeString()}");
                    Console.WriteLine($"{pokemon.Name} has type(s) {pokemon.getTypeString()}");
                    Dictionary<PokemonType.Type, DamageRelations> typeRelations = new Dictionary<PokemonType.Type, DamageRelations>();
                    foreach (PokemonType pokemonType in pokemon.types)
                    foreach (PokemonType pokemonType in pokemon.Types)
                    {
                        TypeResponse response = await getDamageRelations(pokemonType.type.name);
                        typeRelations.Add(pokemonType.type, response.damage_relations);
                    }
                    pokemon.damageRelationsMap = typeRelations;
                    pokemon.printTypeSummaries();
                    pokemon.printDamageRelations();
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
async Task<TypeResponse> getDamageRelations(string p_Name)
{
    TypeResponse result = null;
    TypeResponse? result = null;
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
    string url = $"{BASE_TYPE_ADDRESS}/{p_Name}/";
    HttpResponseMessage response = await client.GetAsync(url);
    if (response.IsSuccessStatusCode)
    {

        result = await response.Content.ReadFromJsonAsync<TypeResponse>();

    }
    return result;
}

async Task<Creature> getPokemon(string p_Name)
{
    Creature result = null;
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
    string url = $"{BASE_POKEMON_ADDRESS}/{p_Name}/";
    HttpResponseMessage response = await client.GetAsync(url);
    if (response.IsSuccessStatusCode)
    {

        result = await response.Content.ReadFromJsonAsync<Creature>();

    }
    return result;

}



string getCommand()
{
    Console.WriteLine("Choose a command: ");
    Console.WriteLine("c: check pokemon name");
    Console.WriteLine("t: check pokemon damage types");
    Console.WriteLine("q: quit");
    Console.Write("Choice: ");
    string? command = Console.ReadLine();
    {
        return "";
    }
    return command;
}

