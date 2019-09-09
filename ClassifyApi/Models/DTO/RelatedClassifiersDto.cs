using Classify;
using System.Collections.Generic;

namespace ClassifyApi
{
    public class RelatedClassifiersDto
    {
        public string ClassifierTypeCode;
        public string ClassifierCode;
        public string AddMemberURI { get => $"/api/type/{ClassifierTypeCode}/member({ClassifierCode}/related"; }

        public List<RelatedClassifierSetDto> RelatedClassifierSets = new List<RelatedClassifierSetDto>();
    }

    public class RelatedClassifierSetDto
    {
        public string RelationshipTypeCode;
        public List<RelatedClassifierDto> RelatedClassifiers = new List<RelatedClassifierDto>();
    }

    public class RelatedClassifierDto
    {
        public string TypeCode;
        public string Code;
        public string Description;
        public double Weight;
    }

}