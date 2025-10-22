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
        public string name { get; set; }
        public string url { get; set; }

    }

    public Type type { get; set; }
    public int slot { get; set; }

}