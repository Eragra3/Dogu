using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Dogu.Backend
{
    public class AssemblyReader : IAssemblyReader, IDisposable
    {
        private readonly Assembly _assembly;

        public AssemblyReader(string assemblyPath)
        {
            _assembly = Assembly.LoadFrom(assemblyPath);
        }

        public IList<Type> GetExportedTypes() => _assembly.ExportedTypes.ToList();

        /// <inheritdoc cref="System.IDisposable.Dispose"/>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        /// <inheritdoc cref="System.IDisposable.Dispose"/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
