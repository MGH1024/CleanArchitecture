namespace Utility
{
    public static class TypeExtention
    {
        public static bool IsUserDefinedClass(this Type type)
        {
            return !type.IsValueType
                   &&
                   !type.IsPrimitive
                   &&
                   !type.IsEnum
                   &&
                   (type.Namespace == null || !type.Namespace.StartsWith("System"));
        }
    }
}
