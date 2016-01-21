using Ancestor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ancestor.DataAccess.Factory
{
    public interface IDBConnection
    {
        IConnection GetConnectionFactory();
    }
}
