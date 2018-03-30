using System.Collections.Generic;
using System.Reflection;

namespace ExpressionTreeTask.Mapper
{
    /// <summary>Defines methods to support the comparison of PropertyInfo for equality.</summary>
    public class PropertyInfoComparer : IEqualityComparer<PropertyInfo>
    {
        /// <summary>Determines whether the specified objects are equal.</summary>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        /// <param name="left">The first object of type PropertyInfo to compare.</param>
        /// <param name="right">The second object of type PropertyInfo to compare.</param>
        public bool Equals(PropertyInfo left, PropertyInfo right)
        {
            if (left == null || right == null)
                return false;

            if (left == right)
                return true;

            return left.Name == right.Name && left.PropertyType.FullName == right.PropertyType.FullName;
        }

        /// <summary>Returns a hash code for the specified object.</summary>
        /// <returns>A hash code for the specified object.</returns>
        /// <param name="obj">The PropertyInfo object for which a hash code is to be returned.</param>
        public int GetHashCode(PropertyInfo obj)
        {
            return obj.Name.Length ^ obj.PropertyType.GetHashCode();
        }
    }
}
