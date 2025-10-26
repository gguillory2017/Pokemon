
using System.Dynamic;

//Stores information about a specific pokemon
public class Creature
{

    //Pokemon name
    public required string Name { get; set; }

    //Pokemon types
    public required List<PokemonType> Types { get; set; }

    //Maps one pokemon type to the other types it has abnormal relationships with 
    public Dictionary<PokemonType.Type, DamageRelations> damageRelationsMap;

    //Types this Creature is weak to (takes double damage, or deals no/half damage)
    public List<PokemonType.Type> TypesWeakTo { get; set; }

    //Types this Creature is strong against (does double damage, or takes no/half damage)
    public List<PokemonType.Type> TypesStrongTo { get; set; }

    //Types this Creature is neutral to (some combination of the condition above)
    public List<PokemonType.Type> TypesNeutralTo { get; set; }

    public String getTypeString()
    {
        return String.Join(", ", Types.Select(p => p.type.Name.ToUpper()));
    }

    public Creature()
    {
        TypesWeakTo = new List<PokemonType.Type>();
        TypesStrongTo = new List<PokemonType.Type>();
        TypesNeutralTo = new List<PokemonType.Type>();
        damageRelationsMap = new Dictionary<PokemonType.Type, DamageRelations>();
    }
    //Set the damage relations map and calculate the strong/weak/neutral types
    public void setDamageRelationsMap(Dictionary<PokemonType.Type, DamageRelations> p_Relations)
    {
        damageRelationsMap = p_Relations;
        HashSet<PokemonType.Type> weakTo = new HashSet<PokemonType.Type>();
        HashSet<PokemonType.Type> strongTo = new HashSet<PokemonType.Type>();

        foreach (var relation in damageRelationsMap)
        {
            weakTo.UnionWith(relation.Value.double_damage_from);
            weakTo.UnionWith(relation.Value.half_damage_to);
            weakTo.UnionWith(relation.Value.no_damage_to);
            strongTo.UnionWith(relation.Value.double_damage_to);
            strongTo.UnionWith(relation.Value.half_damage_from);
            strongTo.UnionWith(relation.Value.no_damage_from);
        }
        HashSet<PokemonType.Type> neutral = new HashSet<PokemonType.Type>(weakTo.Intersect(strongTo));

        weakTo.ExceptWith(neutral);
        strongTo.ExceptWith(neutral);
        TypesWeakTo.AddRange(weakTo);
        TypesStrongTo.AddRange(strongTo);
        TypesNeutralTo.AddRange(neutral);


    }

    public Dictionary<PokemonType.Type, DamageRelations> getDamageRelations()
    {
        return this.damageRelationsMap;
    }

    //Print all the damage relationships for each of this creatures types
    public void printDamageRelations()
    {
        foreach (var relation in damageRelationsMap)
        {
            Console.WriteLine($"Damage relations for type: {relation.Key.Name.ToUpper()}");
            DamageRelations damage_relations = relation.Value;
            Console.WriteLine("DAMAGE DEALT");
            Console.WriteLine("----------------------------");
            Console.WriteLine($"No damage to: {String.Join(", ", damage_relations.no_damage_to.Select(t => t.Name))}");
            Console.WriteLine($"Half damage to: {String.Join(", ", damage_relations.half_damage_to.Select(t => t.Name))}");
            Console.WriteLine($"Double damage to: {String.Join(", ", damage_relations.double_damage_to.Select(t => t.Name))}");
            Console.WriteLine("");
            Console.WriteLine("DAMAGE TAKEN");
            Console.WriteLine("----------------------------");
            Console.WriteLine($"No damage from: {String.Join(", ", damage_relations.no_damage_from.Select(t => t.Name))}");
            Console.WriteLine($"Half damage from: {String.Join(", ", damage_relations.half_damage_from.Select(t => t.Name))}");
            Console.WriteLine($"Double damage from: {String.Join(", ", damage_relations.double_damage_from.Select(t => t.Name))}");
            Console.WriteLine("");
        }

    }

    public void printTypeSummaries()
    {
        Console.WriteLine($"{this.Name} is weak to {String.Join(", ", TypesWeakTo.Select(t => t.Name))}");
        Console.WriteLine($"{this.Name} is strong to {String.Join(", ", TypesStrongTo.Select(t => t.Name))}");
        Console.WriteLine($"{this.Name} is neutral to {String.Join(", ", TypesNeutralTo.Select(t => t.Name))}");

    }

}