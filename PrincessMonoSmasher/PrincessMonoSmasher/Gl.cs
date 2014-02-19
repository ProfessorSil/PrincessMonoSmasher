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
    class Gl
    {
        public static KeyboardState kState, kStateLast;
        public static MouseState mState, mStateLast;
        public static ContentManager Content;
        public static SpriteBatch sB;
        public static GraphicsDevice graphics;
        public static Game1 game;
        public static GameTime gameTime;
        public static Random rand;
        public static Texture2D dot;

        public static Vector2 MousePos
        {
            get { return new Vector2(mState.X, mState.Y); }
        }
        public static Point MousePoint
        {
            get { return new Point(mState.X, mState.Y); }
        }

        public static void Initialize(Game1 game1, SpriteBatch spriteBatch, ContentManager manager, GraphicsDevice device)
        {
            graphics = device;
            game = game1;
            sB = spriteBatch;
            Content = manager;

            rand = new Random();
            dot = new Texture2D(device, 1, 1);
            dot.SetData<Color>(new Color[1] { Color.White });
        }

        public static void Update(GameTime gt)
        {
            kStateLast = kState;
            mStateLast = mState;
            mState = Mouse.GetState();
            kState = Keyboard.GetState();
            gameTime = gt;
        }

        public static bool KeyPress(Keys key)
        {
            return (kState.IsKeyDown(key) && !kStateLast.IsKeyDown(key));
        }
        public static bool KeyRelease(Keys key)
        {
            return (!kState.IsKeyDown(key) && kStateLast.IsKeyDown(key));
        }
        public static bool KeyDown(Keys key)
        {
            return kState.IsKeyDown(key);
        }
        public static bool MousePress(bool left)
        {
            if (left)
                return (mState.LeftButton == ButtonState.Pressed && mStateLast.LeftButton == ButtonState.Released);
            else
                return (mState.RightButton == ButtonState.Pressed && mStateLast.RightButton == ButtonState.Released);
        }
        public static bool MouseRelease(bool left)
        {
            if (left)
                return (mState.LeftButton == ButtonState.Released && mStateLast.LeftButton == ButtonState.Pressed);
            else
                return (mState.RightButton == ButtonState.Released && mStateLast.RightButton == ButtonState.Pressed);
        }
        public static bool MouseDown(bool left)
        {
            if (left)
                return (mState.LeftButton == ButtonState.Pressed);
            else
                return (mState.RightButton == ButtonState.Pressed);
        }

        public static Texture2D Load(string sprite)
        {
            return Content.Load<Texture2D>(sprite);
        }
        public static void DrawLine(Vector2 p1, Vector2 p2, Color color, float thickness = 1f, float depth = 0f)
        {
            sB.Draw(dot, p1, null, color, p1.DirectionTo(p2), new Vector2(0, 0.5f), new Vector2(Vector2.Distance(p1, p2), thickness), SpriteEffects.None, depth);
        }
        public static void DrawRectangle(Rectangle rec, Color color, float depth = 0f)
        {
            sB.Draw(dot, rec, null, color, 0f, Vector2.Zero,  SpriteEffects.None, depth);
        }
        public static void DrawRectangle(Rectangle rec, Color color, Color borderColor, float borderThickness = 1f, float depth = 0f)
        {
            sB.Draw(dot, rec, null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
            if (borderThickness != 0f)
            {
                DrawRectangle(new fRectangle(rec.X, rec.Y, rec.Width, borderThickness), borderColor, depth);
                DrawRectangle(new fRectangle(rec.X, rec.Y, borderThickness, rec.Height), borderColor, depth);
                DrawRectangle(new fRectangle(rec.X, rec.Bottom - borderThickness, rec.Width, borderThickness), borderColor, depth);
                DrawRectangle(new fRectangle(rec.Right - borderThickness, rec.Y, borderThickness, rec.Height), borderColor, depth);
            }
        }
        public static void DrawRectangle(fRectangle rec, Color color, float depth = 0f)
        {
            sB.Draw(dot, rec.Position, null, color, 0f, Vector2.Zero, rec.Size, SpriteEffects.None, depth);
        }
        public static void DrawRectangle(fRectangle rec, Color color, Color borderColor, float borderThickness = 1f, float depth = 0f)
        {
            sB.Draw(dot,  rec.Position, null, color, 0f, Vector2.Zero, rec.Size, SpriteEffects.None, depth);
            if (borderThickness != 0f)
            {
                DrawRectangle(new fRectangle(rec.X, rec.Y, rec.Width, borderThickness), borderColor, depth);
                DrawRectangle(new fRectangle(rec.X, rec.Y, borderThickness, rec.Height), borderColor, depth);
                DrawRectangle(new fRectangle(rec.X, rec.Bottom - borderThickness, rec.Width, borderThickness), borderColor, depth);
                DrawRectangle(new fRectangle(rec.Right - borderThickness, rec.Y, borderThickness, rec.Height), borderColor, depth);
            }
        }
        public static void DrawOBB(OBB rec, Color color, float depth = 0f)
        {
            sB.Draw(dot, rec.Center, null, color, rec.Rotation, new Vector2(0.5f, 0.5f), rec.Size, SpriteEffects.None, depth);
        }
        public static void DrawOBB(OBB rec, Color color, Color borderColor, float borderThickness = 1f, float depth = 0f)
        {
            sB.Draw(dot, rec.Center, null, color, rec.Rotation, new Vector2(0.5f, 0.5f), rec.Size, SpriteEffects.None, depth);
            if (borderThickness != 0f)
            {
                sB.Draw(dot, rec.TopLeft, null, borderColor, rec.Rotation, Vector2.Zero, new Vector2(rec.Width, borderThickness), SpriteEffects.None, depth);
                sB.Draw(dot, rec.BottomLeft, null, borderColor, rec.Rotation - MathHelper.PiOver2, Vector2.Zero, new Vector2(rec.Height, borderThickness), SpriteEffects.None, depth);
                sB.Draw(dot, rec.TopRight, null, borderColor, rec.Rotation + MathHelper.PiOver2, Vector2.Zero, new Vector2(rec.Height, borderThickness), SpriteEffects.None, depth);
                sB.Draw(dot, rec.BottomRight, null, borderColor, rec.Rotation + MathHelper.Pi, Vector2.Zero, new Vector2(rec.Width, borderThickness), SpriteEffects.None, depth);
            }
        }
    }

    static class Calc
    {
        public static float Direction(Vector2 p1, Vector2 p2)
        {
            return (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
        }
        public static float DirectionTo(this Vector2 p1, Vector2 p2)
        {
            return (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
        }

        //Other Class Extensions
        public static bool Intersects(Circle circle, Rectangle rec)
        {
            ///Closest point to circle
            Vector2 recVec = new Vector2(MathHelper.Clamp(circle.center.X, rec.Left, rec.Right), MathHelper.Clamp(circle.center.Y, rec.Top, rec.Bottom));

            ///Distance between circle and rectangle
            float Distance = Vector2.Distance(recVec, circle.center);

            ///If they are intersecting
            return (Distance <= circle.radius);
        }
        public static fRectangle Convert(this Rectangle rec)
        {
            return new fRectangle(rec.X, rec.Y, rec.Width, rec.Height);
        }
        public static Rectangle Convert(this fRectangle rec)
        {
            return new Rectangle((int)rec.X, (int)rec.Y, (int)rec.Width, (int)rec.Height);
        }
        public static Point Convert(Vector2 pos)
        {
            return new Point((int)pos.X, (int)pos.Y);
        }
        public static Vector2 Convert(Point pos)
        {
            return new Vector2(pos.X, pos.Y);
        }
    }

    struct Circle
    {
        public Vector2 center;
        public float radius;

        public Circle(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        /// <summary>
        /// Returns a bool indicating whether the point lies inside the circle (inclusive)
        /// </summary>
        public bool Contains(Vector2 point)
        {
            return (Math.Pow(center.Y - point.Y, 2) + Math.Pow(center.X - point.X, 2) <= radius * radius);
        }
        public static bool Contains(Circle circle, Vector2 point)
        {
            return (Math.Pow(circle.center.Y - point.Y, 2) + Math.Pow(circle.center.X - point.X, 2) <= circle.radius * circle.radius);
        }
        /// <summary>
        /// Returns a bool indicating whether the two circles intersect
        /// </summary>
        public bool Intersects(Circle circle)
        {
            return (Math.Pow(center.Y - circle.center.Y, 2) + Math.Pow(center.X - circle.center.X, 2) < Math.Pow(radius + circle.radius, 2));
        }
        public static bool Intersects(Circle circleA, Circle circleB)
        {
            return (Math.Pow(circleA.center.Y - circleB.center.Y, 2) + Math.Pow(circleA.center.X - circleB.center.X, 2) < Math.Pow(circleA.radius + circleB.radius, 2));
        }
        /// <summary>
        /// Returns a bool indicating whether the circle and rectangle intersect (inclusive)
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        public bool Intersects(Rectangle rec)
        {
            ///Closest point to circle
            Vector2 recVec = new Vector2(MathHelper.Clamp(center.X, rec.Left, rec.Right), MathHelper.Clamp(center.Y, rec.Top, rec.Bottom));

            ///Distance between circle and rectangle
            float Distance = Vector2.Distance(recVec, center);

            ///If they are intersecting
            return (Distance <= radius);
        }
        /// <summary>
        /// Returns the min distance between the edge of both circles
        /// </summary>
        public float Distance(Circle circle)
        {
            return (Vector2.Distance(center, circle.center) - radius - circle.radius);
        }
        public static float Distance(Circle circleA, Circle circleB)
        {
            return (Vector2.Distance(circleA.center, circleB.center) - circleA.radius - circleB.radius);
        }

        public static Circle operator +(Circle c, Vector2 p)
        {
            return new Circle(c.center + p, c.radius);
        }
        public static Circle operator -(Circle c, Vector2 p)
        {
            return new Circle(c.center - p, c.radius);
        }
    }

    /// <summary>
    /// Oriented Bounding Box
    /// </summary>
    struct OBB
    {
        private float rotation;
        private Vector2 center, size;

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public Vector2 Center
        {
            get { return center; }
            set { center = value; }
        }
        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }
        public float Width
        {
            get { return size.X; }
            set { size.X = value; }
        }
        public float Height
        {
            get { return size.Y; }
            set { size.Y = value; }
        }
        /// <summary>
        /// Returns a vector that goes from the center to the mid-point of the right side
        /// </summary>
        public Vector2 RightVector
        {
            get
            {
                return new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * (size.X / 2f);
            }
        }
        /// <summary>
        /// Returns a vector that goes from the center to the mid-point of the left side
        /// </summary>
        public Vector2 LeftVector
        {
            get
            {
                return new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * -(size.X / 2f);
            }
        }
        /// <summary>
        /// Returns a vector that goes from the center to the mid-point of the bottom side
        /// </summary>
        public Vector2 BottomVector
        {
            get
            {
                return new Vector2((float)Math.Cos(rotation + MathHelper.PiOver2), (float)Math.Sin(rotation + MathHelper.PiOver2)) * (size.Y / 2f);
            }
        }
        /// <summary>
        /// Returns a vector that goes from the center to the mid-point of the top side
        /// </summary>
        public Vector2 TopVector
        {
            get
            {
                return new Vector2((float)Math.Cos(rotation + MathHelper.PiOver2), (float)Math.Sin(rotation + MathHelper.PiOver2)) * -(size.Y / 2f);
            }
        }
        public Vector2 TopLeft
        {
            get { return LeftVector + TopVector + center; }
        }
        public Vector2 TopRight
        {
            get { return RightVector + TopVector + center; }
        }
        public Vector2 BottomLeft
        {
            get { return LeftVector + BottomVector + center; }
        }
        public Vector2 BottomRight
        {
            get { return RightVector + BottomVector + center; }
        }

        public OBB(float centerX, float centerY, float width, float height, float rotation)
        {
            this.center = new Vector2(centerX, centerY);
            this.size = new Vector2(width, height);
            this.rotation = rotation;
        }
        public OBB(Vector2 center, float width, float height, float rotation)
        {
            this.center = center;
            this.size = new Vector2(width, height);
            this.rotation = rotation;
        }
        public OBB(Vector2 topLeft, Vector2 bottomRight, float rotation)
        {
            this.center = (topLeft + bottomRight) / 2f;
            this.rotation = rotation;
            Vector2 p1, p2;
            p1 = new Vector2((float)Math.Cos(-rotation), (float)Math.Sin(-rotation)) * (topLeft.X - center.X) +
                new Vector2((float)Math.Cos(-rotation + MathHelper.PiOver2), (float)Math.Sin(-rotation + MathHelper.PiOver2)) * (topLeft.Y - center.Y);
            p2 = new Vector2((float)Math.Cos(-rotation), (float)Math.Sin(-rotation)) * (bottomRight.X - center.X) +
                new Vector2((float)Math.Cos(-rotation + MathHelper.PiOver2), (float)Math.Sin(-rotation + MathHelper.PiOver2)) * (bottomRight.Y - center.Y);
            this.size = new Vector2(p2.X - p1.X, p2.Y - p1.Y);
        }

        public static OBB operator +(OBB rec, Vector2 p)
        {
            return new OBB(rec.Center + p, rec.Size.X, rec.Size.Y, rec.rotation);
        }
        public static OBB operator -(OBB rec, Vector2 p)
        {
            return new OBB(rec.Center - p, rec.Size.X, rec.Size.Y, rec.rotation);
        }
    }

    struct fRectangle
    {
        /// P = position
        /// S = size
        /// 
        /// S-------->
        /// | P------*
        /// | |      |
        /// | |      |
        /// V *------*
        private Vector2 position, size;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public float X
        {
            get { return position.X; }
            set { position.X = value; }
        }
        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }
        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }
        public float Width
        {
            get { return size.X; }
            set { size.X = value; }
        }
        public float Height
        {
            get { return size.Y; }
            set { size.Y = value; }
        }
        public Vector2 Center
        {
            get { return position + size / 2f; }
            set { position = value - size / 2f; }
        }
        public float Left
        {
            get { return position.X; }
            set { position.X = value; }
        }
        public float Right
        {
            get { return position.X + size.X; }
            set { position.X = value - size.X; }
        }
        public float Top
        {
            get { return position.Y; }
            set { position.Y = value; }
        }
        public float Bottom
        {
            get { return position.Y + size.Y; }
            set { position.Y = value - size.Y; }
        }

        public fRectangle(float x, float y, float width, float height)
        {
            this.position = new Vector2(x, y);
            this.size = new Vector2(width, height);
        }
        public fRectangle(Vector2 p1, Vector2 p2)
        {
            this.position = p1;
            this.size = p2 - p1;
        }
        public fRectangle(Vector2 center, float width, float height)
        {
            this.size = new Vector2(width, height);
            this.position = center - size / 2f;
        }

        /// <summary>
        /// Translates the rectangle according to offset
        /// </summary>
        public void Offset(Vector2 offset)
        {
            position += offset;
        }
        public void Offset(float xOffset, float yOffset)
        {
            position += new Vector2(xOffset, xOffset);
        }

        public bool Contains(Vector2 point)
        {
            return (point.X >= Left && point.X <= Right && point.Y >= Top && point.Y <= Bottom);
        }
        public bool Contains(Point point)
        {
            return (point.X >= Left && point.X <= Right && point.Y >= Top && point.Y <= Bottom);
        }
        public static bool Contains(fRectangle rec, Vector2 point)
        {
            return (point.X >= rec.Left && point.X <= rec.Right && point.Y >= rec.Top && point.Y <= rec.Bottom);
        }
        public static bool Contains(fRectangle rec, Point point)
        {
            return (point.X >= rec.Left && point.X <= rec.Right && point.Y >= rec.Top && point.Y <= rec.Bottom);
        }

        /// <summary>
        /// Returns a bool indicating whether the two rectangles overlap (inclusive)
        /// </summary>
        public bool Intersects(fRectangle rec)
        {
            return (Math.Abs(Center.X - rec.Center.X) * 2 <= (Width + rec.Width)) &&
                (Math.Abs(Center.Y - rec.Center.Y) * 2 <= (Height + rec.Height));
        }
        public static bool Intersects(fRectangle recA, fRectangle recB)
        {
            return (Math.Abs(recA.Center.X - recB.Center.X) * 2 <= (recA.Width + recB.Width)) &&
                (Math.Abs(recA.Center.Y - recA.Center.Y) * 2 <= (recA.Height + recB.Height));
        }
        /// <summary>
        /// Same as Intersects between fRectangles but with XNA Rectangles
        /// </summary>
        public bool Intersects(Rectangle rec)
        {
            return (Math.Abs(Center.X - rec.Center.X) <= (Width + rec.Width)) &&
                (Math.Abs(Center.Y - rec.Center.Y) <= (Height + rec.Height));
        }
        public static bool Intersects(fRectangle recA, Rectangle recB)
        {
            return (Math.Abs(recA.Center.X - recB.Center.X) <= (recA.Width + recB.Width)) &&
                (Math.Abs(recA.Center.Y - recA.Center.Y) <= (recA.Height + recB.Height));
        }
        public static bool Intersects(Rectangle recA, fRectangle recB)
        {
            return (Math.Abs(recA.Center.X - recB.Center.X) <= (recA.Width + recB.Width)) &&
                (Math.Abs(recA.Center.Y - recA.Center.Y) <= (recA.Height + recB.Height));
        }
        /// <summary>
        /// Returns a bool indicating whether the circle and rectangle intersect (inclusive)
        /// </summary>
        public bool Intersects(Circle c)
        {
            ///Closest point to circle
            Vector2 recVec = new Vector2(MathHelper.Clamp(c.center.X, Left, Right), MathHelper.Clamp(c.center.Y, Top, Bottom));

            ///Distance between circle and rectangle
            float Distance = Vector2.Distance(recVec, c.center);

            ///If they are intersecting
            return (Distance <= c.radius);
        }

        public static fRectangle operator +(fRectangle rec, Vector2 p)
        {
            return new fRectangle(rec.Position + p, rec.Size.X, rec.Size.Y);
        }
        public static fRectangle operator -(fRectangle rec, Vector2 p)
        {
            return new fRectangle(rec.Position - p, rec.Size.X, rec.Size.Y);
        }
    }
}
