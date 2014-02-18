#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace PrincessMonoSmasher
{
    public enum Clients
    {
        Menu,
        Game,
        Intro
    }

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Clients current;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Gl.Initialize(this, spriteBatch, Content, GraphicsDevice);
            //TODO: add background loading while intro client is still in progress
            GameClient.LoadContent();
            MenuClient.LoadContent();
            IntroClient.LoadContent();
            
            
        }

        public void GotoClient(Clients client, string roomName = "defualt")
        {
            if (current != client)
            {
                current = client;
                if (current == Clients.Game)
                    GameClient.Initialize(roomName);
                else if (current == Clients.Intro)
                    IntroClient.Initialize();
                else if (current == Clients.Menu)
                    MenuClient.Initialize();
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Gl.Update(gameTime);

            if (current == Clients.Game)
                GameClient.Update();
            else if (current == Clients.Intro)
                IntroClient.Update();
            else if (current == Clients.Menu)
                MenuClient.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            if (current == Clients.Game)
                GameClient.Draw();
            else if (current == Clients.Intro)
                IntroClient.Draw();
            else if (current == Clients.Menu)
                MenuClient.Draw();

            //spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
