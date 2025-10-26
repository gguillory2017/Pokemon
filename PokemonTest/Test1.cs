namespace PokemonTest;

using System.Net.Http.Json;
using Pokemon;
[TestClass]
public sealed class Test1
{
    [TestMethod]
    public async Task Test_getPokemonResponse()
    {
        string validName = "charizard";
        HttpResponseMessage validMessage = await Program.getPokemonResponse(validName);
        Assert.IsTrue(validMessage.IsSuccessStatusCode);
        Creature? charizard = await validMessage.Content.ReadFromJsonAsync<Creature>();
        Assert.IsFalse(charizard is null);
        Assert.IsTrue(charizard.Name.Equals(validName));
        foreach (PokemonType t in charizard.Types)
        {
            Assert.IsTrue(t.type.Name.Equals("flying") || t.type.Name.Equals("fire"));
        }

        string invalidName = "notAPokemon";
        HttpResponseMessage invalidMessage = await Program.getPokemonResponse(invalidName);
        Assert.IsFalse(invalidMessage.IsSuccessStatusCode);

    }

    [TestMethod]
    public async Task Test_getDamageRelationsResponse()
    {
        string validType = "normal";
        HttpResponseMessage? validMessage = await Program.getDamageRelationsResponse(validType);
        Assert.IsTrue(validMessage.IsSuccessStatusCode);
        TypeResponse? validResponse = await validMessage.Content.ReadFromJsonAsync<TypeResponse>();
        Assert.IsFalse(validResponse is null);
        DamageRelations validRelations = validResponse.damage_relations;
        Assert.IsTrue(validRelations.double_damage_from.Count() == 1);
        Assert.IsTrue(validRelations.double_damage_from[0].Name.Equals("fighting"));
        Assert.IsTrue(validRelations.double_damage_from.Count() == 1);
        Assert.IsTrue(validRelations.no_damage_from.Count() == 1);
        Assert.IsTrue(validRelations.no_damage_to.Count() == 1);
        foreach (PokemonType.Type t in validRelations.half_damage_to)
        {
            Assert.IsTrue(t.Name.Equals("rock") || t.Name.Equals("steel"));
        }





    }
}
