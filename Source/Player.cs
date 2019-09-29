using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace PolyLightSim.Source
{
    public class Player
    {
        private readonly Game game;

        public Vector2 position;
        public Vector2 velocity;
        public Vector2 acceleration;
        public int radialRays = 0;

        private const int mouseRays = 150;
        private const float speedMax = 10f;
        private const float accel = 0.9f;

        public Vector2 MousePosition => new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        public Vector2 ScreenSize => new Vector2(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);

        private readonly List<Ray> rays = new List<Ray>();
        private KeyboardState lastKeyboard;

        public Player(Game game)
        {
            this.game = game;
            position = ScreenSize / 2;
        }

        public bool Holding(Keys key) => Keyboard.GetState().IsKeyDown(key);
        public bool Pressed(Keys key) => Keyboard.GetState().IsKeyDown(key) && !lastKeyboard.IsKeyDown(key);

        public void Update()
        {
            UpdateControls();
            UpdateMovement();
            ResetMovement();
            UpdateRays();
        }
        private void UpdateControls()
        {
            if (Pressed(Keys.R))
            {
                position = ScreenSize / 2;
                velocity = acceleration = Vector2.Zero;
            }

            if (Holding(Keys.OemPlus))
            {
                radialRays += 3;
            }
            else if (Holding(Keys.OemMinus))
            {
                radialRays -= 5;
            }
            radialRays = Math.Max(radialRays, 0);

            if (Pressed(Keys.Enter))
            {
                (game as PolyLightSim).level = new Level(this, 4, 7);
            }

            if (Holding(Keys.W))
            {
                acceleration.Y = -accel;
            }
            else if (Holding(Keys.S))
            {
                acceleration.Y = +accel;
            }

            if (Holding(Keys.A))
            {
                acceleration.X = -accel;
            }
            else if (Holding(Keys.D))
            {
                acceleration.X = +accel;
            }

            lastKeyboard = Keyboard.GetState();
        }
        private void UpdateMovement()
        {
            velocity += acceleration;
            velocity = velocity.SafeNormalize() * MathHelper.Min(speedMax, velocity.Length());

            position += velocity;
        }
        private void ResetMovement()
        {
            acceleration = Vector2.Zero;
            velocity *= 0.9f;
        }
        private void UpdateRays()
        {
            rays.Clear();
            for (int i = 0; i < radialRays; i++)
            {
                float rotation = MathHelper.TwoPi * i / radialRays;
                rays.Add(new Ray(Color.Crimson, position - new Vector2(5, 6), rotation.ToVector2(), 100));

                //const float rotV = 0.01f;

                //if (rays.Count < segs.Count * 6)
                //    for (int i1 = 0; i1 < segs.Count * 6 - rays.Count; i1++)
                //    {
                //        rays.Add(new Ray());
                //    }
                //else
                //{
                //    Vector2 dirA = segs[i / 6].A - me.position;
                //    Vector2 dirB = segs[i / 6].B - me.position;
                //    dirA.Normalize(); dirB.Normalize();

                //    float distA = (segs[i / 6].A - me.position).Length();
                //    float distB = (segs[i / 6].B - me.position).Length();

                //    rays[i + 0] = new Ray(me.position, dirA, distA);
                //    rays[i + 1] = new Ray(me.position, dirA.RotatedBy(-rotV), distA + 5);
                //    rays[i + 2] = new Ray(me.position, dirA.RotatedBy(+rotV), distA + 5);

                //    rays[i + 3] = new Ray(me.position, dirB, distB);
                //    rays[i + 4] = new Ray(me.position, dirB.RotatedBy(-rotV), distB + 5);
                //    rays[i + 5] = new Ray(me.position, dirB.RotatedBy(+rotV), distB + 5);
                //}
            }

            for (int i = 0; i < mouseRays; i++)
            {
                const float span = MathHelper.PiOver4 / 2f;

                float mouseRot = span * i / mouseRays;
                mouseRot -= span / 2f;
                mouseRot += (MousePosition - position).ToRotation();
                rays.Add(new Ray(Color.LightYellow, position - new Vector2(5, 6), mouseRot.ToVector2(), 200));
            }
        }

        public void Draw(SpriteBatch sb)
        {
            DrawPlayer(sb, 7);
            DrawMouse(sb, 4);
            DrawRays(sb);
        }

        private void DrawPlayer(SpriteBatch sb, int halfWidth)
        {
            Rectangle destinationRectangle = new Rectangle((int)position.X - halfWidth, (int)position.Y - halfWidth, halfWidth * 2, halfWidth * 2);
            sb.Draw(PolyLightSim.Pixel, destinationRectangle, null, Color.Violet, velocity.ToRotation(), new Vector2(0.5f, 0.5f), 0, 0);
        }

        private void DrawMouse(SpriteBatch sb, int halfWidth)
        {
            Rectangle destinationRectangle = new Rectangle((int)MousePosition.X - halfWidth, (int)MousePosition.Y - halfWidth, halfWidth * 2, halfWidth * 2);
            sb.Draw(PolyLightSim.Pixel, destinationRectangle, null, Color.SkyBlue);
        }

        private void DrawRays(SpriteBatch sb)
        {
            sb.End();
            sb.Begin(blendState: BlendState.Additive);
            foreach (var ray in rays)
            {
                ray.Draw(sb, (game as PolyLightSim).level.GetSegments());
            }
            sb.End();
            sb.Begin();
        }
    }
}
