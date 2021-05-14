﻿using System.Collections.Generic;
using System.Linq;
using Unite.Data.Entities.Mutations;
using Unite.Data.Services;
using Unite.Mutations.Feed.Mutations.Data.Models;

namespace Unite.Mutations.Feed.Mutations.Data.Repositories
{
    internal class MutationRepository
    {
        private readonly UniteDbContext _dbContext;


        public MutationRepository(UniteDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public Mutation FindOrCreate(MutationModel mutationModel)
        {
            return Find(mutationModel) ?? Create(mutationModel);
        }

        public Mutation Find(MutationModel mutationModel)
        {
            var mutation = _dbContext.Mutations.FirstOrDefault(mutation =>
                mutation.Code == mutationModel.Code
            );

            return mutation;
        }

        public Mutation Create(MutationModel mutationModel)
        {
            var mutation = Convert(mutationModel);

            _dbContext.Mutations.Add(mutation);
            _dbContext.SaveChanges();

            return mutation;
        }

        public IEnumerable<Mutation> CreateMissing(IEnumerable<MutationModel> mutationModels)
        {
            var mutationsToAdd = new List<Mutation>();

            foreach (var mutationModel in mutationModels)
            {
                var mutation = Find(mutationModel);

                if (mutation == null)
                {
                    mutation = Convert(mutationModel);

                    mutationsToAdd.Add(mutation);
                }
            }

            if (mutationsToAdd.Any())
            {
                _dbContext.Mutations.AddRange(mutationsToAdd);
                _dbContext.SaveChanges();
            }

            return mutationsToAdd.ToArray();
        }


        private Mutation Convert(MutationModel mutationModel)
        {
            var mutation = new Mutation();

            mutation.Code = mutationModel.Code;
            mutation.ChromosomeId = mutationModel.Chromosome;
            mutation.SequenceTypeId = mutationModel.SequenceType;
            mutation.Start = mutationModel.Start;
            mutation.End = mutationModel.End;
            mutation.ReferenceBase = mutationModel.ReferenceBase;
            mutation.AlternateBase = mutationModel.AlternateBase;
            mutation.TypeId = mutationModel.Type;

            return mutation;
        }
    }
}
