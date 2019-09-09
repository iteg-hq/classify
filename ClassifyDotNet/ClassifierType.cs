using System;
using System.Collections.Generic;
using System.Linq;

namespace Classify
{
    public class ClassifierType : IClassifierType
    {
        private readonly DAL dal;

        public ClassifierType(DAL dal, string code)
        {
            this.dal = dal;
            Code = code;
        }
        public override string ToString()
        {
            return $"ClassifierType {Code}";
        }

        public string Code { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Classifier AddMember(IClassifier classifier)
            => AddMember(classifier.Code, classifier.Name, classifier.Description);

        public Classifier AddMember(string code, string name, string description)
        {
            return dal.SaveClassifier(Code, code, name, description);
        }

        public ICollection<Classifier> GetMembers()
            => dal.GetClassifiers(Code).ToList();

        public Classifier GetMember(string classifierCode)
        {
            return GetMembers().Single(c => c.Code == classifierCode);
        }

        public Classifier this[string code] => GetMember(code);

        public bool DeleteMember(string classifierCode)
        {
            return dal.TryDeleteClassifier(Code, classifierCode);
        }
    }
}