﻿using System.Text.Json.Serialization;
using Unite.Data.Entities.Mutations.Enums;

namespace Unite.Mutations.Annotations.Clients.Ensembl.Resources
{
    public class GeneResource : IEnsemblResource
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("display_name")]
        public string Symbol { get; set; }

        [JsonPropertyName("biotype")]
        public string Biotype { get; set; }

        [JsonPropertyName("seq_region_name")]
        [JsonConverter(typeof(JsonStringEnumMemberConverter))]
        public Chromosome Chromosome { get; set; }

        [JsonPropertyName("start")]
        public int Start { get; set; }

        [JsonPropertyName("end")]
        public int End { get; set; }

        [JsonPropertyName("strand")]
        public int? Strand { get; set; }
    }
}
