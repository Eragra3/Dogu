using System.Collections.Generic;

namespace Dogu.Backend.Structures.Parameters
{
    public class EnumParameters : TopLevelTypeParameters
    {
        public IDictionary<string, string> Values { get; set; }
    }
}
