using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace TestScene
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Camera
        private CameraFPS camera;

        // Player
        private Player player;
        private float mouseSensitivity;

        // Models
        private Model plane;
        private Matrix planeWorld;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            player = new Player(new Vector3(0f, 2f, 5f));
            camera = new CameraFPS(Vector3.Zero, GraphicsDevice.DisplayMode.AspectRatio);
            mouseSensitivity = 100f;

            planeWorld = Matrix.Identity;

            IsMouseVisible = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            plane = Content.Load<Model>("floor");
        }

        private void HandleInputs(float dt)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Player movement
            Vector3 direction = Vector3.Zero;
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                direction.X -= 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                direction.X += 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                direction.Z -= 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                direction.Z += 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                direction.Y += 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
            {
                direction.Y -= 1f;
            }

            player.SetMoveDirection(direction);

            // Camera rotation
            int width = _graphics.PreferredBackBufferWidth;
            int height = _graphics.PreferredBackBufferHeight;
            MouseState mouseState = Mouse.GetState();
            int dX = mouseState.X - width / 2;
            int dY = mouseState.Y - height / 2;
            Mouse.SetPosition(width / 2, height / 2);
            float sensitivity = mouseSensitivity * dt;

            camera.Rotate(-dX * sensitivity / width, -dY * sensitivity / height);
        }

        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (IsActive) { HandleInputs(dt); }

            // TODO: check for collisions
            //player.CheckCollision(plane);

            // Solve collisions
            player.Update(dt);

            // Sync camera & player
            camera.SetPosition(player.Transform().position);
            player.SetRotation(camera.Transform().rotation);

            base.Update(gameTime);
        }

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Disable backface culling
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            // Matrices
            Matrix view = camera.GetViewMatrix();
            Matrix projection = camera.GetProjectionMatrix();

            // Render
            DrawModel(plane, planeWorld, view, projection);

            base.Draw(gameTime);
        }
    }
}