
namespace ProgramManager.ViewModels.Settings
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using Base;

    /// <summary>
    /// The general view model.
    /// </summary>
    public class GeneralViewModel : PropertiesChanged
    {
        /// <summary>
        /// The document name.
        /// </summary>
        private const string DocumentName = "../../Resources/Settings.xml";

        /// <summary>
        /// The document root.
        /// </summary>
        private readonly XElement _documentRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralViewModel"/> class.
        /// </summary>
        public GeneralViewModel()
        {
            this._documentRoot = XElement.Load(DocumentName);
        }

        /// <summary>
        /// Gets or sets a value indicating whether applications start when the system starts.
        /// </summary>
        public bool AutoStart
        {
            get => bool.Parse(this._documentRoot.Element("AutoStartApp")?.FirstAttribute.Value ?? throw new InvalidOperationException());
            set
            {
                if (this._documentRoot != null)
                {
                    this._documentRoot.Element("AutoStartApp").FirstAttribute.Value = value.ToString();
                    this._documentRoot.Save(DocumentName);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether update notify.
        /// </summary>
        public bool UpdateNotify
        {
            get => bool.Parse(this._documentRoot.Element("UpdateNotify")?.FirstAttribute.Value ?? throw new InvalidOperationException());
            set
            {
                var documentRoot = this._documentRoot;

                if (documentRoot != null)
                {
                    documentRoot.Element("UpdateNotify").FirstAttribute.Value = value.ToString();
                    documentRoot.Save(DocumentName);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether shown package background.
        /// </summary>
        public bool ShownPackBackground
        {
            get => bool.Parse(this._documentRoot.Element("ShownPackBack")?.FirstAttribute.Value ?? throw new InvalidOperationException());
            set
            {
                if (this._documentRoot != null)
                {
                    this._documentRoot.Element("ShownPackBack").FirstAttribute.Value = value.ToString();
                    this._documentRoot.Save(DocumentName);
                }
            }
        }

        /// <summary>
        /// Gets or sets the theme.
        /// </summary>
        public string Theme
        {
            get => this._documentRoot.Element("Themes")?.FirstAttribute.Value;
            set
            {
                this._documentRoot.Element("Themes").FirstAttribute.Value = value;
                this._documentRoot.Save(DocumentName);
            }
        }

        /// <summary>
        /// Gets themes list.
        /// </summary>
        public Array Themes => this._documentRoot.Element("Themes")?.Elements().Select(p => p.FirstAttribute.Value).ToArray();
    }
}
