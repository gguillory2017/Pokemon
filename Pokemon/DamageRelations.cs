//Describes the special damage relations for a given type. Used with JSON serialization
public class DamageRelations
{
    public required List<PokemonType.Type> no_damage_to { get; set; }
    public required List<PokemonType.Type> half_damage_to { get; set; }

    public required List<PokemonType.Type> double_damage_to { get; set; }

    public required List<PokemonType.Type> no_damage_from { get; set; }
    public required List<PokemonType.Type> half_damage_from { get; set; }
    public required List<PokemonType.Type> double_damage_from { get; set; }


}