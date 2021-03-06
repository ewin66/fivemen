﻿using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PDA.DbManager;

namespace PDA
{
    public partial class MoveLocDetailHadTray : Form
    {
        public MoveLocDetailHadTray()
        {
            InitializeComponent();

        }

        private void txt_TrayNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!this.cb_Rollback.Checked)
                {
                    this.txt_NewLoc.Focus();
                }
                else
                {
                    this.RemoveData();
                    this.RebindData();
                    MessageBox.Show("撤消扫描成功！");
                    ClearInput();
                    cb_Rollback.Checked = false;
                }
            }
        }

        private void txt_NewLoc_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //txt_SN.Focus();
                if (KuWeiManager.IsExists(this.txt_NewLoc.Text.Trim()))
                {
                    if (!this.cb_Rollback.Checked)
                    {
                        this.SaveData();
                        this.RebindData();
                    }
                }
                else
                {
                    MessageBox.Show("库位:" + this.txt_NewLoc.Text.Trim() + "已经不再使用！");
                }
                ClearInput();
                txt_TrayNo.Focus();
                //this.txt_TrayNo.Enabled = false;
            }

        }

        private void ClearInput()
        {
            this.txt_TrayNo.Text = string.Empty;
            this.txt_NewLoc.Text = string.Empty;
        }

        private void ck_Rollback_CheckStateChanged(object sender, EventArgs e)
        {
            ClearInput();
            this.txt_TrayNo.Focus();
        }
        private void RebindData()
        {
            this.dg_ScanList.DataSource = AnTuoYiKuManager.GetUserData(Program.UserID);
        }

        private AnTuoYiKu ComputeData()
        {

            AnTuoYiKu entity = new AnTuoYiKu();
            entity.Mdkw = this.txt_NewLoc.Text.Trim();
            entity.Tph = this.txt_TrayNo.Text.Trim();
            entity.Scaner = Program.UserID;
            entity.date = System.DateTime.Now;
            return entity;
        }

        private void RemoveData()
        {
            AnTuoYiKu entity = this.ComputeData();
            if (!AnTuoYiKuManager.CheckExists(entity))
            {
                MessageBox.Show("托盘尚未扫描，不能撤消！");
                return;
            }
            AnTuoYiKuManager.Delete(entity);
        }

        private void SaveData()
        {
            AnTuoYiKu entity = this.ComputeData();
            if (AnTuoYiKuManager.CheckExists(entity))
            {
                MessageBox.Show("您已经扫描过该产品！");
            }
            else
            {
                AnTuoYiKuManager.Save(entity);
            }

        }

        private void MoveLocDetailHadTray_Load(object sender, EventArgs e)
        {
            this.RebindData();
        }
    }
}