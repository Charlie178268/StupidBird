using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms;
using StupidBird.Properties;
namespace StupidBird
{
    public partial class Form1 : Form
    {
        EntryForm m_form;

        public Form1(EntryForm form)
        {
            this.m_form = form;
            InitializeComponent();
            InitialGame();
        }
        private bool gameOver = false;//判断游戏是否结束
        private Pipe pipeUp;
        private Pipe pipeDown;
        private static int grade = 0;//玩家分数,过一个加1
        private Button restartBtn;
        private Label overLabel;
        /*控制底面移动的变量*/
        Image groundImg = Resources.ground;
        int groundX = 0;//底面的x坐标

        /*初始化游戏*/
        public void InitialGame()
        {
            SingleObject.GetSingle().AddGameObject(new Bird(50, 200, 0));
            pipeUp = new Pipe(100, -600, pipeDirection.Up);
            pipeDown = new Pipe(100, 400, pipeDirection.Down);
            d = () => { };//用匿名函数初始化委托
            d += GroundMove;//地面动
        }
        /*加载窗体事件*/
        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = StupidBird.Properties.Resources.bg;//设置窗口背景图片
            this.BackgroundImageLayout = ImageLayout.Stretch;//设置图片随窗体的显示效果
        }

        /*绘制窗体事件*/
        private void Form_Paint(object sender, PaintEventArgs e)
        {
            /*绘制窗体的时候，画鸟*/
            SingleObject.GetSingle().DrawGameObject(e.Graphics);
            /*绘制管道*/
            pipeDown.DrawPipe(e.Graphics);
            pipeUp.DrawPipe(e.Graphics);
            /*地面移动*/
            e.Graphics.DrawImage(groundImg, groundX, 540);
           
            d();//改变groundX的值
        }
        /*底面移动*/
        private delegate void GroundMoveDelegate();
        GroundMoveDelegate d; 
        private void GroundMove()
        {
            groundX -= 10;
            if (groundX <= -422)
            {
                groundX = 0;
            }
        }
    /*使控件的整个画面无效并重新绘制窗口,而重新绘制窗口时会调用Draw方法使动画播放*/
    private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
            /*判断是否游戏结束并且小鸟落地,落地就停止timer，让小鸟翅膀停止播放*/
            if (gameOver==true && SingleObject.GetSingle().singleBird.y >= 510)
            {
                timer1.Stop();
                timer2.Stop();
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            /*点击鼠标左键移动*/
            System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(Application.StartupPath + @"/Sound/sfx_wing.wav");
            sndPlayer.Play();
            SingleObject.GetSingle().singleBird.currentSpeed = 10f;//首先设置当前速度
            SingleObject.GetSingle().Move();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            /*按下空格键移动*/
            System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(Application.StartupPath + @"/Sound/sfx_wing.wav");
            sndPlayer.Play();
            if (e.KeyCode == Keys.Space)
            {
                SingleObject.GetSingle().singleBird.currentSpeed = 10f;//首先设置当前速度
                SingleObject.GetSingle().Move();
            }
        }
        /*控制小鸟下落的重复执行事件*/
        private void timer2_Tick(object sender, EventArgs e)
        {
            //首先要获得小鸟下降的高度
            var height = Gravity.GetHeight(SingleObject.GetSingle().singleBird.currentSpeed, 
                SingleObject.GetSingle().singleBird.durationTime * 0.001f);//将毫秒转换成帧  Time.DeltaTime
            //获得小鸟下落后的新坐标
            int y = SingleObject.GetSingle().singleBird.y + (int)height;

            //对y做限定
            //限定了小鸟的y坐标 最高就顶着窗体的头
            y = y < 0 ? 0 : y;
            //限定小鸟 不让他掉落到地面的下面
            y = y > 510  ? 510 : y;
            //将新坐标y，赋值给小鸟的坐标
            SingleObject.GetSingle().singleBird.y = y;
            //v=v0+at;
            SingleObject.GetSingle().singleBird.currentSpeed = SingleObject.GetSingle().singleBird.currentSpeed + 
                SingleObject.GetSingle().singleBird.durationTime * Gravity._g * 0.001f;


            //当我们点击鼠标或者敲击空格的时候  应该让小鸟有一个向上的固定值
        }
        private int pipeDistance = 150;
        /*让管道从右到左移动*/
        private void PipeMove()
        {
            pipeDown.x -= 10;
            pipeUp.x -= 10;
            //当管道移出窗口的时候设置它的坐标在右边出现,-128是管道的宽度
            if (pipeDown.x < -128)
            {
                pipeDown.x = this.Width * 4 / 3 -124;
                pipeUp.x = this.Width * 4 / 3 -128;
                //MessageBox.Show("pipeW="+(this.Width*4/3).ToString());
                //下面要解决的问题 就是让两个管子 有高有底
                pipeUp.pipeHeight = SetRandomHeight();
                //上面的管子高度确定了 下面的也就确定了
                pipeDown.pipeHeight = 540 - pipeDistance - pipeUp.pipeHeight;

                //最终还是要根据高度 计算出来y坐标赋值给管道
                pipeUp.y = pipeUp.pipeHeight - 830;
                pipeDown.y = pipeUp.pipeHeight + pipeDistance;
            }
        }
        /// <summary>
        /// 返回高度的随机值
        /// </summary>
        /// <returns></returns>
        public int SetRandomHeight()
        {
            Random r = new Random();
            int totalHeight = this.Size.Height - 137;
            return r.Next(90, totalHeight - 90 - pipeDistance);
        }


        private void timer3_Tick(object sender, EventArgs e)
        {
            PipeMove();//管道移动
            GradeAdd();//分数增加
            /*碰撞检测,检测鸟和管道的矩形是否相交*/
            var pipeUpJudge = new Rectangle(pipeUp.x+10, 0, pipeUp.pipeWidth, pipeUp.pipeHeight-37);
            var pipeDownJudge = new Rectangle(pipeDown.x + 10, pipeDown.y + 20, pipeDown.pipeWidth, pipeDown.pipeHeight);
            var birdJudge = SingleObject.GetSingle().singleBird.GetRectangle();
            if (pipeUpJudge.IntersectsWith(birdJudge) || pipeDownJudge.IntersectsWith(birdJudge))
            {
                GameOver();
            }
        }
        /*游戏结束*/
        private void GameOver()
        {
            gameOver = true;
            System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(Application.StartupPath + @"/Sound/sfx_die.wav");
            sndPlayer.Play();
            /*移除点击和按键事件*/
            this.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);

            this.timer3.Stop();//停止柱子移动
            //this.timer1.Stop();//停止鸟飞和底面动
            d -= GroundMove;
            /*显示gameover和重新开始的按钮*/
            overLabel = new System.Windows.Forms.Label();
            overLabel.AutoSize = true;
            overLabel.BackColor = System.Drawing.Color.Lime;
            overLabel.Font = new System.Drawing.Font("Lucida Sans", 28F, System.Drawing.FontStyle.Bold);
            overLabel.ForeColor = System.Drawing.Color.Blue;
            overLabel.Location = new System.Drawing.Point(60, 226);
            overLabel.Text = "GAME OVER";
            overLabel.Size = new System.Drawing.Size(0, 27);
            overLabel.TabIndex = 10;
            this.Controls.Add(overLabel);

            restartBtn = new System.Windows.Forms.Button();
            restartBtn.BackColor = System.Drawing.SystemColors.Control;
            restartBtn.BackgroundImage = global::StupidBird.Properties.Resources.start_btn;
            restartBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            restartBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            restartBtn.Location = new System.Drawing.Point(150, 300);
            restartBtn.Name = "restartBtn";
            restartBtn.Size = new System.Drawing.Size(75, 45);
            restartBtn.UseVisualStyleBackColor = false;
            this.Controls.Add(restartBtn);
            restartBtn.Click += new System.EventHandler(this.restartBtn_Click);
        }
        /*分数增加*/
        private void GradeAdd()
        {
            if (SingleObject.GetSingle().singleBird.x == pipeDown.x)
            {
                grade++;
                System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(Application.StartupPath + @"/Sound/sfx_point.wav");
                sndPlayer.Play();
                /*显示分数*/
                this.label2.Text = grade.ToString();
            }
        }

        /*重新开始*/
        private void restartBtn_Click(object sender, EventArgs e)
        {
            System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(Application.StartupPath + @"/Sound/btn_press.wav");
            sndPlayer.Play();
            /*增加点击和按键事件*/
            SingleObject.GetSingle().singleBird.x = 50;
            SingleObject.GetSingle().singleBird.y = 200;
            pipeDown.x = 300;
            pipeUp.x = 300;

            InitialGame();
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.timer3.Start();
            this.timer2.Start();
            this.timer1.Start();
            gameOver = false;
            /*移除gameover和按钮*/
            this.Controls.Remove(restartBtn);
            this.Controls.Remove(overLabel);
            /*分数归0*/
            grade = 0;
            this.label2.Text = "0";
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //关闭前一个窗口
            m_form.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
