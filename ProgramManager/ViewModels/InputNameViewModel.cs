<<<<<<< HEAD
﻿
namespace ProgramManager.ViewModels
{
    using System.Windows.Input;
    using Base;
    using GalaSoft.MvvmLight.Messaging;
    using Views.DialogPacks;

    /// <summary>
    /// The input name view model.
    /// </summary>
    public class InputNameViewModel
    {
        /// <summary>
        /// The input name.
        /// </summary>
        private static InputName _inputName;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The input name.
        /// </summary>
        public ICommand InputName => new RelayCommand(obj => Messenger.Default.Send(new InputNameViewModel { Name = Name }));

        /// <summary>
        /// The cancel.
        /// </summary>
        public ICommand Cancel => new RelayCommand(obj =>
        {
            _inputName = obj as InputName;
            _inputName?.Close();
        });
    }
}
=======
﻿using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Views.DialogPacks;

namespace ProgramManager.ViewModels
{
    public class InputNameViewModel
    {
        private static InputName _inputName;
        public string Name { get; set; }
        public ICommand InputName => new RelayCommand(obj => Messenger.Default.Send(new InputNameViewModel { Name = Name }));
        public ICommand Cancel => new RelayCommand(obj =>
        {
            _inputName = obj as InputName;
            if (_inputName != null) _inputName.Close();
        });
    }
}
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
