using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CubicleWarsLibrary;

namespace CubicleWars
{
	public class UnitController : DrawableGameComponent, UnityObject
	{
		protected String player;
		protected Model model;
		protected UnitData initialData;
		protected Unit unit;

		public int InitialHealth {
			get { 
				return initialData.Health;
			}
		}

		public string Name {
			get {
				return initialData.Name;
			}
		}
		
		public UnitController (Game game, String player, UnitData initialData) : base(game)
		{
			var warGame = game as Startup;
			this.player = player;
			this.initialData = initialData;

			unit = new StandardUnit(GameData.Resolver, this);

			warGame.MouseClick += (s, args) => CheckMouseClick(s, args as ClickEventArgs);
		}
		
		protected override void LoadContent ()
		{
			model = Game.Content.Load<Model> (initialData.Model);
			Game.Components.Add (new UnitView(Game, player, model, initialData, unit));
			
			CubicleWarsGame.stateMachine.AddUnitToPlayer(player, unit);
		}
		
		protected void CheckMouseClick(object sender, ClickEventArgs args)
		{
			var sphere = model.Meshes[0].BoundingSphere;
			sphere.Center = GameData.GlobalData.Ground + initialData.Location;
			
			Nullable<float> result = args.pickingRay.Intersects (sphere);
			
			if (result.HasValue && result.Value < float.MaxValue) {
				OnMouseClick ();
			}
		}
		
		protected virtual void OnMouseClick ()
		{
			CubicleWarsGame.stateMachine.Select(unit);
		}
	}
}

