using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace PrincessMonoSmasher
{
    class GameClient
    {
        public const float GRID_SIZE = 16;
        public static Texture2D tileSheet, wallSheet, groundSheet;
        public static Song gameSong;
        public static SoundEffect sndDrown, sndBlockWater, sndBurn, sndBlockBurn, sndPusher, sndFall, sndPickup;

        public static View view;
        public static string currentRoomName;

        public static Tile[,] grid;
        private static int width, height;
        public static List<Entity> entities;
        public static List<Point> deadPlayers;

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
            wallSheet = Gl.Load("wall");
            groundSheet = Gl.Load("ground");
            gameSong = Gl.Content.Load<Song>("Music/WeAreAllOnDrugs.wav");
            sndBlockBurn = Gl.Content.Load<SoundEffect>("SoundEffects/blockBurn");
            sndBurn = Gl.Content.Load<SoundEffect>("SoundEffects/burn");
            sndPusher = Gl.Content.Load<SoundEffect>("SoundEffects/push");
            sndPickup = Gl.Content.Load<SoundEffect>("SoundEffects/pickup");
            sndBlockWater = Gl.Content.Load<SoundEffect>("SoundEffects/blockWater");
            sndDrown = Gl.Content.Load<SoundEffect>("SoundEffects/splash");
            sndFall = Gl.Content.Load<SoundEffect>("SoundEffects/fall");
        }

        //####INITIATE########INITIATE########INITIATE########INITIATE########INITIATE########INITIATE########INITIATE########INITIATE########INITIATE########INITIATE####
        public static void Initialize(string roomName)
        {
            if (MediaPlayer.Queue.ActiveSong.Name != gameSong.Name)
            {
                if (MediaPlayer.State == MediaState.Playing)
                    MediaPlayer.Stop();
                if (GameSettings.MusicOn)
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Volume = GameSettings.MusicVolume;
                    MediaPlayer.Play(gameSong);
                }
            }

            deadPlayers = new List<Point>();
            currentRoomName = roomName;
            RestartRoom();
        }
        //####INITIATE########INITIATE########INITIATE########INITIATE########INITIATE########INITIATE########INITIATE########INITIATE########INITIATE########INITIATE####

        public static void RestartRoom()
        {
            view = new View(Vector2.Zero, 2f, 0f, 10f);
            LoadRoom(currentRoomName);
        }

        public static void LoadRoom(string room)
        {
            currentRoomName = room;

            LevelFile file = FileHandling.LoadLevel(room);
            grid = new Tile[file.Width, file.Height];
            width = file.Width;
            height = file.Height;

            view.LeftMax = 0;
            view.TopMax = 0;
            view.BottomMax = height * GRID_SIZE;
            view.RightMax = width * GRID_SIZE;

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
                            entities.Add(new BoxEntity(pos, false));
                        }
                        else if (boxType == "l" || boxType == "light" || boxType == "1")
                        {
                            entities.Add(new BoxEntity(pos, true));
                        }
                        else
                        {
                            entities.Add(new BoxEntity(pos, false));
                        }
                    }
                    else
                    {
                        entities.Add(new BoxEntity(pos, false));
                    }
                    #endregion
                }
                else if (type == "g" || type == "gem")
                {
                    #region Gem
                    //ADD GEM
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

        //#####UPDATE##########UPDATE##########UPDATE##########UPDATE##########UPDATE##########UPDATE##########UPDATE##########UPDATE##########UPDATE##########UPDATE#####
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

            #region Entity Updating
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update();
                if (!entities[i].alive)
                {
                    if (i == 0)//If this is the player that's done dying
                    {
                        RestartRoom();
                    }
                    else
                    {
                        entities.RemoveAt(i);
                        i--;
                    }
                }
            }
            #endregion

            view.positionGoto = player.DrawPosition + new Vector2(GRID_SIZE / 2f);
            pusherAnimationFrame += 1;
            if (pusherAnimationFrame >= 12)
                pusherAnimationFrame = 0;

            if (Gl.KeyPress(Keys.R) && !player.IsDead)
            {
                player.Kill(DeathType.Generic);
            }

            view.Update();
        }
        //#####UPDATE##########UPDATE##########UPDATE##########UPDATE##########UPDATE##########UPDATE##########UPDATE##########UPDATE##########UPDATE##########UPDATE#####

        //######DRAW############DRAW############DRAW############DRAW############DRAW############DRAW############DRAW############DRAW############DRAW############DRAW######
        public static void Draw()
        {
            //                       This makes the pixelated graphics stay pixelated---V
            view.BeginDraw(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    DrawGridSpace(x, y);
                }
            }
            foreach (Point p in GameClient.deadPlayers)
            {
                Gl.sB.Draw(Player.SpriteSheet, new Vector2(p.X, p.Y) * GameClient.GRID_SIZE, new Rectangle(3 * 16, 8 * 16, 16, 16), Color.White);
            }
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                entities[i].Draw();
            }

            Gl.sB.End();
        }
        //######DRAW############DRAW############DRAW############DRAW############DRAW############DRAW############DRAW############DRAW############DRAW############DRAW######

        static int pusherAnimationFrame = 0;
        private static void DrawGridSpace(int x, int y)
        {
            Point type = grid[x, y].type;
            if (type == new Point(1, 0))// || type == new Point(0,0)) //<--Uncomment if you want ground to tile together
            {
                #region Wall
                bool left = (x > 0) ? grid[x - 1, y].type == type : true;
                bool right = (x < Width - 1) ? grid[x + 1, y].type == type : true;
                bool up = (y > 0) ? grid[x, y - 1].type == type : true;
                bool down = (y < Height - 1) ? grid[x, y + 1].type == type : true;
                bool upLeft = (y > 0 && x > 0) ? grid[x - 1, y - 1].type == type : true;
                bool upRight = (y > 0 && x < Width - 1) ? grid[x + 1, y - 1].type == type : true;
                bool downLeft = (y < Height - 1 && x > 0) ? grid[x - 1, y + 1].type == type : true;
                bool downRight = (y < Height - 1 && x < Width - 1) ? grid[x + 1, y + 1].type == type : true;
                Point drawCell = new Point(0, 0);
                #region All and None
                if (left && right && up && down)
                {
                    drawCell = new Point(0, 1);
                }
                else if (!left && !right && !up && !down)
                {
                    drawCell = new Point(0, 0);
                }
                #endregion
                #region Corners
                else if (!left && right && !up && down)
                {
                    drawCell = new Point(1, 0);
                }
                else if (left && !right && !up && down)
                {
                    drawCell = new Point(2, 0);
                }
                else if (!left && right && up && !down)
                {
                    drawCell = new Point(1, 1);
                }
                else if (left && !right && up && !down)
                {
                    drawCell = new Point(2, 1);
                }
                #endregion
                #region Tips
                else if (!left && right && !up && !down)
                {
                    drawCell = new Point(0, 2);
                }
                else if (!left && !right && !up && down)
                {
                    drawCell = new Point(1, 2);
                }
                else if (left && !right && !up && !down)
                {
                    drawCell = new Point(2, 2);
                }
                else if (!left && !right && up && !down)
                {
                    drawCell = new Point(3, 2);
                }
                #endregion
                #region Sides
                else if (left && !right && up && down)
                {
                    drawCell = new Point(0, 3);
                }
                else if (left && right && up && !down)
                {
                    drawCell = new Point(1, 3);
                }
                else if (!left && right && up && down)
                {
                    drawCell = new Point(2, 3);
                }
                else if (left && right && !up && down)
                {
                    drawCell = new Point(3, 3);
                }
                #endregion
                #region 2 Sides
                else if (!left && !right && up && down)
                {
                    drawCell = new Point(3, 0);
                }
                else if (left && right && !up && !down)
                {
                    drawCell = new Point(3, 1);
                }
                #endregion

                Gl.sB.Draw((type == new Point(1,0)) ? wallSheet : groundSheet, new Vector2(x * GRID_SIZE, y * GRID_SIZE),
                        new Rectangle(drawCell.X * 16, drawCell.Y * 16, 16, 16), Color.White);

                #region Inner Corners
                drawCell = new Point(-1, -1);
                if (up && left && !upLeft)
                {
                    drawCell = new Point(1, 4);
                    Gl.sB.Draw((type == new Point(1, 0)) ? wallSheet : groundSheet, new Vector2(x * GRID_SIZE, y * GRID_SIZE),
                            new Rectangle(drawCell.X * 16, drawCell.Y * 16, 16, 16), Color.White);
                }
                if (up && right && !upRight)
                {
                    drawCell = new Point(0, 4);
                    Gl.sB.Draw((type == new Point(1, 0)) ? wallSheet : groundSheet, new Vector2(x * GRID_SIZE, y * GRID_SIZE),
                            new Rectangle(drawCell.X * 16, drawCell.Y * 16, 16, 16), Color.White);
                }
                if (down && left && !downLeft)
                {
                    drawCell = new Point(2, 4);
                    Gl.sB.Draw((type == new Point(1, 0)) ? wallSheet : groundSheet, new Vector2(x * GRID_SIZE, y * GRID_SIZE),
                            new Rectangle(drawCell.X * 16, drawCell.Y * 16, 16, 16), Color.White);
                }
                if (down && right && !downRight)
                {
                    drawCell = new Point(3, 4);
                    Gl.sB.Draw((type == new Point(1, 0)) ? wallSheet : groundSheet, new Vector2(x * GRID_SIZE, y * GRID_SIZE),
                            new Rectangle(drawCell.X * 16, drawCell.Y * 16, 16, 16), Color.White);
                }
                #endregion
                #endregion
            }
            else if (type == new Point(2, 0) || type == new Point(4, 0) || type == new Point(5, 0))
            {
                #region Hole, lava, and water
                Gl.sB.Draw(tileSheet, new Vector2(x * GRID_SIZE, y * GRID_SIZE),
                        new Rectangle(grid[x, y].type.X * 16, grid[x, y].type.Y * 16, 16, 16), Color.White);
                bool left = (x > 0) ? grid[x - 1, y].type == type : true;
                bool up = (y > 0) ? grid[x, y - 1].type == type : true;
                bool upLeft = (y > 0 && x > 0) ? grid[x - 1, y - 1].type == type : true;
                if (!up && !left)
                {
                    Gl.sB.Draw(tileSheet, new Vector2(x * GRID_SIZE, y * GRID_SIZE),
                            new Rectangle(3 * 16, 8 * 16, 16, 16), Color.White);
                }
                else if (!up && left)
                {
                    Gl.sB.Draw(tileSheet, new Vector2(x * GRID_SIZE, y * GRID_SIZE),
                            new Rectangle(4 * 16, 8 * 16, 16, 16), Color.White);
                }
                else if (up && !left)
                {
                    Gl.sB.Draw(tileSheet, new Vector2(x * GRID_SIZE, y * GRID_SIZE),
                            new Rectangle(3 * 16, 9 * 16, 16, 16), Color.White);
                }
                else if (!upLeft && up && left)
                {
                    Gl.sB.Draw(tileSheet, new Vector2(x * GRID_SIZE, y * GRID_SIZE),
                            new Rectangle(4 * 16, 9 * 16, 16, 16), Color.White);
                }
                #endregion
            }
            else if (type.Y == 1 && type.X < 4)
            {
                #region Pushers
                Point drawCell = new Point(6 + (pusherAnimationFrame / 3), 6 + type.X);
                Gl.sB.Draw(tileSheet, new Vector2(x * GRID_SIZE, y * GRID_SIZE),
                        new Rectangle(drawCell.X * 16, drawCell.Y * 16, 16, 16), Color.White);
                #endregion
            }
            else
            {
                Gl.sB.Draw(tileSheet, new Vector2(x * GRID_SIZE, y * GRID_SIZE),
                        new Rectangle(grid[x, y].type.X * 16, grid[x, y].type.Y * 16, 16, 16), Color.White);
            }
        }

        public static void PlaySoundEffect(SoundEffect snd)
        {
            if (GameSettings.SoundEffectsOn)
            {
                snd.Play(GameSettings.SoundEffectsVolume, 0, 0);
            }
        }
        /// <summary>
        /// Calculates how loud a sound should be played, depends on the view position at the time
        /// Sound effect volume is a little buggy!
        /// </summary>
        public static void PlaySoundEffectAt(Vector2 position, SoundEffect snd)
        {
            if (GameSettings.SoundEffectsOn)
            {
                float distance = Vector2.Distance(position, GameClient.view.position) - 100;
                float maxDistance = (float)Math.Sqrt(Math.Pow(GameClient.view.ViewSize.X, 2) + Math.Pow(GameClient.view.ViewSize.Y, 2)) - 100;
                float volume = GameSettings.SoundEffectsVolume * (1 - distance / maxDistance);
                if (distance <= 0)
                    volume = GameSettings.SoundEffectsVolume;
                float pan = (position.X - view.position.X) / (view.ViewSize.X / 2f);
                if (pan > 1)
                    pan = 1;
                else if (pan < -1)
                    pan = -1;
                if (volume > 0)
                {
                    snd.Play(volume, 0, pan);
                }
            }
        }
    }
}
