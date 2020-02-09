using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Dogu.Assembly
{
    public class AssemblyReader : IDisposable
    {
        private readonly MetadataLoadContext _metadataLoadContext;
        private readonly System.Reflection.Assembly _assembly;

        public AssemblyReader(string assemblyPath)
        {
            string assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);
            string coreLibPath = AppDomain.CurrentDomain
                .GetAssemblies()
                .First(x => x.GetName().Name == "System.Private.CoreLib")
                .Location;


            var resolver = new PathAssemblyResolver(new[] {assemblyPath, coreLibPath});
            _metadataLoadContext = new MetadataLoadContext(resolver, "System.Private.CoreLib");
            _assembly = _metadataLoadContext.LoadFromAssemblyName(assemblyName);
        }

        public IEnumerable<TypeInfo> DefinedTypes => _assembly.DefinedTypes;

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
