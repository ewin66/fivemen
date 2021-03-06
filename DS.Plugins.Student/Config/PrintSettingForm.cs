using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FT.Commons.Tools;
using System.Xml;

namespace DS.Plugins.Student
{
    public partial class PrintSettingForm : BaseSkinForm
    {
        public PrintSettingForm()
        {
            InitializeComponent();
        }

        private void PrintSettingForm_Load(object sender, EventArgs e)
        {
            AllPrinterConfig config = AllPrinterConfig.GetPrinterConfig();
            this.numApplyUp.Value = config.ApplyConfig.Up;
            this.numApplyDown.Value = config.ApplyConfig.Down;
            this.numApplyLeft.Value = config.ApplyConfig.Left;
            this.numApplyRight.Value = config.ApplyConfig.Right;
            this.cbApplyProfile.Checked = config.ApplyConfig.PrintProfile;
            this.cbApply2Dimension.Checked = config.ApplyConfig.Allow2Dimension;
            this.cbPrintXiangCun.Checked = config.ApplyConfig.PrintXiangCun;
            this.cbPrintApplyDate.Checked = config.ApplyConfig.PrintApplyDate;
            this.cbApplyBodyCheck.Checked = config.ApplyConfig.IsBodyCheck;
            this.numApplyNameDown.Value = config.ApplyConfig.NameDown;
            this.numApplyNameLeft.Value = config.ApplyConfig.NameLeft;
            this.numApplyDownCar.Value=config.ApplyConfig.CarDown ;
            this.numApplyUpCar.Value=config.ApplyConfig.CarUp;
            this.numApplyLeftCar.Value=config.ApplyConfig.CarLeft;
            this.numApplyRightCar.Value=config.ApplyConfig.CarRight ;

            this.numGouDown.Value= config.ApplyConfig.GouDown;
            this.numGouUp.Value= config.ApplyConfig.GouUp;
            this.numGouLeft.Value=config.ApplyConfig.GouLeft;
            this.numGouRight.Value=config.ApplyConfig.GouRight;

            this.numF2Up.Value = config.F2Config.Up;
            this.numF2Down.Value = config.F2Config.Down;
            this.numF2Left.Value = config.F2Config.Left;
            this.numF2Right.Value = config.F2Config.Right;
            this.cbF2CheckC1.Checked = config.F2Config.CheckC1;
            this.cbF2PrintDate.Checked = config.F2Config.PrintDate;

            this.numF3Up.Value = config.F3Config.Up;
            this.numF3Down.Value = config.F3Config.Down;
            this.numF3Left.Value = config.F3Config.Left;
            this.numF3Right.Value = config.F3Config.Right;
            this.cbF3Profile.Checked = config.F3Config.PrintProfile;

            this.numF4Up.Value = config.F4Config.Up;
            this.numF4Down.Value = config.F4Config.Down;
            this.numF4Left.Value = config.F4Config.Left;
            this.numF4Right.Value = config.F4Config.Right;
            this.cbF4CheckC1.Checked = config.F4Config.CheckC1;
            this.cbF4PrintDate.Checked = config.F4Config.PrintDate;

            this.numF6Up.Value = config.F6Config.Up;
            this.numF6Down.Value = config.F6Config.Down;
            this.numF6Left.Value = config.F6Config.Left;
            this.numF6Right.Value = config.F6Config.Right;
            this.txtExamIdPrefix.Text = config.F6Config.ExamIdPrefix;
            this.cbPrintCompany.Checked = config.F6Config.PrintCompany;
            this.txtExamAddress.Text = config.F6Config.ExamAddress;
            this.cbF6CheckC1.Checked = config.F6Config.CheckC1;

            this.cbUseIDCard.Checked = config.SysConfig.UseIDCard;
            this.cbApplySkin.Checked = config.SysConfig.ApplySkin;
            this.txtIp.Text = config.SysConfig.Ip;
            this.txtDbName.Text = config.SysConfig.DbName;
            this.txtUID.Text = config.SysConfig.UID;
            this.cbDbType.Text = config.SysConfig.DbType;
            this.txtPwd.Text = config.SysConfig.Pwd;
            this.txtSysDefaultEye.Text = config.SysConfig.DefaultEye;

            if(config.ShareConfig!=null)
                this.txtShareDbIp.Text = config.ShareConfig.Ip;

            
            

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                AllPrinterConfig config = AllPrinterConfig.GetPrinterConfig();
                config.ApplyConfig.Up = (int)this.numApplyUp.Value;
                config.ApplyConfig.Down = (int)this.numApplyDown.Value;
                config.ApplyConfig.Left = (int)this.numApplyLeft.Value;
                config.ApplyConfig.Right = (int)this.numApplyRight.Value;
                config.ApplyConfig.PrintProfile = this.cbApplyProfile.Checked;
                config.ApplyConfig.Allow2Dimension = this.cbApply2Dimension.Checked;
                config.ApplyConfig.PrintXiangCun = this.cbPrintXiangCun.Checked;
                config.ApplyConfig.PrintApplyDate=this.cbPrintApplyDate.Checked;
                config.ApplyConfig.IsBodyCheck = this.cbApplyBodyCheck.Checked;
                config.ApplyConfig.NameDown = (int)this.numApplyNameDown.Value;
                config.ApplyConfig.NameLeft = (int)this.numApplyNameLeft.Value;

                config.ApplyConfig.CarDown = (int)this.numApplyDownCar.Value;
                config.ApplyConfig.CarUp = (int)this.numApplyUpCar.Value;
                config.ApplyConfig.CarLeft = (int)this.numApplyLeftCar.Value;
                config.ApplyConfig.CarRight = (int)this.numApplyRightCar.Value;

                config.ApplyConfig.GouDown = (int)this.numGouDown.Value;
                config.ApplyConfig.GouUp = (int)this.numGouUp.Value;
                config.ApplyConfig.GouLeft = (int)this.numGouLeft.Value;
                config.ApplyConfig.GouRight = (int)this.numGouRight.Value;


                config.F2Config.Up = (int)this.numF2Up.Value;
                config.F2Config.Down = (int)this.numF2Down.Value;
                config.F2Config.Left = (int)this.numF2Left.Value;
                config.F2Config.Right = (int)this.numF2Right.Value;
                config.F2Config.CheckC1 = this.cbF2CheckC1.Checked;
                config.F2Config.PrintDate = this.cbF2PrintDate.Checked;

                config.F3Config.Up = (int)this.numF3Up.Value;
                config.F3Config.Down = (int)this.numF3Down.Value;
                config.F3Config.Left = (int)this.numF3Left.Value;
                config.F3Config.Right = (int)this.numF3Right.Value;
                config.F3Config.PrintProfile = this.cbF3Profile.Checked;
                

                config.F4Config.Up = (int)this.numF4Up.Value;
                config.F4Config.Down = (int)this.numF4Down.Value;
                config.F4Config.Left = (int)this.numF4Left.Value;
                config.F4Config.Right = (int)this.numF4Right.Value;
                config.F4Config.CheckC1 = this.cbF4CheckC1.Checked;
                config.F4Config.PrintDate = this.cbF4PrintDate.Checked;

                config.F6Config.Up = (int)this.numF6Up.Value;
                config.F6Config.Down = (int)this.numF6Down.Value;
                config.F6Config.Left = (int)this.numF6Left.Value;
                config.F6Config.Right = (int)this.numF6Right.Value;
                config.F6Config.ExamIdPrefix = this.txtExamIdPrefix.Text.Trim();
                config.F6Config.PrintCompany = this.cbPrintCompany.Checked;
                config.F6Config.ExamAddress = this.txtExamAddress.Text.Trim();
                config.F6Config.CheckC1 = this.cbF6CheckC1.Checked;

                config.SysConfig.ApplySkin = this.cbApplySkin.Checked;
                config.SysConfig.UseIDCard = this.cbUseIDCard.Checked;
                config.SysConfig.Ip = this.txtIp.Text.Trim();
                config.SysConfig.DbName = this.txtDbName.Text.Trim();
                config.SysConfig.UID = this.txtUID.Text.Trim();
                config.SysConfig.Pwd = this.txtPwd.Text.Trim();
                config.SysConfig.DbType = this.cbDbType.Text.Trim();
                config.SysConfig.DefaultEye = this.txtSysDefaultEye.Text.Trim();

                if (config.ShareConfig == null)
                {
                    config.ShareConfig = new ShareDbConfig();
                }
                config.ShareConfig.Ip = this.txtShareDbIp.Text.Trim();

                AllPrinterConfig.Save();
                MessageBoxHelper.Show("保存配置成功！");
            }
            catch (Exception ex)
            {
                MessageBoxHelper.Show("保存配置失败，错误："+ex.Message);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            AllPrinterConfig.BackUp();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            AllPrinterConfig.Restore();
        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ip =this.txtIp.Text.Trim() ;
            string dbname = this.txtDbName.Text.Trim() ;
            string userid = this.txtUID.Text.Trim();
            string pwd = this.txtPwd.Text.Trim();
            if (dbname.Length > 0 && userid.Length > 0)
            {
                FT.DAL.IDataAccess access = null;
                if (this.cbDbType.Text.Trim().ToLower() == "sqlserver")
                {
                    access = new FT.DAL.SqlServer.SqlServerDataHelper(ip, dbname, userid, pwd);
                }
                else if (this.cbDbType.Text.Trim().ToLower() == "oracle")
                {
                    access = new FT.DAL.Oracle.OracleDataHelper(dbname, userid, pwd);
                }
                else if (this.cbDbType.Text.Trim().ToLower() == "access")
                {
                    if(pwd.Length>0)
                    {
                        access=new FT.DAL.Access.AccessDataHelper("Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=true;Data Source=" + dbname + ";Jet OLEDB:Database Password="+pwd);
                    }
                    else
                    {
                    access = new FT.DAL.Access.AccessDataHelper(dbname, userid, pwd);
                    }
                }
                try
                {
                    if (access.Open())
                    {
                        MessageBox.Show("测试成功！", "提示");
                        access.Close();
                    }
                    else
                    {
                        MessageBox.Show("测试失败！", "提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxHelper.Show(ex.ToString());
                }
            }
            else
            {
                MessageBoxHelper.Show("必须填写数据库名和登陆名");
            }
        }

        private const string connKey = "DefaultConnString";

        private void btnTestShareDb_Click(object sender, EventArgs e)
        {
            FT.DAL.Access.AccessDataHelper access = new FT.DAL.Access.AccessDataHelper("Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=true;Data Source=\\\\" + this.txtShareDbIp.Text.Trim() + "\\db\\db.mdb;Jet OLEDB:Database Password=SQL");
            try
            {
                if (access.Open())
                {
                    MessageBox.Show("连接成功！", "提示");
                    access.Close();
                }
                else
                {
                    MessageBox.Show("连接" + this.txtShareDbIp.Text.Trim() + "失败,请检查目标机器是否可用，\r\n是否已经共享预录入系统的db文件夹！", "提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FT.DAL.Access.AccessDataHelper access = new FT.DAL.Access.AccessDataHelper("Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=true;Data Source=\\\\" + this.txtShareDbIp.Text.Trim() + "\\db\\db.mdb;Jet OLEDB:Database Password=SQL");
            try
            {
                if (access.Open())
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(Application.ExecutablePath + ".config");

                    XmlNode node1 = doc.SelectSingleNode(@"//add[@key='" + connKey + "']");
                    XmlElement ele1 = (XmlElement)node1;
                    ele1.SetAttribute("value", FT.Commons.Security.SecurityFactory.GetSecurity().Encrypt("Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=true;Data Source=\\\\" + this.txtShareDbIp.Text.Trim() + "\\db\\db.mdb;Jet OLEDB:Database Password=SQL"));
                    doc.Save(Application.ExecutablePath + ".config");
                    MessageBoxHelper.Show("保存成功，请退出重进！");
                    access.Close();
                }
                else
                {
                    MessageBox.Show("连接" + this.txtShareDbIp.Text.Trim() + "失败,请检查目标机器是否可用，\r\n是否已经共享预录入系统的db文件夹！", "提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }

      

      
      

       
    }
}

