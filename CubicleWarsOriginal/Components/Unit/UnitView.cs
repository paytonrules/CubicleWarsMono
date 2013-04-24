using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CubicleWarsLibrary;
using CubicleWarsLibrary.Effects;

namespace CubicleWars
{
	public class UnitView : DrawableGameComponent
	{
		protected Model model;
		protected Unit unit;
		protected UnitData initialData;
		protected SineWave wave;
		protected string player;
		protected bool waiting;

		const float AMPLITUDE = 1.0f / 6.0f;
		const float FREQUENCY = 6.0f;
		const float OFFSET = 0.9f;
		Vector3 BASE_EMISSIVE_COLOR = new Vector3(0.3f, 0.3f, 0.3f);

		public UnitView (Game game, String player, Model model, UnitData initialData, Unit unit) : base(game)
		{
			this.model = model;
			this.unit = unit;
			this.initialData = initialData;
			this.player = player;
			wave = new SineWave(AMPLITUDE, FREQUENCY, OFFSET);
			waiting = false;

			unit.Waiting += () => waiting = true;
			unit.DoneWaiting += () => waiting = false;
		}

		public override void Draw(GameTime time)
		{
			var world = Matrix.CreateScale(initialData.Scale) * 
						Matrix.CreateRotationX(MathHelper.ToRadians(initialData.RotationX)) * 
						Matrix.CreateRotationZ(MathHelper.ToRadians(initialData.RotationZ)) *
						Matrix.CreateTranslation(GameData.GlobalData.Ground + initialData.Location);
			
			DrawModel(time, world);
		}
		
		protected void DrawModel(GameTime time,	Matrix world)
		{
			var game = Game as Startup;
			foreach (ModelMesh mesh in model.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.LightingEnabled = true;
					effect.AmbientLightColor = GameData.DataFor(player).TintColor;

					// Maybe Ambient Light doesn't change or changes are too small
					// You're recreating the 'tint' 
					if (waiting) {
						var multiplicationFactor =  wave.at((float) time.TotalGameTime.TotalSeconds);
						//effect.AmbientLightColor = GameData.DataFor(player).TintColor * wave.at(time.TotalGameTime.Ticks);
						effect.EmissiveColor = BASE_EMISSIVE_COLOR * multiplicationFactor;
					} else {
						Console.WriteLine("I AM ALWAYS WAITING");
						effect.EmissiveColor = Vector3.Zero;
					}

					effect.World = world;
					effect.View = game.View;
					effect.Projection = game.Projection;
				}
				
				mesh.Draw();
			}
		}
	}
}

