using System;
using System.Text.RegularExpressions;

namespace Genbox.WolframAlpha
{
    public class WolframAlphaConfig
    {
        private readonly Regex _appIdRegex = new Regex(@"^[0-9A-Z]{6}\-[0-9A-Z]{10}", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private string _appId;

        public string AppId
        {
            get => _appId;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("AppId must not be null or empty", nameof(value));

                if (value.Length != 17)
                    throw new ArgumentException("Length of AppId must be 17", nameof(value));

                if (!_appIdRegex.IsMatch(value))
                    throw new ArgumentException("Your AppId is invalid", nameof(value));

                _appId = value;
            }
        }
    }
}