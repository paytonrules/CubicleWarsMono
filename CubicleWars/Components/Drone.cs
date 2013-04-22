using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CubicleWars
{
	public class DrawableUnit : DrawableGameComponent
	{
		protected String player;
		protected Model unit;
		protected dynamic initialData;

		public DrawableUnit (Game game, String player) : base(game)
		{
			var warGame = game as Startup;
			this.player = player;
			
			warGame.MouseClick += (s, args) => CheckMouseClick(s, args as ClickEventArgs);
		}

		protected void CheckMouseClick(object sender, ClickEventArgs args)
		{
			var sphere = unit.Meshes[0].BoundingSphere;
			sphere.Center = GameData.GlobalData.Ground + initialData.Location;
			
			Nullable<float> result = args.pickingRay.Intersects (sphere);
			
			if (result.HasValue && result.Value < float.MaxValue) {
				OnMouseClick ();
			}
		}

		protected virtual void OnMouseClick ()
		{
		}

		public override void Draw(GameTime time)
		{
			var world = Matrix.CreateScale(initialData.Scale) * 
						Matrix.CreateRotationX(MathHelper.ToRadians(initialData.RotationX)) * 
						Matrix.CreateRotationZ(MathHelper.ToRadians(initialData.RotationZ)) *
						Matrix.CreateTranslation(GameData.GlobalData.Ground + initialData.Location);
			
			DrawModel(unit, world);
		}

		protected void DrawModel(Model model, Matrix world)
		{
			var game = Game as Startup;
			foreach (ModelMesh mesh in model.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					/*					effect.LightingEnabled = true;
					effect.AmbientLightColor = new Vector3(1, 0, 0);*/
					effect.World = world;
					effect.View = game.View;
					effect.Projection = game.Projection;
				}
				
				mesh.Draw();
			}
		}

	}

	public class Drone : DrawableUnit
	{
		public Drone(Game game, String player) : base(game, player)
		{
			initialData = GameData.DataFor(player).Drone;
		}

		protected override void LoadContent ()
		{
			unit = Game.Content.Load<Model> ("stapler");
		}

		protected override void OnMouseClick()
		{
		}

		public override void Update (GameTime gameTime)
		{
			base.Update (gameTime);
		}
	}
}

