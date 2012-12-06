using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;


namespace WebAPI
{
    public partial class CloudControl : System.Web.UI.UserControl
    {

        private int _minCount;

        public int MinCount
        {
            get { return _minCount; }
            set { _minCount = value; }
        }

        private int _tagsToShow;

        public int TagsToShow
        {
            get { return _tagsToShow; }
            set { _tagsToShow = value; }
        }

        public void DataBindCloudControl()
        {
            // if _tagsToShow not set, use minCount.  that is, tagsToShow overrides
            ObjectDataSourceCloudControl.SelectParameters[0].DefaultValue = _minCount.ToString();

            if (_tagsToShow > 0)
            {
                SessionTagsODS stODS = new SessionTagsODS();
                int showValue = stODS.GetCloudCountByTagsToShow(_tagsToShow);
                ObjectDataSourceCloudControl.SelectParameters[0].DefaultValue = showValue.ToString();
            }

            //ASPxCloudControl1.DataBind();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            LabelCodeCampYearId.Text = Utils.GetCurrentCodeCampYear().ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_tagsToShow == 0)
            {
                _tagsToShow = Utils.DefaultCloudTagsToShow;
            }
            DataBindCloudControl();
        }
    }
}