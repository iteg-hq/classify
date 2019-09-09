using System;
using System.Collections.Generic;
using System.Linq;

namespace Classify
{
    public class Classifier : IClassifier
    {
        private readonly DAL dal;
        private readonly string typeCode;
        public Classifier(DAL dal, string typeCode, string code)
        {
            this.dal = dal;
            this.typeCode = typeCode;
            Code = code;
        }

        public override string ToString()
        {
            return $"Classifier {ClassifierType.Code}:{Code}";
        }

        public ClassifierType ClassifierType
        {
            get => dal.GetClassifierType(typeCode);
        }

        public string Code { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string TypeCode => ClassifierType.Code;

        public IEnumerable<ClassifierRelationship> GetRelated()
        {
            return dal.GetClassifierRelationships(typeCode, Code);
        }
        public IEnumerable<Classifier> GetRelated(string relationshipTypeCode)
        {
            return GetRelated().Where(r => r.RelationshipTypeCode == relationshipTypeCode).Select(r => r.RelatedClassifier);
        }

        public Classifier AddRelated(string relationshipType, Classifier relatedClassifier, string description="", double weight=100.0)
        {
            dal.SaveClassifierRelationship(ClassifierType.Code, Code, relationshipType, relatedClassifier.TypeCode, relatedClassifier.Code, description, weight);
            return this;
        }

        public IEnumerable<string> GetRelationshipTypeCodes()
        {
            return GetRelated().Select(r => r.RelationshipTypeCode).Distinct();
        }
    }
}
