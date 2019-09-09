using System;
using System.Collections.Generic;
using System.Linq;

namespace Classify
{
    public interface IClassifier
    {
        string TypeCode { get; }
        string Code { get; }
        string Name { get; }
        string Description { get; }
    }
}
