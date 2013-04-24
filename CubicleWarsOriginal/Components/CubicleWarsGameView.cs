using System;
using Microsoft.Xna.Framework;
using CubicleWarsLibrary;

namespace CubicleWars
{
	public class CubicleWarsGameView : DrawableGameComponent
	{
		String winningPlayer;

		public CubicleWarsGameView (Game game, StateMachine machine) : base(game)
		{
			machine.GameOver += (player) => winningPlayer = player;
		}

		public override void Draw (GameTime gameTime)
		{
			base.Draw (gameTime);
		}
	}
}

