namespace PokemonTest;

using Pokemon;

[TestClass]
public class Test1
{
    [TestMethod]
    public void Test_getPokemonResponse()
    {
        string validName = "charizard";
        Program.getPokemonResponse(validName);



    }
}



