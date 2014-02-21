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
    class PressurePadEntity : Entity
    {
        private bool isPressed;

        public bool IsPressed
        {
            get
            {
                return isPressed;
            }
        }

        public PressurePadEntity(Point position)
            : base(new Point(5, 0), position, true, false)
        {
            this.isPressed = false;
        }

        public override void Update()
        {
            base.Update();
            Entity e = GameClient.GetSolidEntityAt(Position.X, Position.Y);
            isPressed = false;
            if (e is BoxEntity && ((BoxEntity)e).isLightBox && e.movePercent == 1)
            {
                ((BoxEntity)e).isLightOn = true;
                isPressed = true;
            }
        }
    }
}
