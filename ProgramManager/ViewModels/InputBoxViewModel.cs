using System.Windows;
using System.Windows.Input;
using ProgramManager.Converters;
using ProgramManager.Enums;
using ProgramManager.ViewModels.Base;

namespace ProgramManager.ViewModels
{
    /// <summary>
    /// Модель представления для окна ввод
    /// </summary>
    class InputBoxViewModel : PropertiesChanged
    {
        private string _text;
        private ICommand _action;
        private Actions _userAction;
        /// <summary>
        /// Свойство содержит имя действия, которое должен 
        /// совершить пользователь при нажатии этой <see cref="Views.InputBox.Action1"/> кнопки.
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
        /// За логику отвечает конвертер <converter cref="ResourceNameValidation"/>, 
        /// а также свойство <property cref="Text"/> которое обновляет это свойство.
        /// </summary>
        public bool IsEnableAction { get; set; }
        /// <summary>
        /// Свойство <see cref="Text"/> содержит имя которое пользователь
        /// вводит в поле окна <see cref="Views.InputBox.TextBox1"/>.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
                OnPropertyChanged("IsEnableAction");
            }
        }
        /// <summary>
        /// Команда <see cref="Action"/> отвечает за действие которое необходимо выполнить,
        /// реализация команды находится здесь <inheritdoc cref="IconEditorViewModel.RenameIcon"/>
        /// </summary>
        public ICommand Action
        {
            get { return _action; }
            set
            {
                _action = value;
                OnPropertyChanged("Action");
            }
        }
        /// <summary>
        /// Команда <see cref="Cancel"/> отвечает за безопасное закрытие окна ввода имени.
        /// </summary>
        public ICommand Cancel => new RelayCommand(obj =>
        {
            // Так как поля Store хранит значения, которое используется для проверки существования ресурса 
            // в словаре ресурсов, его необходимо чистить после изменения имени ресурса, данная строка решает эту проблему.
            // TODO: Найти другой способ занулить это свойство, чтобы этот класс не знал про существования данного конвертера, а сам конвертер был самодостаточным.
            //ResourceNameValidation.Store = null;
            Window win = (Window) obj;
            win.Visibility = Visibility.Hidden;
        });
    }
}
