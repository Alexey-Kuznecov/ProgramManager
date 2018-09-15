using System.Collections.Generic;
using ProgramManager.Models;

namespace ProgramManager.ViewModels
{
    public partial class MainViewModel
    {
        private void FixStatePackage()
        {
            if (_filterPackages == null || _filterPackages == "")
                _storage = Packages;
            else
                Packages = _storage;
        }
        private void SetCurrentPackage(PackageModel obj)
        {
            List<DataPanelModel> current = new List<DataPanelModel>();

            current.Add(new DataPanelModel() { Label = "Имя:", Field = obj.Name });
            current.Add(new DataPanelModel() { Label = "Автор:", Field = obj.Author });
            current.Add(new DataPanelModel() { Label = "Версия:", Field = obj.Version });
            current.Add(new DataPanelModel() { Label = "Категория:", Field = obj.Category });
            current.Add(new DataPanelModel() { Label = "Тип:", Field = obj.Subcategory });
            current.Add(new DataPanelModel() { Label = "Описания:", Field = obj.Description });

            _currentPackage = current;
        }
    }
}
