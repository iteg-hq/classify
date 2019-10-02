using Classify;
using System;
using System.Collections.Generic;
using System.Web;

namespace ClassifyApi
{
    public class ClassifierDto : IClassifier
    {
        public string URI => $"/api/classifiers?ClassifierTypeCode={TypeCode}&ClassifierCode={Code}";
        public string DeleteURI => URI;
        public string ChangeURI => URI;
        public string GetRelatedURI => $"/api/relationships?ClassifierTypeCode={TypeCode}&ClassifierCode={Code}";
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string TypeCode { get; set; }
        public List<ClassifierCollectionDto> Relationships { get; set; }
        public ClassifierDto() { }
        public ClassifierDto(Classifier classifier, bool addRelationships = true)
        {
            TypeCode = classifier.TypeCode;
            Code = classifier.Code;
            Name = classifier.Name;
            Description = classifier.Description;
            UpdatedBy = classifier.UpdatedBy;
            UpdatedOn = classifier.UpdatedOn;
            if (addRelationships)
            {
                Relationships = new List<ClassifierCollectionDto>();
                var map = new Dictionary<string, List<ClassifierRelationshipDto>>();
                foreach (var r in classifier.GetRelated())
                {
                    if (r.IsInbound) continue;
                    if (!map.ContainsKey(r.RelationshipTypeCode))
                    {
                        map[r.RelationshipTypeCode] = new List<ClassifierRelationshipDto>();
                    }
                    map[r.RelationshipTypeCode].Add(new ClassifierRelationshipDto(r));
                }

                foreach (var relationshipTypeCode in map.Keys)
                {
                    Relationships.Add(new ClassifierCollectionDto
                    {
                        RelationshipTypeCode = relationshipTypeCode,
                        Relationships = map[relationshipTypeCode]
                    });
                }
            }
        }
    }

    public class ClassifierCollectionDto
    {
        public List<ClassifierRelationshipDto> Relationships { get; set; }
        public string RelationshipTypeCode { get; set; }
    }

}
