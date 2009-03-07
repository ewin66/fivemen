using System;
using System.Collections.Generic;
using System.Text;
using FT.Windows.Forms;
using FT.Windows.Forms.Plugins;
using FT.Commons.Tools;
using FT.Commons.WebCatcher;
using FT.DAL;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace FT.Windows.ExternalTool
{
    [Plugin(ChangeLogPath = "ExternalTool-ChangeLog.txt", Company = "Fight Together", Developer = "deadshot123",
         Email = "deadshot123@qq.com", MainVersion = "1.0", Name = "开发工具插件", Tel = "15814584509", Url = "http://deadshot123.cnblogs.com")]
    public class ExternalToolPlugin : FT.Windows.Forms.Plugins.AbstractWindowPlugin
    {
        public override void EmmitMenu()
        {
            //throw new Exception("The method or operation is not implemented.");
            ToolStripMenuItem top = this.AddToMenu("开发工具(&D)");
            ToolStripMenuItem tmp = this.BuildSubMenu("加密解密", typeof(SecurityForm));
            top.DropDownItems.Add(tmp);
            tmp = this.BuildSubMenu("运行时查看器", typeof(RuntimeView));
            top.DropDownItems.Add(tmp);

        }

        public override void EmmitToolBar()
        {
          
        }
    }
}
