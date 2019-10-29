using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESL.DataLayer.MetaData
{
    internal class MetaData_Menu
    {
        public int Menu_ID { get; set; }
        public System.Guid Menu_Guid { get; set; }
        public string Menu_Name { get; set; }
        public string Menu_Display { get; set; }
        public int Menu_Order { get; set; }
        public bool Menu_HasSubMenu { get; set; }
    }

    [MetadataType(typeof(MetaData_Menu))]
    public partial class Tbl_Menu { }
}
