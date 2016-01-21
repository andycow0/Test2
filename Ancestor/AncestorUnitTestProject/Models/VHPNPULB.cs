using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AncestorUnitTestProject.Models
{
    public class VHPNPULB
    {
        public string IDNO { get; set; }
        public string CNM { get; set; }
        public string ORGNID { get; set; }
        public Nullable<DateTime> INBUSDAT { get; set; }
        public Nullable<DateTime> INDAT { get; set; }
        public Nullable<DateTime> OUTDAT { get; set; }
        public string OUTKD { get; set; }
        public string DUTID { get; set; }
        public double DUTPPT { get; set; }
        public string LOC { get; set; }
        public string DPID { get; set; }
        public string RESGRS { get; set; }
        public Nullable<DateTime> LOCTXDAT { get; set; }
        public Nullable<DateTime> DPTXDAT { get; set; }
        public Nullable<DateTime> DUTEFDAT { get; set; }
        public Nullable<DateTime> JBEFDAT { get; set; }
        public Nullable<DateTime> DUTPFRDAT { get; set; }
        public Nullable<DateTime> BRNDAT { get; set; }
        public string SEX { get; set; }
    }
}
