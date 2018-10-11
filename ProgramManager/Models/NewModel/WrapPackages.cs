using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ProgramManager.Models.NewModel
{
    public class WrapPackages
    {
        public string Name { get; set; }
        public List<ProgramModel> Packages { get; set; }
        /// <summary>
        /// Метод совершает обертывания объектов потомков PackageBase, создает оболочку для представления.
        /// Основная идея создания оболочки для представления, это привязка объектов(пакетов) к определнным именам (тегам),
        /// обращение к объектам из предстваления по их именам позволеяет отсеивать лишние объекты
        /// не пренадлижащие текущиему контексту. Метод упаковывает объекты в свойство коллекции Packages.
        /// </summary>
        /// <param name="collection">Применает коллекцию объектов типа ProgramModel</param>
        /// <returns>Возращает объект оболочку для предстваления</returns>
        public static List<WrapPackages> WrapTagModel(List<ProgramModel> @collection)
        {
            List<WrapPackages> tags = TagFinder();
            int index = 0;

            foreach (var tag in tags)
            {
                tags[index].Packages = new List<ProgramModel>();

                foreach (var package in @collection)
                {
                    if (package.TagOne != null)
                    {
                        if (tag.Name.Contains(package.TagOne))
                            tags[index].Packages.Add(package);
                    }
                }
                // Данная часть кода отвечает за пакеты которые могут иметь больше одного тега 
                foreach (var package in @collection)
                {
                    if (package.TagList.Count != 0)
                    {
                        var count = tags[index].Packages.Count;

                        foreach (var packageTag in package.TagList)
                        {
                            if (tag.Name.Contains(packageTag))
                                tags[index].Packages.Insert(count, package);
                        }
                    }
                }
                index++;
            }
            return tags;
        }
        /// <summary>
        /// Метод является вспомогательным для метода WrapTagModel который призыван совершать поиск
        /// по xml документу и выберать только те значения которые явлеяються уникальными.
        /// </summary>
        /// <returns>Возращает коллекцию уникальных значений</returns>
        private static List<WrapPackages> TagFinder()
        {
            string xmlDoc = "ProgramPackages.xml";
            XElement root = XElement.Load(xmlDoc);

            var queryTag = (from e in root.Descendants("Package").Elements()
                where e.Name == "Tag"
                select e.Value).ToList();

            var queryTags = root.Descendants("Package").Elements().Elements()
                .Where(e => e.Name == "Tag")
                .Select(e => e.Value).ToList();

            queryTag.AddRange(queryTags);

            return queryTag.Distinct().OrderBy(x => x.Substring(0, 3)).Select(element => new WrapPackages() { Name = element }).ToList();
        }
    }
}
