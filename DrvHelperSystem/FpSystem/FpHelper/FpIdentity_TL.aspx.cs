﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FT.Web.Tools;
using FT.Commons.Tools;
using System.Threading;
using log4net;

public partial class FpSystem_FpHelper_FpIndentityLesson_TL : System.Web.UI.Page
{

    

    private   FpBase _FP;
    private static Boolean gBlIdentityStrat;
    private string gStrTargetFrame="";
    private string gStrCheckinLogFrame = "";
    protected ILog log = log4net.LogManager.GetLogger("DrvHelperSystem");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack||_FP==null)
        {
            _FP = new FpBase(this, new EventHandler(FpVerifyHandler), true);
        }

        gStrTargetFrame = StringHelper.fnFormatNullOrBlankString( Request.Params["targetFrame"],"");
         gStrCheckinLogFrame = StringHelper.fnFormatNullOrBlankString(Request.Params["checkinLogFrame"],"");

         //_FP.FpIdentityUser();
    }

    protected void btnIdentity_Click(object sender, EventArgs e)
    {
        
        int result = _FP.FpIdentityUser();
       // while (result != FpBase.SUCCESSED && retryCount++ < 5)
       // {
       //     Thread.Sleep(2000);
        //    result = _FP.FpIdentityUser();
       // }
    }

    private void FpVerifyHandler(object sender, EventArgs e)
    {
        //string SCP_SCRIPT_START = "\n<script language=\"javascript\">\n";
        string SCP_ALERT = "";
        //string SCP_SCRIPT_END = "</script>\n";

        ResultCodeArgs re = (ResultCodeArgs)e;
       // TempLog.Info("验证结果为："+re.ResultCode+"验证结果说明："+re.ResultMessage);
        if (re.ResultCode == 215)
        {
            return;
        }
        //log.Error("re.ResultMessage:"+re.ResultMessage);
        string[] lArrIdCards = FpBase.getUserIds(re);
       // TempLog.Info("验证返回的身份证明号码字符串数组长度为->"+lArrIdCards.Length.ToString());
        string idcard = lArrIdCards.Length > 0 ? lArrIdCards[0].ToString().Split('_')[0] : "";
       // idcard = Server.UrlEncode(idcard);
        string lStrSearch = string.Format("?{0}={1}",FPSystemBiz.PARAM_RESULT, idcard);
       // Session[FPSystemBiz.PARAM_RESULT] = idcard;

        SCP_ALERT += string.Format("window.parent.document.frames['{0}'].location.search='{1}';", gStrTargetFrame, lStrSearch);
        SCP_ALERT += string.Format("window.parent.document.frames['{0}'].location.reload();", gStrCheckinLogFrame);
        ClientScriptManager newCSM = Page.ClientScript;
        //newCSM.RegisterStartupScript(this.GetType(), this.GetHashCode().ToString(), SCP_SCRIPT_START + SCP_ALERT + SCP_SCRIPT_END);
        WebTools.WriteScript(SCP_ALERT);
        //WebTools.ShowModalWindows
        //TempLog.Info("输出的script内容为->"+SCP_ALERT);
      //  this.ClientScript.RegisterStartupScript(typeof(int), "alertmsg", "<script language='javascript'>" + SCP_ALERT + "</script>");
        
        
               
                if (cboAuto.Checked)
                {
                    //Thread.Sleep(200);
                    int retryCount = 0;
                    int result = _FP.FpIdentityUser();
                    if (result != FpBase.SUCCESSED) {
                        Thread.Sleep(1500);
                        //_FP = new FpBase(this, new EventHandler(FpVerifyHandler), true );
                        result = _FP.FpIdentityUser();
                        //btnIdentity_Click(null, null);
                    }
                    // while (result != FpBase.SUCCESSED && retryCount++ <5)
                    //{
                    //     Thread.Sleep(2000);
                    //     result = _FP.FpIdentityUser();
                    // }


                }
                
                
          
    }
}
