using System;
using System.Collections.Generic;
using System.Reflection;

namespace Dogu.Backend
{
    public interface IAssemblyReader
    {
        IEnumerable<Type> ExportedTypes { get; }
    }
}
