using MyCryptographyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.Converters
{
    internal class OperationArgumentDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PathArgumentTemplate { get; set; }

        public DataTemplate KeyArgumentTemplate { get; set; }


        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case PathOperationArgumentViewModel:
                    return PathArgumentTemplate;
                case KeyOperationArgumentViewModel:
                    return KeyArgumentTemplate;
                default:
                    throw new NotImplementedException();
            };
        }
    }
}
