using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Globalegrow.Toolkit
{
    public static class XAttributeExtensions
    {
        public static bool Exists(this IEnumerable<XAttribute> attributes, string attributeName)
        {
            return attributes.Any(x => x.Name == attributeName);
        }
    }
}