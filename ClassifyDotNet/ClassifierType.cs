using System;
using System.Collections.Generic;
using System.Linq;

namespace Classify
{
    public class ClassifierType : BaseClassifier
    {
        public ICollection<Classifier> Members = new List<Classifier>();

        public Classifier this[string code]
        {
            get
            {
                return Members.First(t => t.Code == code);
            }
        }
        public ClassifierType(string code, string description = "")
            : base(code, description)
        {
        }

        public Classifier AddMember(string code, string description = "")
        {
            if(Members.Any(c => c.Code == code))
            {
                throw new Exception(code);
            }
            var classifier = new Classifier(this, code, description);
            Members.Add(classifier);
            return classifier;
        }
    }
}