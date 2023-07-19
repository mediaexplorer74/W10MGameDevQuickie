using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Win8.Core.Tasks.Attributes;

namespace Win8.Core.Helpers
{
    /// <summary>
    /// Helper used for gathering properties for logging purposes.
    /// </summary>
    public static class DebugHelper
    {
        const string pre = "    ";

        /// <summary>
        /// Log properties of all objects in input collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">List of input objects.</param>
        /// <param name="sb">Input string builder, where to write the data.</param>
        /// <param name="sensitiveProperties">List of properties to ommit from the log.</param>
        public static void LoadProperties<T>(IEnumerable<T> items, StringBuilder sb, IList<string> sensitiveProperties, string prefix)
        {
            foreach (T item in items)
            {
                PropertyInfo[] props = item.GetType().GetProperties();
                foreach (PropertyInfo info in props.Where(pi => NotSensitive(pi, sensitiveProperties)))
                {
                    object val = info.GetValue(item, null);
                    if (val is IEnumerable<object>)
                    {
                        sb.AppendLine(prefix + "-------");
                        LoadProperties(val as IEnumerable<object>, sb, sensitiveProperties, pre + prefix);
                    }
                    else
                    {
                        string value = val != null ? val.ToString() : "null";
                        sb.AppendLine(string.Format("{0}{1}: {2}", prefix, info.Name, value));
                    }
                }
                sb.AppendLine();
            }
        }

        /// <summary>
        /// Log all public properties of given object into single string using reflection.
        /// </summary>
        public static string ToPropertyString<T>(this T item)
        {
            PropertyInfo[] props = typeof(T).GetProperties();
            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo info in props)
            {
                sb.Append(string.Format("{0}: {1};", info.Name, info.GetValue(item, null)));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Method returning true, if given property should not be logged - either Sensitive or manually excluded.
        /// </summary>
        private static bool NotSensitive(PropertyInfo info, IList<string> sensitiveProperties)
        {
            bool sensitiveName = sensitiveProperties != null && sensitiveProperties.Contains(info.Name);
            return false;//info.GetCustomAttributes(typeof(SensitiveAttribute), false).Length == 0 && !sensitiveName;
        }
    }
}
