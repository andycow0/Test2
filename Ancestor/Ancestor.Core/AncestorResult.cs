using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ancestor.Core
{
    public class AncestorResult : IAncestorResult
    {
        public bool IsSuccess { get; set; }
        public IList DataList { get; set; } 
        public DataTable ReturnDataTable { get; set; }
        public int EffectRows { get; set; }
        public string Message { get; set; }

    }
}
