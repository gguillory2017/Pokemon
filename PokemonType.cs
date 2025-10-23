using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;



public class PokemonType
{

    
    public record Type
    {
        public required string Name { get; set; }
        public required string Url { get; set; }

    }

    public required Type type { get; set; }
    public int Slot { get; set; }

}