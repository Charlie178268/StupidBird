using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace StupidBird
{
    /// <summary>
    /// 单例模式,获取唯一的游戏对象,使用自定义的类，在窗体中调用
    /// </summary>
    class SingleObject
    {
        private SingleObject() { }
        private static SingleObject _singleObject = null;
        public static SingleObject GetSingle()
        {
            if (_singleObject == null)
            {
                _singleObject = new SingleObject();
            }
            return _singleObject;
        }
        /*存储小鸟对象*/
        public Bird singleBird { get; set; }
        /*添加游戏元素,用基类参数屏蔽差异*/
        public void AddGameObject(GameObject go)
        {
            singleBird = (Bird)go;
        }
        public void DrawGameObject(Graphics g)
        {
            singleBird.Draw(g);
        }
        public void Move()
        {
            singleBird.Move();
        }
    }
}
