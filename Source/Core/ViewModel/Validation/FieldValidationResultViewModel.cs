using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MultiLanguage.Core.ViewModel.Validation
{
    public class FieldValidationResultViewModel
    {
        internal FieldValidationResultViewModel(string fieldName, ModelStateEntry modelState)
        {
            FieldName = fieldName;

            Rules = modelState.Errors
                .Select(x => JsonSerializer.Deserialize<ValidationRuleResultViewModel>(x.ErrorMessage)).ToList();
        }

        public FieldValidationResultViewModel()
        {
            Rules = new List<ValidationRuleResultViewModel>();
        }

        #region Properties

        public string FieldName { get; set; }

        public List<ValidationRuleResultViewModel> Rules { get; set; }

        #endregion

        
    }
}