using Classify;
using System.Collections.Generic;

namespace ClassifyApi
{
    public class ClassifierDto : IClassifier
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TypeCode { get; set; }
        public string DeleteURI => $"/api/type/{TypeCode}/member/{Code}";
        public string GetRelatedURI => $"/api/type/{TypeCode}/member/{Code}/related";
        
        public ClassifierDto() { }
        public ClassifierDto(Classifier classifier)
        {
            TypeCode = classifier.TypeCode;
            Code = classifier.Code;
            Name = classifier.Name;
            Description = classifier.Description;
        }
    }
}
