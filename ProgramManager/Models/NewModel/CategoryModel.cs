using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramManager.Models.NewModel
{
    public class CategoryModel
    {
        public string Name { get; set; }

        public static List<CategoryModel> Categories { get; set; }
    }
}
