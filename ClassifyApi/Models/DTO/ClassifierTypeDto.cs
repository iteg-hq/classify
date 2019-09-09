using Classify;
using System.Collections.Generic;
using System.Linq;

namespace ClassifyApi
{

    public class ClassifierTypeDto : IClassifierType
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ChangeURI => $"/api/type/{Code}";
        public string DeleteURI => $"/api/type/{Code}";
        public string AddMemberURI => $"/api/type/{Code}/members";
        public string GetMembersURI => $"/api/type/{Code}/members";

        public ClassifierTypeDto() { }
        public ClassifierTypeDto(IClassifierType classifierType)
        {
            Code = classifierType.Code;
            Name = classifierType.Name;
            Description = classifierType.Description;
        }
    }
}
