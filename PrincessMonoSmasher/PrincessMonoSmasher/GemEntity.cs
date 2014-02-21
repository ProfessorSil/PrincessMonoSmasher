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
    class GemEntity : Entity
    {

        public GemEntity(Point position)
            : base(new Point(3, 0), position, true, false)
        {

        }

        public override void Update()
        {
            base.Update();
            if (GameClient.player.Position == Position)
            {
                this.alive = false;
                GameClient.PlaySoundEffect(GameClient.sndPickup);
                GameClient.score++;
            }
        }
    }
}
