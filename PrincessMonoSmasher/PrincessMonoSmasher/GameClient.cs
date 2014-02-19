using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;

namespace PrincessMonoSmasher
{
    class GameClient
    {
        public const float GRID_SIZE = 16;
        public static Texture2D tileSheet;
        public static Song gameSong;

        public static View view;
        public static string currentRoomName;

        public static Tile[,] grid;
        private static int width, height;
        public static List<Entity> entities;

        public static Player player
        {
            get
            {
                //Player should always be the first entity in the list
                return (Player)entities[0];
            }
        }
        public static int Width
        {
            get { return width; }
        }
        public static int Height
        {
            get { return height; }
        }

        public static void LoadContent()
        {
            Player.SpriteSheet = Gl.Load("playerSheet");
            Entity.sheet = Gl.Load("entitiesSheet");
            tileSheet = Gl.Load("tileSheet");
            //gameSong = Gl.Content.Load<Song>("Music/WeAreAllOnDrugs.wav");
        }

        public static void Initialize(string roomName)
        {
            view = new View(Vector2.Zero, 2f, 0f, 10f);
            

            LoadRoom(roomName);
        }

        public static void LoadRoom(string room)
        {
            currentRoomName = room;
            //TODO: Add room loading

            //Placeholder:
            {
                width = 20;
                height = 20;
                view.LeftMax = 0;
                view.TopMax = 0;
                view.BottomMax = height * GRID_SIZE;
                view.RightMax = width * GRID_SIZE;
                grid = new Tile[width, height];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                            grid[x, y] = new Tile(new Point(1, 0));
                        else
                            grid[x, y] = new Tile(new Point(0, 0));
                    }
                }
                grid[8, 1] = new Tile(new Point(3, 0));
                grid[9, 1] = new Tile(new Point(2, 0));
                grid[10, 1] = new Tile(new Point(4, 0));
                grid[11, 1] = new Tile(new Point(5, 0));
                entities = new List<Entity>();
                entities.Add(new Player(new Point(1, 1)));
                entities.Add(new BoxEntity(new Point(8, 2)));
            }
        }

        /// <summary>
        /// This does not account for if there are two entities in one spot (which shouldn't happen when it's important)
        /// </summary>
        public static Entity GetEntityAt(int x, int y)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].Position == new Point(x, y))
                    return entities[i];
            }

            return null;
        }

        public static void Update()
        {
            #region Test View Movement
            {
                float speed = 5f;
                if (Gl.KeyDown(Keys.W))
                    view.positionGoto += view.UpVector * speed;
                if (Gl.KeyDown(Keys.A))
                    view.positionGoto += view.LeftVector * speed;
                if (Gl.KeyDown(Keys.S))
                    view.positionGoto += view.DownVector * speed;
                if (Gl.KeyDown(Keys.D))
                    view.positionGoto += view.RightVector * speed;

                //if (Gl.KeyDown(Keys.Right))
                //    view.rotation += MathHelper.Pi / 60f;
                //if (Gl.KeyDown(Keys.Left))
                //    view.rotation -= MathHelper.Pi / 60f;
                //if (Gl.KeyDown(Keys.Up))
                //    view.zoom += 0.05f;
                //if (Gl.KeyDown(Keys.Down))
                //    view.zoom -= 0.05f;
            }
            #endregion

            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update();
                if (!entities[i].alive)
                {
                    if (i == 0)//If this is the player that's done dying
                    {
                        RestartLevel();
                    }
                    else
                    {
                        entities.RemoveAt(i);
                        i--;
                    }
                }
            }
            view.positionGoto = player.DrawPosition + new Vector2(GRID_SIZE / 2f);

            if (Gl.KeyPress(Keys.R) && !player.IsDead)
            {
                player.Kill(DeathType.Generic);
            }

            view.Update();
        }

        public static void RestartLevel()
        {
            Initialize(currentRoomName);
        }

        public static void Draw()
        {
            //                       This makes the pixelated graphics stay pixelated---V
            view.BeginDraw(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Gl.sB.Draw(tileSheet, new Vector2(x * GRID_SIZE, y * GRID_SIZE), 
                        new Rectangle(grid[x,y].type.X * 16, grid[x,y].type.Y * 16, 16, 16), Color.White);
                }
            }
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                entities[i].Draw();
            }

            Gl.sB.End();
        }
    }
}
