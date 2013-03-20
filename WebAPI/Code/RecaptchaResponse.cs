// Type: Recaptcha.RecaptchaResponse
// Assembly: Recaptcha, Version=1.0.5.0, Culture=neutral, PublicKeyToken=9afc4d65b28c38c2
// Assembly location: C:\temp\WebApplication1\WebApplication1\bin\Recaptcha.dll

using System;

namespace WebAPI.Code
{
    public class RecaptchaResponse
    {
        public static readonly RecaptchaResponse Valid = new RecaptchaResponse(true, string.Empty);
        public static readonly RecaptchaResponse InvalidChallenge = new RecaptchaResponse(false, "Invalid reCAPTCHA request. Missing challenge value.");
        public static readonly RecaptchaResponse InvalidResponse = new RecaptchaResponse(false, "Invalid reCAPTCHA request. Missing response value.");
        public static readonly RecaptchaResponse InvalidSolution = new RecaptchaResponse(false, "The verification words are incorrect.");
        public static readonly RecaptchaResponse RecaptchaNotReachable = new RecaptchaResponse(false, "The reCAPTCHA server is unavailable.");
        private bool isValid;
        private string errorMessage;

        public bool IsValid
        {
            get
            {
                return this.isValid;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }
        }

        static RecaptchaResponse()
        {
        }

        internal RecaptchaResponse(bool isValid, string errorMessage)
        {
            RecaptchaResponse recaptchaResponse = (RecaptchaResponse)null;
            if (this.IsValid)
            {
                recaptchaResponse = RecaptchaResponse.Valid;
            }
            else
            {
                switch (errorMessage)
                {
                    case "incorrect-captcha-sol":
                        recaptchaResponse = RecaptchaResponse.InvalidSolution;
                        break;
                    case null:
                        throw new ArgumentNullException("errorMessage");
                }
            }
            if (recaptchaResponse != null)
            {
                this.isValid = recaptchaResponse.IsValid;
                this.errorMessage = recaptchaResponse.ErrorMessage;
            }
            else
            {
                this.isValid = isValid;
                this.errorMessage = errorMessage;
            }
        }

        public override bool Equals(object obj)
        {
            RecaptchaResponse recaptchaResponse = (RecaptchaResponse)obj;
            if (recaptchaResponse == null)
                return false;
            else
                return recaptchaResponse.IsValid == this.isValid && recaptchaResponse.ErrorMessage == this.errorMessage;
        }

        public override int GetHashCode()
        {
            return this.isValid.GetHashCode() ^ this.errorMessage.GetHashCode();
        }
    }
}
