using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using StupidBird.Properties;
namespace StupidBird
{
    /*枚举管道的方向，管道分为两种 一个是脑袋向上的管道 一个是脑袋向下的管道*/
        public enum pipeDirection
        {
            Up,
            Down
        }
        public class Pipe
        {
            public int x { get; set; }
            public int y { get; set; } 

            public pipeDirection pipedirction { get; set; }

            public Pipe(int x, int y, pipeDirection pd)
            {
                this.x = x;
                this.y = y;
                this.pipeWidth = 127;
                this.pipedirction = pd;
            }

            public int pipeHeight { get; set; }

            public int pipeWidth { get; set; }

            //获得管道的图片
            public static Image pipeImg = Resources.flappy_packer;

            //在图片中将你想要的图片 拿出来

            /// <summary>
            /// 从大图中截取想要的图片矩形 返回
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public static Image GetImage(int x, int y, int w, int h)
            {
                //创建位图对象 宽128 高 830
                Bitmap bmp = new Bitmap(w, h);
                //将截取出来的图片 通过GDI+对象 绘制到位图上
                Graphics g = Graphics.FromImage(bmp);
                //开始绘制
                //第一个矩形表示我要绘制到bmp上矩形的大小
                //第二个矩形表示我从原图的那个位置开始截取图片
                g.DrawImage(pipeImg, new Rectangle(0, 0, w, h), new Rectangle(x, y, w, h), GraphicsUnit.Pixel);
                return bmp;
            }

            public void DrawPipe(Graphics g)
            {
                //在绘制管道的时候 应该根据不同的方向 绘制不同的管道
                switch (this.pipedirction)
                {
                    case pipeDirection.Up:
                        g.DrawImage(GetImage(160, 495, 128, 830), this.x, this.y);
                        break;
                    case pipeDirection.Down:
                        g.DrawImage(GetImage(10, 459, 128, 830), this.x, this.y);
                        break;
                }
            }
        /*获得管道范围，用于碰撞检测*/
        public Rectangle GetRectangle()
        {
            return new Rectangle(this.x, this.y, this.pipeWidth, this.pipeHeight);
        }
        }
}
