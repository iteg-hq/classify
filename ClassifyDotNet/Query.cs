using Classify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Classify
{
    public interface IClassifierTypeQuery
    {
        bool Filter(ClassifierType t);
    }

    public class ClassifierTypeQuery
    {
        public string ClassifierTypeCode { get; set; }
        public bool Filter(ClassifierType t) => t.Code == ClassifierTypeCode;
    }

    public class ClassifierQuery
    {
        public string ClassifierTypeCode { get; set; }
        public string ClassifierCode { get; set; }
        public bool Filter(Classifier c) => c.Code == ClassifierCode && c.TypeCode == ClassifierTypeCode;
    }

    public class ClassifierRelationshipQuery
    {
        public string ClassifierTypeCode { get; set; }
        public string ClassifierCode { get; set; }
        public string RelationshipTypeCode { get; set; }
        public string RelatedClassifierTypeCode { get; set; }
        public string RelatedClassifierCode { get; set; }

        public bool Filter(ClassifierRelationship r) => (
            r.Classifier.Code == ClassifierCode && 
            r.Classifier.TypeCode == ClassifierTypeCode &&
            r.RelationshipTypeCode == RelationshipTypeCode &&
            r.RelatedClassifier.TypeCode == RelatedClassifierTypeCode &&
            r.RelatedClassifier.Code == RelatedClassifierCode
            );
    }
}