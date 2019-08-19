using System;
using System.Collections.Generic;

namespace Classify
{
    public class Classifier : BaseClassifier
    {
        public ClassifierType ClassifierType;

        public ICollection<ClassifierRelationship> Relationships = new List<ClassifierRelationship>();

        public IEnumerable<Classifier> this[string relationshipCode] => GetRelatedByRelationshipCode(relationshipCode);

        public IEnumerable<Classifier> GetRelatedByRelationshipCode(string relationshipCode)
        {
            bool found = false;
            foreach (var r in Relationships)
            {
                if (r.RelationshipTypeCode == relationshipCode)
                {
                    found = true;
                    yield return r.RelatedClassifier;
                }
            }
            if (!found) throw new KeyNotFoundException(relationshipCode);
        }

        public Classifier(ClassifierType classifierType, string code, string description = "")
            : base(code, description)
        {
            ClassifierType = classifierType;
        }
    }
}
