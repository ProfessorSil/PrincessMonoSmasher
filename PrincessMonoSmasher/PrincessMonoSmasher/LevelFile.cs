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
    class LevelFile
    {
        public Point[,] grid;
        public List<string[]> entities;

        public int Width
        {
            get { return grid.GetLength(0); }
        }
        public int Height
        {
            get { return grid.GetLength(1); }
        }

        public LevelFile(int width, int height)
        {
            grid = new Point[width, height];
            entities = new List<string[]>();
        }

        public void AddEntity(string type, string v1 = "", string v2 = "", string v3 = "")
        {
            int length = ((v1 != "") ? 1 : 0) + ((v2 != "") ? 1 : 0) + ((v3 != "") ? 1 : 0);
            entities.Add(new string[length]);
            if (v1 != "")
                entities[entities.Count - 1][0] = v1;
            if (v2 != "")
                entities[entities.Count - 1][1] = v2;
            if (v3 != "")
                entities[entities.Count - 1][2] = v3;
        }
        public void AddEntity(List<string> vals)
        {
            entities.Add(vals.ToArray());
        }
    }
}
