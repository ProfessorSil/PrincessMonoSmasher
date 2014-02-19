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
    class MenuClient
    {
        static Song MenuSong;

        public static void LoadContent()
        {
            MenuSong = Gl.Content.Load<Song>("Music/ElScorchoSong.wav");

        }

        public static void Initialize()
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(MenuSong);

        }

        public static void Update()
        {

        }

        public static void Draw()
        {
            Gl.graphics.Clear(Color.LightBlue);


        }
    }
}