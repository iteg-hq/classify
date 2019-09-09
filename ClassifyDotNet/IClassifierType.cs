using System;
using System.Collections.Generic;
using System.Linq;

namespace Classify
{
    public interface IClassifierType
    {
        string Code { get; }
        string Name { get; }
        string Description { get; }
    }
}