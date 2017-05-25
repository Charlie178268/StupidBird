using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using StupidBird.Properties;

namespace StupidBird
{
    class Bird:GameObject
    {
        /*获得鸟的图片*/
        private static Image[] birdsImgs =
        {
            Resources._0,
            Resources._1,
            Resources._2
        };
        /// <summary>
        /// 存储小鸟图片的索引,方便绘制到我们的窗体上
        /// </summary>
        public int birdIndex { get; set; }
        public float currentSpeed//当前下落速度
        {
            set; get;
        }
        public int durationTime { set; get; }//下落时间
        /*三张图片高度和宽度一样,用一张图的数据就好了*/
        public Bird(int x, int y, int birdIndex):base(x, y, birdsImgs[0].Width, birdsImgs[0].Height){
            this.birdIndex = birdIndex;
            this.durationTime = 80;
            this.currentSpeed = 0;
        }
        /// <summary>
        /// 绘制小鸟,重写Draw方法
        /// </summary>
      
        public override void Draw(Graphics g)
        {
            /*让图片依次播放*/
            switch(this.birdIndex){
                case 0:g.DrawImage(birdsImgs[0], this.x, this.y); break;
                case 1:g.DrawImage(birdsImgs[1], this.x, this.y); break;
                case 2:g.DrawImage(birdsImgs[2], this.x, this.y); break;
            }
            /*每绘制完一张图片之后索引++*/
            this.birdIndex++;
            if (this.birdIndex > 2)
            {
                this.birdIndex = 0;
            }
        }
        public override void Move()
        {
            this.y -= 30;//每移动一次，向上40
            if (this.y <= 0)
            {
                this.y = 0;//当到达顶部时停住
            }
        }
        /*获得小鸟范围，用于碰撞检测*/
        public Rectangle GetRectangle()
        {
            return new Rectangle(this.x, this.y, birdsImgs[0].Width, birdsImgs[0].Height);
        }
    }
}
