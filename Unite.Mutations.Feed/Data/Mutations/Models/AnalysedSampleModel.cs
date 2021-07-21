﻿using System.Collections.Generic;

namespace Unite.Mutations.Feed.Data.Mutations.Models
{
    public class AnalysedSampleModel
    {
        public SampleModel AnalysedSample { get; set; }
        public SampleModel MatchedSample { get; set; }

        public IEnumerable<MutationModel> Mutations { get; set; }
    }
}
