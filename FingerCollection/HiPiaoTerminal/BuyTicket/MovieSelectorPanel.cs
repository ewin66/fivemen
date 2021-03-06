﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HiPiaoInterface;
using HiPiaoTerminal.ConfigModel;
using FT.Commons.Tools;
using HiPiaoTerminal.Account;
using System.Threading;
using FT.Commons.Win32;

namespace HiPiaoTerminal.BuyTicket
{
    public partial class MovieSelectorPanel : HiPiaoTerminal.UserControlEx.OperationTimeParentPanel
    {
       
        public MovieSelectorPanel()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            try
            {
                DateTime now = System.DateTime.Now;

                this.btnToday.Text = string.Format(this.btnToday.Text, now.ToString("MM月dd日"));
                this.btnTomorrow.Text = string.Format(this.btnTomorrow.Text, now.AddDays(1).ToString("MM月dd日"));
                this.btnThreeDay.Text = string.Format(this.btnThreeDay.Text, now.AddDays(2).ToString("MM月dd日"));
                this.SetSepartor(false);

               
               
                //WinFormHelper.InitButtonStyle(this.btnThreeDay);
                //WinFormHelper.InitButtonStyle(this.btnToday);
               // WinFormHelper.InitButtonStyle(this.btnTomorrow);
               // WinFormHelper.InitButtonStyle(this.btnReturn);
            }
            catch (Exception ex)
            {
            }
        }

        private void MovieSelectorPanel_Load(object sender, EventArgs e)
        {
           
            DateTime now = System.DateTime.Now;
            //this.InitMovies(now);
            this.InitMoviesThread(now);
            this.SetOperationTime(60);
        }

        //private static Font MovieNameColor=new Font("",

        private DateTime tmpDt;

        private Thread thread;

        private void ExitThread()
        {
            try
            {
                thread.Abort();
            }
            catch
            {
            }
        }

        private bool loaded = true;

        private void InitMoviesThread(DateTime dt)
        {
            if (loaded)
            {
               
                this.ExitThread();
                this.loaded = false;
                this.tmpDt = dt;
                thread = new Thread(new ThreadStart(InitMovies));
                thread.IsBackground = true;
                thread.Start();
            }
        }

        private void InitMovies()
        {
            this.InitMovies(tmpDt);
            this.loaded = true;
        }

        private void InitMovies(DateTime dt)
        {
            SystemConfig config = FT.Commons.Cache.StaticCacheManager.GetConfig<SystemConfig>();
            List<MovieObject> all = HiPiaoCache.GetHotMovie(config.Province, config.City);

            List<MovieObject> lists= HiPiaoCache.GetDayMovie(config.City,config.CinemaId, dt,all);
            if (lists == null || lists.Count == 0)
            {
                //274, 42
                //745, 374
                PictureBox pic = new PictureBox();
                pic.BackgroundImage = Properties.Resources.BuyTick_No_Plan;
                pic.Width = 745;
                pic.Height = 374;
                pic.Location = new Point(274, 42);
                this.panelContent.Controls.Clear();
                //this.panelContent.Controls.Add(pic);
                WindowFormDelegate.AddControlTo(this.panelContent, pic);
                //this.picShowNoPlanHint.Visible = true;
            }
           
            else if (lists != null&&lists.Count>0 )
            {
                this.panelContent.Controls.Clear();
                int x=100;
                int y=0;
                int col=0;
                int row=0;
                int linecount=6;
                for (int i = 0; i < lists.Count; i++)
                {
                    PictureBox pc = new PictureBox();
                    pc.Width = picWid;
                    pc.Height = picHeight;
                    pc.Tag = lists[i];
                    if(lists[i].AdImagePath.Length>0)
                        pc.Image = Image.FromFile(lists[i].AdImagePath);
                    pc.Location =new Point( x + (i % linecount)*(picWid+colSep),y + (i / linecount) * (picHeight + rowSep));
                    Label lb = new Label();
                    lb.Tag = lists[i];
                    lb.AutoSize = false;
                    lb.Width = picWid;
                    //lb.BackColor = Color.Red;
                    lb.Height = 28;
                    lb.Location = new Point(pc.Location.X,pc.Location.Y + picHeight);
                    lb.ForeColor = Color.FromArgb(69, 68, 68);
                    lb.TextAlign = ContentAlignment.MiddleCenter;
                    lb.Font = new Font("方正兰亭纤黑简体", 18,FontStyle.Bold);
                    //string name = "很长的测试电影名字如果爱琴海岛上没有海盗";
                    string name = lists[i].Name;
                    if (name.Length > 6)
                    {
                        
                        lb.Text = name.Substring(0, 6);
                       
                    }
                    else
                    {
                         lb.Text = name;
                    }
                    lb.MouseHover += new EventHandler(lb_MouseHover);
                   
                    pc.Click += new EventHandler(pc_Click);
                    pc.MouseHover += new EventHandler(lb_MouseHover);
                    WindowFormDelegate.AddControlTo(this.panelContent, pc);
                    WindowFormDelegate.AddControlTo(this.panelContent, lb);
                    //this.panelContent.Controls.Add(pc);
                    //this.panelContent.Controls.Add(lb);
                }
            }
        }

        void lb_MouseHover(object sender, EventArgs e)
        {
            
                Control ctr = sender as Control;
                MovieObject movie = ctr.Tag as MovieObject;
                //this.toolTip1.to
                this.toolTip1.Tag = movie.Name.ToString();
                this.toolTip1.SetToolTip(ctr, movie.Name.ToString());
           
        }

        void pc_Click(object sender, EventArgs e)
        {
            if (loaded)
            {
                PictureBox pic=sender as PictureBox;
                MovieObject movie = pic.Tag as MovieObject;
               
                GlobalTools.GoPanel(new MoviePlanSelectorPanel(movie, tmpDt));
            }
        }

        private static int colSep = 20;
        private static int rowSep = 40;

        private static int picWid=172;
        private static int picHeight=240;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (this.loaded)
            {
                this.ExitThread();
                GlobalTools.ReturnMain();
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (this.loaded)
            {
                this.ExitThread();
                GlobalTools.ReturnMain();
            }
        }

        private void btnThreeDay_Click(object sender, EventArgs e)
        {
            if (this.loaded)
            {
                this.InitMoviesThread(System.DateTime.Now.AddDays(2));
                // this.InitMovies(System.DateTime.Now.AddDays(2));
                this.btnToday.Image = Properties.Resources.BuyTicket_Select_Day_Two;
                this.btnTomorrow.Image = Properties.Resources.BuyTicket_Select_Day_Two;
                this.btnThreeDay.Image = Properties.Resources.BuyTicket_Select_Day_Today;
            }
        }

        private void btnTomorrow_Click(object sender, EventArgs e)
        {
            if (this.loaded)
            {
                this.InitMoviesThread(System.DateTime.Now.AddDays(1));
                // this.InitMovies(System.DateTime.Now.AddDays(1));
                this.btnToday.Image = Properties.Resources.BuyTicket_Select_Day_Two;
                this.btnTomorrow.Image = Properties.Resources.BuyTicket_Select_Day_Today;
                this.btnThreeDay.Image = Properties.Resources.BuyTicket_Select_Day_Two;
            }
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            if (this.loaded)
            {
                this.InitMoviesThread(System.DateTime.Now);
                //this.InitMovies(System.DateTime.Now);
                this.btnToday.Image = Properties.Resources.BuyTicket_Select_Day_Today;
                this.btnTomorrow.Image = Properties.Resources.BuyTicket_Select_Day_Two;
                this.btnThreeDay.Image = Properties.Resources.BuyTicket_Select_Day_Two;
            }
        }

        Font fontHint = new Font("方正兰亭黑简体", 18);

        private void toolTip1_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.DrawBackground();

            e.Graphics.DrawString(e.ToolTipText, fontHint, Brushes.Black, new PointF(0, 0));
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
            //int x=
            if (this.toolTip1.Tag!=null)
            e.ToolTipSize = e.AssociatedControl.CreateGraphics().MeasureString(this.toolTip1.Tag.ToString(), fontHint).ToSize();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
