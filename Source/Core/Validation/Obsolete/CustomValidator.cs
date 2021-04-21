namespace MultiLanguage.Core.Validation.Obsolete
{
    /*
    [Obsolete]
    public static class CustomValidator
    {
        private static readonly ValidationAttributeStore _store = ValidationAttributeStore.Instance;

        public static bool TryValidateProperty(
            object value,
            ValidationContext validationContext,
            ICollection<CustomValidationResult> validationResults)
        {
            var propertyType = _store.GetPropertyType(validationContext);
            EnsureValidPropertyType(validationContext.MemberName, propertyType, value);
            var flag = true;
            var breakOnFirstError = validationResults == null;
            var validationAttributes =
                _store.GetPropertyValidationAttributes(validationContext);
            foreach (var validationError in GetValidationErrors(value,
                validationContext, validationAttributes, breakOnFirstError))
            {
                flag = false;
                validationResults?.Add(validationError.ValidationResult);
            }

            return flag;
        }

        public static bool TryValidateObject(
            object instance,
            ValidationContext validationContext,
            ICollection<CustomValidationResult> validationResults)
        {
            return TryValidateObject(instance, validationContext, validationResults, false);
        }

        public static bool TryValidateObject(
            object instance,
            ValidationContext validationContext,
            ICollection<CustomValidationResult> validationResults,
            bool validateAllProperties)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (validationContext != null && instance != validationContext.ObjectInstance)
                throw new ArgumentException(nameof(instance));
            var flag = true;
            var breakOnFirstError = validationResults == null;
            foreach (var objectValidationError in GetObjectValidationErrors(instance,
                validationContext, validateAllProperties, breakOnFirstError))
            {
                flag = false;
                validationResults.Add(objectValidationError.ValidationResult);
            }

            return flag;
        }

        public static bool TryValidateValue(
            object value,
            ValidationContext validationContext,
            ICollection<CustomValidationResult> validationResults,
            IEnumerable<CustomValidationAttribute> validationAttributes)
        {
            var flag = true;
            var breakOnFirstError = validationResults == null;
            foreach (var validationError in GetValidationErrors(value,
                validationContext, validationAttributes, breakOnFirstError))
            {
                flag = false;
                validationResults?.Add(validationError.ValidationResult);
            }

            return flag;
        }

        public static void ValidateProperty(object value, ValidationContext validationContext)
        {
            var propertyType = _store.GetPropertyType(validationContext);
            EnsureValidPropertyType(validationContext.MemberName, propertyType, value);
            var validationAttributes =
                _store.GetPropertyValidationAttributes(validationContext);
            GetValidationErrors(value, validationContext, validationAttributes, false)
                .FirstOrDefault()?.ThrowValidationException();
        }

        public static void ValidateObject(object instance, ValidationContext validationContext)
        {
            ValidateObject(instance, validationContext, false);
        }

        public static void ValidateObject(
            object instance,
            ValidationContext validationContext,
            bool validateAllProperties)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (validationContext == null)
                throw new ArgumentNullException(nameof(validationContext));
            if (instance != validationContext.ObjectInstance)
                throw new ArgumentException(nameof(instance));
            GetObjectValidationErrors(instance, validationContext, validateAllProperties, false)
                .FirstOrDefault()?.ThrowValidationException();
        }

        public static void ValidateValue(
            object value,
            ValidationContext validationContext,
            IEnumerable<CustomValidationAttribute> validationAttributes)
        {
            if (validationContext == null)
                throw new ArgumentNullException(nameof(validationContext));
            GetValidationErrors(value, validationContext, validationAttributes, false)
                .FirstOrDefault()?.ThrowValidationException();
        }


        private static ValidationContext CreateValidationContext(
            object instance,
            ValidationContext validationContext)
        {
            return new ValidationContext(instance, validationContext, validationContext.Items);
        }

        private static bool CanBeAssigned(Type destinationType, object value)
        {
            if (value != null)
                return destinationType.IsInstanceOfType(value);
            if (!destinationType.IsValueType)
                return true;
            if (destinationType.IsGenericType)
                return destinationType.GetGenericTypeDefinition() == typeof(Nullable<>);
            return false;
        }

        private static void EnsureValidPropertyType(
            string propertyName,
            Type propertyType,
            object value)
        {
            if (!CanBeAssigned(propertyType, value))
                throw new ArgumentException($"{propertyName} {propertyType} {nameof(value)}");
        }

        private static IEnumerable<CustomValidationError> GetObjectValidationErrors(
            object instance,
            ValidationContext validationContext,
            bool validateAllProperties,
            bool breakOnFirstError)
        {
            if (validationContext == null)
                throw new ArgumentNullException(nameof(validationContext));
            var source1 = new List<CustomValidationError>();
            source1.AddRange(GetObjectPropertyValidationErrors(instance, validationContext, validateAllProperties, breakOnFirstError));
            if (source1.Any())
                return source1;
            var validationAttributes =
                _store.GetTypeValidationAttributes(validationContext);
            source1.AddRange(GetValidationErrors(instance, validationContext, validationAttributes, breakOnFirstError));
            if (source1.Any())
                return source1;
            /*
            var validatableObject = instance as IValidatableObject;
            var source2 = validatableObject?.Validate(validationContext);
            if (source2 != null)
            {
                foreach (var validationResult in source2.Where(r => r != ValidationResult.Success))
                {
                   // source1.Add(new CustomValidationError(null, instance, validationResult));
                }
            }
            
            return source1;
        }

        private static IEnumerable<CustomValidationError> GetObjectPropertyValidationErrors(
            object instance,
            ValidationContext validationContext,
            bool validateAllProperties,
            bool breakOnFirstError)
        {
            var propertyValues =
                GetPropertyValues(instance, validationContext);
            var source = new List<CustomValidationError>();
            foreach (var keyValuePair in
                propertyValues)
            {
                var validationAttributes =
                    _store.GetPropertyValidationAttributes(keyValuePair.Key);
                if (validateAllProperties)
                {
                    source.AddRange(GetValidationErrors(keyValuePair.Value, keyValuePair.Key,
                        validationAttributes, breakOnFirstError));
                }
                else
                {
                    var requiredAttribute = validationAttributes.OfType<MultiLanguage.Attribute.Validation.RequiredAttribute>()
                        .FirstOrDefault();
                    if (requiredAttribute != null)
                    {
                        var validationResult =
                            requiredAttribute.GetValidationResult(keyValuePair.Value, keyValuePair.Key);
                        if (validationResult != CustomValidationResult.Success)
                            source.Add(new CustomValidationError(requiredAttribute,
                                keyValuePair.Value, validationResult));
                    }
                }

                if (breakOnFirstError)
                    if (source.Any())
                        break;
            }

            return source;
        }

        private static ICollection<KeyValuePair<ValidationContext, object>> GetPropertyValues(
            object instance,
            ValidationContext validationContext)
        {
            var source = instance.GetType().GetRuntimeProperties().Where(
                p =>
                {
                    if (ValidationAttributeStore.IsPublic(p))
                        return !p.GetIndexParameters().Any();
                    return false;
                });
            var keyValuePairList =
                new List<KeyValuePair<ValidationContext, object>>(source.Count());
            foreach (var propertyInfo in source)
            {
                var validationContext1 = CreateValidationContext(instance, validationContext);
                validationContext1.MemberName = propertyInfo.Name;
                if (_store.GetPropertyValidationAttributes(validationContext1).Any())
                    keyValuePairList.Add(new KeyValuePair<ValidationContext, object>(validationContext1,
                        propertyInfo.GetValue(instance, null)));
            }

            return keyValuePairList;
        }

        private static IEnumerable<CustomValidationError> GetValidationErrors(
            object value,
            ValidationContext validationContext,
            IEnumerable<CustomValidationAttribute> attributes,
            bool breakOnFirstError)
        {
            if (validationContext == null)
                throw new ArgumentNullException(nameof(validationContext));
            var validationErrorList = new List<CustomValidationError>();
            var requiredAttribute =
                attributes.OfType<RequiredAttribute>().FirstOrDefault();

            if (requiredAttribute != null && !TryValidate(value, validationContext,
                    requiredAttribute, out var validationError))
            {
                validationErrorList.Add(validationError);
                return validationErrorList;
            }

            foreach (var attribute in attributes)
                if (attribute != requiredAttribute &&
                    !TryValidate(value, validationContext, attribute, out validationError))
                {
                    validationErrorList.Add(validationError);
                    if (breakOnFirstError)
                        break;
                }

            return validationErrorList;
        }

        private static bool TryValidate(
            object value,
            ValidationContext validationContext,
            CustomValidationAttribute attribute,
            out CustomValidationError customValidationError)
        {
            var validationResult = attribute.GetValidationResult(value, validationContext);
            if (validationResult != CustomValidationResult.Success)
            {
                customValidationError = new CustomValidationError(attribute, value, validationResult);
                return false;
            }

            customValidationError = null;
            return true;
        }

        private class CustomValidationError
        {
            private readonly CustomValidationAttribute _validationAttribute;
            private readonly object _value;

            internal CustomValidationError(
                CustomValidationAttribute validationAttribute,
                object value,
                CustomValidationResult validationResult)
            {
                _validationAttribute = validationAttribute;
                ValidationResult = validationResult;
                //ValidationResult.ErrorMessage = MultiLanguageError.GetErrorCode(validationAttribute.GetType()).ToString();

                _value = value;
            }

            internal CustomValidationResult ValidationResult { get; }

            internal void ThrowValidationException()
            {
                throw new CustomValidationException(ValidationResult, _validationAttribute, _value);
            }
        }
    }
*/
}