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
        public string RelatedURI => $"/api/relationships?ClassifierTypeCode={TypeCode}&ClassifierCode={Code}";
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string TypeCode { get; set; }
        public List<ClassifierRelationshipDto> Relationships = new List<ClassifierRelationshipDto>();
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
                foreach (var relationship in classifier.GetRelated())
                    Relationships.Add(new ClassifierRelationshipDto(relationship));
            }
        }
    }
}
