using Classify;
using System.Collections.Generic;
using System.Linq;

namespace ClassifyApi
{

    public class ClassifierTypeCollectionDto
    {
        public ICollection<ClassifierTypeDto> Members = new List<ClassifierTypeDto>();

        public string AddURI = "/api/types";
        public string GetURI = "/api/types";

        public ClassifierTypeCollectionDto(IEnumerable<IClassifierType> classifierTypes)
        {
            foreach (var ct in classifierTypes)
            {
                Members.Add(new ClassifierTypeDto(ct));
            }
        }
    }
}
