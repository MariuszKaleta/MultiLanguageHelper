using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MultiLanguage.Core.Validation.Obsolete
{
    [Obsolete]
    internal class ValidationAttributeStore
    {
        private readonly Dictionary<Type, TypeStoreItem> _typeStoreItems = new Dictionary<Type, TypeStoreItem>();

        internal static ValidationAttributeStore Instance { get; } = new ValidationAttributeStore();

        internal IEnumerable<CustomValidationAttribute> GetTypeValidationAttributes(
            ValidationContext validationContext)
        {
            EnsureValidationContext(validationContext);
            return GetTypeStoreItem(validationContext.ObjectType).ValidationAttributes;
        }

        internal DisplayAttribute GetTypeDisplayAttribute(
            ValidationContext validationContext)
        {
            EnsureValidationContext(validationContext);
            return GetTypeStoreItem(validationContext.ObjectType).DisplayAttribute;
        }

        internal IEnumerable<CustomValidationAttribute> GetPropertyValidationAttributes(
            ValidationContext validationContext)
        {
            EnsureValidationContext(validationContext);
            return GetTypeStoreItem(validationContext.ObjectType).GetPropertyStoreItem(validationContext.MemberName)
                .ValidationAttributes;
        }

        internal DisplayAttribute GetPropertyDisplayAttribute(
            ValidationContext validationContext)
        {
            EnsureValidationContext(validationContext);
            return GetTypeStoreItem(validationContext.ObjectType).GetPropertyStoreItem(validationContext.MemberName)
                .DisplayAttribute;
        }

        internal Type GetPropertyType(ValidationContext validationContext)
        {
            EnsureValidationContext(validationContext);
            return GetTypeStoreItem(validationContext.ObjectType).GetPropertyStoreItem(validationContext.MemberName)
                .PropertyType;
        }

        internal bool IsPropertyContext(ValidationContext validationContext)
        {
            EnsureValidationContext(validationContext);
            PropertyStoreItem propertyStoreItem;
            return GetTypeStoreItem(validationContext.ObjectType)
                .TryGetPropertyStoreItem(validationContext.MemberName, out propertyStoreItem);
        }

        private TypeStoreItem GetTypeStoreItem(
            Type type)
        {
            lock (_typeStoreItems)
            {
                TypeStoreItem typeStoreItem;
                if (!_typeStoreItems.TryGetValue(type, out typeStoreItem))
                {
                    var customAttributes = CustomAttributeExtensions.GetCustomAttributes(type, true);
                    typeStoreItem = new TypeStoreItem(type, customAttributes);
                    _typeStoreItems[type] = typeStoreItem;
                }

                return typeStoreItem;
            }
        }

        private static void EnsureValidationContext(ValidationContext validationContext)
        {
            if (validationContext == null)
                throw new ArgumentNullException(nameof(validationContext));
        }

        internal static bool IsPublic(PropertyInfo p)
        {
            if (p.GetMethod != null && p.GetMethod.IsPublic)
                return true;
            if (p.SetMethod != null)
                return p.SetMethod.IsPublic;
            return false;
        }

        internal static bool IsStatic(PropertyInfo p)
        {
            if (p.GetMethod != null && p.GetMethod.IsStatic)
                return true;
            if (p.SetMethod != null)
                return p.SetMethod.IsStatic;
            return false;
        }

        private abstract class StoreItem
        {
            internal StoreItem(IEnumerable<Attribute> attributes)
            {
                ValidationAttributes = attributes.OfType<CustomValidationAttribute>();
                DisplayAttribute = attributes.OfType<DisplayAttribute>().SingleOrDefault();
            }

            internal IEnumerable<CustomValidationAttribute> ValidationAttributes { get; }

            internal DisplayAttribute DisplayAttribute { get; }
        }

        private class TypeStoreItem : StoreItem
        {
            private readonly object _syncRoot = new object();
            private readonly Type _type;
            private Dictionary<string, PropertyStoreItem> _propertyStoreItems;

            internal TypeStoreItem(Type type, IEnumerable<Attribute> attributes)
                : base(attributes)
            {
                _type = type;
            }

            internal PropertyStoreItem GetPropertyStoreItem(
                string propertyName)
            {
                PropertyStoreItem propertyStoreItem;
                if (!TryGetPropertyStoreItem(propertyName, out propertyStoreItem))
                    throw new ArgumentException($"{_type.Name} {propertyName} {nameof(propertyName)}");
                return propertyStoreItem;
            }

            internal bool TryGetPropertyStoreItem(
                string propertyName,
                out PropertyStoreItem item)
            {
                if (string.IsNullOrEmpty(propertyName))
                    throw new ArgumentNullException(nameof(propertyName));
                if (_propertyStoreItems == null)
                    lock (_syncRoot)
                    {
                        if (_propertyStoreItems == null)
                            _propertyStoreItems = CreatePropertyStoreItems();
                    }

                return _propertyStoreItems.TryGetValue(propertyName, out item);
            }

            private Dictionary<string, PropertyStoreItem> CreatePropertyStoreItems()
            {
                var dictionary = new Dictionary<string, PropertyStoreItem>();
                foreach (var element in _type.GetRuntimeProperties().Where(prop =>
                {
                    if (IsPublic(prop))
                        return !prop.GetIndexParameters().Any();
                    return false;
                }))
                {
                    var propertyStoreItem = new PropertyStoreItem(element.PropertyType,
                        CustomAttributeExtensions.GetCustomAttributes(element, true));
                    dictionary[element.Name] = propertyStoreItem;
                }

                return dictionary;
            }
        }

        private class PropertyStoreItem : StoreItem
        {
            internal PropertyStoreItem(Type propertyType, IEnumerable<Attribute> attributes)
                : base(attributes)
            {
                PropertyType = propertyType;
            }

            internal Type PropertyType { get; }
        }
    }
}