namespace PokemonTest;

using System.Dynamic;
using System.Net.Http.Json;
using Pokemon;

[TestClass]
public sealed class CreatureProcessorTest
{
    [TestMethod]
    public async Task Test_getPokemonResponse()
    {
        string validName = "charizard";
        CreatureProcessor processor = new CreatureProcessor();
        HttpResponseMessage validMessage = await processor.getPokemonResponse(validName);
        Assert.IsTrue(validMessage.IsSuccessStatusCode);
        Creature? charizard = await validMessage.Content.ReadFromJsonAsync<Creature>();
        Assert.IsFalse(charizard is null);
        Assert.IsTrue(charizard.Name.Equals(validName));
        Assert.IsTrue(charizard.TypesNeutralTo.Count() == 0);
        Assert.IsTrue(charizard.TypesStrongTo.Count() == 0);
        Assert.IsTrue(charizard.TypesWeakTo.Count() == 0);
        Assert.IsTrue(charizard.damageRelationsMap.Count() == 0);
        Assert.IsNotNull(charizard.Types);
        foreach (PokemonType t in charizard.Types)
        {
            Assert.IsTrue(t.type.Name.Equals("flying") || t.type.Name.Equals("fire"));
        }

        string invalidName = "notAPokemon";
        HttpResponseMessage invalidMessage = await processor.getPokemonResponse(invalidName);
        Assert.IsFalse(invalidMessage.IsSuccessStatusCode);
    }



    [TestMethod]
    public async Task Test_getDamageRelationsResponse()
    {
        string validType = "normal";
        CreatureProcessor processor = new CreatureProcessor();
        HttpResponseMessage? validMessage = await processor.getDamageRelationsResponse(validType);
        Assert.IsTrue(validMessage.IsSuccessStatusCode);
        TypeResponse? validResponse = await validMessage.Content.ReadFromJsonAsync<TypeResponse>();
        Assert.IsFalse(validResponse is null);
        DamageRelations validRelations = validResponse.damage_relations;
        Assert.IsTrue(validRelations.double_damage_from.Count() == 1);
        Assert.IsTrue(validRelations.double_damage_from[0].Name.Equals("fighting"));
        Assert.IsTrue(validRelations.double_damage_from.Count() == 1);
        Assert.IsTrue(validRelations.no_damage_from.Count() == 1);
        Assert.IsTrue(validRelations.no_damage_to.Count() == 1);
        Assert.IsTrue(validRelations.no_damage_from[0].Name.Equals("ghost"));
        Assert.IsTrue(validRelations.no_damage_to[0].Name.Equals("ghost"));
        foreach (PokemonType.Type t in validRelations.half_damage_to)
        {
            Assert.IsTrue(t.Name.Equals("rock") || t.Name.Equals("steel"));
        }
    }

    [TestMethod]
    public async Task Test_createCreatureDualType()
    {

        string charizardName = "charizard";
        CreatureProcessor processor = new CreatureProcessor();
        Creature? charizard = await processor.createCreature(charizardName);
        Assert.IsNotNull(charizard);
        Assert.IsTrue(charizard.Name.Equals(charizardName));
        foreach (PokemonType t in charizard.Types)
        {
            Assert.IsTrue(t.type.Name.Equals("flying") || t.type.Name.Equals("fire"));
        }
        HashSet<String> charizardWeaknesses = ["water", "rock", "dragon", "electric"];
        HashSet<String> charizardStrengths = ["grass", "bug", "fairy", "fighting"];
        HashSet<String> charizardNeutral = ["steel", "ice", "fire", "ground"];

        Assert.IsTrue(charizard.TypesStrongTo.All(t => charizardStrengths.Contains(t.Name)));
        Assert.IsTrue(charizard.TypesWeakTo.All(t => charizardWeaknesses.Contains(t.Name)));
        Assert.IsTrue(charizard.TypesNeutralTo.All(t => charizardNeutral.Contains(t.Name)));



    }

    [TestMethod]
    public async Task Test_createCreatureSingleType()
    {

        string meowthName = "meowth";
        CreatureProcessor processor = new CreatureProcessor();
        Creature? meowth = await processor.createCreature(meowthName);
        Assert.IsNotNull(meowth);
        Assert.IsTrue(meowth.Name.Equals(meowthName));
        Assert.IsTrue(meowth.Types.Count() == 1);
        Assert.IsTrue(meowth.Types[0].type.Name == "normal");

        HashSet<String> meowthWeaknesses = ["rock", "steel", "fighting"];
        HashSet<String> meowthStrengths = [];
        HashSet<String> meowthNeutral = ["ghost"];

        Assert.IsTrue(meowth.TypesStrongTo.Count() == 0);
        Assert.IsTrue(meowth.TypesWeakTo.All(t => meowthWeaknesses.Contains(t.Name)));
        Assert.IsTrue(meowth.TypesNeutralTo.All(t => meowthNeutral.Contains(t.Name)));



    }
}
