using Classify;
using System.Collections.Generic;
using System.Linq;

namespace ClassifyApi
{

    public class ClassifierCollectionDto
    {
        public List<ClassifierDto> Members = new List<ClassifierDto>();

        public ClassifierCollectionDto(ClassifierType classifierType)
        {
            Members.AddRange(classifierType.GetMembers().Select(c => new ClassifierDto(c)));
        }
    }
}
