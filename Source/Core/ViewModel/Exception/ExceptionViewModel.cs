using System;
using MultiLanguage.Core.Exception;

namespace MultiLanguage.Core.ViewModel.Exception
{
    public class ExceptionViewModel
    {
        public ExceptionViewModel()
        {
            Args = new string[ushort.MinValue];
        }

        public ExceptionViewModel(MultiLanguageException exception) : this(exception.Pattern, exception.Args)
        {
        }

        public ExceptionViewModel(System.Exception exception) : this(exception.Message)
        {
        }

        public ExceptionViewModel(string errorCode, params string[] args)
        {
            Args = args ?? throw new NullReferenceException(nameof(args));
            ErrorCode = errorCode;
        }

        public string[] Args { get; set; }

        public string ErrorCode { get; set; }
    }
}