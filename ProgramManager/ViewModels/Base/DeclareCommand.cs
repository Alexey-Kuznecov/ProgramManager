using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramManager.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace ProgramManager.ViewModels
{
    public partial class MainViewModel
    {
        public void FixStatePackage()
        {
            if (_filterPackages == null || _filterPackages == "")
                _storage = Packages;
            else
                Packages = _storage;
        }
        private void SetCurrentPackage(PackageModel obj)
        {
            List<PackageModel> current = new List<PackageModel>();

            current.Add(new PackageModel() { Label = "Имя:", Field = obj.Name });
            current.Add(new PackageModel() { Label = "Автор:", Field = obj.Author });
            current.Add(new PackageModel() { Label = "Версия:", Field = obj.Version });
            current.Add(new PackageModel() { Label = "Категория:", Field = obj.Category });
            current.Add(new PackageModel() { Label = "Тип:", Field = obj.Subcategory });
            current.Add(new PackageModel() { Label = "Описания:", Field = obj.Description });

            _currentPackage = current;
        }
    }
}
