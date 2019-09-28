using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PongProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D pongBal, pongRood, pongBlauw, pongLevens;
        Vector2 pongBalPositie, pongRoodPositie, pongBlauwPositie, pongBalSnelheid, pongBalBeginSnelheid, pongBlauwLevensPositie, pongRoodLevensPositie;
        Rectangle pongBlauwBat, pongRoodBat, pongBalHitbox;
        SpriteFont font;
        int windowHoogte, windowBreedte;
        int pongRoodLevens = 3;
        int pongBlauwLevens = 3;
        int pongBatHoogte = 96;
        int pongBatBreedte = 16;
        int pongBalBreedte = 16;
        int pongBalHoogte = 16;
        int winner = -1;
        int gameState = 0;
        int AI = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            windowBreedte = graphics.PreferredBackBufferWidth = 616;
            windowHoogte = graphics.PreferredBackBufferHeight = 616;
            graphics.ApplyChanges();
            pongBalPositie = new Vector2(300, 300);
            pongBalBeginSnelheid = new Vector2(2, 3);
            pongBalSnelheid = pongBalBeginSnelheid;
            pongRoodPositie = new Vector2(0, 0);
            pongBlauwPositie = new Vector2(600, 0);
            pongBlauwLevensPositie = new Vector2(windowBreedte / 2, windowHoogte - 32);
            pongRoodLevensPositie = new Vector2(windowBreedte / 2, 0);
            KeyboardState currentKeyboardState;

            base.Initialize();
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pongBal = Content.Load<Texture2D>("bal");
            pongRood = Content.Load<Texture2D>("rodeSpeler");
            pongBlauw = Content.Load<Texture2D>("blauweSpeler");
            pongLevens = Content.Load<Texture2D>("levens");
            font = Content.Load<SpriteFont>("Font");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (gameState == 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.G)) AI = 1; //toggle AI
                if (Keyboard.GetState().IsKeyDown(Keys.H)) AI = 0; //toggle AI
                if (Keyboard.GetState().IsKeyDown(Keys.Space)) gameState = 1;
            }
            else if (gameState == 1)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Exit();
                }
                if (AI == 0)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.W) && pongRoodPositie.Y > 0)
                    {
                        pongRoodPositie.Y += -5;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.S) && pongRoodPositie.Y < (windowHoogte - pongBatHoogte))
                    {
                        pongRoodPositie.Y += 5;
                    }
                }
                else if(AI == 1) //als AI op 1 staat beweegt het rode batje automatisch;
                {
                    if(pongBalPositie.Y > pongRoodPositie.Y && pongRoodPositie.Y < (windowHoogte - pongBatHoogte))
                    {
                        pongRoodPositie.Y += 5;
                    }
                    if (pongBalPositie.Y < pongRoodPositie.Y && pongRoodPositie.Y > 0)
                    {
                        pongRoodPositie.Y += -5;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up) && pongBlauwPositie.Y > 0)

                {
                    pongBlauwPositie.Y += -5;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down) && ((pongBlauwPositie.Y + pongBatHoogte) < windowHoogte))
                {
                    pongBlauwPositie.Y += 5;
                }

                if (pongBalPositie.Y < 0 || pongBalPositie.Y > windowHoogte - pongBalHoogte)
                {
                    pongBalSnelheid.Y *= -1;
                }
                if (pongBalPositie.X < -10)
                {
                    pongRoodLevens--;
                    pongBalReset();
                }
                if (pongBalPositie.X > windowBreedte + 10)
                {
                    pongBlauwLevens--;
                    pongBalReset();
                }
                if (pongRoodLevens == 0)
                {
                    winner = 0;
                }
                if (pongBlauwLevens == 0)
                {
                    winner = 1;
                }
                pongBlauwBat = new Rectangle((int)pongBlauwPositie.X, (int)pongBlauwPositie.Y, pongBatBreedte, pongBatHoogte);
                pongRoodBat = new Rectangle((int)pongRoodPositie.X, (int)pongRoodPositie.Y, pongBatBreedte, pongBatHoogte);
                pongBalHitbox = new Rectangle((int)pongBalPositie.X, (int)pongBalPositie.Y, pongBalBreedte, pongBalHoogte);

                if (pongBlauwBat.Intersects(pongBalHitbox) || pongRoodBat.Intersects(pongBalHitbox))
                {
                    double v = pongBalSnelheid.X;
                    pongBalSnelheid.X = (int)(v * -1.5);
                }
                pongBalPositie = pongBalPositie + pongBalSnelheid;
                base.Update(gameTime);
            }
        }

        void pongBalReset()
        {
            pongBalPositie = new Vector2(windowBreedte / 2, windowHoogte / 2);
            pongBalSnelheid = pongBalBeginSnelheid;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (gameState == 0) gameStart();
            else
            {
                if (winner == -1)
                {
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin();
                    spriteBatch.Draw(pongBal, pongBalPositie, Color.White);
                    spriteBatch.Draw(pongRood, pongRoodPositie, Color.White);
                    spriteBatch.Draw(pongBlauw, pongBlauwPositie, Color.White);
                    for (int i = pongRoodLevens; i > 0; i--)
                    {
                        spriteBatch.Draw(pongLevens, pongRoodLevensPositie + new Vector2(i * -32, 0), Color.White);
                    }
                    for (int i = pongBlauwLevens; i > 0; i--)
                    {
                        spriteBatch.Draw(pongLevens, pongBlauwLevensPositie + new Vector2(i * 32, 0), Color.White);
                    }
                    spriteBatch.End();
                }
                else gameEnd();
            }
            // TODO: Add your drawing code here
            //base.Draw(gameTime);
        }
        public void gameEnd()
        {
            if (winner == 1)
            {
                GraphicsDevice.Clear(Color.Red);
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Red wins!", new Vector2(300, 300), Color.White);
                spriteBatch.End();
            }
            else
            {
                GraphicsDevice.Clear(Color.Blue);
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Blue wins!", new Vector2(300, 300), Color.White);
                spriteBatch.End();
            }
        }

        public void gameStart()
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Press space to start the game!", new Vector2(200, 300), Color.Black);
            if(AI == 1)
            {
                spriteBatch.DrawString(font, "AI: Enabled, press H to disable", new Vector2(200, 350), Color.Black);

            }
            else
            {
                spriteBatch.DrawString(font, "AI: Disabled, press G to enable", new Vector2(200, 350), Color.Gray);
            }
            spriteBatch.End();
        }
    }
}

