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
	public class ClickEventArgs : EventArgs
	{
		public Ray pickingRay { get; set; }
	}
	
	public class Startup : Game
	{
		public Matrix View { get; protected set; }
		public Matrix Projection { get; protected set; }
		public event EventHandler MouseClick = delegate {};

		MouseState lastMouseState;
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		// First User Story is:
			// Player One selects an attacker
		 		// Place all the models
				// Make the PlayerOne ones flash

		public Startup()
		{
			graphics = new GraphicsDeviceManager (this);

			graphics.IsFullScreen = false;

			Content.RootDirectory = "Content";

			View = Matrix.CreateLookAt(new Vector3(0, -2, 10), new Vector3(0, 0, 0), Vector3.UnitY);
			Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);

			Components.Add(new Floor(this));
			Components.Add(new Drone(this, GameData.GlobalData.PlayerOneName));
			Components.Add (new Drone(this, GameData.GlobalData.PlayerTwoName));
		}

		protected override void Initialize ()
		{
			this.IsMouseVisible = true;

			base.Initialize ();
		}

		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (graphics.GraphicsDevice);
		}

		protected override void Update (GameTime gameTime)
		{
			// Fire a click event
			var mouseState = Mouse.GetState ();

			if (lastMouseState.LeftButton == ButtonState.Pressed &&
				mouseState.LeftButton == ButtonState.Released) {
				Console.WriteLine ("CLICK");
				int mouseX = mouseState.X;
				int mouseY = mouseState.Y;

				var nearsource = new Vector3 ((float)mouseX, (float)mouseY, 0f);
				var farsource = new Vector3 ((float)mouseX, (float)mouseY, 1f);
				
				Matrix world = Matrix.CreateTranslation (0, 0, 0);
				
				Vector3 nearPoint = graphics.GraphicsDevice.Viewport.Unproject (nearsource, Projection, View, world);
				
				Vector3 farPoint = graphics.GraphicsDevice.Viewport.Unproject (farsource, Projection, View, world);

				// Create a ray from the near clip plane to the far clip plane.
				Vector3 direction = farPoint - nearPoint;
				direction.Normalize ();
				Ray pickRay = new Ray (nearPoint, direction);

				MouseClick(this, new ClickEventArgs {pickingRay = pickRay});
			}

			lastMouseState = mouseState;
			base.Update (gameTime);
		}

		protected override void Draw (GameTime gameTime)
		{
			// Clear the backbuffer
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);

			spriteBatch.Begin ();

			spriteBatch.End ();

			base.Draw (gameTime);
		}
	}
}
