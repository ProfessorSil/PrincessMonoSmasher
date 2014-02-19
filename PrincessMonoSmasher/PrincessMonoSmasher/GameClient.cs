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
        public static View view;
        public static string currentRoomName;

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
            view.BeginDraw(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);



            Gl.sB.End();
        }
    }
}
