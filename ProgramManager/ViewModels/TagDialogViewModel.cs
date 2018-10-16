using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Models;
using ProgramManager.ViewModels.Base;

namespace ProgramManager.ViewModels
{
    public class TagDialogModel
    {
        public string Name { get; set; }
        public bool TagChecked { get; }

        //public ICommand Checked => new RelayCommand(obj =>
        //{
        //    MessageBox.Show("it works! " + obj.ToString());
        //    //List<string> tags = new List<string> { obj.ToString() };
        //});
    }

    public class TagDialogViewModel : PropertiesChanged
    {
        public static ObservableCollection<TagDialogModel> TagList { get; set; }

        public static void DisplayTagList(object sender, ConnectorEventArgs obj)
        {


            List<WrapPackage> packs = obj.Package as List<WrapPackage>;
            TagList = new ObservableCollection<TagDialogModel>();

            if (packs != null)
            {
                foreach (var item in packs)
                    TagList.Add(new TagDialogModel() { Name = item.Name });                             
            }

        }      
    }
}
