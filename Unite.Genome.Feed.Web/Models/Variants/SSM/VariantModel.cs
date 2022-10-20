﻿using Unite.Data.Entities.Genome.Enums;
using Unite.Data.Utilities.Mutations;

namespace Unite.Genome.Feed.Web.Models.Variants.SSM;

public class VariantModel
{
    private Chromosome? _chromosome;
    private string _position;
    private string _ref;
    private string _alt;


    /// <summary>
    /// Mutation chromosome
    /// </summary>
    public Chromosome? Chromosome { get => _chromosome; set => _chromosome = value; }

    /// <summary>
    /// Mutation position in chromosome (Number "10110" or range "10110-10115" string)
    /// </summary>
    public string Position { get => _position?.Trim(); set => _position = value; }

    /// <summary>
    /// Reference base
    /// </summary>
    public string Ref { get => _ref?.Trim(); set => _ref = value; }

    /// <summary>
    /// Alternate base
    /// </summary>
    public string Alt { get => _alt?.Trim(); set => _alt = value; }


    public string GetCode()
    {
        return HGVsCodeGenerator.Generate(Chromosome.Value, Position, Ref, Alt);
    }
}
