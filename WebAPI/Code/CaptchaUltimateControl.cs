/*
Copyright © 2006, Peter Kellner
All rights reserved.
http://peterkellner.net

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

- Redistributions of source code must retain the above copyright
notice, this list of conditions and the following disclaimer.

- Neither Peter Kellner, nor the names of its
contributors may be used to endorse or promote products
derived from this software without specific prior written 
permission. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE 
COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES INCLUDING,
BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
POSSIBILITY OF SUCH DAMAGE.
*/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Image=System.Web.UI.WebControls.Image;

[assembly: TagPrefix("PeterKellner.Utils", "CAPTCHA")]

/// <summary>
/// Create new EventArgs class for verifying
/// </summary>
public class VerifyingEventArgs : EventArgs
{
    // Methods
    public VerifyingEventArgs()
    {
    }

    public VerifyingEventArgs(bool forceVerify)
    {
        ForceVerify = forceVerify;
    }

    // Properties
    public bool ForceVerify { get; set; }
}


namespace PeterKellner.Utils
{
    public delegate void VerifyingEventHandler(object sender, VerifyingEventArgs e);

    [Designer(typeof(TemplateControlDesigner))]
    [ParseChildren(true)]
    [PersistChildren(false)]
    [DefaultProperty("Title")]
    [DefaultEvent("OnVerified")]
    [ToolboxBitmap(typeof(CaptchaUltimateControl), "toolbaricon.bmp")]
    [ToolboxData("<{0}:CaptchaUltimateControl runat=server></{0}:CaptchaUltimateControl>")]
    public class CaptchaUltimateControl : CompositeControl
    {
        private static readonly char[] _validCharacters = {
                                                              '@', '#', '+', '=', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'
                                                              ,
                                                              'k', 'm', 'n', 'p', 'q', 'r', 's', 't', 'w', 'x', 'y', 'z'
                                                              ,
                                                              '2', '3', '4', '5', '6', '7', '8', '9'
                                                          };

        private static readonly object EventFailedVerify = new object();
        private static readonly object EventVerified = new object();
        private static readonly object EventVerifying = new object();

        private Image _image; // the image control.

        private ITemplate _itemTemplate;
        private Button _redisplayButton; // forces a redisplay of new captcha number
        private TextBox _userInput; // Where Users Inputs alphanumberic captcha
        private CustomValidator _validator; // compares what user types in with graphic captcha
        private List<Control> listOfControls;

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate ItemTemplate
        {
            get { return _itemTemplate; }
            set { _itemTemplate = value; }
        }

        /// <summary>
        /// Can be used for arbitrary data to pass into the control
        /// </summary>
        [Browsable(false)]
        [Category("Behavior")]
        [Themeable(true)]
        [Description("Command Arg1")]
        [Bindable(true)]
        public string CommandArg1
        {
            get
            {
                object o = ViewState["CommandArg1"];
                if (o == null)
                    return string.Empty;
                return (string)o;
            }
            set { ViewState["CommandArg1"] = value; }
        }

        [Browsable(false)]
        [Category("Behavior")]
        [Themeable(true)]
        [Description("Command Arg2")]
        [Bindable(true)]
        public string CommandArg2
        {
            get
            {
                object o = ViewState["CommandArg2"];
                if (o == null)
                    return string.Empty;
                return (string)o;
            }
            set { ViewState["CommandArg2"] = value; }
        }

        [Browsable(false)]
        [Category("Behavior")]
        [Themeable(true)]
        [Description("Command Arg3")]
        [Bindable(true)]
        public string CommandArg3
        {
            get
            {
                object o = ViewState["CommandArg3"];
                if (o == null)
                    return string.Empty;
                return (string)o;
            }
            set { ViewState["CommandArg3"] = value; }
        }

        [Browsable(false)]
        [Category("Behavior")]
        [Themeable(true)]
        [Description("Command Arg4")]
        [Bindable(true)]
        public string CommandArg4
        {
            get
            {
                object o = ViewState["CommandArg4"];
                if (o == null)
                    return string.Empty;
                return (string)o;
            }
            set { ViewState["CommandArg4"] = value; }
        }

        [Browsable(false)]
        [Category("Behavior")]
        [Themeable(true)]
        [Description("Command Arg5")]
        [Bindable(true)]
        public string CommandArg5
        {
            get
            {
                object o = ViewState["CommandArg5"];
                if (o == null)
                    return string.Empty;
                return (string)o;
            }
            set { ViewState["CommandArg5"] = value; }
        }


        [Category("Behavior")]
        [Themeable(true)]
        [Description("Width of Captcha Control")]
        public int WidthCaptchaPixels
        {
            get
            {
                object o = ViewState["WidthCaptchaPixels"];
                if (o == null)
                    return 140;
                return (int)o;
            }
            set { ViewState["WidthCaptchaPixels"] = value; }
        }


        [Category("Behavior")]
        [Themeable(true)]
        [Description("Length of Captcha to use.")]
        public int CaptchaLength
        {
            get
            {
                object o = ViewState["CaptchaLength"];
                if (o == null)
                    return 4;
                return (int)o;
            }
            set { ViewState["CaptchaLength"] = value; }
        }

        [Category("Behavior")]
        [Themeable(true)]
        [Description("Encrypted Value.  Convert to string to display")]
        [Browsable(false)]
        public string EncryptedValue
        {
            get
            {
                object o = ViewState["EncryptedValue"];
                if (o == null)
                    return string.Empty;
                return (string)o;
            }
            set { ViewState["EncryptedValue"] = value; }
        }

        [Category("Appearance")]
        [Themeable(true)]
        [Description("Font Family String")]
        public string FontFamilyString
        {
            get
            {
                object o = ViewState["FontFamilyString"];
                if (o == null)
                    return "Courrier New";
                return (string)o;
            }
            set { ViewState["FontFamilyString"] = value; }
        }

        [Category("Appearance")]
        [Themeable(true)]
        [Description("Captcha Background Color")]
        public Color CaptchaBackgroundColor
        {
            get
            {
                object o = ViewState["CaptchaBackgroundColor"];
                if (o == null)
                    return Color.White;
                return (Color)o;
            }
            set { ViewState["CaptchaBackgroundColor"] = value; }
        }

        [Category("Appearance")]
        [Themeable(true)]
        [Description("Height of Captcha Control")]
        public int HeightCaptchaPixels
        {
            get
            {
                object o = ViewState["HeightCaptchaPixels"];
                if (o == null)
                    return 50;
                return (int)o;
            }
            set { ViewState["HeightCaptchaPixels"] = value; }
        }

        [Category("Appearance")]
        [Themeable(true)]
        [Description("Height of Captcha Control")]
        public int CaptchaBorder
        {
            get
            {
                object o = ViewState["CaptchaBorder"];
                if (o == null)
                    return 1;
                return (int)o;
            }
            set { ViewState["CaptchaBorder"] = value; }
        }

        [Category("Behavior")]
        [Themeable(true)]
        [Description("PlainValue Captcha Value")]
        [Browsable(false)]
        public string PlainValue
        {
            get
            {
                object o = ViewState["PlainValue"];
                if (o == null)
                    return string.Empty;
                return (string)o;
            }
            set { ViewState["PlainValue"] = value; }
        }

        /// <summary>
        /// Shows Title label in template control with ID=Title
        /// </summary>
        /// 
        [Category("Appearance")]
        [Themeable(true)]
        [Description("Show Title")]
        public bool ShowTitle
        {
            get
            {
                object o = ViewState["ShowTitle"];
                if (o == null)
                    return true;
                return (bool)o;
            }
            set { ViewState["ShowTitle"] = value; }
        }

        /// <summary>
        /// Shows Title label in template control with ID=Title
        /// </summary>
        /// 
        [Category("Behavior")]
        [Themeable(true)]
        [Description("Bypass the Captcha When the User is Authenticated")]
        public bool ByPassCaptchaWhenAuthenticated
        {
            get
            {
                object o = ViewState["ByPassCaptchaWhenAuthenticated"];
                if (o == null)
                    return true;
                return (bool)o;
            }
            set { ViewState["ByPassCaptchaWhenAuthenticated"] = value; }
        }

        /// <summary>
        /// Shows Title label in template control with ID=Title
        /// </summary>
        [Category("Appearance")]
        [Themeable(true)]
        [Description("Show Promotional Tag Line.  :)")]
        public bool ShowPromo
        {
            get
            {
                object o = ViewState["ShowPromo"];
                if (o == null)
                    return true;
                return (bool)o;
            }
            set { ViewState["ShowPromo"] = value; }
        }

        [Category("Behavior")]
        [Themeable(true)]
        [Description("Gets and sets Captcha Redisplay Button Text")]
        public string ButtonRedisplayCaptchaText
        {
            get
            {
                object o = ViewState["ButtonRedisplayCaptchaText"];
                if (o == null)
                    return "Generate New Display Number";
                return (string)o;
            }
            set { ViewState["ButtonRedisplayCaptchaText"] = value; }
        }


        /// <summary>
        /// Shows Title label in template control with ID=Title
        /// </summary>
        /// 
        [Category("Behavior")]
        [Themeable(true)]
        [Description("Show Button to Recalculate Captcha")]
        public bool ShowRecalculateCaptchaButton
        {
            get
            {
                object o = ViewState["ShowRecalculateCaptchaButton"];
                if (o == null)
                    return true;
                return (bool)o;
            }
            set { ViewState["ShowRecalculateCaptchaButton"] = value; }
        }

        [Category("Behavior")]
        [Themeable(true)]
        [Description("Type of Captcha to use.  choices are 1 or 2.")]
        public int CaptchaType
        {
            get
            {
                //return captchaType;
                object o = ViewState["CaptchaType"];
                if (o == null)
                    return 2;
                return (int)o;
            }
            set
            {
                //captchaType = value;
                ViewState["CaptchaType"] = value;
            }
        }

        [Category("Behavior")]
        [Themeable(true)]
        [Description("Gets and sets Title shown in control with ID=Title")]
        public string Title
        {
            get
            {
                object o = ViewState["Title"];
                if (o == null)
                    return "Captcha Control";
                return (string)o;
            }
            set { ViewState["Title"] = value; }
        }

        [Category("Behavior")]
        [Themeable(true)]
        [Description("Sets the String to be displayed when entered text does not match Captcha Display")]
        public string InvalidCaptchaMessage
        {
            get
            {
                object o = ViewState["InvalidCaptchaMessage"];
                if (o == null)
                    return "***";
                return (string)o;
            }
            set { ViewState["InvalidCaptchaMessage"] = value; }
        }

        /// <summary>
        /// not sure if need this because using compositcontrols as base class
        /// </summary>
        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        /// <summary>
        /// Iterate through all controls on page.
        /// Borrowed From Scott Mitchell's 
        /// "Working with Dynamically Created Controls, Part 2
        /// http://aspnet.4guysfromrolla.com/articles/082102-1.2.aspx
        /// </summary>
        /// <param name="parent"></param>
        private void IterateThroughChildren(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                listOfControls.Add(c);
                if (c.Controls.Count > 0)
                {
                    IterateThroughChildren(c);
                }
            }
        }

        /// <summary>
        /// Allows you to verify without entering alpha string
        /// </summary>
        public event VerifyingEventHandler Verifying
        {
            add { Events.AddHandler(EventVerifying, value); }
            remove { Events.RemoveHandler(EventVerifying, value); }
        }

        /// <summary>
        /// Allows you to perform some sort of act upon successful verification
        /// </summary>
        public event EventHandler Verified
        {
            add { Events.AddHandler(EventVerified, value); }
            remove { Events.RemoveHandler(EventVerified, value); }
        }

        public event EventHandler FailedVerify
        {
            add { Events.AddHandler(EventFailedVerify, value); }
            remove { Events.RemoveHandler(EventFailedVerify, value); }
        }

        /// <summary>
        /// Meant to be overridden by users control
        /// </summary>
        /// <param name="ve"></param>
        protected virtual void OnVerifying(VerifyingEventArgs ve)
        {
            // person still sees captcha, but they are not bothered if they don't enter it or do it incorrectly
            if (Context.User.Identity.IsAuthenticated && ByPassCaptchaWhenAuthenticated)
            {
                ve.ForceVerify = true;
            }

            var handler = (VerifyingEventHandler)Events[EventVerifying];
            if (handler != null) // null check is necessary in case no one has subscribed to this event
            {
                handler(this, ve);
            }
        }

        /// <summary>
        /// Meant to be overridden by users control
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnVerified(EventArgs e)
        {
            var handler = (EventHandler)Events[EventVerified];
            if (handler != null) // null check is necessary in case no one has subscribed to this event
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Meant to be overridden by users control
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnFailedVerify(EventArgs e)
        {
            var handler = (EventHandler)Events[EventFailedVerify];
            if (handler != null) // null check is necessary in case no one has subscribed to this event
            {
                handler(this, e);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            listOfControls = new List<Control>();
            Page.RegisterRequiresControlState(this);
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!DesignMode)
            {
                if (Context.Items["_redisplayButton_Click"] != null &&
                    ((String)Context.Items["_redisplayButton_Click"]).Equals("clicked"))
                {
                    //CalculateCaptchaValue();  // new captcha number
                    UpdateImageProperties(); // make sure image tag knows of new number
                    Page.Validate(); // this causes another trip through the validation because we changed the captcha #
                }
            }
            base.OnPreRender(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                if (!Page.IsPostBack)
                {
                    GenerateNewCaptchaNumber();
                }

                base.OnLoad(e);
            }
            //EnsureChildControls();
        }

        /// <summary>
        /// Generates a new captcha number and stores in cache for 5 minutes by default
        /// </summary>
        private void GenerateNewCaptchaNumber()
        {
            PlainValue = CreateCaptchaText(); // Property
            //Guid guidActual = Guid.NewGuid();
            //EncryptedValue = guidActual.ToString(); // Property
            EncryptedValue = CaptchaImageUtils.Encrypt(PlainValue, CaptchaImageUtils.SymetricKey, true);

            var cp = new CaptchaParams(
                PlainValue, EncryptedValue, HeightCaptchaPixels, WidthCaptchaPixels,
                CaptchaType, FontFamilyString);
        }

        private void Page_PreRenderComplete(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                // necessary because of where number generated
                UpdateImageProperties();
            }
        }

        private void ShowCaptchaPreview()
        {
            if (CaptchaType == 1)
            {
                _image.ImageUrl = Page.ClientScript.GetWebResourceUrl
                    (GetType(), "CaptchaUltimateCustomControl.Images.CaptchaType1.gif");
                _image.Width = WidthCaptchaPixels;
                _image.Height = HeightCaptchaPixels;
            }
            else if (CaptchaType == 2)
            {
                _image.ImageUrl = Page.ClientScript.GetWebResourceUrl
                    (GetType(), "CaptchaUltimateCustomControl.Images.CaptchaType2.gif");
                _image.Width = WidthCaptchaPixels;
                _image.Height = HeightCaptchaPixels;
            }
            else
            {
                throw new ApplicationException("CaptchaType Must Be 1 or 2");
            }
        }


        /// <summary>
        /// Recalculate captcha when button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _redisplayButton_Click(object sender, EventArgs e)
        {
            GenerateNewCaptchaNumber();

            // Since this clicks is processed after onload and after the validation, we need
            // to remember that a click happened so in the prerender event (which always fires
            // after this) we can call Validate() and force the validation to happen again and
            // check the validity of the text against the picture value.  (mouthful, but worth it)
            Context.Items["_redisplayButton_Click"] = "clicked";
        }


        /// <summary>
        /// Check and see if control ID exists in control collection. It seems I should be able to 
        /// to this with findcontrol but it doesn't seem to work.  
        /// </summary>
        /// <param name="controlID"></param>
        /// <returns></returns>
        private bool IsControlExists(string controlID)
        {
            foreach (Control ctrl in listOfControls)
            {
                if (ctrl.ID != null && ctrl.ID.Equals(controlID))
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateImageProperties()
        {
            if (DesignMode)
            {
                ShowCaptchaPreview();
            }
            else
            {
                // We do not need to use Session or Cache because we are depending on a key in this class to encrypt and decrypt the value
                string encryptedValueEncoded = HttpContext.Current.Server.UrlEncode(EncryptedValue);

                if (!String.IsNullOrEmpty(encryptedValueEncoded))
                {
                    _image.ImageUrl = "~/CaptchaType.ashx?encryptedvalue=" + encryptedValueEncoded +
                                      "&heightcaptchapixels=" + HeightCaptchaPixels +
                                      "&widthcaptchapixels=" + WidthCaptchaPixels +
                                      "&captchatype=" + CaptchaType +
                                      "&fontfamilystring=" + FontFamilyString;
                }


            }
        }

        private void _validator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.Compare(_userInput.Text, PlainValue, true) == 0)
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }

            // Force override if person set ForceVerify true in event on their page
            var ve = new VerifyingEventArgs();
            OnVerifying(ve);
            if (ve.ForceVerify)
            {
                args.IsValid = true;
            }

            if (args.IsValid)
            {
                OnVerified(EventArgs.Empty);
            }
            else
            {
                OnFailedVerify(EventArgs.Empty);
            }
        }

        public void UpdateAllControls()
        {
            Controls.Clear();
        }

        protected string CreateCaptchaText()
        {
            var rand = new Random();
            if (CaptchaLength <= 0)
            {
                throw new ArgumentOutOfRangeException("length", CaptchaLength, "Length value should be greater than 0");
            }
            int maximumLength = _validCharacters.Length;
            var sb = new StringBuilder(CaptchaLength);
            for (int i = 0; i < CaptchaLength; ++i)
            {
                char c = _validCharacters[rand.Next(maximumLength)];
                if (rand.Next(2) == 1)
                {
                    c = char.ToUpper(c);
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            // For template existing and not in design mode.
            // (don't mess with controls if in design mode or screws up template design)
            if (ItemTemplate != null && !DesignMode)
            {
                _itemTemplate.InstantiateIn(this);

                // Build a list of current controls because the controls of type
                // HtmlTable are nested ones (rows, cells, etc.)
                IterateThroughChildren(this);

                foreach (Control ctrl in listOfControls)
                {
                    switch (ctrl.ID)
                    {
                        case "VerificationID":
                            if (ctrl.GetType().ToString().EndsWith("TextBox"))
                            {
                                _userInput = (TextBox)ctrl;
                            }
                            else
                            {
                                throw new ApplicationException(
                                    "Control ID=VerificationID must be of type TextBox control");
                            }
                            break;
                        case "TitleID":
                            if (ctrl.GetType().ToString().EndsWith("Label"))
                            {
                                ((Label)ctrl).Text = Title;
                                ctrl.Visible = ShowTitle;
                            }
                            else
                            {
                                throw new ApplicationException("Control ID=TitleID Must be of type Label control");
                            }
                            break;
                        case "CaptchaImageID":
                            if (ctrl.GetType().ToString().EndsWith("Image"))
                            {
                                _image = (Image)ctrl;
                                UpdateImageProperties();
                            }
                            else
                            {
                                throw new ApplicationException(
                                    "Control ID=CaptchaImageID Must be of type Image control");
                            }
                            break;
                        case "CustomValidatorID":
                            if (ctrl.GetType().ToString().EndsWith("CustomValidator"))
                            {
                                // need this to add event later
                                _validator = (CustomValidator)ctrl;
                                // set default message if none specified
                                _validator.ErrorMessage = InvalidCaptchaMessage;
                            }
                            else
                            {
                                throw new ApplicationException(
                                    "Control ID=CustomValidatorID Must be of type Image CustomValidator");
                            }
                            break;

                        case "ButtonDisplayNextID":
                            if (ctrl.GetType().ToString().EndsWith("Button"))
                            {
                                _redisplayButton = (Button)ctrl;
                                if (String.IsNullOrEmpty(_redisplayButton.Text))
                                {
                                    _redisplayButton.Text = ButtonRedisplayCaptchaText;
                                }
                                if (ShowRecalculateCaptchaButton)
                                {
                                    _redisplayButton.Visible = true;
                                }
                                else
                                {
                                    _redisplayButton.Visible = false;
                                }
                            }
                            else
                            {
                                throw new ApplicationException("ID CaptchaImageID Must be of type Image control");
                            }
                            break;
                        default:
                            break;
                    }
                }

                // only do this if there is a template.  normally, they should
                // have all these controls, but if for some reason they did not
                // put them down, then we will.  Kind of ugly, but they need to use
                // all the fields or the control will not work right. (sort of)
                if (!IsControlExists("VerificationID"))
                {
                    _userInput = new TextBox();
                    _userInput.ID = "VerificationID";
                    _userInput.Text = string.Empty;
                    Controls.Add(_userInput);
                }

                if (!IsControlExists("CustomValidatorID"))
                {
                    _validator = new CustomValidator();
                    _validator.ID = "CustomValidatorID";
                    _validator.ErrorMessage = InvalidCaptchaMessage;
                    Controls.Add(_validator);
                }

                if (!IsControlExists("ButtonDisplayNextID"))
                {
                    _redisplayButton = new Button();
                    _redisplayButton.Text = ButtonRedisplayCaptchaText;
                    _redisplayButton.ID = "ButtonDisplayNextID";
                    if (ShowRecalculateCaptchaButton)
                    {
                        _redisplayButton.Visible = true;
                    }
                    else
                    {
                        _redisplayButton.Visible = false;
                    }
                    Controls.Add(_redisplayButton);
                }

            }
            else if (ItemTemplate != null && DesignMode)
            {
                _itemTemplate.InstantiateIn(this);
            }
            else if (ItemTemplate == null)
            {
                var table = new HtmlTable();
                table.Border = CaptchaBorder;
                table.BgColor = "Aqua";

                var tableRow = new HtmlTableRow();

                var tableCell = new HtmlTableCell();
                tableCell.Attributes.Add("style", "text-align: center;");
                tableCell.ColSpan = 2;
                var label = new Label();
                label.ID = "TitleID";
                label.Text = Title;
                label.Visible = ShowTitle;
                tableCell.Controls.Add(label);
                tableRow.Controls.Add(tableCell);
                table.Controls.Add(tableRow);
                // Row Break

                tableRow = new HtmlTableRow();
                tableCell = new HtmlTableCell();
                _userInput = new TextBox();
                _userInput.ID = "VerificationID";
                tableCell.Controls.Add(_userInput);
                tableRow.Controls.Add(tableCell);

                tableCell = new HtmlTableCell();
                _validator = new CustomValidator();
                _validator.Text = InvalidCaptchaMessage;
                _validator.ID = "CustomValidatorID";
                tableCell.Controls.Add(_validator);
                tableRow.Controls.Add(tableCell);
                table.Controls.Add(tableRow);
                // Row Break

                tableRow = new HtmlTableRow();
                tableCell = new HtmlTableCell();
                tableCell.Attributes.Add("style", "text-align: center;");
                tableCell.ColSpan = 2;
                _image = new Image();
                _image.ID = "CaptchaImageID";
                UpdateImageProperties();
                if (DesignMode)
                {
                    //ShowCaptchaPreview();
                }
                tableCell.Controls.Add(_image);
                tableRow.Controls.Add(tableCell);
                table.Controls.Add(tableRow);
                // Row Break

                tableRow = new HtmlTableRow();
                tableCell = new HtmlTableCell();
                tableCell.Attributes.Add("style", "text-align: center;");
                tableCell.ColSpan = 2;
                _redisplayButton = new Button();
                _redisplayButton.ID = "ButtonDisplayNextID";
                _redisplayButton.Visible = ShowRecalculateCaptchaButton;
                _redisplayButton.Text = ButtonRedisplayCaptchaText;
                tableCell.Controls.Add(_redisplayButton);
                tableRow.Controls.Add(tableCell);
                table.Controls.Add(tableRow);
                Controls.Add(table);
            }

            if (ShowPromo)
            {
                var hyperLink = new HyperLink();
                hyperLink.Font.Size = 6;
                hyperLink.Font.Italic = true;
                hyperLink.NavigateUrl = "http://peterkellner.net/needhelp?id=102";
                hyperLink.ToolTip = "If you need help with asp.net including custom server control development " +
                                    "or training click here to be directed to the author of this Captcha custom control.";
                hyperLink.Text = "Need Custom Controls Work or Training?";
                Controls.Add(hyperLink);
            }

            // One way or another, we have a validator so lets add the event handler
            if (_validator != null && !DesignMode)
            {
                _validator.EnableClientScript = false;
                _validator.ServerValidate += _validator_ServerValidate;
            }

            // force a recalc of captcha
            if (_redisplayButton != null && !DesignMode)
            {
                _redisplayButton.Click += _redisplayButton_Click;
            }

            base.CreateChildControls();
        }
    }

    /// <summary>
    /// Mostly Written by Jason Diamond.  Modified to work with the
    /// CaptchaUltimateControl.  Someday, I hope to really understand how
    /// this works.
    /// 
    /// </summary>
    public class TemplateControlDesigner : CompositeControlDesigner
    {
        private DesignerActionListCollection _actionLists;

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (_actionLists == null)
                {
                    _actionLists = new DesignerActionListCollection();
                    _actionLists.AddRange(base.ActionLists);
                    _actionLists.Add(new ActionList(this));
                }

                return _actionLists;
            }
        }

        public override TemplateGroupCollection TemplateGroups
        {
            get
            {
                var templateGroups = new TemplateGroupCollection();
                var control = (CaptchaUltimateControl) Component;
                var group = new TemplateGroup("Templates");
                var template =
                    new TemplateDefinition(this, "ItemTemplate", control, "ItemTemplate", false);
                group.AddTemplateDefinition(template);
                templateGroups.Add(group);
                return templateGroups;
            }
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            SetViewFlags(ViewFlags.TemplateEditing, true);
        }

        private void ConvertToTemplate()
        {
            InvokeTransactedChange(Component, ConvertToTemplateChangeCallback, null,
                                   "Convert TemplateControl to template");
        }

        private bool ConvertToTemplateChangeCallback(object context)
        {
            PropertyDescriptor templateDescriptor = TypeDescriptor.GetProperties(Component).Find("ItemTemplate", false);
            ITemplate template = new DefaultTemplate();
            templateDescriptor.SetValue(Component, template);
            return true;
        }

        private void Reset()
        {
            InvokeTransactedChange(Component, ResetChangeCallback, null, "Reset TemplateControl");
        }

        private bool ResetChangeCallback(object context)
        {
            PropertyDescriptor templateDescriptor = TypeDescriptor.GetProperties(Component).Find("ItemTemplate", false);
            templateDescriptor.SetValue(Component, null);
            return true;
        }

        #region Nested type: ActionList

        public class ActionList : DesignerActionList
        {
            private readonly TemplateControlDesigner _parent;
            private DesignerActionItemCollection _items;

            public ActionList(TemplateControlDesigner parent)
                : base(parent.Component)
            {
                _parent = parent;
            }

            public override DesignerActionItemCollection GetSortedActionItems()
            {
                if (_items == null)
                {
                    _items = new DesignerActionItemCollection();
                    _items.Add(new DesignerActionMethodItem(this, "ConvertToTemplate", "Convert to template", true));
                    _items.Add(new DesignerActionMethodItem(this, "Reset", "Reset", true));
                }

                return _items;
            }

            private void ConvertToTemplate()
            {
                _parent.ConvertToTemplate();
            }

            private void Reset()
            {
                _parent.Reset();
            }
        }

        #endregion

        #region Nested type: DefaultTemplate

        private class DefaultTemplate : ITemplate
        {
            #region ITemplate Members

            public void InstantiateIn(Control container)
            {
                // The next several parameters are hard coded.  It would be nice
                // to be able to pull these from the control but I don't yet know
                // how to do that.
                const string Title = "Captcha Control";
                const bool ShowTitle = true;
                const string InvalidCaptchaMessage = "***";
                const bool ShowRecalculateCaptchaButton = true;
                const string ButtonRedisplayCaptchaText = "Generate New Display Number";

                var table = new HtmlTable();
                table.Border = 1;
                table.BgColor = "Aqua";

                var tableRow = new HtmlTableRow();

                var tableCell = new HtmlTableCell();
                tableCell.Attributes.Add("style", "text-align: center;");
                tableCell.ColSpan = 2;
                var label = new Label();
                label.ID = "TitleID";
                label.Text = Title;
                label.Visible = ShowTitle;
                tableCell.Controls.Add(label);
                tableRow.Controls.Add(tableCell);
                table.Controls.Add(tableRow);
                // Row Break

                tableRow = new HtmlTableRow();
                tableCell = new HtmlTableCell();
                var _userInput = new TextBox();
                _userInput.ID = "VerificationID";
                tableCell.Controls.Add(_userInput);
                tableRow.Controls.Add(tableCell);

                tableCell = new HtmlTableCell();
                var _validator = new CustomValidator();
                _validator.Text = InvalidCaptchaMessage;
                _validator.ID = "CustomValidatorID";
                tableCell.Controls.Add(_validator);
                tableRow.Controls.Add(tableCell);
                table.Controls.Add(tableRow);
                // Row Break

                tableRow = new HtmlTableRow();
                tableCell = new HtmlTableCell();
                tableCell.Attributes.Add("style", "text-align: center;");
                tableCell.ColSpan = 2;
                var _image = new Image();
                _image.ID = "CaptchaImageID";
                _image.ImageUrl = "~/CaptchaType.ashx"; // this will get overridden at runtime

                tableCell.Controls.Add(_image);
                tableRow.Controls.Add(tableCell);
                table.Controls.Add(tableRow);
                // Row Break

                tableRow = new HtmlTableRow();
                tableCell = new HtmlTableCell();
                tableCell.Attributes.Add("style", "text-align: center;");
                tableCell.ColSpan = 2;
                var _redisplayButton = new Button();
                _redisplayButton.ID = "ButtonDisplayNextID";
                _redisplayButton.Visible = ShowRecalculateCaptchaButton;
                _redisplayButton.Text = ButtonRedisplayCaptchaText;
                tableCell.Controls.Add(_redisplayButton);
                tableRow.Controls.Add(tableCell);
                table.Controls.Add(tableRow);
            }

            #endregion
        }

        #endregion
    }
}