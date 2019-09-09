using System;
using System.Collections.Generic;

namespace Classify
{
    public class BaseClassifier
    {
        public string Code;
        public string Name;
        public string Description;

        public BaseClassifier(string code, string name, string description = "")
        {
            Code = code;
            Name = name;
            Description = description;
        }
    }
}
