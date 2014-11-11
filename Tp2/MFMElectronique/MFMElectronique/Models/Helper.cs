using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFMElectronique.Models
{
    public static class Helper
    {
        public static T FirstOrNull<T>(this IEnumerable<T> list, Func<T, bool> predicate) where T : class
        {   
            if (list != null)
                foreach (T item in list)
                    if (predicate(item))
                        return item;
            return null;
        }
        public static T FirstOrNull<T>(this IEnumerable<T> list) where T : class
        {
            if (list != null)
                foreach (T item in list)
                    return item;
            return null;
        }
        
        public static bool Contains<T>(this IEnumerable<T> list, Func<T, bool> predicate) where T : class
        {
            if (list != null)
                foreach (T item in list)
                    if (predicate(item))
                        return true;
            return false;
        }
        
    }
}