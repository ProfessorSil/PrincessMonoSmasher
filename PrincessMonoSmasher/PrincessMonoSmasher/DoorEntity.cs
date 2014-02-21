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
    class DoorEntity : Entity
    {
        public int type;
        public bool isOpen;

        public DoorEntity(Point position, int type)
            : base(new Point(type, 1), position, true, true)
        {
            this.type = type;
            if (type == 4)
                texture = new Point(4, 0);
            this.isOpen = false;
        }

        public void Open()
        {
            this.isOpen = true;
            this.texture = new Point(9, 9);
            if (type == 4)
                this.texture = new Point(4, 1);
            this.isSolid = false;

            //TODO: Add door opening sound effect
        }
        public void Close()
        {
            this.isOpen = false;
            this.texture = new Point(type, 1);
            if (type == 4)
                this.texture = new Point(4, 0);
            this.isSolid = true;

            //TODO: Add door closing sound effect
        }

        public override void Update()
        {
            base.Update();

            if (type == 4) //Pressure activated door
            {
                bool found = false;
                foreach (Entity e in GameClient.entities)
                {
                    //If we can find one pad that isn't pressed
                    if (e is PressurePadEntity && !((PressurePadEntity)e).IsPressed)
                    {
                        //then the door should be closed
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Open();
                }
                else if (isOpen)
                {
                    Close();
                }
            }
        }
    }
}
