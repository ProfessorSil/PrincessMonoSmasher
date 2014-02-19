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
    class GameClient
    {
        public const float GRID_SIZE = 16;

        public static View view;
        public static string currentRoomName;

        public static Tile[,] grid;
        public static List<Entity> entities;

        public static void LoadContent()
        {

        }

        public static void Initialize(string roomName)
        {
            view = new View(Vector2.Zero, 1f, 0f, 10f);


        }

        public static void LoadRoom(string room)
        {
            currentRoomName = room;
            //TODO: Add room loading
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



            view.Update();
        }

        public static void Draw()
        {
            //                       This makes the pixelated graphics stay pixelated---V
            view.BeginDraw(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);



            Gl.sB.End();
        }
    }
}
