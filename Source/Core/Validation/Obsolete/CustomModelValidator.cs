namespace MultiLanguage.Core.Validation.Obsolete
{
    /*
    [Obsolete]
    public class CustomModelValidator : IObjectModelValidator
    {
        public void Validate(
            ActionContext actionContext,
            ValidationStateDictionary validationState,
            string prefix,
            object model)
        {
            var validationContext = new ValidationContext(model);
            var collection = new List<CustomValidationResult>();

            var type = model.GetType();

            if (type.IsPrimitive || type == typeof(string))
            {
                if (!actionContext.ModelState.TryGetValue(prefix, out var entry)) return;

                entry.ValidationState = ModelValidationState.Valid;
            }
            else
            {
                if (CustomValidator.TryValidateObject(model, validationContext, collection, true))
                {
                    return;
                }

                foreach (var validationResult in collection)
                {
                    actionContext.ModelState.AddModelError(validationResult.MemberName,validationResult.CombineText);
                }
            }
        }
    }
    */
}