
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml.Linq;
using ProgramManager.ViewModels.Base;

namespace ProgramManager.ViewModels.Settings
{
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    class GeneralViewModel : PropertiesChanged
    {
        private readonly XElement _documentRoot;
        private const string DocumentName = "../../Resources/Settings.xml";

        public GeneralViewModel()
        {
            _documentRoot = XElement.Load(DocumentName);
        }

        public bool AutoStartApp
        {
            get {
                return bool.Parse(_documentRoot.Element("AutoStartApp")?
                    .FirstAttribute.Value);
            }
            set {
                _documentRoot.Element("AutoStartApp").FirstAttribute.Value = value.ToString();
                _documentRoot.Save(DocumentName);
            }
        }

        public bool UpdateNotify
        {
            get {
                return bool.Parse(_documentRoot.Element("UpdateNotify")?
                    .FirstAttribute.Value);
            }
            set {
                _documentRoot.Element("UpdateNotify").FirstAttribute.Value = value.ToString();
                _documentRoot.Save(DocumentName);
            }
        }

        public bool ShownPackBack
        {
            get {
                return bool.Parse(_documentRoot.Element("ShownPackBack")?
                    .FirstAttribute.Value);
            }
            set {
                _documentRoot.Element("ShownPackBack").FirstAttribute.Value = value.ToString();
                _documentRoot.Save(DocumentName);
            }
        }

        public string Theme
        {
            get { return _documentRoot.Element("Themes")?
                .FirstAttribute.Value; }
            set
            {
                _documentRoot.Element("Themes").FirstAttribute.Value = value;
                _documentRoot.Save(DocumentName);
            }
        }

        public Array Themes
        {
            get {
                return _documentRoot.Element("Themes").Elements()
                    .Select(p => p.FirstAttribute.Value).ToArray();
            }
        }
    }
}
