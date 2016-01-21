using Ancestor.Core;
using System.Text;
using Ancestor.DataAccess.DBAction;

namespace Ancestor.DataAccess.DAO
{
    public abstract class DataAccessObject
    {
        public DBObject DbObject { get; set; }
        public IDbAction DB { get; set; }
        public StringBuilder SqlString { get; set; }
        internal string DbSymbolize { get; set; }
        internal string DbLikeSymbolize { get; set; }
        internal virtual object GetDbType(string typeString)
        {
            return null;
        }
    }
}
