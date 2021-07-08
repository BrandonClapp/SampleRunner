using System.Collections.Generic;
using System.Reflection;

namespace ConsoleApp1.Core
{
    public class Sample
    {
        public string ClassName { get; set; }
        public object Instance { get; set; }
        public IEnumerable<MethodInfo> Methods { get; set; }
    }
}