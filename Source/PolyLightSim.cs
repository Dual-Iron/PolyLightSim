using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PolyLightSim.Source
{
    public class PolyLightSim : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static readonly Random random = new Random();
        public static SpriteFont Font { get; private set; }
        public static Texture2D Pixel { get; private set; }

        /// <summary>
        /// The player handles all the rays, movement, and controls for the user
        /// </summary>
        public Player player;

        /// <summary>
        /// The level contains all the Shapes which contain Segments which contain Vector2s
        /// </summary>
        public Level level;

        private bool OpenMenu = false;
        private bool ShowShapes = false;

        public PolyLightSim()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 900,
                PreferredBackBufferHeight = 600
            };
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            player = new Player(this);
            level = new Level(player, 4, 7);
            // Change the parameters of Level generation as you please 
            // Recommended poly count is 1-5, points per poly 3-8

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData(new Color[] { Color.White });

            Font = Content.Load<SpriteFont>("Font");
        }

        protected override void Update(GameTime gameTime)
        {
            // These controls are handled in the Game class to access OpenMenu and ShowShapes easily
            if (player.Pressed(Keys.H))
                OpenMenu = !OpenMenu;
            if (player.Pressed(Keys.V))
                ShowShapes = !ShowShapes;

            // Update the player: movement, controls, and rays
            player.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Draw the player & the level
            spriteBatch.Begin();
            player.Draw(spriteBatch);
            if (ShowShapes)
                level.Draw(spriteBatch);
            spriteBatch.End();

            // Draw the menu overtop
            spriteBatch.Begin();
            if (OpenMenu)
            {
                DrawMenu();
                DrawDebug();
            }
            else
            {
                DrawHelp();
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawMenu()
        {
            const int w = 180, h = 110;

            Vector2 drawPos = new Vector2(10, player.ScreenSize.Y - h - 10);
            Vector2 drawEnd = drawPos + new Vector2(w, h);

            spriteBatch.Draw(Pixel, new Rectangle(drawPos.ToPoint(), (drawEnd - drawPos).ToPoint()), null, new Color(68, 60, 80, 220));

            spriteBatch.DrawString(Font,
                "+ to increase radial ray count\n" +
                "- to decrease radial ray count\n" +
                "H to toggle this menu\n" +
                "R to reset player\n" +
                "V to toggle polygon visibility\n" +
                "WASD to move\n" +
                "Enter to reset level", drawPos + new Vector2(4, 4), Color.White);
        }

        private void DrawDebug()
        {
            // Most of this was hardcoded because I'm lazy lol

            const int tOffset = 85;
            
            Vector2 drawPos = player.ScreenSize - new Vector2(10, 10);
            Vector2 textPos = drawPos - Font.MeasureString("Mouse Position: ") - Vector2.UnitX * tOffset;
            Vector2 numberPos = drawPos - Font.MeasureString(player.MousePosition.ToString());

            Vector2 rectStart = player.ScreenSize - new Vector2(200, 50);
            spriteBatch.Draw(Pixel, new Rectangle(rectStart.ToPoint(), (drawPos - rectStart + new Vector2(4, 4)).ToPoint()), null, new Color(68, 60, 80, 220));

            spriteBatch.DrawString(Font, "Mouse Position: ", textPos, Color.White);
            spriteBatch.DrawString(Font, player.MousePosition.ToString(), numberPos, Color.White);

            drawPos = player.ScreenSize - new Vector2(10, 30);
            textPos = drawPos - Font.MeasureString("Player Position: ") - Vector2.UnitX * tOffset;
            numberPos = drawPos - Font.MeasureString(player.position.ToPoint().ToString());

            spriteBatch.DrawString(Font, "Player Position: ", textPos, Color.White);
            spriteBatch.DrawString(Font, player.position.ToPoint().ToString(), numberPos, Color.White);
        }

        private void DrawHelp()
        {
            Vector2 drawPos = player.ScreenSize - new Vector2(10, 10);
            Vector2 textPos = drawPos - Font.MeasureString("Press H for help");

            spriteBatch.DrawString(Font, "Press H for help", textPos, Color.White);
        }
    }
}
