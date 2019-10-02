using System;
using System.Collections.Generic;
using System.Linq;

namespace Classify
{
    public class Classifier : IClassifier
    {
        private readonly DAL dal;
        public string TypeCode { get; private set; }
        public Classifier(DAL dal, string typeCode, string code)
        {
            this.dal = dal;
            this.TypeCode = typeCode;
            Code = code;
        }

        public override string ToString()
        {
            return $"Classifier {ClassifierType.Code}:{Code}";
        }

        public ClassifierType ClassifierType
        {
            get => dal.GetClassifierType(TypeCode);
        }

        public string Code { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }


        public IEnumerable<ClassifierRelationship> GetRelated()
        {
            return dal.GetClassifierRelationships(TypeCode, Code);
        }
        public IEnumerable<Classifier> GetRelated(string relationshipTypeCode)
        {
            return GetRelated().Where(r => r.RelationshipTypeCode == relationshipTypeCode).Select(r => r.RelatedClassifier);
        }

        public IEnumerable<Classifier> this[string relationshipTypeCode] => GetRelated(relationshipTypeCode);

        public Classifier AddRelated(string relationshipType, IClassifier relatedClassifier, string description = "", double weight = 100.0)
        {
            dal.SaveClassifierRelationship(
                ClassifierType.Code, 
                Code, 
                relationshipType, 
                relatedClassifier.TypeCode, 
                relatedClassifier.Code, 
                description, weight);
            return this;
        }

        public IEnumerable<string> GetRelationshipTypeCodes()
        {
            return GetRelated().Select(r => r.RelationshipTypeCode).Distinct();
        }
    }
}
