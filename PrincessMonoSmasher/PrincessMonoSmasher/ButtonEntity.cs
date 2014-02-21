
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
    class ButtonEntity : Entity
    {
        public Point targetPos;
        private Entity target;
        private bool needToFindTarget;
        /// <summary>
        /// isBeingPressed will be updated by the PlayerEntity class
        /// </summary>
        public bool isBeingPressed, isBeingPressedLast;
        public bool isOneTimeUse;
        /// <summary>
        /// 0 = CustomTarget
        /// 1 = yellow
        /// 2 = orange
        /// 3 = blue 
        /// 4 = green
        /// 5 = purple
        /// </summary>
        public int type;

        public bool Pressed
        {
            get { return isBeingPressed && !isBeingPressedLast; }
        }

        public ButtonEntity(Point position, int type, bool isOneTimeUse)
            : base(new Point(4 + type, 2), position, true, true)
        {
            this.type = type;
            this.isOneTimeUse = isOneTimeUse;
            this.isBeingPressed = false;
            this.targetPos = new Point(-1, -1);
            needToFindTarget = false;
        }
        public ButtonEntity(Point position, int type, bool isOneTimeUse, Point targetPos)
            : base(new Point(4 + type, 2), position, true, true)
        {
            this.type = type;
            this.isOneTimeUse = isOneTimeUse;
            this.isBeingPressed = false;
            this.targetPos = targetPos;
            if (type == 0)
                needToFindTarget = true;
            else
                needToFindTarget = false;
        }

        public override void Update()
        {
            base.Update();

            if (needToFindTarget)
            {
                needToFindTarget = false;
                if (targetPos != new Point(-1, -1))
                {
                    for (int i = GameClient.entities.Count - 1; i >= 0; i--)
                    {
                        Entity e = GameClient.entities[i];
                        if (e.Position == targetPos)
                        {
                            if (e is DoorEntity || e is SwitchEntity)
                            {
                                target = e;
                                break;
                            }
                        }
                    }
                }
            }

            if (Pressed)
            {
                GameClient.PlaySoundEffect(GameClient.sndButton);
                if (type == 0 && target != null)
                {
                    if (target is DoorEntity)
                    {
                        ((DoorEntity)target).Open();
                    }
                    else if (target is SwitchEntity)
                    {
                        ((SwitchEntity)target).Switch();
                    }
                }
                else if (type > 0)
                {
                    for (int i = 0; i < GameClient.entities.Count; i++)
                    {
                        if (GameClient.entities[i] is SwitchEntity && ((SwitchEntity)GameClient.entities[i]).type == type)
                        {
                            ((SwitchEntity)GameClient.entities[i]).Switch();
                        }
                    }
                }
            }

            isBeingPressedLast = isBeingPressed;

            if (!isOneTimeUse)
                isBeingPressed = false;
        }

        public override void Draw()
        {
            if (isBeingPressed || isBeingPressedLast)
                texture.Y = 3;
            else
                texture.Y = 2;

            base.Draw();
        }
    }
}
