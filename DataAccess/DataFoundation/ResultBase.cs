using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;

namespace CodeCampSV
{
    [Serializable]
    [DataContract]
    public class ResultBase
    {
        [DataObjectField(true, false, false)]
        virtual public int Id
        {
            get;
            set;
        }
    }
}
