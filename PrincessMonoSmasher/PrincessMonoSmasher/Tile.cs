using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace PrincessMonoSmasher
{
    struct Tile
    {
        private bool isSolid;
        public Point type;

        public bool IsSolid
        {
            get { return isSolid; }
        }

        public Tile(Point type)
        {
            if (type == new Point(1, 0))
                this.isSolid = true;
            else if (type.Y == 1 && type.X >= 4 && type.X <= 7)
                this.isSolid = true;
            else
                this.isSolid = false;

            this.type = type;
        }
    }
}
