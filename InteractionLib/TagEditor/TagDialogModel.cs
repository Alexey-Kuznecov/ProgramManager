
namespace InteractionLib.TagEditor
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using AlexLibWpf.Mvvm.Base;

    /// <summary>
    /// The tag dialog model.
    /// </summary>
    public class TagDialogModel : PropertiesChanged
    {
        /// <summary>
        /// The tag is checked.
        /// </summary>
        private bool _isChecked;

        /// <summary>
        /// Gets or sets the tag name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether
        /// the tag is selected in the tags list.
        /// </summary>
        public bool IsChecked
        {
            get => this._isChecked;
            set
            {
                this._isChecked = value;
                this.OnPropertyChanged("IsChecked");
            }
        }

        /// <summary>
        /// Command marks the tag as select.
        /// </summary>
        public ICommand Checked => new RelayCommand(obj =>
        {
            if ((bool)obj)
            {
                List.Add(Name);
            }
            else
            {
                List.Remove(Name);
            }
        });

        /// <summary>
        /// Gets or sets the tags list.
        /// </summary>
        protected List<string> List { get; set; }
    }
}
