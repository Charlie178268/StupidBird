using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StupidBird
{
    class Gravity
    {
        public static float _g = 9.8f;
        public static float GetHeight(float v, float t)
        {
            /*位移x = vt+0.5gt^2*/
            return v * t + (float)(0.5 * _g * t * t);
        } 
    }
}
