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

            LevelFile file = FileHandling.LoadLevel(room);
            grid = new Tile[file.Width, file.Height];
            width = file.Width;
            height = file.Height;
            for (int x = 0; x < file.Width; x++)
            {
                for (int y = 0; y < file.Height; y++)
                {
                    grid[x, y] = new Tile(file.grid[x, y]);
                }
            }
            entities = new List<Entity>();
            for (int i = 0; i < file.entities.Count; i++)
            {
                #region Entities
                //TODO: THIS IS JUST A FRAMEWORK, ONLY REGULAR BOXES WORK AT THE MOMENT!!!

                //start (I): Start position of player
                //box: Regular Box
                //teleport: Teleporter
                //exit: Exit
                //button (U): Button
                //switch: Switch Block
                //key: Key
                //door: Door
                //pad: Pressure Pad (For light up box)
                string type = file.entities[i][0].ToLower();
                Point pos = FileHandling.ParsePoint(file.entities[i][1]);
                if (type == "b" || type == "box")
                {
                    #region Boxes
                    if (file.entities[i].Length >= 3)
                    {
                        string boxType = file.entities[i][2].ToLower();
                        if (boxType == "n" || boxType == "normal" || boxType == "0")
                        {
                            entities.Add(new BoxEntity(pos));
                        }
                        else if (boxType == "l" || boxType == "light" || boxType == "1")
                        {
                            //CREATE LIGHT BOX
                        }
                        else
                        {
                            entities.Add(new BoxEntity(pos));
                        }
                    }
                    else
                    {
                        entities.Add(new BoxEntity(pos));
                    }
                    #endregion
                }
                else if (type == "t" || type == "teleporter")
                {
                    #region Teleporters
                    if (file.entities[i].Length >= 2)
                    {
                        string teleType = file.entities[i][3].ToLower();
                        if (teleType == "red" || teleType == "0")
                        {
                            //CREATE RED TELEPORT
                        }
                        else if (teleType == "green" || teleType == "1")
                        {
                            //CREATE GREEN TELEPORT
                        }
                        else if (teleType == "blue" || teleType == "2")
                        {
                            //CREATE BLUE TELEPORT
                        }
                        else
                        {
                            //ADD DEFAULT TELEPORT
                        }
                    }
                    else
                    {
                        //ADD DEFAULT TELEPORT
                    }
                    #endregion
                }
                else if (type == "e" || type == "exit")
                {
                    #region Exit
                    //ADD EXIT
                    #endregion
                }
                else if (type == "u" || type == "button")
                {
                    #region Buttons
                    if (file.entities[i].Length >= 3)
                    {
                        string buttonType = file.entities[i][2].ToLower();
                        //gray, yellow, orange, blue, green, purple
                        if (buttonType == "gray" || buttonType == "0")
                        {
                            //CREATE GRAY BUTTON
                        }
                        else if (buttonType == "yellow" || buttonType == "1")
                        {
                            //CREATE YELLOW BUTTON
                        }
                        else if (buttonType == "orange" || buttonType == "2")
                        {
                            //CREATE ORANGE BUTTON
                        }
                        else if (buttonType == "blue" || buttonType == "3")
                        {
                            //CREATE BLUE BUTTON
                        }
                        else if (buttonType == "green" || buttonType == "4")
                        {
                            //CREATE GREEN BUTTON
                        }
                        else if (buttonType == "purple" || buttonType == "5")
                        {
                            //CREATE PURPLE BUTTON
                        }
                        else
                        {
                            //ADD DEFAULT BUTTON
                        }
                    }
                    else
                    {
                        //ADD DEFAULT BUTTON
                    }
                    #endregion
                }
                else if (type == "s" || type == "switch")
                {
                    #region Switch Blocks
                    if (file.entities[i].Length >= 3)
                    {
                        string switchType = file.entities[i][2].ToLower();
                        //yellow, orange, blue, green, purple
                        //There is no 'gray' switch blocks only 'gray' buttons
                        if (switchType == "yellow" || switchType == "0")
                        {
                            //CREATE YELLOW SWITCH
                        }
                        else if (switchType == "orange" || switchType == "1")
                        {
                            //CREATE ORANGE SWITCH
                        }
                        else if (switchType == "blue" || switchType == "2")
                        {
                            //CREATE BLUE SWITCH
                        }
                        else if (switchType == "green" || switchType == "3")
                        {
                            //CREATE GREEN SWITCH
                        }
                        else if (switchType == "purple" || switchType == "4")
                        {
                            //CREATE PURPLE SWITCH
                        }
                        else
                        {
                            //ADD DEFAULT SWITCH
                        }
                    }
                    else
                    {
                        //ADD DEFAULT SWITCH
                    }
                    #endregion
                }
                else if (type == "k" || type == "key")
                {
                    #region Keys
                    if (file.entities[i].Length >= 3)
                    {
                        string keyType = file.entities[i][2].ToLower();
                        if (keyType == "gray" || keyType == "0")
                        {
                            //CREATE GRAY KEY
                        }
                        else if (keyType == "blue" || keyType == "1")
                        {
                            //CREATE BLUE KEY
                        }
                        else if (keyType == "green" || keyType == "2")
                        {
                            //CREATE GREEN KEY
                        }
                        else if (keyType == "brown" || keyType == "3")
                        {
                            //CREATE BROWN KEY
                        }
                        else
                        {
                            //ADD DEFAULT KEY
                        }
                    }
                    else
                    {
                        //ADD DEFAULT KEY
                    }
                    #endregion
                }
                else if (type == "d" || type == "door")
                {
                    #region Doors
                    if (file.entities[i].Length >= 3)
                    {
                        string doorType = file.entities[i][2].ToLower();
                        if (doorType == "gray" || doorType == "0")
                        {
                            //CREATE GRAY DOOR
                        }
                        else if (doorType == "blue" || doorType == "1")
                        {
                            //CREATE BLUE DOOR
                        }
                        else if (doorType == "green" || doorType == "2")
                        {
                            //CREATE GREEN DOOR
                        }
                        else if (doorType == "brown" || doorType == "3")
                        {
                            //CREATE BROWN DOOR
                        }
                        else if (doorType == "pressure" || doorType == "4")
                        {
                            //CREATE PRESSURE DOOR
                        }
                        else
                        {
                            //ADD DEFAULT DOOR
                        }
                    }
                    else
                    {
                        //ADD DEFAULT DOOR
                    }
                    #endregion
                }
                else if (type == "p" || type == "pad")
                {
                    #region Exit
                    //ADD PRESSURE PAD
                    #endregion
                }
                else if (type == "i" || type == "start")
                {
                    entities.Insert(0, new Player(pos));
                }
                else
                {
                    //Didn't recognize the entity type but would still like to see it
                    //loaded and shown when debugDraw is on
                    entities.Add(new Entity(new Point(0, 3), pos, true, false));
                }
                #endregion
            }

            if (!(entities[0] is Player))
            {
                entities.Insert(0, new Player(new Point(1, 1)));
            }
        }

        public static void CreateEmptyRoom(int width, int height)
        {
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
            entities = new List<Entity>();
            entities.Add(new Player(new Point(1, 1)));
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
