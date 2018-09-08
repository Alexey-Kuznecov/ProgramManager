using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ProgramManager.Model
{
    class PackageAccess
    {
        internal static List<PackageModel> GetPackages()
        {
            List<PackageModel> package = new List<PackageModel>() {
                new PackageModel {
                    Name = "Photoshop",
                    Author = "Adobe",
                    Category = "Программы",
                    Subcategory = "Редактор графики",
                    Version = "Cloud CC6",
                    Description = "Программа для оформления фотографий",
                },
                new PackageModel {
                    Name = "Illustrator",
                    Author = "Adobe",
                    Category = "Программы",
                    Subcategory = "Редактор графики",
                    Version = "Cloud CC6",
                    Description = "Программа для оформления фотографий"
                },
                new PackageModel {
                    Name = "Axure",
                    Author = "Axure Software Solution",
                    Category = "Программы",
                    Subcategory = "Инструменты вебразработчика",
                    Version = "RP 8",
                    Description = "Программа для создания прототипов сайтов"
                 },
                 new PackageModel {
                    Name = "LazyCure",
                    Author = "Andrei Kulabukhau",
                    Category = "Программы",
                    Subcategory = "Органазеры",
                    Version = "0.10",
                    Description = "Программа для измерения времени"
                 },
                 new PackageModel {
                    Name = "Firefox",
                    Author = "Mozilla",
                    Category = "Программы",
                    Subcategory = "Браузеры",
                    Version = "7.9.4",
                    Description = "Программа для просмотра вебстраниц"
                 },
                 new PackageModel {
                    Name = "Chrome",
                    Author = "Google",
                    Category = "Программы",
                    Subcategory = "Браузеры",
                    Version = "8.5.0",
                    Description = "Программа для просмотра вебстраниц"
                 },
                 new PackageModel {
                    Name = "Microsoft Outlook",
                    Author = "Microsoft",
                    Category = "Программы",
                    Subcategory = "Офисные",
                    Version = "2017",
                    Description = "Программа для организации электронных писем"
                 },
                 new PackageModel {
                    Name = "Microsoft Word",
                    Author = "Microsoft",
                    Category = "Программы",
                    Subcategory = "Офисные",
                    Version = "2017",
                    Description = "Программа для написания, редактирования, оформления текста и многое другое."
                 },
                 new PackageModel {
                    Name = "CCleaner",
                    Author = "Piriform",
                    Category = "Программы",
                    Subcategory = "Оптимизатры",
                    Version = "5.43.6520",
                    Description = "Программа очиски компьютора"
                 },
                 new PackageModel {
                    Name = "Reg Organizer",
                    Author = "ChemTable Software",
                    Category = "Программы",
                    Subcategory = "Оптимизатры",
                    Version = "8.16",
                    Description = "Программа для оптимизации, отчиски, дефрагментации реестра и многое другое"
                 },
                 new PackageModel {
                    Name = "LeaderTask",
                    Author = "Неизвестно",
                    Category = "Программы",
                    Subcategory = "Органазеры",
                    Version = "6.0",
                    Description = "Программа организации задач"
                 },
                 new PackageModel {
                    Name = "Kasperski",
                    Author = "Kasperski Lab",
                    Category = "Программы",
                    Subcategory = "Антивирусы",
                    Version = "6.0",
                    Description = "Программа для защиты компютера от вирусов"
                 },
                 new PackageModel {
                    Name = "Sublime Text 3",
                    Author = "HQ Pty Ltd",
                    Category = "Программы",
                    Subcategory = "Редакторы кода",
                    Version = "3.1.1 Build 3176",
                    Description = "Многофункциональный редактор кода"
                 },
                 new PackageModel {
                    Name = "Notepad++",
                    Author = "Notepad++Team",
                    Category = "Программы",
                    Subcategory = "Редакторы кода",
                    Version = "7.4.2",
                    Description = "Удобный и мультифункциональный редактор кода"
                 },
                 new PackageModel {
                    Name = "Microsoft Visual Studio",
                    Author = "Microsoft",
                    Category = "Программы",
                    Subcategory = "IDE",
                    Version = "14.0",
                    Description = "Интегрированая среда разработки"
                 },
                 new PackageModel {
                    Name = "Zbrush",
                    Author = "Poligonic",
                    Category = "Программы",
                    Subcategory = "3D Редакторы",
                    Version = "10",
                    Description = "Программа для создания 3D моделей"
                 },
                 new PackageModel {
                    Name = "NetBeans",
                    Author = "NetBeans Software",
                    Category = "Программы",
                    Subcategory = "IDE",
                    Version = "9.1.0",
                    Description = "Интегрированая среда разработки"
                 },
                 new PackageModel {
                    Name = "Discord",
                    Author = "Discord Team",
                    Category = "Программы",
                    Subcategory = "Меседжеры",
                    Version = "3.0",
                    Description = "Программа для создания сообществ"
                 },
                 new PackageModel {
                    Name = "Slack",
                    Author = "Slack Team",
                    Category = "Программы",
                    Subcategory = "Меседжеры",
                    Version = "2.2",
                    Description = "Программа для создания сообществ"
                 },
                 new PackageModel {
                    Name = "The Witcher 3 Wild Hunt",
                    Author = "CD Project Red",
                    Category = "Игры",
                    Subcategory = "RPG",
                    Version = "3.31",
                    Description = "Игра для романтиков на досуге мясников"
                 },
                 new PackageModel {
                    Name = "Empire Earth 2",
                    Author = "Mod Doc Software",
                    Category = "Игры",
                    Subcategory = "Стратегии",
                    Version = "2.2",
                    Description = "Игра для настоящих царей"
                 },
                 new PackageModel {
                    Name = "High Definition Audio",
                    Author = "Realtek",
                    Category = "Драйвера",
                    Subcategory = "Звуковая карта",
                    Version = "3.31",
                    Description = ""
                 },
                 new PackageModel {
                    Name = "Logitech HD Webcam C270",
                    Author = "Logitech",
                    Category = "Драйвера",
                    Subcategory = "Аксессуары",
                    Version = "2.2",
                    Description = ""
                 },
                 new PackageModel {
                    Name = "High Definition Audio",
                    Author = "Realtek",
                    Category = "Драйвера",
                    Subcategory = "Звуковая карта",
                    Version = "3.31",
                    Description = ""
                 },
                 new PackageModel {
                    Name = "1.000 Times Better 2.3.1",
                    Author = "Logitech",
                    Category = "Моды",
                    Subcategory = "Реворк",
                    Version = "2.3.1",
                    Description = "Качественный мод для 3 Ведьмака, который улучшит качество графики в игре."
                 },
                 new PackageModel {
                    Name = "Counter Strike GO",
                    Author = "Valve",
                    Category = "Игры",
                    Subcategory = "Shooter",
                    Version = "2.2",
                    Description = "Игра для настоящих ассов"
                 },
            };

            return package;
        }

        internal static void UpdatePackage(PackageModel _currentCustomer)
        {
            throw new NotImplementedException();
        }

        internal static void InsertPackage(object _newCustomer)
        {
            throw new NotImplementedException();
        }

    }
}
