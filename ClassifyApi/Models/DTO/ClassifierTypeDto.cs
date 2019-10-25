using Classify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClassifyApi
{

    public class ClassifierTypeDto : IClassifierType
    {
        public string URI => $"/api/types?ClassifierTypeCode={Code}";
        public string ChangeURI => URI;
        public string DeleteURI => URI;
        public string AddMemberURI => $"/api/classifiers";
        public string GetMembersURI => $"/api/classifiers?ClassifierTypeCode={Code}";
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public List<ClassifierDto> members = new List<ClassifierDto>();

        public ClassifierTypeDto() { }
        public ClassifierTypeDto(ClassifierType classifierType, bool addClassifiers = true, bool addRelationships = true)
        {
            Code = classifierType.Code;
            Name = classifierType.Name;
            Description = classifierType.Description;
            UpdatedBy = classifierType.UpdatedBy;
            UpdatedOn = classifierType.UpdatedOn;
            if (addClassifiers)
            {
                foreach(Classifier classifier in classifierType.GetMembers())
                {
                    members.Add(new ClassifierDto(classifier, addRelationships));
                }
            }
        }
    }
}

