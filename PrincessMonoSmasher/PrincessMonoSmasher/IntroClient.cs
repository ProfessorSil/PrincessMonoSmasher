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
    class IntroClient
    {
        private static int time;
        static Texture2D logoTexture;
        static SpriteFont font;
        static Song introSong;

        public static void LoadContent()
        {
            logoTexture = Gl.Load("pendeproLogo");
            font = Gl.Content.Load<SpriteFont>("Font1");
            introSong = Gl.Content.Load<Song>("Music/ElScorchoIntro.wav");
        }

        public static void Initialize()
        {
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(introSong);
        }

        public static void Update()
        {
            time++;

            if (Gl.KeyPress(Keys.Space) || Gl.KeyPress(Keys.Enter) || Gl.KeyPress(Keys.Escape))
                time = 241;
            if (time > 330)
            {
                MediaPlayer.Stop();
                if (!Gl.game.isLoading)
                {
                    Gl.game.GotoClient(Clients.Menu);
                }
            }
        }

        public static void Draw()
        {
            Gl.graphics.Clear(Color.Black);

            Gl.sB.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            if (Gl.game.isLoading)
            {
                Gl.sB.DrawString(font, "Loading...", Vector2.Zero, Color.White);
            }

            #region Logo
            float alpha = 1;
            if (time >= 30 && time < 125)
                alpha = (time - 30) / 95f;
            else if (time >= 170)
                alpha = 0;
            Gl.sB.Draw(logoTexture, new Vector2(Gl.graphics.Viewport.Width / 2f, Gl.graphics.Viewport.Height / 2f), null, Color.White * alpha, 0f,
                new Vector2(logoTexture.Width / 2f, logoTexture.Height / 2f), 1f, SpriteEffects.None, 0f);
            #endregion

            Gl.sB.End();
        }
    }
}