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

        public ClassifierTypeDto() { }
        public ClassifierTypeDto(ClassifierType classifierType)
        {
            Code = classifierType.Code;
            Name = classifierType.Name;
            Description = classifierType.Description;
            UpdatedBy = classifierType.UpdatedBy;
            UpdatedOn = classifierType.UpdatedOn;
        }
    }
}

