using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

/// <summary>
/// 游戏中所有的元素的基类
/// </summary>
namespace StupidBird
{
    public abstract class GameObject
    {
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public GameObject(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
        /*小鸟和柱子的移动方式不一样,所以定义为抽象方法*/
        public abstract void Move();
        /*小鸟和柱子的绘制方式也不同,定义为抽象方法*/
        public abstract void Draw(Graphics g);    
    }
}
