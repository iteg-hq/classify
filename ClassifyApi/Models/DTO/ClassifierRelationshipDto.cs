using Classify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClassifyApi
{
    public class ClassifierRelationshipDto
    {
        public ClassifierDto Classifier { get; set; }
        public ClassifierDto RelatedClassifier { get; set; }

        public string RelationshipTypeCode;
        public string Description;
        public double Weight = 100.0;
        public string UpdatedBy;
        public DateTime UpdatedOn;
        public bool IsInbound;

        public string URI => $"/api/relationships?" +
            $"ClassifierTypeCode={Classifier.TypeCode}&" +
            $"ClassifierCode={Classifier.Code}&" +
            $"RelationshipTypeCode={RelationshipTypeCode}&" +
            $"RelatedClassifierTypeCode={RelatedClassifier.TypeCode}&" +
            $"RelatedClassifierCode={RelatedClassifier.Code}";

        public string DeleteURI => URI;

        public string UpdateURI => URI;

        public ClassifierRelationshipDto() { }
        public ClassifierRelationshipDto(ClassifierRelationship relationship)
        {
            Classifier = new ClassifierDto(relationship.Classifier, false);
            RelatedClassifier = new ClassifierDto(relationship.RelatedClassifier, false);
            RelationshipTypeCode = relationship.RelationshipTypeCode;
            Description = relationship.Description;
            Weight = relationship.Weight;
            UpdatedBy = relationship.UpdatedBy;
            UpdatedOn = relationship.UpdatedOn;
            IsInbound = relationship.IsInbound;
        }
    }
}