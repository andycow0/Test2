using Ancestor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AncestorUnitTestProject.Models
{
    public class ROPRPAINVISIT : IModel
    {
        public string CHTNO { get; set; }
        public string CSN { get; set; }
        public string OPERNO { get; set; }
        public string VSDAT { get; set; }
        public Nullable<int> SQNO { get; set; }
        public Nullable<double> EB1 { get; set; }
        public string EB1UINIT { get; set; }
        public Nullable<double> EB2 { get; set; }
        public Nullable<DateTime> CPROOTTIME { get; set; }
    }
}
