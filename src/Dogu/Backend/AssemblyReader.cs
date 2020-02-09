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
        private readonly MetadataLoadContext _metadataLoadContext;
        private readonly Assembly _assembly;

        private readonly string[] _coreAssemblies = new[] {"System.Runtime", "System.Private.CoreLib"};

        public AssemblyReader(string assemblyPath)
        {
            string assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);
            string[] coreLibPaths = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => _coreAssemblies.Contains(x.GetName().Name))
                .Select(x => x.Location)
                .ToArray();

            var resolver = new PathAssemblyResolver(new[] {assemblyPath}.Concat(coreLibPaths));
            _metadataLoadContext = new MetadataLoadContext(resolver);
            _assembly = _metadataLoadContext.LoadFromAssemblyName(assemblyName);
        }

        public IEnumerable<Type> ExportedTypes => _assembly.ExportedTypes;

        /// <inheritdoc cref="System.IDisposable.Dispose"/>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _metadataLoadContext.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
