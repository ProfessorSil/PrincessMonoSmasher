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
        static Texture2D logoTexture, banner, btStart, btSettings, btEditor, btExit;
        static SpriteFont font;
        static Song introSong;

        public static void LoadContent()
        {
            logoTexture = Gl.Load("pendeproLogo");
            font = Gl.Content.Load<SpriteFont>("Font1");
            introSong = Gl.Content.Load<Song>("Music/ElScorchoIntro.wav");
            //Needed for drawing fake menu
            banner = Gl.Load("banner");
            btStart = Gl.Load("buttonStart");
            btSettings = Gl.Load("buttonSettings");
            btEditor = Gl.Load("buttonEditor");
            btExit = Gl.Load("buttonExit");
        }

        public static void Initialize()
        {
            if (GameSettings.MusicOn)
            {
                MediaPlayer.IsRepeating = false;
                MediaPlayer.Volume = GameSettings.MusicVolume;
                MediaPlayer.Play(introSong);
            }
        }

        public static void Update()
        {
            time++;

            if (Gl.KeyPress(Keys.Space) || Gl.KeyPress(Keys.Enter) || Gl.KeyPress(Keys.Escape) || Gl.MousePress(true))
                time = 331;
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

            #region Intro Sequence
            //Logo
            float alpha = 1;
            if (time >= 30 && time < 125)
                alpha = (time - 30) / 95f;
            else if (time >= 170)
                alpha = 0;
            Gl.sB.Draw(logoTexture, new Vector2(Gl.graphics.Viewport.Width / 2f, Gl.graphics.Viewport.Height / 2f), null, Color.White * alpha, 0f,
                new Vector2(logoTexture.Width / 2f, logoTexture.Height / 2f), 1f, SpriteEffects.None, 0f);

            //Banner
            if (time > 230)
            {
                Gl.sB.Draw(banner, new Rectangle(0, 0, Gl.graphics.Viewport.Width, banner.Height * (Gl.graphics.Viewport.Width / banner.Width)), Color.White);
            }
            //Buttons
            if (time > 280)
            {
                fRectangle reference = new fRectangle(Gl.graphics.Viewport.Width / 2f, Gl.graphics.Viewport.Height / 3f * 2f, btStart.Width * 4, btStart.Height * 2);
                Gl.sB.Draw(btStart, (reference + new Vector2(-100, -50)).Convert(), new Rectangle(0, 0, btStart.Width, btStart.Height / 2), Color.White);
                Gl.sB.Draw(btSettings, (reference + new Vector2(100, -50)).Convert(), new Rectangle(0, 0, btSettings.Width, btSettings.Height / 2), Color.White);
                Gl.sB.Draw(btEditor, (reference + new Vector2(-100, 50)).Convert(), new Rectangle(0, 0, btEditor.Width, btEditor.Height / 2), Color.White);
                Gl.sB.Draw(btExit, (reference + new Vector2(100, 50)).Convert(), new Rectangle(0, 0, btExit.Width, btExit.Height / 2), Color.White);
            }
            #endregion

            Gl.sB.End();
        }
    }
}