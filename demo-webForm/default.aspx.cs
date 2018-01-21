using common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace demo_webForm
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LogHelper.Info("日志初始化");
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            LogHelper.Debug("button click");
        }
    }
}