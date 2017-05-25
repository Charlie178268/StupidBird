using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StupidBird.Properties;

namespace StupidBird
{
    public partial class EntryForm : Form
    {
        Bird bird1;
        Image groundImg = Resources.ground;
        int groundX = 0;//底面的x坐标
        System.Timers.Timer t = new System.Timers.Timer(50); //设置时间间隔为100毫秒
        public EntryForm()
        {
            InitializeComponent();
            //小鸟的播放
            bird1 = new Bird(180, 160, 0);
            t.Elapsed += new System.Timers.ElapsedEventHandler(Timer_TimesUp);
            t.AutoReset = true; //每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
            t.Enabled = true; //是否触发Elapsed事件
            t.Start();
        }

        private void Timer_TimesUp(object sender, System.Timers.ElapsedEventArgs e)
        {
            //到达指定时间触发该事件，使窗口发生重绘,执行EntryForm_Paint函数
            this.Invalidate();
        }

        private void EntryForm_Paint(object sender, PaintEventArgs e)
        {
            bird1.Draw(e.Graphics);
            //设置底面动起来
            e.Graphics.DrawImage(groundImg, groundX, 350);
            groundX -= 5;
            if (groundX <= -422)
            {
                groundX = 0;
            }
        }
        /*开始按钮*/
        private void button1_Click(object sender, EventArgs e)
        {
            //播放按钮声音
            System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(Application.StartupPath + @"/Sound/btn_press.wav");
            sndPlayer.Play();
            //MessageBox.Show(Application.StartupPath + @"\sound\sfx_wing.wav");
            t.Stop();//停止timer
            this.Hide();//隐藏当前窗口,如果使用this.close()两个窗口都会关闭。
            Form1 newWd = new Form1(this);
            newWd.ShowDialog();//显示窗口
        }
    }
}
