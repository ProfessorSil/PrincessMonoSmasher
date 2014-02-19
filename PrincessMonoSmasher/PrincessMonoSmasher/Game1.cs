#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Threading;
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
        public bool isLoading = true;
        Thread loadThread;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 400;
            graphics.PreferredBackBufferHeight = 400;
            graphics.ApplyChanges();
            this.IsMouseVisible = true;
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
            IntroClient.LoadContent();
            GotoClient(Clients.Intro);

            loadThread = new Thread(new ThreadStart(ThreadedLoadContent));
            isLoading = true;
            loadThread.Start();
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

        private void ThreadedLoadContent()
        {
            //isLoading = true;

            GameClient.LoadContent();
            MenuClient.LoadContent();

            //isLoading = false;
        }

        protected override void UnloadContent()
        {
            if (loadThread.ThreadState == ThreadState.Running)
            {
                loadThread.Abort();
            }
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

            if (loadThread.ThreadState == ThreadState.Stopped)
            {
                loadThread.Abort();
                isLoading = false;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

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
