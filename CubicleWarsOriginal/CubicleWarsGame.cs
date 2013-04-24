using System;
using Microsoft.Xna.Framework;
using CubicleWarsLibrary;

namespace CubicleWars
{
	public class CubicleWarsGame
	{
		public static CubicleWarsStateMachine stateMachine;

		public static void Initialize (Game game)
		{
			stateMachine = new CubicleWarsStateMachine(
				new HumanPlayer(GameData.GlobalData.PlayerOneName),
				new HumanPlayer(GameData.GlobalData.PlayerTwoName));

			game.Components.Add (new CubicleWarsGameView(game, stateMachine));
		}
	}
}

