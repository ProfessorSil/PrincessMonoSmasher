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
    class View
    {
        public Vector2 position, positionGoto; //Middle of the screen
        public float lag; //1 = no lag, 2 = 1/2 distance every step
        public float zoom; //1 = normal, 2 = 2x magnification
        public float rotation; //In radians please
        public float LeftMax = -float.MaxValue, RightMax = float.MaxValue, TopMax = -float.MaxValue, BottomMax = float.MaxValue;

        public Vector2 Center
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                positionGoto = value;
            }
        }
        public Vector2 ScreenSize
        {
            get
            {
                return new Vector2(Gl.graphics.Viewport.Width, Gl.graphics.Viewport.Height);
            }
        }
        public Vector2 ViewSize
        {
            get
            {
                return ScreenSize / zoom;
            }
        }
        /// <summary>
        /// Only applies if there is no rotation
        /// If rotating use ViewOBB
        /// </summary>
        public fRectangle ViewPort
        {
            get
            {
                return new fRectangle(position.X - ViewSize.X / 2f, position.Y - ViewSize.Y / 2f, ViewSize.X, ViewSize.Y);
            }
            set
            {
                //Fit the inputed rectangle inside the screen
                //First find how zoomed in we should be
                float pX = ViewSize.X / value.Width;
                float pY = ViewSize.Y / value.Height;
                if (pX <= pY)
                    zoom = pX;
                else
                    zoom = pY;
                position = value.Center;
                positionGoto = position;
                rotation = 0;
            }
        }
        public OBB ViewOBB
        {
            get
            {
                return new OBB(position, ViewSize.X, ViewSize.Y, rotation);
            }
            set
            {
                //Fit the inputed OBB inside the screen
                //First find how zoomed in we should be
                float pX = ViewSize.X / value.Width;
                float pY = ViewSize.Y / value.Height;
                if (pX <= pY)
                    zoom = pX;
                else
                    zoom = pY;
                position = value.Center;
                positionGoto = position;
                rotation = value.Rotation;
            }
        }
        public Vector2 TopLeft
        {
            get
            {
                if (rotation == 0)
                    return position - ViewSize / 2f;
                else
                    return ViewOBB.TopLeft;
            }
            set
            {
                if (rotation == 0)
                    position = value + ViewSize / 2f;
                else
                    throw new NotImplementedException("Setting 'TopLeft' while the view has a rotation value other than 0.");
            }
        }
        public Vector2 TopRight
        {
            get
            {
                if (rotation == 0)
                    return position - new Vector2(-ViewSize.X / 2f, ViewSize.Y / 2f);
                else
                    return ViewOBB.TopRight;
            }
            set
            {
                if (rotation == 0)
                    position = value + new Vector2(-ViewSize.X / 2f, ViewSize.Y / 2f);
                else
                    throw new NotImplementedException("Setting 'TopRight' while the view has a rotation value other than 0.");
            }
        }
        public Vector2 BottomLeft
        {
            get
            {
                if (rotation == 0)
                    return position - new Vector2(ViewSize.X / 2f, -ViewSize.Y / 2f);
                else
                    return ViewOBB.BottomLeft;
            }
            set
            {
                if (rotation == 0)
                    position = value + new Vector2(ViewSize.X / 2f, -ViewSize.Y / 2f);
                else
                    throw new NotImplementedException("Setting 'BottomLeft' while the view has a rotation value other than 0.");
            }
        }
        public Vector2 BottomRight
        {
            get
            {
                if (rotation == 0)
                    return position + ViewSize / 2f;
                else
                    return ViewOBB.BottomRight;
            }
            set
            {
                if (rotation == 0)
                    position = value - ViewSize / 2f;
                else
                    throw new NotImplementedException("Setting 'BottomRight' while the view has a rotation value other than 0.");
            }
        }
        public float Width
        {
            get
            {
                return ViewSize.X;
            }
        }
        public float Height
        {
            get
            {
                return ViewSize.Y;
            }
        }
        public Vector2 UpVector
        {
            get
            {
                return new Vector2(-(float)Math.Cos(rotation + MathHelper.PiOver2), -(float)Math.Sin(rotation + MathHelper.PiOver2));
            }
        }
        public Vector2 DownVector
        {
            get
            {
                return new Vector2((float)Math.Cos(rotation + MathHelper.PiOver2), (float)Math.Sin(rotation + MathHelper.PiOver2));
            }
        }
        public Vector2 RightVector
        {
            get
            {
                return new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            }
        }
        public Vector2 LeftVector
        {
            get
            {
                return new Vector2(-(float)Math.Cos(rotation), -(float)Math.Sin(rotation));
            }
        }
        public Vector2 MousePos
        {
            get
            {
                return ConvertToWorld(Gl.MousePos);
            }
        }
        public Matrix Transform
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) * Matrix.CreateRotationZ(-rotation) * Matrix.CreateScale(zoom) * Matrix.CreateTranslation(ScreenSize.X / 2f, ScreenSize.Y / 2f, 0f);
            }
        }

        public View(Vector2 position, float zoom = 1f, float rotation = 0f, float lag = 1f)
        {
            this.position = position;
            this.positionGoto = position;
            this.zoom = zoom;
            this.rotation = rotation;
            this.lag = lag;
        }

        public void Update()
        {
            position += (positionGoto - position) / lag;

            CheckBoundries();
        }

        public void CheckBoundries()
        {
            if (TopMax != -float.MaxValue)
            {
                float y;
                if (rotation == 0f)
                {
                    y = position.Y - ViewSize.Y / 2f;
                }
                else
                {
                    y = Math.Min(TopLeft.Y, Math.Min(TopRight.Y, Math.Min(BottomLeft.Y, BottomRight.Y)));
                }
                if (y < TopMax)
                {
                    position.Y += TopMax - y;
                    positionGoto.Y = position.Y;
                }
            }
            if (BottomMax != float.MaxValue)
            {
                float y;
                if (rotation == 0f)
                {
                    y = position.Y + ViewSize.Y / 2f;
                }
                else
                {
                    y = Math.Max(TopLeft.Y, Math.Max(TopRight.Y, Math.Max(BottomLeft.Y, BottomRight.Y)));
                }
                if (y > BottomMax)
                {
                    position.Y -= y - BottomMax;
                    positionGoto.Y = position.Y;
                }
            }
            if (LeftMax != -float.MaxValue)
            {
                float x;
                if (rotation == 0f)
                {
                    x = position.X - ViewSize.X / 2f;
                }
                else
                {
                    x = Math.Min(TopLeft.X, Math.Min(TopRight.X, Math.Min(BottomLeft.X, BottomRight.X)));
                }
                if (x < LeftMax)
                {
                    position.X += LeftMax - x;
                    positionGoto.X = position.X;
                }
            }
            if (RightMax != float.MaxValue)
            {
                float x;
                if (rotation == 0f)
                {
                    x = position.X + ViewSize.X / 2f;
                }
                else
                {
                    x = Math.Max(TopLeft.X, Math.Max(TopRight.X, Math.Max(BottomLeft.X, BottomRight.X)));
                }
                if (x > RightMax)
                {
                    position.X -= x - RightMax;
                    positionGoto.X = position.X;
                }
            }
        }

        public Vector2 ConvertToScreen(Vector2 pos)
        {
            Vector2 newPos = pos - position;
            Vector2 dX = new Vector2((float)Math.Cos(-rotation), (float)Math.Sin(-rotation));
            Vector2 dY = new Vector2((float)Math.Cos(-rotation + MathHelper.PiOver2), (float)Math.Sin(-rotation + MathHelper.PiOver2));
            newPos = dX * newPos.X + dY * newPos.Y;
            newPos *= zoom;
            newPos += ScreenSize / 2f;
            return newPos;
        }
        public Vector2 ConvertToWorld(Vector2 pos)
        {
            float dist = Vector2.Distance(ScreenSize / 2f, pos) / zoom;
            double dir = Math.Atan2(pos.Y - ScreenSize.Y / 2f, pos.X - ScreenSize.X / 2f);
            return (new Vector2((float)Math.Cos(dir + rotation) * dist, (float)Math.Sin(dir + rotation) * dist) + position);
        }

        /// <summary>
        /// Helper function. Calls Gl.sB.Begin() for you with Matrix Transform
        /// </summary>
        public void BeginDraw(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState)
        {
            Gl.sB.Begin(sortMode, blendState, samplerState, DepthStencilState.Default, RasterizerState.CullNone, null, Transform);
        }
    }
}
