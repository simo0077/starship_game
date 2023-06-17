using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starship
{
    public  class Saucer : Mobile
    {

        public Saucer(Vector2 position, Texture2D mobileTexture, bool isActive)
        {
            this.position = position;
            this.mobileTexture = mobileTexture;
            this.isActive = isActive;
            this.isInScreen = true;
            
        }
        public bool isActive;
        public bool isInScreen;
        

        

        





    }
}
