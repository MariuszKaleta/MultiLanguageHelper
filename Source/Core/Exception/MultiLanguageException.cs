using System;

namespace MultiLanguage.Core.Exception
{

    public class MultiLanguageException : System.Exception
    {
        /// <summary>
        /// Create exception from pattern text and args.
        /// </summary>
        /// <param name="pattern">Text in reference language</param>
        /// <param name="args">Universal for all languages text arguments</param>
        public MultiLanguageException(string pattern, params string[] args)
        {
            Pattern = pattern;
            Args = args ?? throw new NullReferenceException();
        }

        #region Fields

        public readonly string[] Args;

        public readonly string Pattern;

        #endregion

        #region Properties

        public override string Message => string.Format(Pattern, Args);

        #endregion
    }
}