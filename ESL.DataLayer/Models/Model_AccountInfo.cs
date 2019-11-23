using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESL.DataLayer.Models
{
    public class Model_AccountInfo
    {
        public Guid UserGuid { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }
    }
}
