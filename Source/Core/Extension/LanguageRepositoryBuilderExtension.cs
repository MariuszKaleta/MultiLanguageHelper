using System;
using System.Collections.Generic;
using System.Reflection;
using MultiLanguage.Core.Service;

namespace MultiLanguage.Core.Extension
{
    public static class LanguageRepositoryBuilderExtension
    {
        public const BindingFlags NonInstance = BindingFlags.Public |
                                                BindingFlags.Static |
                                                BindingFlags.DeclaredOnly |
                                                BindingFlags.FlattenHierarchy;

        public static LanguageRepositoryBuilder Register<T>(this LanguageRepositoryBuilder builder)
            where T : class
        {
            builder.Register(typeof(T));
            return builder;
        }

        public static LanguageRepositoryBuilder Register(this LanguageRepositoryBuilder builder, Type type)
        {
            builder.Register(GetAllMembers(type));
            return builder;
        }

        public static LanguageRepositoryBuilder Register(this LanguageRepositoryBuilder builder,
            IEnumerable<string> elements)
        {
            foreach (var element in elements) builder.Register(element);

            return builder;
        }

        private static IEnumerable<string> GetAllMembers(Type type)
        {
            foreach (var field in type.GetFields(NonInstance))
                if (field.GetValue(null) is string value)
                    yield return value;

            foreach (var member in type.GetProperties())
                if (member.GetValue(null) is string value)
                    yield return value;
        }
    }
}