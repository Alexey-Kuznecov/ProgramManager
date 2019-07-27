using System;
using System.Windows.Media;

namespace ProgramManager.Plugins
{
    /// <summary>
    /// Dialog package data, declared here properties can be using
    /// for creating the package editor plugins.
    /// </summary>
    struct InteractonPackageEditor
    {
        private static string _iconDefault;
        private static SolidColorBrush _iconBackDefault;
        private static SolidColorBrush _iconForeDefault;
        /// <summary>
        /// This property used to set icon name by defualt.
        /// </summary>
        public static string IconDefault
        {
            get
            {
                if (_iconDefault == null)
                    _iconDefault = "AddNewIcon";
                return _iconDefault;
            }
            set { _iconDefault = value; }
        }
        /// <summary>
        /// This property used to set icon background by defualt.
        /// </summary>
        public static SolidColorBrush IconBackDefault
        {
            get
            {
                if (_iconBackDefault == null)
                    _iconBackDefault = new SolidColorBrush(Color.FromRgb(54, 118, 174));
                return _iconBackDefault;
            }
            set { _iconBackDefault = value; }
        }
        /// <summary>
        /// This property used to set icon foreground by defualt.
        /// </summary>
        public static SolidColorBrush IconForeDefault
        {
            get
            {
                if (_iconForeDefault == null)
                    _iconForeDefault =  new SolidColorBrush(Color.FromRgb(255,255,255));
                return _iconForeDefault;
            }
            set { _iconForeDefault = value; }
        }
    }
}
