using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeCampSV
{
    [Serializable]
    public class QueryBase
    {
        [AutoGenColumnAttribute]
        public int Start { get; set; }
        [AutoGenColumnAttribute]
        public int Limit { get; set; }
        [AutoGenColumnAttribute]
        public int OutputTotal { get; set; }

        [AutoGenColumnAttribute]
        private bool _isMaterializeResult = true; // force always materialize list.

        [AutoGenColumnAttribute]
        public bool IsMaterializeResult
        {
            get { return _isMaterializeResult; }
            set { _isMaterializeResult = value; }
        }

        public void ResetPaging()
        {
            Start = 0;
        }
    }
}
