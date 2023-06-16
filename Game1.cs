using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Starship
{
    public enum GameState
    {
        FirstScreen,
        Home,
        HowToPlay,
        Playing,
        Scores,
        GameOver
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        private Texture2D backgroundTexture;
        private Texture2D flyingSaucer;
        private Texture2D tir;
        private Texture2D vaisseau;
        private Vector2 vaisseauPosition;
        private float saucerSpeed = 5f;

        private List<Texture2D> tirTextures;
        private List<Vector2> tirPositions;
        private float tirSpeed = 8f;
        private bool isFiring = false;

        private Random random;
        private List<Texture2D> flyingTextures;
        private List<Vector2> flyingPositions;
        private List<bool> flyingActiveStates;
        private float flyingSpeed = 3f;
        private float flyingInterval = 2f; // en secondes
        private float flyingTimer = 0f;
        private int score = 0;
        private SpriteFont scoreFont;

        private bool previousEnterState = false;
        private bool previousBackState = false;
        private bool previousExitStateReleased = true;
        private bool leftButtonReleased = true;

        private GameState gameState = GameState.FirstScreen;
        private string playerName = string.Empty;
        Texture2D buttonTexture;
        

        //create 3 rectangles for buttons
        Rectangle button1;
        Rectangle button2;
        Rectangle button3;
        Rectangle button4;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            random = new Random();
            tirTextures = new List<Texture2D>();
            tirPositions = new List<Vector2>();
            flyingTextures = new List<Texture2D>();
            flyingPositions = new List<Vector2>();
            flyingActiveStates = new List<bool>();
            button1 = new Rectangle((GraphicsDevice.Viewport.Width / 2) - 100, (GraphicsDevice.Viewport.Height / 2) - 100, 200, 50);
            button2 = new Rectangle((GraphicsDevice.Viewport.Width / 2) - 100, (GraphicsDevice.Viewport.Height / 2), 200, 50);
            button3 = new Rectangle((GraphicsDevice.Viewport.Width / 2) - 100, (GraphicsDevice.Viewport.Height / 2) + 100, 200, 50);
            button4 = new Rectangle((GraphicsDevice.Viewport.Width / 2) - 100, (GraphicsDevice.Viewport.Height / 2) + 200, 200, 50);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundTexture = Content.Load<Texture2D>("ciel-nocturne");
            vaisseau = Content.Load<Texture2D>("vaisseau-ballon-petit");
            tir = Content.Load<Texture2D>("tir");
            flyingSaucer = Content.Load<Texture2D>("flyingSaucer-petit");
            scoreFont = Content.Load<SpriteFont>("galleryFont");
            Color buttonColor = Color.Black; // Replace with your desired button color

            // Create a 1x1 pixel texture with the button color
            buttonTexture = new Texture2D(GraphicsDevice, 1, 1);
            buttonTexture.SetData(new[] { buttonColor });


        }

        protected override void Update(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.Home:
                    UpdateHome(gameTime);
                    break;
                case GameState.Playing:
                    UpdateGame(gameTime);
                    break;
                case GameState.FirstScreen:
                    UpdateFirstScreen(gameTime);
                    break;
                case GameState.HowToPlay:
                    UpdateHowToPlay(gameTime);
                    break;
                case GameState.GameOver:
                    UpdateGameOver(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        


      
        



        private void UpdateHome(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || (Keyboard.GetState().IsKeyDown(Keys.Escape)))
            {
                gameState = GameState.FirstScreen;
                previousExitStateReleased = false;
            }
            KeyboardState keyboardState = Keyboard.GetState();
            bool currentEnterState = keyboardState.IsKeyDown(Keys.Enter);
            bool currentBackState = keyboardState.IsKeyDown(Keys.Back);

            if (currentEnterState && !previousEnterState && !string.IsNullOrWhiteSpace(playerName))
            {
                gameState = GameState.Playing;
                InitializeGame();
            }
            else if (currentBackState && !previousBackState && playerName.Length > 0)
            {
                playerName = playerName.Remove(playerName.Length - 1);
            }
            else
            {
                Keys[] pressedKeys = keyboardState.GetPressedKeys();

                foreach (Keys key in pressedKeys)
                {
                    if (key >= Keys.A && key <= Keys.Z)
                    {
                        playerName += key.ToString();
                    }
                }
            }

            previousEnterState = currentEnterState;
            previousBackState = currentBackState;
        }

        private void UpdateGameOver(GameTime gameTime)
        {

            
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && previousExitStateReleased == true)
            {
                gameState = GameState.FirstScreen;
                previousExitStateReleased = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Escape))
                previousExitStateReleased = true;


            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Released)
                leftButtonReleased = true;
            if (mouseState.LeftButton == ButtonState.Pressed && leftButtonReleased)
            {
                Rectangle mouseRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1);

                if (mouseRectangle.Intersects(button1) )
                {
                    gameState = GameState.Playing;
                    leftButtonReleased = false;
                }
                else if (mouseRectangle.Intersects(button2) )
                {
                    gameState = GameState.FirstScreen;
                    leftButtonReleased = false;

                }
                else if (mouseRectangle.Intersects(button3) )
                {
                    //gameState = GameState.Scores;
                    leftButtonReleased = false;

                }
            }

        }

        private void UpdateHowToPlay(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||  (Keyboard.GetState().IsKeyDown(Keys.Escape)))
            {
                gameState = GameState.FirstScreen;
                previousExitStateReleased = false;
            }
                
        }

        private void UpdateFirstScreen(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyUp(Keys.Escape))
                previousExitStateReleased = true;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) && previousExitStateReleased)
                Exit();

            //add the logic for clicking on buttons in the menu
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Released)
                leftButtonReleased = true;
            if (mouseState.LeftButton == ButtonState.Pressed && leftButtonReleased)
            {
                Rectangle mouseRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1);

                if (mouseRectangle.Intersects(button1))
                {
                    gameState = GameState.Home;
                }
                else if (mouseRectangle.Intersects(button2))
                {
                    gameState = GameState.HowToPlay;
                }
                else if (mouseRectangle.Intersects(button3))
                {
                    //gameState = GameState.Options;
                }
                else if (mouseRectangle.Intersects(button4))
                {
                    Exit();
                }
            }
        }


        private void UpdateGame(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyUp(Keys.Escape))
                previousExitStateReleased = true;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) && previousExitStateReleased)
            {
                gameState = GameState.GameOver;
                previousExitStateReleased = false;
            }
                

            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                // Vérifier si le vaisseau dépasse le bord supérieur
                if (vaisseauPosition.Y > 0)
                    vaisseauPosition.Y -= saucerSpeed;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                // Vérifier si le vaisseau dépasse le bord inférieur
                if (vaisseauPosition.Y < GraphicsDevice.Viewport.Height - vaisseau.Height)
                    vaisseauPosition.Y += saucerSpeed;
            }

            bool isSpacePressed = keyboardState.IsKeyDown(Keys.Space);

            if (isSpacePressed && !isFiring)
            {
                Texture2D newTirTexture = tir;
                Vector2 newTirPosition = new Vector2(vaisseauPosition.X + vaisseau.Width, vaisseauPosition.Y + vaisseau.Height / 2 - tir.Height / 2);

                tirTextures.Add(newTirTexture);
                tirPositions.Add(newTirPosition);

                isFiring = true;
            }
            else if (!isSpacePressed)
            {
                isFiring = false;
            }

            for (int i = tirPositions.Count - 1; i >= 0; i--)
            {
                Vector2 currentTirPosition = tirPositions[i];
                currentTirPosition.X += tirSpeed;

                tirPositions[i] = currentTirPosition;

                if (currentTirPosition.X > GraphicsDevice.Viewport.Width)
                {
                    tirPositions.RemoveAt(i);
                    tirTextures.RemoveAt(i);
                }
                else
                {
                    // Vérifier les collisions avec les secoupes volantes
                    Rectangle tirRect = new Rectangle((int)currentTirPosition.X, (int)currentTirPosition.Y, tir.Width, tir.Height);
                    for (int j = flyingPositions.Count - 1; j >= 0; j--)
                    {
                        Rectangle flyingRect = new Rectangle((int)flyingPositions[j].X, (int)flyingPositions[j].Y, flyingSaucer.Width, flyingSaucer.Height);
                        if (flyingPositions[j].X <= 1)
                        {
                            score=0;
                        }
                        if (tirRect.Intersects(flyingRect))
                        {
                            score++;
                            flyingActiveStates[j] = false;
                            tirPositions.RemoveAt(i);
                            tirTextures.RemoveAt(i);
                            break; // Sortir de la boucle, car un tir ne peut toucher qu'un seul vaisseau volant
                        }


                    }
                }
            }

            flyingTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (flyingTimer >= flyingInterval)
            {
                SpawnFlying();
                flyingTimer = 0f;
            }

            for (int i = flyingPositions.Count - 1; i >= 0; i--)
            {
                if (flyingActiveStates[i])
                {
                    Vector2 currentFlyingPosition = flyingPositions[i];
                    currentFlyingPosition.X -= flyingSpeed;

                    flyingPositions[i] = currentFlyingPosition;

                    if (currentFlyingPosition.X + flyingSaucer.Width < 0)
                    {
                        
                        flyingPositions.RemoveAt(i);
                        flyingTextures.RemoveAt(i);
                        flyingActiveStates.RemoveAt(i);
                        gameState = GameState.GameOver;
                        previousExitStateReleased = false;
                    }
                }
                else
                {
                    // Faire tomber le vaisseau volant verticalement
                    Vector2 currentFlyingPosition = flyingPositions[i];
                    currentFlyingPosition.Y += flyingSpeed;

                    flyingPositions[i] = currentFlyingPosition;

                    // Supprimer le vaisseau volant s'il sort de l'écran
                    if (currentFlyingPosition.Y > GraphicsDevice.Viewport.Height)
                    {
                        
                        flyingPositions.RemoveAt(i);
                        flyingTextures.RemoveAt(i);
                        flyingActiveStates.RemoveAt(i);
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            switch (gameState)
            {
                case GameState.Home:
                    DrawHome();
                    break;
                case GameState.Playing:
                    DrawGame();
                    break;
                case GameState.FirstScreen:
                    DrawFirstScreen();
                    break;
                case GameState.HowToPlay:
                    DrawHowToPlay();
                    break;
                case GameState.GameOver:
                    DrawGameOver();
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawHome()
        {
            spriteBatch.DrawString(scoreFont, "Enter your name:", new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(scoreFont, playerName, new Vector2(10, 40), Color.White);
        }

        private void DrawGameOver()
        {
            //Draw game over text in center of the screen
            spriteBatch.DrawString(scoreFont, "Game Over", new Vector2(GraphicsDevice.Viewport.Width / 2 - 200, GraphicsDevice.Viewport.Height / 2 - 200), Color.White);
            //Draw current score
            spriteBatch.DrawString(scoreFont, "Your score is: " + score, new Vector2(GraphicsDevice.Viewport.Width / 2 - 200, GraphicsDevice.Viewport.Height / 2 - 200 + 30), Color.White);
            //draw three buttons as rectangles (play again, main menu, high scores)
            spriteBatch.Draw(buttonTexture,button1, Color.White);
            spriteBatch.Draw(buttonTexture,button2, Color.White);
            spriteBatch.Draw(buttonTexture,button3, Color.White);
            //draw text on top of the buttons
            spriteBatch.DrawString(scoreFont, "Play again", new Vector2(button1.X + 10, button1.Y + 10), Color.White);
            spriteBatch.DrawString(scoreFont, "Main menu", new Vector2(button2.X + 10, button2.Y + 10), Color.White);
            spriteBatch.DrawString(scoreFont, "High scores", new Vector2(button3.X + 10, button3.Y + 10), Color.White);

        }

        private void DrawHowToPlay()
        {
            //draw text in center of the screen
            spriteBatch.DrawString(scoreFont, "How to play", new Vector2(GraphicsDevice.Viewport.Width / 2 - 200, GraphicsDevice.Viewport.Height/2 - 200), Color.White);
            spriteBatch.DrawString(scoreFont, "Use the up and down arrow keys to move the spaceship", new Vector2(GraphicsDevice.Viewport.Width / 2 - 200, GraphicsDevice.Viewport.Height / 2 - 200 + 30), Color.White);
            spriteBatch.DrawString(scoreFont, "Press space to shoot", new Vector2(GraphicsDevice.Viewport.Width / 2 - 200, GraphicsDevice.Viewport.Height / 2 - 200 + 60), Color.White);
            spriteBatch.DrawString(scoreFont, "Press escape to return to the main menu", new Vector2(GraphicsDevice.Viewport.Width / 2 - 200, GraphicsDevice.Viewport.Height / 2 - 200 + 90), Color.White);

        }

        private void DrawFirstScreen()
        {
            
            //draw 4 buttons in the center of the screen
            spriteBatch.Draw(buttonTexture, button1, Color.White);
            spriteBatch.Draw(buttonTexture, button2, Color.White);
            spriteBatch.Draw(buttonTexture, button3, Color.White);
            spriteBatch.Draw(buttonTexture, button4, Color.White);

            //draw text on buttons
            spriteBatch.DrawString(scoreFont, "Play", new Vector2(button1.X + 10, button1.Y + 10), Color.White);
            spriteBatch.DrawString(scoreFont, "How to play", new Vector2(button2.X + 10, button2.Y + 10), Color.White);
            spriteBatch.DrawString(scoreFont, "High scores", new Vector2(button3.X + 10, button3.Y + 10), Color.White);
            spriteBatch.DrawString(scoreFont, "Exit", new Vector2(button4.X + 10, button4.Y + 10), Color.White);
        }


        private void DrawGame()
        {
            Rectangle backgroundRectangle = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            spriteBatch.Draw(backgroundTexture, backgroundRectangle, Color.White);

            spriteBatch.Draw(vaisseau, vaisseauPosition, Color.White);

            for (int i = 0; i < tirPositions.Count; i++)
            {
                spriteBatch.Draw(tirTextures[i], tirPositions[i], Color.White);
            }

            for (int i = 0; i < flyingPositions.Count; i++)
            {
                spriteBatch.Draw(flyingTextures[i], flyingPositions[i], Color.White);
            }

            spriteBatch.DrawString(scoreFont, "Score: " + score, new Vector2(10, 10), Color.White);
        }

        private void InitializeGame()
        {
            // Réinitialiser les paramètres du jeu
            vaisseauPosition = new Vector2(10, GraphicsDevice.Viewport.Height / 2 - vaisseau.Height / 2);
            tirTextures.Clear();
            tirPositions.Clear();
            flyingTextures.Clear();
            flyingPositions.Clear();
            flyingActiveStates.Clear();
            score = 0;

            // Initialiser le timer des vaisseaux volants
            flyingTimer = 0f;
        }

        private void SpawnFlying()
        {
            Texture2D newFlyingTexture = flyingSaucer;
            float newFlyingY = random.Next(0, GraphicsDevice.Viewport.Height - flyingSaucer.Height);
            Vector2 newFlyingPosition = new Vector2(GraphicsDevice.Viewport.Width, newFlyingY);

            flyingTextures.Add(newFlyingTexture);
            flyingPositions.Add(newFlyingPosition);
            flyingActiveStates.Add(true);
        }
    }
}
