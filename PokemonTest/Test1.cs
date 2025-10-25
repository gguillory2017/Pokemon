namespace PokemonTest;

using Pokemon;
[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void TestMethod1()
    {
        Program.getPokemonResponse("charizard");
    }
}
