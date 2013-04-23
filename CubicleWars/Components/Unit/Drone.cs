using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CubicleWars
{
	public class Drone : DrawableUnitController
	{
		public Drone(Game game, String player) : base(game, player, (UnitData) GameData.DataFor(player).Drone)
		{
		}
	}
}

