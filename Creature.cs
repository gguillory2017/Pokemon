
public class Creature
{


    public string name { get; set; }
    public List<PokemonType> types { get; set; }

    public Dictionary<PokemonType.Type, DamageRelations> damageRelationsMap { get; set; }

    public String getTypeString()
    {
        return String.Join(", ", types.Select(t => t.type.name.ToUpper()));
    }

    public void printDamageRelations()
    {
        foreach (var relation in damageRelationsMap)
        {
            Console.WriteLine($"Damage relations for type: {relation.Key.name.ToUpper()}");
            DamageRelations damage_relations = relation.Value;
            Console.WriteLine("DAMAGE DEALT");
            Console.WriteLine("----------------------------");
            Console.WriteLine($"No damage to: {String.Join(", ", damage_relations.no_damage_to.Select(t => t.name))}");
            Console.WriteLine($"Half damage to: {String.Join(", ", damage_relations.half_damage_to.Select(t => t.name))}");
            Console.WriteLine($"Double damage to: {String.Join(", ", damage_relations.double_damage_to.Select(t => t.name))}");
            Console.WriteLine("");
            Console.WriteLine("DAMAGE TAKEN");
            Console.WriteLine("----------------------------");
            Console.WriteLine($"No damage from: {String.Join(", ", damage_relations.no_damage_from.Select(t => t.name))}");
            Console.WriteLine($"Half damage from: {String.Join(", ", damage_relations.half_damage_from.Select(t => t.name))}");
            Console.WriteLine($"Double damage from: {String.Join(", ", damage_relations.double_damage_from.Select(t => t.name))}");
            Console.WriteLine("");
        }

    }

    public void printTypeSummaries()
    {
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
       
        Console.WriteLine($"{this.name} is weak to {String.Join(", ", weakTo.Select(t => t.name))}");
        Console.WriteLine($"{this.name} is strong to {String.Join(", ", strongTo.Select(t => t.name))}");
        Console.WriteLine($"{this.name} is neutral to {String.Join(", ", neutral.Select(t => t.name))}");

    }

}