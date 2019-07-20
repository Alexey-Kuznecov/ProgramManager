using System;
using System.IO;
using ProgramManager.Contracts;
using System.Windows;
using System.Windows.Markup;

namespace ProgramManager.Services
{
    class XamlFileService : IFileService
    {
        /// <summary>
        /// Загружает корневой элемент из файла словаря ресурсов.
        /// </summary>
        /// <param name="filepath">Путь к файлу ресурсов.</param>
        /// <returns>Корневой элемент.</returns>
        public object Open(string filepath)
        {
            DependencyObject rootXaml;
            try
            {
                using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate))
                    rootXaml = (DependencyObject)XamlReader.Load(fs);
                return rootXaml;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
        /// <summary>
        /// Сохраняет(сериализует) словарь ресурсов. Словарь ресурсов обязательно должен 
        /// состоять из корневого элемента ResourceDictionary.
        /// </summary>
        /// <param name="resPath">Путь к словаря ресурсов.</param>
        /// <param name="resDict">Словарь корнем которого ожидается элемент ResourceDictionary</param>
        public void Save(string resPath, object resDict)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(resPath))
                {
                    XamlWriter.Save(resDict, writer);
                } 
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }   
        }
    }
}
