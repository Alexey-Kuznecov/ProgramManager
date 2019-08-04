using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using ProgramManager.Converters;
using ProgramManager.ViewModels.Base;

namespace ProgramManager.Components.InputBox
{
    /// <summary>
    /// Модель представления для окна ввод
    /// </summary>
    class InputBoxViewModel : PropertiesChanged
    {
        private string _text;
        private Actions _userAction;
        public static List<string> ForbidWord = new List<string>(new []{ "dick", "fool", "Bitch", ""});
        /// <summary>
        /// Свойство содержит имя действия, которое должен 
        /// совершить пользователь при нажатии кнопки.
        /// </summary>
        public Actions UserAction
        {
            get { return _userAction; }
            set
            {
                _userAction = value;
                SetProperty(ref _userAction, value, () => UserAction);
            }
        }
        /// <summary>
        /// Блокирует кнопку если ListBox содержит имя исключение и разблокирует если не содержит.
        /// </summary>
        public bool IsEnableAction { get; set; } = true;
        /// <summary>
        /// Свойство <see cref="Text"/> содержит имя которое пользователь
        /// вводит в поле окна/>.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                IsEnableAction = !ForbidWord.Contains(_text);

                OnPropertyChanged("Text");
                OnPropertyChanged("IsEnableAction");
            }
        }
        /// <summary>
        /// Команда <see cref="Action"/> отвечает за действие которое необходимо выполнить.
        /// </summary>
        public ICommand Action { get; set; }
        /// <summary>
        /// Команда <see cref="Cancel"/> отвечает за безопасное закрытие окна ввода имени.
        /// </summary>
        public ICommand Cancel => new RelayCommand(obj =>
        {
            Window win = (Window) obj;
            win.Visibility = Visibility.Hidden;
        });
        /// <summary>
        /// Intializaion inputbox by constructor argument 
        /// </summary>
        /// <param name="text">Text to be transferred to the window text box.</param>
        /// <param name="action">Action that to be performed.</param>
        /// <param name="actionType">Action type that be interpreted how button press.</param>
        public InputBoxViewModel(ICommand action, Actions actionType, string text)
        {
            ForbidWord.Add(text);
            Text = text;
            UserAction = actionType;
            Action = action;
        }
    }
}
