using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace BitLab.Message
{
    public class PayloadAttribute : Attribute
    {
        private static Dictionary<Type, string> _typDoNazwy = new Dictionary<Type, string>();
        private static Dictionary<string, Type> _nazwaDoTypu = new Dictionary<string, Type>();


        static PayloadAttribute()
        {
            foreach (var data in PayloadAttribute.GetLoadableTypes(typeof(PayloadAttribute).GetTypeInfo().Assembly)
                .Where<TypeInfo>((Func<TypeInfo, bool>) (t => t.Namespace == typeof(PayloadAttribute).Namespace))
                .Where<TypeInfo>((Func<TypeInfo, bool>) (t => t.IsDefined(typeof(PayloadAttribute), true))).Select(t =>
                    new
                    {
                        Attr = t.GetCustomAttributes(typeof(PayloadAttribute), true).OfType<PayloadAttribute>()
                            .First<PayloadAttribute>(),
                        Type = t
                    }))
            {
                PayloadAttribute._nazwaDoTypu.Add(data.Attr.Name, data.Type.AsType());
                PayloadAttribute._typDoNazwy.Add(data.Type.AsType(), data.Attr.Name);
            }
        }

        private static IEnumerable<TypeInfo> GetLoadableTypes(Assembly assembly)
        {
            return assembly.DefinedTypes;
        }

        public static string GetCommandName(Type type)
        {
            string name;
            if (!PayloadAttribute._typDoNazwy.TryGetValue(type, out name) &&
                !PayloadAttribute._typDoNazwy.TryGetValue(type.GetTypeInfo().BaseType, out name))
            {
                throw new Exception(type.FullName + "to nie jest payload");
            }

            return name;
        }

        public PayloadAttribute(string commandName)
        {
            this.Name = commandName;
        }

        public string Name { get; set; }
    }
}