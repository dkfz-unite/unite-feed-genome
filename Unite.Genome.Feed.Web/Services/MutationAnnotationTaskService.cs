﻿using Unite.Data.Entities.Genome.Variants.SSM;
using Unite.Data.Services;

namespace Unite.Genome.Feed.Web.Services;

public class MutationAnnotationTaskService : VariantAnnotationTaskService<Variant>
{
    public MutationAnnotationTaskService(DomainDbContext dbContext) : base(dbContext)
    {
    }
}
