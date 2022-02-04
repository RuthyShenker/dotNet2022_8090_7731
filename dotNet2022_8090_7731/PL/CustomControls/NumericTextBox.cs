using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.CustomControls
{
    public class NumericTextBox : TextBox, IDataErrorInfo
    {
        static readonly Regex regex = new Regex("[^0-9.-]+");

        public NumericTextBox()
        {
            //this.PreviewTextInput += NumericTextBox_PreviewTextInput;
        }
        //private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    if (IsTextAllowed(e.Text))
        //    {

        //    }
        //}
        private bool IsTextAllowed(string text)
        {
            return !regex.IsMatch(text);
        }

        #region IDataErrorInfo Members

        string IDataErrorInfo.Error { get { return null; } }

        string IDataErrorInfo.this[string propertyName]
        {
            get { return this.GetValidationError(propertyName); }
        }

        #endregion // IDataErrorInfo Members

        #region Validation

        /// <summary>
        /// Returns true if this object has no validation errors.
        /// </summary>
        //public bool IsValid
        //{
        //    get
        //    {
        //        foreach (string property in ValidatedProperties)
        //            if (GetValidationError(property) != null)
        //                return false;

        //        return true;
        //    }
        //}

        string GetValidationError(string propertyName)
        {
            if (!IsTextAllowed(this.Text))
            {
                return "error!";
            }
            else
                return string.Empty;
            //string error = null;

            //switch (propertyName)
            //{
            //    case "Email":
            //        error = this.ValidateEmail();
            //        break;

            //    case "FirstName":
            //        error = this.ValidateFirstName();
            //        break;

            //    case "LastName":
            //        error = this.ValidateLastName();
            //        break;

            //    default:
            //        Debug.Fail("Unexpected property being validated on Customer: " + propertyName);
            //        break;
            //}

            //return error;
        }

        //string ValidateEmail()
        //{
        //    if (IsStringMissing(this.Email))
        //    {
        //        return Strings.Customer_Error_MissingEmail;
        //    }
        //    else if (!IsValidEmailAddress(this.Email))
        //    {
        //        return Strings.Customer_Error_InvalidEmail;
        //    }
        //    return null;
        //}

        //string ValidateFirstName()
        //{
        //    if (IsStringMissing(this.FirstName))
        //    {
        //        return Strings.Customer_Error_MissingFirstName;
        //    }
        //    return null;
        //}

        //string ValidateLastName()
        //{
        //    if (this.IsCompany)
        //    {
        //        if (!IsStringMissing(this.LastName))
        //            return Strings.Customer_Error_CompanyHasNoLastName;
        //    }
        //    else
        //    {
        //        if (IsStringMissing(this.LastName))
        //            return Strings.Customer_Error_MissingLastName;
        //    }
        //    return null;
        //}

        //static bool IsStringMissing(string value)
        //{
        //    return
        //        String.IsNullOrEmpty(value) ||
        //        value.Trim() == String.Empty;
        //}

        //static bool IsValidEmailAddress(string email)
        //{
        //    if (IsStringMissing(email))
        //        return false;

        //    // This regex pattern came from: http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
        //    string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        //    return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        //}

        #endregion // Validation
    }
}
