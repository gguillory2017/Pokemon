public class TypeResponse
{
    public required DamageRelations damage_relations { get; set; }

    public void printDamageRelations()
    {
        Console.WriteLine("DAMAGE DEALT");
        Console.WriteLine("----------------------------");
        Console.WriteLine($"No damage to: {String.Join(",", damage_relations.no_damage_to.Select(t => t.Name))}");
        Console.WriteLine($"Half damage to: {String.Join(",", damage_relations.half_damage_to.Select(t => t.Name))}");
        Console.WriteLine($"Double damage to: {String.Join(",", damage_relations.double_damage_to.Select(t => t.Name))}");
        Console.WriteLine("----------------------------\n");
        Console.WriteLine("DAMAGE TAKEN");
        Console.WriteLine("----------------------------");
        Console.WriteLine($"No damage from: {String.Join(",", damage_relations.no_damage_from.Select(t => t.Name))}");
        Console.WriteLine($"Half damage from: {String.Join(",", damage_relations.half_damage_from.Select(t => t.Name))}");
        Console.WriteLine($"Double damage from: {String.Join(",", damage_relations.double_damage_from.Select(t => t.Name))}");
        Console.WriteLine("----------------------------\n");
    }

}