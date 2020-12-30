using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLIE_FiBu_FV1.Controllers.DataHandlers
{
    abstract class Template
    {
        public abstract List<object> Read(ClassType class_type);
        public abstract object Read(Int32 primary_key, ClassType class_type);
        public abstract object Write(object obj, bool delete);
    }
}
