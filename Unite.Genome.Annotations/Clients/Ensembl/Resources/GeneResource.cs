﻿using System.Text.Json.Serialization;
using Unite.Data.Entities.Genome.Enums;

namespace Unite.Genome.Annotations.Clients.Ensembl.Resources;

public class GeneResource : LookupResource
{
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
