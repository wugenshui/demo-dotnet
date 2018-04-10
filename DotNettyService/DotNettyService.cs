using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DotNettyService
{
    public partial class DotNettyService : ServiceBase
    {
        public DotNettyService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            NettyServerHelper.init();
        }

        protected override void OnStop()
        {
        }
    }
}
