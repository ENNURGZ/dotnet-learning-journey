using System.Reflection;

[assembly: CLSCompliant(true)]

namespace Reflection
{
    public static class ReflectionOperations
    {
        public static string GetTypeName(object? obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            Type type = obj.GetType();
            return type.Name;
        }

        public static string GetFullTypeName<T>()
        {
            Type type = typeof(T);
            return type.FullName!;
        }

        public static string GetAssemblyQualifiedName<T>()
        {
            Type type = typeof(T);
            return type.AssemblyQualifiedName!;
        }

        public static string[] GetPrivateInstanceFields(object? obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            Type type = obj.GetType();

            FieldInfo[] fields =
                type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            return fields.Select(f => f.Name).ToArray();
        }

        public static string[] GetPublicStaticFields(object? obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            Type type = obj.GetType();

            FieldInfo[] fields =
                type.GetFields(BindingFlags.Public | BindingFlags.Static);

            return fields.Select(f => f.Name).ToArray();
        }

        public static string?[] GetInterfaceDataDetails(object? obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            Type type = obj.GetType();
            Type[] interfaces = type.GetInterfaces();

            return interfaces.Select(i => i.FullName).ToArray();
        }

        public static string?[] GetConstructorsDataDetails(object? obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            Type type = obj.GetType();
            ConstructorInfo[] constructors = type.GetConstructors();

            return constructors.Select(c => c.ToString()).ToArray();
        }

        public static string?[] GetTypeMembersDataDetails(object? obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            Type type = obj.GetType();
            MemberInfo[] members = type.GetMembers();

            return members.Select(m => m.ToString()).ToArray();
        }

        public static string?[] GetMethodDataDetails(object? obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            Type type = obj.GetType();
            MethodInfo[] methods = type.GetMethods();

            return methods.Select(m => m.ToString()).ToArray();
        }

        public static string?[] GetPropertiesDataDetails(object? obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            return properties.Select(p => p.ToString()).ToArray();
        }
    }
}
