using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
/*
Attempts to separate logic for making API calls from logic used for processing commands
*/
public class CreatureProcessor
{

    string BASE_POKEMON_ADDRESS = "https://pokeapi.co/api/v2/pokemon";

    string BASE_TYPE_ADDRESS = "https://pokeapi.co/api/v2/type";
    HttpClient client = new HttpClient();
    public string? Name { get; set; }
    public Creature? Pokemon { get; set; }

    public Boolean initialized = false;

    public CreatureProcessor()
    {

    }

    public async Task<Boolean> BuildCreatureProcessorAsync(string p_Name)
    {
        Creature? creature = await createCreature(p_Name);
        if (creature is null)
        {
            return false;
        }
        this.Name = p_Name;
        this.Pokemon = creature;

        //return new CreatureProcessor(p_Name, creature);
        return true;
    }

    public async Task<Creature?> createCreature(string p_Name)
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
        pokemon.setDamageRelationsMap(typeRelations);
        return pokemon;
    }
    //Method responsible for making the PokeAPI request for pokemon information
    public async Task<HttpResponseMessage> getPokemonResponse(string p_Name)
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
        string url = $"{BASE_POKEMON_ADDRESS}/{p_Name}/";
        return await client.GetAsync(url);
    }

    //Method responsible for making PokeAPI request for type information
    public async Task<HttpResponseMessage> getDamageRelationsResponse(string p_Name)
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
        string url = $"{BASE_TYPE_ADDRESS}/{p_Name}/";
        return await client.GetAsync(url);
    }



}