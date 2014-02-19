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
    class Entity
    {
        public static float moveSpeed = 1 / 10f;
        //This should be getting handled by GameClient.LoadContent()
        public static Texture2D sheet;

        public bool isSolid, isStatic;
        private Point position, positionLast;
        public Point texture;
        public float movePercent;
        /// <summary>
        /// This is strictly for getting itself removed from the entities list in GameClient
        /// </summary>
        public bool alive;

        public Point Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        public Point PositionLast
        {
            get
            {
                return positionLast;
            }
            set
            {
                positionLast = value;
            }
        }
        public Vector2 DrawPosition
        {
            get
            {
                return GameClient.GRID_SIZE * new Vector2(positionLast.X + (position.X - positionLast.X) * movePercent,
                    positionLast.Y + (position.Y - positionLast.Y) * movePercent);
            }
        }
        public bool IsMoving
        {
            get
            {
                return (movePercent != 1f);
            }
        }

        public Entity(Point texture, Point position, bool isStatic, bool isSolid)
        {
            this.texture = texture;
            this.position = position;
            this.positionLast = position;
            this.isStatic = isStatic;
            this.isSolid = isSolid;
            this.movePercent = 1f;
            this.alive = true;
        }

        /// <summary>
        /// This doesn't check whether or not we can actually move here
        /// That should be done with TryMove() of CanMove()
        /// </summary>
        public void Move(int dX, int dY)
        {
            positionLast = position;
            position = new Point(position.X + dX, position.Y + dY);
            movePercent = 0f;
        }
        
        /// <summary>
        /// This checks if we can move to desired position.
        /// If yes then it will move and return true.
        /// If no then it will stay put and return false.
        /// </summary>
        /// <param name="force">How many entities this push can affect in a row</param>
        public bool TryMove(int dX, int dY, int force)
        {
            if (!isStatic && !GameClient.grid[position.X + dX, position.Y + dY].IsSolid)
            {
                Entity e = GameClient.GetEntityAt(position.X + dX, position.Y + dY);

                if (e == null)
                {
                    Move(dX, dY);
                    return true;
                }
                else if (!e.isSolid)
                {
                    Move(dX, dY);
                    return true;
                }
                else if (force >= 1 && e.TryMove(dX, dY, force - 1))
                {
                    Move(dX, dY);
                    return true;
                }
            }
            //We cannot be moved in that direction
            return false;
        }

        public virtual void Update()
        {
            if (IsMoving)
            {
                movePercent += moveSpeed;
                if (movePercent >= 1f)
                {
                    movePercent = 1f;
                    CheckRestingPos();
                }
            }
            else
            {
                //Does anything need to go here?
            }
        }

        public virtual void CheckRestingPos()
        {
            //Static entities should never have this function called automatically
            //Will be overriden in child classes
            //Box and player will make the most use of this function
            //as well as anything that needs to be checked everytime an entity 
            //stops at a position
        }

        public virtual void Draw()
        {
            if (GameSettings.DebugDrawOn)
            {
                Gl.DrawRectangle(new fRectangle(DrawPosition.X, DrawPosition.Y, GameClient.GRID_SIZE, GameClient.GRID_SIZE), Color.Green * 0.8f, Color.Black, 1f);
            }

            Gl.sB.Draw(sheet, DrawPosition, new Rectangle(texture.X * 16, texture.Y * 16, 16, 16), Color.White);
        }
    }
}
