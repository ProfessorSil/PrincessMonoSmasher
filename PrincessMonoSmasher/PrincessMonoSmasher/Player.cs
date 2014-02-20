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
    public enum DeathType
    {
        Fall,
        Burn,
        Drown,
        Generic
    }

    class Player : Entity
    {
        //This should get handled by the GameClient.LoadContent()
        public static Texture2D SpriteSheet;

        //0-Down 1-Up 2-Left 3-Right
        public int facingDirection;
        private bool isDying;
        private DeathType typeOfDeath;
        private int deathTimer;

        public bool IsDead
        {
            get { return isDying; }
        }

        public Player(Point position)
            : base(new Point(0, 0), position, false, true)
        {
            this.facingDirection = 0;
            this.isDying = false;
            this.typeOfDeath = DeathType.Generic;

        }

        public override void Update()
        {
            if (!isDying)
            {
                base.Update();

                if (!IsMoving)
                {
                    #region Movement
                    if (Gl.KeyDown(Keys.Left))
                    {
                        TryMove(-1, 0, 1);
                        facingDirection = 2;
                    }
                    if (Gl.KeyDown(Keys.Right) && !IsMoving)
                    {
                        TryMove(1, 0, 1);
                        facingDirection = 3;
                    }
                    if (Gl.KeyDown(Keys.Up) && !IsMoving)
                    {
                        TryMove(0, -1, 1);
                        facingDirection = 1;
                    }
                    if (Gl.KeyDown(Keys.Down) && !IsMoving)
                    {
                        TryMove(0, 1, 1);
                        facingDirection = 0;
                    }
                    #endregion
                }
            }
            else
            {
                #region Death Handling
                deathTimer++;
                if (typeOfDeath == DeathType.Fall)
                {
                    if (deathTimer > 32)
                    {
                        alive = false;
                    }
                }
                else if (typeOfDeath == DeathType.Burn)
                {
                    if (deathTimer > 64)
                    {
                        alive = false;
                    }
                }
                else if (typeOfDeath == DeathType.Drown)
                {
                    if (deathTimer > 64)
                    {
                        alive = false;
                    }
                }
                else if (typeOfDeath == DeathType.Generic)
                {
                    if (deathTimer > 96)
                    {
                        alive = false;
                        GameClient.deadPlayers.Add(Position);
                    }
                }
                #endregion
            }
        }

        public override void CheckRestingPos()
        {
            base.CheckRestingPos();

            Tile t = GameClient.grid[Position.X, Position.Y];
            if (t.type == new Point(0, 1)) //Pusher Right
            {
                if (TryMove(1, 0, 10))
                    GameClient.PlaySoundEffect(GameClient.sndPusher);
            }
            else if (t.type == new Point(1, 1)) //Pusher Down
            {
                if (TryMove(0, 1, 10))
                    GameClient.PlaySoundEffect(GameClient.sndPusher);
            }
            else if (t.type == new Point(2, 1)) //Pusher Left
            {
                if (TryMove(-1, 0, 10))
                    GameClient.PlaySoundEffect(GameClient.sndPusher);
            }
            else if (t.type == new Point(3, 1)) //Pusher Up
            {
                if (TryMove(0, -1, 10))
                    GameClient.PlaySoundEffect(GameClient.sndPusher);
            }
            else if (t.type == new Point(2, 0)) //Hole
            {
                Kill(DeathType.Fall);
            }
            else if (t.type == new Point(4, 0)) //Lava
            {
                Kill(DeathType.Burn);
            }
            else if (t.type == new Point(5, 0)) //Water
            {
                Kill(DeathType.Drown);
            }
            else if (t.type == new Point(3, 0)) //Ice
            {
                TryMove(Position.X - PositionLast.X, Position.Y - PositionLast.Y, 1);//<--Force might change here depending on preference
            }
        }

        public void Kill(DeathType type)
        {
            isDying = true;
            deathTimer = 0;
            typeOfDeath = type;
            isStatic = true;
            if (type == DeathType.Burn)
                GameClient.PlaySoundEffect(GameClient.sndBurn);
            else if (type == DeathType.Drown)
                GameClient.PlaySoundEffect(GameClient.sndDrown);
            else if (type == DeathType.Fall)
                GameClient.PlaySoundEffect(GameClient.sndFall);
        }

        public override void Draw()
        {
            //base.Draw(); <--We don't want it to draw our sprite automatically
            if (GameSettings.DebugDrawOn)
            {
                Gl.DrawRectangle(new fRectangle(DrawPosition.X, DrawPosition.Y, GameClient.GRID_SIZE, GameClient.GRID_SIZE), Color.Green * 0.8f, Color.Black, 1f);
            }

            Point cell = new Point(0, 0);
            if (!isDying)
            {
                cell.Y = (IsMoving) ? 1 : 0;
                cell.X = facingDirection;
                Gl.sB.Draw(SpriteSheet, DrawPosition, new Rectangle(cell.X * 16, cell.Y * 16, 16, 16), Color.White);
            }
            else
            {
                #region Death Drawing
                if (typeOfDeath == DeathType.Fall)
                {
                    cell = new Point(deathTimer / 8, 2);
                    Gl.sB.Draw(SpriteSheet, DrawPosition, new Rectangle(cell.X * 16, cell.Y * 16, 16, 16), Color.White);
                }
                if (typeOfDeath == DeathType.Burn)
                {
                    cell = new Point(deathTimer / 8, 4);
                    if (cell.X > 3)
                    {
                        cell.X -= 4;
                        cell.Y++;
                    }
                    Gl.sB.Draw(SpriteSheet, DrawPosition, new Rectangle(cell.X * 16, cell.Y * 16, 16, 16), Color.White);
                }
                if (typeOfDeath == DeathType.Drown)
                {
                    cell = new Point(deathTimer / 8, 3);
                    if (cell.X > 3)
                        cell.X -= 4;
                    Gl.sB.Draw(SpriteSheet, DrawPosition, new Rectangle(cell.X * 16, cell.Y * 16, 16, 16), Color.White);
                }
                if (typeOfDeath == DeathType.Generic)
                {
                    cell = new Point(deathTimer / 8, 6);
                    if (cell.X > 3)
                    {
                        cell.X -= 4;
                        cell.Y++;
                    }
                    if (cell.X > 3)
                    {
                        cell.X -= 4;
                        cell.Y++;
                    }
                    Gl.sB.Draw(SpriteSheet, DrawPosition, new Rectangle(cell.X * 16, cell.Y * 16, 16, 16), Color.White);
                }
                #endregion
            }
        }
    }
}
