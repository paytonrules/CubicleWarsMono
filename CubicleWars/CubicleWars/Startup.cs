using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

			CubicleWarsGame.Initialize(this);
			Components.Add (new Floor (this));
			Components.Add (new UnitController(this, 
			                                   GameData.GlobalData.PlayerOneName, 
			                                   (UnitData) GameData.DataFor(GameData.GlobalData.PlayerOneName).Drone));
			Components.Add (new UnitController(this, 
			                                   GameData.GlobalData.PlayerOneName, 
			                                   (UnitData) GameData.DataFor(GameData.GlobalData.PlayerOneName).Sales));
			/*Components.Add (new UnitController(this, 
			                                   GameData.GlobalData.PlayerOneName, 
			                                   (UnitData) GameData.DataFor(GameData.GlobalData.PlayerOneName).Hacker));

			Components.Add (new UnitController(this, 
			                                   GameData.GlobalData.PlayerTwoName, 
			                                   (UnitData) GameData.DataFor(GameData.GlobalData.PlayerTwoName).Drone));
			Components.Add (new UnitController(this, 
			                                   GameData.GlobalData.PlayerTwoName, 
			                                   (UnitData) GameData.DataFor(GameData.GlobalData.PlayerTwoName).Sales));
			Components.Add (new UnitController(this, 
			                                   GameData.GlobalData.PlayerTwoName, 
			                                   (UnitData) GameData.DataFor(GameData.GlobalData.PlayerTwoName).Hacker));*/

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
				var mouseX = mouseState.X;
				var mouseY = mouseState.Y;

				var nearsource = new Vector3 (mouseX, mouseY, 0f);
				var farsource = new Vector3 (mouseX, mouseY, 1f);
				
				var world = Matrix.CreateTranslation (0, 0, 0);
				
				var nearPoint = graphics.GraphicsDevice.Viewport.Unproject (nearsource, Projection, View, world);
				
				var farPoint = graphics.GraphicsDevice.Viewport.Unproject (farsource, Projection, View, world);

				// Create a ray from the near clip plane to the far clip plane.
				var direction = farPoint - nearPoint;
				direction.Normalize ();
				var pickRay = new Ray (nearPoint, direction);

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
