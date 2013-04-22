using System;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Dynamic;

namespace CubicleWars
{
	public class GameData
	{
		private static Hashtable data = null;
		private static dynamic globalData = null; 

		static public dynamic GlobalData {
			get {
				if (globalData == null) {
					globalData = new ExpandoObject();
					globalData.Ground = new Vector3 (0, 0, -4);
					globalData.PlayerOneName = "Player One";
					globalData.PlayerTwoName = "Player Two";
				}
				return globalData;
			}
		}

		public static dynamic DataFor (string playerName)
		{
			if (data == null) {
				data = new Hashtable();

				dynamic playerOne = new ExpandoObject();
				playerOne.TintColor = new Vector3(0.545f, 0.227f, 0.227f);
				playerOne.Drone = new ExpandoObject();
				playerOne.Drone.Location = new Vector3(3, 2, 0);
				playerOne.Drone.Scale = 0.5f;
				playerOne.Drone.RotationX = 270;
				playerOne.Drone.RotationZ = 35;

				dynamic playerTwo = new ExpandoObject();
				playerTwo.TintColor = new Vector3(0.251f, 0.376f, 0.663f);
				playerTwo.Drone = new ExpandoObject();
				playerTwo.Drone.Location = new Vector3(-3, -2, 0);
				playerTwo.Drone.Scale = 0.5f;
				playerTwo.Drone.RotationX = 270;
				playerTwo.Drone.RotationZ = -140;

				data[GlobalData.PlayerOneName] = playerOne;
				data[GlobalData.PlayerTwoName] = playerTwo;
			}
			return data[playerName];
		}
	}
}

