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
using FT.DAL.Orm;
using FT.Commons.Tools;


public partial class FpSystem_FpHelper_FpAttendLesson : System.Web.UI.Page
{
    private FpBase _FP;

    protected void Page_Load(object sender, EventArgs e)
    {
       if( IsPostBack)
           _FP = new FpBase(this, new EventHandler(FpVerifyHandler),true);
  
       
    }

    private void FpVerifyHandler(object o, EventArgs e)
    {
        ResultCodeArgs re = (ResultCodeArgs)e;
       string[] lStrUserIds= FpBase.getUserIds(re);

       string idcard =lStrUserIds.Length>0?lStrUserIds[0].ToString():"";

       int rcode= FPSystemBiz.fnIdendityStudentLesson(idcard);

       if (rcode == FPSystemBiz.CHECHIN_NO_RECARD)
       {
           this.lbStudentAlertMsg.Text = "没有该学员的指纹信息";
           return;
       }
       FpStudentObject fso = SimpleOrmOperator.Query<FpStudentObject>(idcard);
       if (fso == null)
       {
           this.lbStudentAlertMsg.Text = "没有该学员的个人信息";
           return;
       }
       this.fnUILoadStudentRecord(fso,rcode);
       
    }




    protected void btnIdentity_Click(object sender, EventArgs e)
    {

        _FP.FpIdentityUser();

    }

    private void fnUILoadStudentRecord(FpStudentObject pFso,int pResultCode)
    {
        this.lbStrName.Text = pFso.NAME;
        this.lbStuIdCard.Text = pFso.IDCARD;
        this.lbStuLessonEnter1.Text = DateTimeHelper.fnIsNewDateTime(pFso.LESSON_ENTER_1)?"":pFso.LESSON_ENTER_1.ToString();
        this.lbStuLessonLeave1.Text = "";
        this.lbStuLessonEnter2.Text = DateTimeHelper.fnIsNewDateTime(pFso.LESSON_ENTER_2) ? "" : pFso.LESSON_ENTER_2.ToString();
        this.lbStuLessonLeave2.Text = DateTimeHelper.fnIsNewDateTime(pFso.LESSON_LEAVE_1) ? "" : pFso.LESSON_LEAVE_1.ToString();
           
        if(pResultCode== FPSystemBiz.LESSON_ENTER_1_FAILE)
        {
                this.lbStudentAlertMsg.Text="本次上课与上次不在同一天进行，旧上课记录已被清空，请再次确认上课";
        }
        else if (pResultCode== FPSystemBiz.LESSON_ENTER_2_FAILE) 
        {
                this.lbStudentAlertMsg.Text = "下午上课确认失败";
        }
        else if(pResultCode== FPSystemBiz.LESSON_LEAVE_2_FAILE){
                this.lbStudentAlertMsg.Text="下午离场确认失败";
        }
 
    }
}
