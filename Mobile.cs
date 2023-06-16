using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Starship
{
    public  abstract class Mobile
    {
        private Vector2 postion;
        private Texture2D mobileTexture;
        


        public abstract void move(int pasDeDeplacement);

        public abstract void intersectWith();

       
      

    }
}
