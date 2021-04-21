using System.Collections.Generic;

namespace MultiLanguage.Core.ViewModel.Validation
{
    public class ValidationRuleResultViewModel
    {
        public ValidationRuleResultViewModel()
        {

        }


        public string ErrorCode { get; set; }

        public Dictionary<string, string> Args { get; set; }

        
    }
}