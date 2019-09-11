using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace ProgramManager.Models.Settings
{
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    class GeneralModel
    {
        private readonly XElement _documentRoot;
        private const string DocumentName = "../../Resources/Settings.xml"; 

        public GeneralModel()
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

        public ThemesModel Themes { get; set; }
    }
}
