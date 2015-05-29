using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarryLibrary.Helpers
{
    public static class Enumeration
    {
        public static T GetEnum<T>(int value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }
    }
}
