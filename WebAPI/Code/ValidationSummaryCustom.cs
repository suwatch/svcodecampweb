using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI.WebControls;

namespace CodeCampSV
{
    /// <summary>
    /// This control inheirits the ASP.NET ValidationSummary control but adds
    /// the ability to dynamically add messages without requiring validation 
    /// controls.
    /// http://forums.asp.net/p/1290159/2490507.aspx
    /// </summary>
    public class ValidationSummaryWithAddMessage : System.Web.UI.WebControls.ValidationSummary
    {
        #region Properties
        /// <summary>
        /// Returns true if a validator on page is invalid; otherwise false.
        /// </summary>
        [Browsable(false)]
        public bool HasMessages
        {
            get
            {
                foreach (BaseValidator validator in this.Page.Validators)
                {
                    if (validator.ValidationGroup.Equals(base.ValidationGroup)
                        && !validator.IsValid)
                    {
                        return true;
                    }
                }

                return false;
            }
        } // HasMessages
        #endregion Properties

        #region Public Methods
        /// <summary>
        /// Allows the caller to place exception messages inside the validation
        /// summary control
        /// </summary>
        /// <param name="ex">Exception</param>
        public void AddError(Exception ex)
        {
            string message = (ex.InnerException != null) ?
                ex.InnerException.Message :
                ex.Message;

            this.Page.Validators.Add(new Validator(this, message));
        } // AddError

        /// <summary>
        /// Allows the caller to place custom formatted text messages inside 
        /// the validation summary control
        /// </summary>
        /// <param name="format">Formatted message</param>
        /// <param name="args">Replacement strings</param>
        public void AddFormattedMessage(string format, params object[] args)
        {
            string message = String.Format(format, args);
            this.Page.Validators.Add(new Validator(this, message));
        } // AddFormattedMessage

        /// <summary>
        /// Allows the caller to place custom text messages inside the validation
        /// summary control
        /// </summary>
        /// <param name="message">The message you want to appear in the summary</param>
        public void AddMessage(string message)
        {
            this.Page.Validators.Add(new Validator(this, message));
        } // AddMessage
        #endregion Public Methods
    } // ValidationSummary

    /// <summary>
    /// The validation summary control works by iterating over the Page.Validators
    /// collection and displaying the ErrorMessage property of each validator
    /// that return false for the IsValid() property.  This class will act 
    /// like all the other validators except it always is invalid and thus the 
    /// ErrorMessage property will always be displayed.
    /// </summary>
    internal class Validator : BaseValidator
    {
        #region Constructors
        public Validator(ValidationSummary validationSummary, String errorMessage)
        {
            base.ValidationGroup = validationSummary.ValidationGroup;
            base.ErrorMessage = errorMessage;
            base.IsValid = false;
        } // Validator
        #endregion Constructors

        #region Protected Methods
        protected override bool EvaluateIsValid()
        {
            return false;
        } // EvaluateIsValid
        #endregion Protected Methods
    } // Validator
}