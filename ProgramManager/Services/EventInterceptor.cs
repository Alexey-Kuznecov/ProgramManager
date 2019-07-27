using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramManager.Models;
using ProgramManager.ViewModels;

namespace ProgramManager.Services
{
    class EventInterceptor
    {
        public static List<TagDialogModel> ConvertTotype(EventAggregate e, object package)
        {
            var listTags = new List<TagDialogModel>();
            var wrap = package as List<WrapPackage>;

            if (wrap != null)
                foreach (var tag in wrap)
                    listTags.Add(new TagDialogModel {Name = tag.Name});
            return listTags;
        }
    }
}
