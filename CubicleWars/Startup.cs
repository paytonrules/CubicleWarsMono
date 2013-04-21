#region File Description
//-----------------------------------------------------------------------------
// CubicleWarsGame.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

#endregion

namespace CubicleWars
{
	/// <summary>
	/// Default Project Template
	/// </summary>
	public class Startup : Game
	{

	#region Fields
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Texture2D logoTexture;
		Model stapler;
		bool clickedStapler = false;

		// Defaults
		private Matrix world = Matrix.Identity;
		private Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.UnitY);
		private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);

		// Ground Quad
		Vector3 floorLocation = new Vector3(0, 0, -4);
		VertexPositionNormalTexture[] floorVertices;
		Texture2D colorTexture;
		short[] indexes;
		BasicEffect basicEffect;

		// Mouse State
		MouseState lastMouseState;
	#endregion

	#region Initialization

		public Startup()
		{
			graphics = new GraphicsDeviceManager (this);
			
			Content.RootDirectory = "Content";

			graphics.IsFullScreen = false;
		}

		/// <summary>
		/// Overridden from the base Game.Initialize. Once the GraphicsDevice is setup,
		/// we'll use the viewport to initialize some values.
		/// </summary>
		protected override void Initialize ()
		{
			this.IsMouseVisible = true;

			base.Initialize ();
		}

		/// <summary>
		/// Load your graphics content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be use to draw textures.
			spriteBatch = new SpriteBatch (graphics.GraphicsDevice);
			
			// TODO: use this.Content to load your game content here eg.
			logoTexture = Content.Load<Texture2D> ("logo");

			// Floor
			const int SIZE = 9;
			var normal = new Vector3(0, 0, -1);
			floorVertices = new VertexPositionNormalTexture[4];
			floorVertices[0].Position = new Vector3(-1, 1, 0);
			floorVertices[0].TextureCoordinate = new Vector2(0, 0);
			floorVertices[0].Normal = normal;
			floorVertices[1].Position = new Vector3(1, 1, 0);
			floorVertices[1].TextureCoordinate = new Vector2(SIZE, 0);
			floorVertices[1].Normal = normal;
			floorVertices[2].Position = new Vector3(-1, -1, 0);
			floorVertices[2].TextureCoordinate = new Vector2(0, SIZE);
			floorVertices[2].Normal = normal;
			floorVertices[3].Position = new Vector3(1, -1, 0);
			floorVertices[3].TextureCoordinate = new Vector2(SIZE, SIZE);
			floorVertices[3].Normal = normal;

			indexes = new short[6];
			indexes[0] = 0;
			indexes[1] = 1;
			indexes[2] = 2;
			indexes[3] = 2;
			indexes[4] = 1;
			indexes[5] = 3;

			colorTexture = Content.Load<Texture2D>("seamlesscarpet");

			basicEffect = new BasicEffect( graphics.GraphicsDevice );
			basicEffect.EnableDefaultLighting();
			
			basicEffect.World = Matrix.CreateScale(20) * Matrix.CreateTranslation(floorLocation);
			basicEffect.View = view;
			basicEffect.Projection = projection;
			basicEffect.TextureEnabled = true;
			basicEffect.Texture = colorTexture;

			// Rest of objects

			// Game

			// Extract "MVC" objects
			stapler = Content.Load<Model>("stapler");
		}

	#endregion

	#region Update and Draw

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// Clickable
			var mouseState = Mouse.GetState ();

			if (lastMouseState != null && 
				lastMouseState.LeftButton == ButtonState.Pressed &&
				mouseState.LeftButton == ButtonState.Released) {
				int mouseX = mouseState.X;
				int mouseY = mouseState.Y;

				var nearsource = new Vector3 ((float)mouseX, (float)mouseY, 0f);
				var farsource = new Vector3 ((float)mouseX, (float)mouseY, 1f);
				
				Matrix world = Matrix.CreateTranslation (0, 0, 0);
				
				Vector3 nearPoint = graphics.GraphicsDevice.Viewport.Unproject (nearsource, projection, view, world);
				
				Vector3 farPoint = graphics.GraphicsDevice.Viewport.Unproject (farsource, projection, view, world);

				// Create a ray from the near clip plane to the far clip plane.
				Vector3 direction = farPoint - nearPoint;
				direction.Normalize ();
				Ray pickRay = new Ray (nearPoint, direction);

				var sphere = stapler.Meshes [0].BoundingSphere;
				sphere.Center = floorLocation + new Vector3 (3, 2, 0);

				Nullable<float> result = pickRay.Intersects (sphere);

				if (result.HasValue && result.Value < float.MaxValue) {
					clickedStapler = true;
				}
			}
			
			// TODO: Add your update logic here			
			base.Update (gameTime);
		}
		
		
		/// <summary>
		/// This is called when the game should draw itself. 
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{

			// Clear the backbuffer
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);

			spriteBatch.Begin ();

			// draw the logo
			spriteBatch.Draw (logoTexture, new Vector2 (130, 200), Color.White);

			spriteBatch.End ();

			GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
			GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;

			foreach (var pass in basicEffect.CurrentTechnique.Passes) {
				pass.Apply ();

				GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture> (
					PrimitiveType.TriangleList,
					floorVertices,
					0,
					4,
					indexes,
					0,
					2);
			}

			var staplerWorld = Matrix.CreateScale(0.5f) * 
							   Matrix.CreateRotationX(MathHelper.ToRadians(270)) * 
							   Matrix.CreateTranslation(floorLocation + new Vector3(3, 2, 0));
			DrawModel(stapler, staplerWorld, view, projection);

			base.Draw (gameTime);
		}

		private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
		{
			foreach (ModelMesh mesh in model.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.LightingEnabled = true;
					effect.AmbientLightColor = new Vector3(1, 0, 0);
					effect.World = world;
					effect.View = view;
					effect.Projection = projection;
				}

				mesh.Draw();
			}
		}

	#endregion
	}
}
