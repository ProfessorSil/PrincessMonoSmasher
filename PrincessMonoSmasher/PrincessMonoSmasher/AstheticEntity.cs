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
    class AstheticEntity : Entity
    {
        private bool checkedConnections, connectUp = false, connectDown = false, connectLeft = false, connectRight = false;
        public float alpha;

        public AstheticEntity(Point position, Point texture, float alpha = 1)
            : base(texture, position, true, false)
        {
            if (texture == new Point(0, 3))
                checkedConnections = false;
            else if (texture == new Point(1, 3))
            {
                this.texture = new Point(0, 3);
                this.connectRight = true;
            }
            else if (texture == new Point(2, 3))
            {
                this.texture = new Point(0, 3);
                this.connectDown = true;
            }
            else if (texture == new Point(1, 4))
            {
                this.texture = new Point(0, 3);
                this.connectLeft = true;
            }
            else if (texture == new Point(2, 4))
            {
                this.texture = new Point(0, 3);
                this.connectUp = true;
            }
            else
                checkedConnections = true;
            this.alpha = alpha;
        }

        public override void Update()
        {
            if (!checkedConnections && texture == new Point(0, 3))
            {
                foreach (Entity e in GameClient.entities)
                {
                    if (e is AstheticEntity && e.texture == texture)
                    {
                        if (e.Position == new Point(Position.X + 1, Position.Y))
                            connectRight = true;
                        else if (e.Position == new Point(Position.X - 1, Position.Y))
                            connectLeft = true;
                        else if (e.Position == new Point(Position.X, Position.Y + 1))
                            connectDown = true;
                        else if (e.Position == new Point(Position.X, Position.Y - 1))
                            connectUp = true;
                    }
                }
                checkedConnections = true;
            }
            base.Update();
        }


        public override void Draw()
        {
            if (texture == new Point(0, 3))
            {
                Gl.sB.Draw(sheet, DrawPosition, new Rectangle(texture.X * 16, texture.Y * 16, 16, 16), Color.White * alpha);
                if (connectUp)
                    Gl.sB.Draw(sheet, DrawPosition, new Rectangle(2 * 16, 4 * 16, 16, 16), Color.White * alpha);
                if (connectDown)
                    Gl.sB.Draw(sheet, DrawPosition, new Rectangle(2 * 16, 3 * 16, 16, 16), Color.White * alpha);
                if (connectLeft)
                    Gl.sB.Draw(sheet, DrawPosition, new Rectangle(1 * 16, 4 * 16, 16, 16), Color.White * alpha);
                if (connectRight)
                    Gl.sB.Draw(sheet, DrawPosition, new Rectangle(1 * 16, 3 * 16, 16, 16), Color.White * alpha);
            }
            else
            {
                Gl.sB.Draw(sheet, DrawPosition, new Rectangle(texture.X * 16, texture.Y * 16, 16, 16), Color.White * alpha);
            }
        }
    }
}
