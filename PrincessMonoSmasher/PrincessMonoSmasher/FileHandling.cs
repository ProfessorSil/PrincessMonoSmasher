using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;

namespace PrincessMonoSmasher
{
    class FileHandling
    {
        public static LevelFile LoadLevel(string fileName)
        {
            string path = Gl.Content.RootDirectory + "\\Levels\\" + fileName + ".txt";
            if (!File.Exists(path))
                throw new Exception("Could not find file at '" + path + "'.");

            using (StreamReader reader = new StreamReader(path))
            {
                string line = reader.ReadLine();
                Point size = ParsePoint(line);
                LevelFile level = new LevelFile(size.X, size.Y);
                #region Tile Grid
                for (int y = 0; y < level.Height; y++)
                {
                    if (line != "~")
                        line = reader.ReadLine();

                    List<string> valueList = ParseList(line, '♦');

                    for (int x = 0; x < level.Width; x++)
                    {
                        if (x < valueList.Count)
                        {
                            level.grid[x, y] = ParsePoint(valueList[x]);
                        }
                        else
                        {
                            //Fill unspecified space with holes
                            level.grid[x, y] = new Point(2, 0);
                        }
                    }
                }
                #endregion
                #region Entities
                if (line != "~")
                    line = reader.ReadLine();

                line = reader.ReadLine();
                while (line != null)
                {
                    int indStart = line.IndexOf('('), indEnd = line.IndexOf(')');
                    List<string> vals = ParseList(line.Substring(indStart + 1, indEnd - indStart - 1), '♦');
                    vals.Insert(0, line.Substring(0, indStart));
                    level.AddEntity(vals);
                    line = reader.ReadLine();
                }
                #endregion

                return level;
            }
        }

        public static Point ParsePoint(string s)
        {
            int index = s.IndexOf(',');
            Point value;
            if (!int.TryParse(s.Substring(0, index), out value.X))
            {
                throw new Exception("Could not parse point value of '" + s + "'.");
            }
            if (!int.TryParse(s.Substring(index + 1, s.Length - (index + 1)), out value.Y))
            {
                throw new Exception("Could not parse point value of '" + s + "'.");
            }
            return value;
        }

        public static List<string> ParseList(string line, char seperator)
        {
            int lastSeperator = 0;
            List<string> list = new List<string>();
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == seperator && i != lastSeperator)
                {
                    list.Add(line.Substring(lastSeperator, i - lastSeperator));
                    lastSeperator = i + 1;
                }
                else if (i == line.Length - 1 && i != lastSeperator)
                {
                    list.Add(line.Substring(lastSeperator, (i + 1) - lastSeperator));
                    lastSeperator = i + 1;
                }
            }
            return list;
        }
    }
}
