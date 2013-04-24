using System;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using CubicleWarsLibrary;

namespace CubicleWars
{
	public struct UnitData
	{
		public Vector3 Location;
		public float Scale;
		public float RotationX;
		public float RotationZ;
		public string Name;
		public int Health;
		public string Model;
	}

	public class GameData
	{
		private static Hashtable data = null;
		private static dynamic globalData = null; 
		private static RockPaperScissorsConflictResolver resolver = null;

		public static dynamic GlobalData {
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
				playerOne.Drone = new UnitData();
				playerOne.Drone.Location = new Vector3(3, 2, 0);
				playerOne.Drone.Scale = 0.5f;
				playerOne.Drone.RotationX = 270;
				playerOne.Drone.RotationZ = 35;
				playerOne.Drone.Name = "Drone";
				playerOne.Drone.Health = 10;
				playerOne.Drone.Model = "stapler";

				playerOne.Sales = new UnitData();
				/*playerOne.Sales.Location = new Vector3(3, 2, 0);
				playerOne.Sales.Scale = 0.5f;
				playerOne.Sales.RotationX = 270;
				playerOne.Sales.RotationZ = 35;*/
				playerOne.Sales.Name = "Drone";
				playerOne.Sales.Health = 10;
				playerOne.Sales.Model = "stapler";

				dynamic playerTwo = new ExpandoObject();
				playerTwo.TintColor = new Vector3(0.251f, 0.376f, 0.663f);
				playerTwo.Drone = new UnitData();
				playerTwo.Drone.Location = new Vector3(-3, -2, 0);
				playerTwo.Drone.Scale = 0.5f;
				playerTwo.Drone.RotationX = 270;
				playerTwo.Drone.RotationZ = -140;
				playerTwo.Drone.Name = "Drone";
				playerTwo.Drone.Health = 10;
				playerTwo.Drone.Model = "stapler";



				data[GlobalData.PlayerOneName] = playerOne;
				data[GlobalData.PlayerTwoName] = playerTwo;
			}
			return data[playerName];
		}

		public static RockPaperScissorsConflictResolver Resolver {
			get {
				if (resolver == null) {
					var conflictTable = new Dictionary<string, Dictionary<string, int>> ();

					conflictTable ["Hacker"] = new Dictionary<string, int> { { "Sales", 1 } };
					conflictTable ["Sales"] = new Dictionary<string, int> { { "Drone", 1 } };
					conflictTable ["Drone"] = new Dictionary<string, int> { { "Hacker", 1 } };

					resolver = new RockPaperScissorsConflictResolver (conflictTable);
				}
				return resolver;
			}
		}
	}
}

