﻿using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Data.Entities.Genome.Analysis.Dna.Sm;
using Unite.Essentials.Extensions;
using Unite.Genome.Annotations.Clients.Ensembl.Configuration.Options;
using Unite.Genome.Annotations.Services.Models.Dna;

namespace Unite.Genome.Annotations.Services.Vep;


public class SmsAnnotationService
{
    private readonly IDbContextFactory<DomainDbContext> _dbContextFactory;
    private readonly AnnotationsDataLoader _dataLoader;


    public SmsAnnotationService(
        IDbContextFactory<DomainDbContext> dbContextFactory,
        IEnsemblDataOptions ensemblOptions,
        IEnsemblVepOptions ensemblVepOptions
        )
    {
        _dbContextFactory = dbContextFactory;
        _dataLoader = new AnnotationsDataLoader(ensemblOptions, ensemblVepOptions, dbContextFactory);
    }


    public EffectsDataModel[] Annotate(int[] variantIds)
    {
        var variants = LoadVariants(variantIds);

        var codes = variants.Select(GetVepVariantCode).ToArray();

        var annotations = _dataLoader.LoadData(codes).Result;

        return annotations;
    }


    private Variant[] LoadVariants(int[] variantIds)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return dbContext.Set<Variant>()
            .AsNoTracking()
            .Where(entity => variantIds.Contains(entity.Id))
            .OrderBy(entity => entity.ChromosomeId)
            .ThenBy(entity => entity.Start)
            .ToArray();
    }

    private string GetVepVariantCode(Variant variant)
    {
        var id = variant.Id.ToString();
        var chromosome = variant.ChromosomeId.ToDefinitionString();
        var start = variant.Ref != null ? variant.Start : variant.End + 1;
        var end = variant.End;
        var referenceBase = variant.Ref ?? "-";
        var alternateBase = variant.Alt ?? "-";

        return $"{chromosome} {start} {end} {referenceBase}/{alternateBase} + {id}";
    }
}
