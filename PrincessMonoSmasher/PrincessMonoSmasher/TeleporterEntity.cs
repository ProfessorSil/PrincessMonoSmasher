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
    class TeleporterEntity : Entity
    {
        public int type;

        public TeleporterEntity(Point position, int type)
            : base(new Point(type, 2), position, true, false)
        {
            this.type = type;

        }

        /// <summary>
        /// Will return (-1,-1) if you cannot teleport, otherwise will return position to teleport to
        /// </summary>
        public Point CanTeleport()
        {
            foreach (Entity e in GameClient.entities)
            {
                if (e.Position != Position && e is TeleporterEntity)
                {
                    TeleporterEntity tele = (TeleporterEntity)e;
                    if (tele.type == type)
                    {
                        if (GameClient.GetSolidEntityAt(tele.Position.X, tele.Position.Y) == null)
                        {
                            return tele.Position;
                        }
                    }
                }
            }

            return new Point(-1, -1);
        }
    }
}
