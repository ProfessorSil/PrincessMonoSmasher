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
    class MenuClient
    {
        static Song MenuSong;
        static SoundEffect selectSound, moveSound;
        static Texture2D banner, btStart, btSettings, btEditor, btExit;
        static fRectangle[] buttonRecs;
        static int hoveringCurrent = -1;

        public static void LoadContent()
        {
            MenuSong = Gl.Content.Load<Song>("Music/ElScorchoSong.wav");
            banner = Gl.Load("banner");
            btStart = Gl.Load("buttonStart");
            btSettings= Gl.Load("buttonSettings");
            btEditor = Gl.Load("buttonEditor");
            btExit = Gl.Load("buttonExit");
            selectSound = Gl.Content.Load<SoundEffect>("SoundEffects/menuSelect");
            moveSound = Gl.Content.Load<SoundEffect>("SoundEffects/menuMove");
        }

        public static void Initialize()
        {
            if (GameSettings.MusicOn)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = GameSettings.MusicVolume;
                MediaPlayer.Play(MenuSong);
            }

            buttonRecs = new fRectangle[4];
            fRectangle reference = new fRectangle(Gl.graphics.Viewport.Width / 2f, Gl.graphics.Viewport.Height / 3f * 2f, btStart.Width * 4, btStart.Height * 2);
            buttonRecs[0] = reference + new Vector2(-100, -50);
            buttonRecs[1] = reference + new Vector2(100, -50);
            buttonRecs[2] = reference + new Vector2(-100, 50);
            buttonRecs[3] = reference + new Vector2(100, 50);

            hoveringCurrent = 0;
        }

        public static void Update()
        {
            for (int i = 0; i < buttonRecs.Length; i++)
            {
                if (buttonRecs[i].Contains(Gl.MousePos))
                {
                    hoveringCurrent = i;
                }
            }
            #region Keyboard Control
            if (Gl.KeyPress(Keys.Right))
            {
                hoveringCurrent++;
                if (hoveringCurrent > 3)
                    hoveringCurrent = 0;
                if (GameSettings.SoundEffectsOn)
                    moveSound.Play(GameSettings.SoundEffectsVolume, 0, 0);
            }
            if (Gl.KeyPress(Keys.Left))
            {
                hoveringCurrent--;
                if (hoveringCurrent < 0)
                    hoveringCurrent = 3;
                if (GameSettings.SoundEffectsOn)
                    moveSound.Play(GameSettings.SoundEffectsVolume, 0, 0);
            }
            if (Gl.KeyPress(Keys.Down))
            {
                hoveringCurrent += 2;
                if (hoveringCurrent > 3)
                    hoveringCurrent -= 4;
                if (GameSettings.SoundEffectsOn)
                    moveSound.Play(GameSettings.SoundEffectsVolume, 0, 0);
            }
            if (Gl.KeyPress(Keys.Up))
            {
                hoveringCurrent -= 2;
                if (hoveringCurrent < 0)
                    hoveringCurrent += 4;
                if (GameSettings.SoundEffectsOn)
                    moveSound.Play(GameSettings.SoundEffectsVolume, 0, 0);
            }
            #endregion

            if (Gl.MousePress(true) || Gl.KeyPress(Keys.Enter))
            {
                if (GameSettings.SoundEffectsOn)
                    selectSound.Play(GameSettings.SoundEffectsVolume, 0, 0);
                if (hoveringCurrent == 0)
                {
                    MediaPlayer.Stop();
                    Gl.game.GotoClient(Clients.Game, "Level1");
                    //TODO: Add smoother transition to game as well as some way to choose levels
                }
                else if (hoveringCurrent == 1)
                {

                }
                else if (hoveringCurrent == 2)
                {

                }
                else if (hoveringCurrent == 3)
                {
                    Gl.game.Exit();
                }
            }
        }

        public static void Draw()
        {
            Gl.graphics.Clear(new Color(20, 20, 20));

            Gl.sB.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            Gl.sB.Draw(btStart, buttonRecs[0].Convert(), new Rectangle(0, (hoveringCurrent == 0) ? btStart.Height / 2 : 0, btStart.Width, btStart.Height / 2), Color.White);
            Gl.sB.Draw(btSettings, buttonRecs[1].Convert(), new Rectangle(0, (hoveringCurrent == 1) ? btSettings.Height / 2 : 0, btSettings.Width, btSettings.Height / 2), Color.White);
            Gl.sB.Draw(btEditor, buttonRecs[2].Convert(), new Rectangle(0, (hoveringCurrent == 2) ? btEditor.Height / 2 : 0, btEditor.Width, btEditor.Height / 2), Color.White);
            Gl.sB.Draw(btExit, buttonRecs[3].Convert(), new Rectangle(0, (hoveringCurrent == 3) ? btExit.Height / 2 : 0, btExit.Width, btExit.Height / 2), Color.White);
            Gl.sB.Draw(banner, new Rectangle(0, 0, Gl.graphics.Viewport.Width, banner.Height * (Gl.graphics.Viewport.Width / banner.Width)), Color.White);

            Gl.sB.End();
        }
    }
}