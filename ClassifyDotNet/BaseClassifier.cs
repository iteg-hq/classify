using System;
using System.Collections.Generic;

namespace Classify
{
    public class BaseClassifier
    {
        private string code;
        private string description;

        public bool IsDirty;
        public string Code
        {
            get { return code; }
            set
            {
                if (value != code)
                {
                    IsDirty = true;
                    code = value;
                }
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                {
                    IsDirty = true;
                    description = value;
                }
            }
        }

        public override string ToString() => Code;

        public BaseClassifier(string code, string description = "")
        {
            Code = code;
            Description = description;
            IsDirty = false;
        }
    }
}
