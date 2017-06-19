using System;
using System.Linq;
using SenseNet.ApplicationModel;
using SenseNet.Configuration;
using SenseNet.ContentRepository;
using SenseNet.ContentRepository.Schema;
using SenseNet.ContentRepository.Storage;

namespace SenseNet.Portal.Portlets.ContentHandlers
{
    [ContentHandler]
    public class SurveyList : ContentList
    {
        // ================================================================================ Constructors

        public SurveyList(Node parent) : this(parent, null) { }
        public SurveyList(Node parent, string nodeTypeName) : base(parent, nodeTypeName) { }
        protected SurveyList(NodeToken nt) : base(nt)	{ }

        // ================================================================================ Properties

        private const string EMAILLIST_PROPERTY = "EmailList";
        [RepositoryProperty(EMAILLIST_PROPERTY, RepositoryDataType.Text)]
        public string EmailList
        {
            get { return base.GetProperty<string>(EMAILLIST_PROPERTY); }
            set { base.SetProperty(EMAILLIST_PROPERTY, value); }
        }

        private const string MAILSUBJECT_PROPERTY = "MailSubject";
        [RepositoryProperty(MAILSUBJECT_PROPERTY, RepositoryDataType.String)]
        public string MailSubject
        {
            get { return base.GetProperty<string>(MAILSUBJECT_PROPERTY); }
            set { base.SetProperty(MAILSUBJECT_PROPERTY, value); }
        }

        private const string EMAILFROM_PROPERTY = "EmailFrom";
        [RepositoryProperty(EMAILFROM_PROPERTY, RepositoryDataType.String)]
        public string EmailFrom
        {
            get { return base.GetProperty<string>(EMAILFROM_PROPERTY); }
            set { base.SetProperty(EMAILFROM_PROPERTY, value); }
        }

        private const string EMAILFIELD_PROPERTY = "EmailField";
        [RepositoryProperty(EMAILFIELD_PROPERTY, RepositoryDataType.String)]
        public string EmailField
        {
            get { return base.GetProperty<string>(EMAILFIELD_PROPERTY); }
            set { base.SetProperty(EMAILFIELD_PROPERTY, value); }
        }

        private const string ADMINEMAILTEMPLATE_PROPERTY = "AdminEmailTemplate";
        [RepositoryProperty(ADMINEMAILTEMPLATE_PROPERTY, RepositoryDataType.Text)]
        public string AdminEmailTemplate
        {
            get { return base.GetProperty<string>(ADMINEMAILTEMPLATE_PROPERTY); }
            set { base.SetProperty(ADMINEMAILTEMPLATE_PROPERTY, value); }
        }

        private const string SUBMITTEREMAILTEMPLATE_PROPERTY = "SubmitterEmailTemplate";
        [RepositoryProperty(SUBMITTEREMAILTEMPLATE_PROPERTY, RepositoryDataType.Text)]
        public string SubmitterEmailTemplate
        {
            get { return base.GetProperty<string>(SUBMITTEREMAILTEMPLATE_PROPERTY); }
            set { base.SetProperty(SUBMITTEREMAILTEMPLATE_PROPERTY, value); }
        }

        private const string ONLYSINGLERESPONSE_PROPERTY = "OnlySingleResponse";
        [RepositoryProperty(ONLYSINGLERESPONSE_PROPERTY, RepositoryDataType.Int)]
        public bool OnlySingleResponse
        {
            get { return base.GetProperty<int>(ONLYSINGLERESPONSE_PROPERTY) != 0; }
            set { base.SetProperty(ONLYSINGLERESPONSE_PROPERTY, value ? 1 : 0); }
        }

        private const string ENABLENOTIFICATIONEMAIL_PROPERTY = "EnableNotificationMail";
        [RepositoryProperty(ENABLENOTIFICATIONEMAIL_PROPERTY, RepositoryDataType.Int)]
        public bool EnableNotificationMail
        {
            get { return base.GetProperty<int>(ENABLENOTIFICATIONEMAIL_PROPERTY) != 0; }
            set { base.SetProperty(ENABLENOTIFICATIONEMAIL_PROPERTY, value ? 1 : 0); }
        }

        // ================================================================================ Overrides

        public override object GetProperty(string name)
        {
            switch (name)
            {
                case EMAILLIST_PROPERTY:
                    return this.EmailList;
                case MAILSUBJECT_PROPERTY:
                    return this.MailSubject;
                case EMAILFROM_PROPERTY:
                    return this.EmailFrom;
                case EMAILFIELD_PROPERTY:
                    return this.EmailField;
                case ADMINEMAILTEMPLATE_PROPERTY:
                    return this.AdminEmailTemplate;
                case SUBMITTEREMAILTEMPLATE_PROPERTY:
                    return this.SubmitterEmailTemplate;
                case ONLYSINGLERESPONSE_PROPERTY:
                    return this.OnlySingleResponse;
                case ENABLENOTIFICATIONEMAIL_PROPERTY:
                    return this.EnableNotificationMail;
                default:
                    return base.GetProperty(name);
            }
        }
        public override void SetProperty(string name, object value)
        {
            switch (name)
            {
                case EMAILLIST_PROPERTY:
                    this.EmailList = (string)value;
                    break;
                case MAILSUBJECT_PROPERTY:
                    this.MailSubject = (string)value;
                    break;
                case EMAILFROM_PROPERTY:
                    this.EmailFrom = (string)value;
                    break;
                case EMAILFIELD_PROPERTY:
                    this.EmailField = (string)value;
                    break;
                case ADMINEMAILTEMPLATE_PROPERTY:
                    this.AdminEmailTemplate = (string)value;
                    break;
                case SUBMITTEREMAILTEMPLATE_PROPERTY:
                    this.SubmitterEmailTemplate = (string)value;
                    break;
                case ONLYSINGLERESPONSE_PROPERTY:
                    this.OnlySingleResponse = (bool)value;
                    break;
                case ENABLENOTIFICATIONEMAIL_PROPERTY:
                    this.EnableNotificationMail = (bool)value;
                    break;
                default:
                    base.SetProperty(name, value);
                    break;
            }
        }

        // ================================================================================ Public API

        /// <summary>
        /// Checks whether the survey has already been filled at least one time by the current user.
        /// </summary>
        public bool IsFilled()
        {
            return IsFilled(User.Current as User);
        }
        /// <summary>
        /// Checks whether the survey has already been filled at least one time by the provided user.
        /// </summary>
        public bool IsFilled(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Content.All.DisableAutofilters().Any(c => c.InTree(this) && c.TypeIs("SurveyListItem") && (int)c["CreatedById"] == user.Id);
        }
        /// <summary>
        /// Checks whether the provided survey has already been filled at least one time by the current user.
        /// </summary>
        /// <param name="content">SurveyList content.</param>
        [ODataFunction]
        public static object IsFilled(Content content)
        {
            // For security reasons this OData function must not allow the client to
            // specifiy a different user other than the current one.

            if (content == null || !content.ContentType.IsInstaceOfOrDerivedFrom(typeof(SurveyList).Name))
                throw new InvalidOperationException("This action can only be called on a Survey.");

            return new
            {
                isFilled = ((SurveyList)content.ContentHandler).IsFilled()
            };
        }

        // ================================================================================ Helper methods

        internal string GetSenderEmail()
        {
            var sender = EmailFrom;
            return !string.IsNullOrEmpty(sender) ? sender : Notification.DefaultEmailSender;
        }
    }
}
