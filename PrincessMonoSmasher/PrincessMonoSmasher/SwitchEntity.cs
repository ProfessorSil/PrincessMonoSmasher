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
    class SwitchEntity : Entity
    {
        public int currentSetting;
        public List<Point> settings;
        public int type;

        public SwitchEntity(Point position, int type, List<string> settings)
            : base(new Point(5 + type - 1, 1), position, true, false)
        {
            this.type = type;
            this.settings = new List<Point>();
            this.currentSetting = 0;
            for (int i = 0; i < settings.Count; i++)
            {
                try
                {
                    Point p = FileHandling.ParsePoint(settings[i]);
                    this.settings.Add(p);
                    if (GameClient.grid[position.X, position.Y].type == p)
                    {
                        currentSetting = this.settings.Count - 1;
                    }
                }
                catch
                {

                }
            }
            if (this.settings.Count == 0)
            {
                this.settings.Add(GameClient.grid[position.X, position.Y].type);
            }
        }

        public void Switch()
        {
            currentSetting++;
            if (currentSetting >= settings.Count)
                currentSetting = 0;
            GameClient.grid[Position.X, Position.Y] = new Tile(settings[currentSetting]);
        }

        public override void Draw()
        {
            Gl.sB.Draw(sheet, DrawPosition, new Rectangle(texture.X * 16, texture.Y * 16, 16, 16), Color.White * 0.75f);
        }
    }
}
