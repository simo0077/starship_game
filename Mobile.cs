using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starship
{
    public  abstract class Mobile
    {
        public Vector2 position;



        public  void move(string direction, float speed)
        {
            if (direction == "left")
            {
                this.position.X -= speed;
            }
            else if (direction == "right")
            {
                this.position.X += speed;
            }
            else if (direction == "up")
            {
                this.position.Y -= speed;
            }
            else if (direction == "down")
            {
                this.position.Y += speed;
            }
        }





    }
}
