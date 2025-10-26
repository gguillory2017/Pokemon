using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
/*
This class wasn't used, but it's my attempt to fix one of the problems I see with the project.
Too much of the application logic related to making the API calls is contained in the "Main" method 
of the Program.cs class. This makes it harder to test. This class aims to fix that problem, but I
didn't have time to re-write the unit test, and so it did not get implemented
*/
public class CreatureProcessor
{

    static string BASE_POKEMON_ADDRESS = "https://pokeapi.co/api/v2/pokemon";

    static string BASE_TYPE_ADDRESS = "https://pokeapi.co/api/v2/type";
    static HttpClient client = new HttpClient();
    public string Name { get; }
    public Creature Pokemon { get; }

    private CreatureProcessor(string p_Name, Creature p_Creature)
    {
        this.Name = p_Name;
        this.Pokemon = p_Creature;
    }
    public static async Task<CreatureProcessor?> BuildCreatureProcessorAsync(string p_Name)
    {

        Creature? creature = await createCreature(p_Name);
        if (creature is null)
        {
            return null;
        }
        return new CreatureProcessor(p_Name, creature);
    }

    private static async Task<Creature?> createCreature(string p_Name)
    {
        Creature? pokemon = null;
        Dictionary<PokemonType.Type, DamageRelations> typeRelations = new Dictionary<PokemonType.Type, DamageRelations>();
        try
        {
            HttpResponseMessage pokemonResponse = await getPokemonResponse(p_Name);
            if (!pokemonResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"Could not find pokemon with name: {p_Name}. Try again!");
                return pokemon;
            }

            pokemon = await pokemonResponse.Content.ReadFromJsonAsync<Creature>();

        }
        catch (OperationCanceledException e)
        {
            Console.Write(e.StackTrace);
            Console.WriteLine("Operation cancelled while trying to derive creature object. Try again!");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occured while getting pokemon {p_Name}");
            Console.Write(e.StackTrace);

        }
        if (pokemon is null)
        {
            Console.WriteLine("Could not derive Creature object from JSON response.");
            return pokemon;
        }
        try
        {
            foreach (PokemonType pokemonType in pokemon.Types)
            {
                string typeName = pokemonType.type.Name;
                HttpResponseMessage damageResponse = await getDamageRelationsResponse(typeName);
                if (!damageResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Could not find damage type: {typeName}. Try again!");
                    return null;

                }
                TypeResponse? response = await damageResponse.Content.ReadFromJsonAsync<TypeResponse>();
                if (response is null)
                {
                    Console.WriteLine("Could not derive TypeResponse object from JSON response below:");
                    Console.Write(damageResponse.Content);
                    return null;

                }
                typeRelations.Add(pokemonType.type, response.damage_relations);


            }

        }
        catch (OperationCanceledException e)
        {
            Console.Write(e.StackTrace);
            Console.WriteLine("Operation cancelled while trying to derive creature object. Try again!");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occured while getting pokemon {p_Name}");
            Console.Write(e.StackTrace);

        }
        pokemon.damageRelationsMap = typeRelations;
        return pokemon;
    }

    private static async Task<HttpResponseMessage> getPokemonResponse(string p_Name)
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
        string url = $"{BASE_POKEMON_ADDRESS}/{p_Name}/";
        return await client.GetAsync(url);
    }

    private static async Task<HttpResponseMessage> getDamageRelationsResponse(string p_Name)
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
        string url = $"{BASE_TYPE_ADDRESS}/{p_Name}/";
        return await client.GetAsync(url);
    }



}