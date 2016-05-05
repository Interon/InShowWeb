using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for _Extensions
/// </summary>

    public static class ObjectExtensions
    {
        public static string StringToNullSafe(this object obj)
        {
            return obj != null ? obj.ToString() : String.Empty;
        }
   
}
