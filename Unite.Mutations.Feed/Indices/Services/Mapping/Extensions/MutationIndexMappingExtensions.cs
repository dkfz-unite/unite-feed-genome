﻿using System.Collections.Generic;
using System.Linq;
using Unite.Data.Entities.Extensions;
using Unite.Data.Entities.Mutations;
using Unite.Indices.Entities.Basic.Mutations;

namespace Unite.Mutations.Feed.Indices.Services.Mapping.Extensions
{
    public static class MutationIndexMappingExtensions
    {
        public static void MapFrom(this MutationIndex index, in Mutation mutation)
        {
            if (mutation == null)
            {
                return;
            }

            index.Id = mutation.Id;
            index.Code = mutation.Code;
            index.Chromosome = mutation.ChromosomeId.ToDefinitionString();
            index.SequenceType = mutation.SequenceTypeId.ToDefinitionString();
            index.Start = mutation.Start;
            index.End = mutation.End;
            index.Type = mutation.TypeId.ToDefinitionString();
            index.Ref = mutation.ReferenceBase;
            index.Alt = mutation.AlternateBase;

            index.AffectedTranscripts = CreateFrom(mutation.AffectedTranscripts);
        }

        private static AffectedTranscriptIndex[] CreateFrom(in IEnumerable<AffectedTranscript> affectedTranscripts)
        {
            if (affectedTranscripts == null)
            {
                return null;
            }

            var indices = affectedTranscripts.Select(affectedTranscript =>
            {
                var index = new AffectedTranscriptIndex();

                index.Id = affectedTranscript.Id;

                index.AminoAcidChange = GetAminoAcidChangeCode(
                    affectedTranscript.ProteinStart,
                    affectedTranscript.ProteinEnd,
                    affectedTranscript.AminoAcidChange
                );

                index.CodonChange = GetCodonCHangeCode(
                    affectedTranscript.CDSStart,
                    affectedTranscript.CDSEnd,
                    affectedTranscript.CodonChange
                );

                index.Gene = CreateFrom(affectedTranscript.Gene);
                index.Transcript = CreateFrom(affectedTranscript.Transcript);
                index.Consequences = CreateFrom(affectedTranscript.Consequences);

                return index;

            }).ToArray();

            return indices;
        }

        private static GeneIndex CreateFrom(in Gene gene)
        {
            if (gene == null)
            {
                return null;
            }

            var index = new GeneIndex();

            index.Id = gene.Id;
            index.Symbol = gene.Symbol;
            index.Strand = gene.Strand;
            index.Biotype = gene.Biotype?.Value;

            index.EnsemblId = gene.Info.EnsemblId;

            return index;
        }

        private static TranscriptIndex CreateFrom(in Transcript transcript)
        {
            if (transcript == null)
            {
                return null;
            }

            var index = new TranscriptIndex();

            index.Id = transcript.Id;
            index.Symbol = transcript.Symbol;
            index.Strand = transcript.Strand;
            index.Biotype = transcript.Biotype?.Value;

            index.EnsemblId = transcript.Info?.EnsemblId;

            return index;
        }

        private static ConsequenceIndex[] CreateFrom(in IEnumerable<AffectedTranscriptConsequence> consequences)
        {
            if (consequences == null)
            {
                return null;
            }

            var indices = consequences.Select(affectedTranscriptConsequence =>
            {
                var index = new ConsequenceIndex();

                index.Type = affectedTranscriptConsequence.Consequence.TypeId.ToDefinitionString();
                index.Impact = affectedTranscriptConsequence.Consequence.ImpactId.ToDefinitionString();
                index.Severity = affectedTranscriptConsequence.Consequence.Severity;

                return index;

            }).ToArray();

            return indices;
        }


        private static string GetAminoAcidChangeCode(int? start, int? end, string change)
        {
            if(start != null && end != null && !string.IsNullOrWhiteSpace(change))
            {
                var position = start == end ? $"{start}" : $"{start}-{end}";
                var referenceBase = ParseChange(change).ReferenceBase;
                var alternateBase = ParseChange(change).AlternateBase;
                
                return $"{referenceBase}{position}{alternateBase}";
            }
            else
            {
                return null;
            }
        }

        private static string GetCodonCHangeCode(int? start, int? end, string change)
        {
            if (start != null && end != null && !string.IsNullOrWhiteSpace(change))
            {
                var position = start == end ? $"{start}" : $"{start}-{end}";
                var referenceBase = ParseChange(change).ReferenceBase?.First(allele => char.IsUpper(allele));
                var alternateBase = ParseChange(change).AlternateBase?.First(allele => char.IsUpper(allele));

                return $"{referenceBase}{position}{alternateBase}";
            }
            else
            {
                return null;
            }
        }

        private static (string ReferenceBase, string AlternateBase) ParseChange(string change)
        {
            var blocks = change.Split("/");

            var referenceBase = blocks.Length > 0 ? blocks[0] : null;
            var alternateBase = blocks.Length > 1 ? blocks[1] : null;

            return (referenceBase, alternateBase);
        }
    }
}
