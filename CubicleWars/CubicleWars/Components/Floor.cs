using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CubicleWars
{
	public class Floor : DrawableGameComponent
	{
		VertexPositionNormalTexture[] floorVertices;
		short[] indexes;
		const int SIZE = 9;
		BasicEffect basicEffect;

		public Floor (Game game) : base(game)
		{
		}

		public override void Initialize () {
			var normal = new Vector3 (0, 0, -1);
			floorVertices = new VertexPositionNormalTexture[4];
			floorVertices [0].Position = new Vector3 (-1, 1, 0);
			floorVertices [0].TextureCoordinate = new Vector2 (0, 0);
			floorVertices [0].Normal = normal;
			floorVertices [1].Position = new Vector3 (1, 1, 0);
			floorVertices [1].TextureCoordinate = new Vector2 (SIZE, 0);
			floorVertices [1].Normal = normal;
			floorVertices [2].Position = new Vector3 (-1, -1, 0);
			floorVertices [2].TextureCoordinate = new Vector2 (0, SIZE);
			floorVertices [2].Normal = normal;
			floorVertices [3].Position = new Vector3 (1, -1, 0);
			floorVertices [3].TextureCoordinate = new Vector2 (SIZE, SIZE);
			floorVertices [3].Normal = normal;
			
			indexes = new short[6];
			indexes [0] = 0;
			indexes [1] = 1;
			indexes [2] = 2;
			indexes [3] = 2;
			indexes [4] = 1;
			indexes [5] = 3;

			base.Initialize();
		}

		protected override void LoadContent ()
		{
			basicEffect = new BasicEffect (Game.GraphicsDevice);
			var colorTexture = Game.Content.Load<Texture2D> ("seamlesscarpet");
			var game = Game as Startup;
			basicEffect.EnableDefaultLighting ();
			
			basicEffect.World = Matrix.CreateScale (20) * 
								Matrix.CreateTranslation (GameData.GlobalData.Ground);

			basicEffect.View = game.View;
			basicEffect.Projection = game.Projection;
			basicEffect.TextureEnabled = true;
			basicEffect.Texture = colorTexture;
		}

		public override void Draw(GameTime gameTime)
		{
			GraphicsDevice.SamplerStates[0] = new SamplerState
			                                      {
			                                          AddressU = TextureAddressMode.Wrap,
                                                      AddressW = TextureAddressMode.Wrap
			                                      };
			
			foreach (var pass in basicEffect.CurrentTechnique.Passes) {
				pass.Apply ();
				
				GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture> (
					PrimitiveType.TriangleList,
					floorVertices,
					0,
					4,
					indexes,
					0,
					2);
			}
		}
	}
}

