using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace TerrorbornMod.Projectiles
{
	abstract class Deathray : ModProjectile
	{
		public float MoveDistance = 20f;
		public float RealMaxDistance = 2000f;
		public float MaxDistance = 2000f;
		public Rectangle bodyRect = new Rectangle();
		public Rectangle tailRect = new Rectangle();
		public Rectangle headRect = new Rectangle();
		public Color drawColor = Color.White;
		public float deathrayWidth = 1f;
		public bool FollowPosition = true;

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.hide = true;
			projectile.timeLeft = 1;
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			projectile.velocity = oldVelocity;
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], Position(), projectile.velocity, bodyRect.Height, -1.57f, 1f, MaxDistance, (int)MoveDistance);
			return false;
		}

		public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, float rotation = 0f, float scale = 1f, float maxDist = 2000f, int transDist = 50)
		{
			float r = unit.ToRotation() + rotation;

			for (float i = transDist; i <= maxDist; i += step)
			{
				Color c = drawColor;
				var origin = start + i * unit;
				Rectangle newBodyRectangle = bodyRect;
				if (deathrayWidth != 1f)
                {
					newBodyRectangle.X = newBodyRectangle.X + (int)((bodyRect.Width / 2) * (1 - deathrayWidth));
					newBodyRectangle.Width = (int)(deathrayWidth * newBodyRectangle.Width);
                }
				spriteBatch.Draw(texture, origin - Main.screenPosition, newBodyRectangle, i < transDist ? Color.Transparent : c, r, newBodyRectangle.Size() / 2, scale, 0, 0);
			}

			Rectangle newTailRectangle = tailRect;
			if (deathrayWidth != 1f)
			{
				newTailRectangle.X = newTailRectangle.X + (int)((tailRect.Width / 2) * (1 - deathrayWidth));
				newTailRectangle.Width = (int)(deathrayWidth * newTailRectangle.Width);
			}
			spriteBatch.Draw(texture, start + unit * (transDist - step + 2) + unit - Main.screenPosition, newTailRectangle, drawColor, r, newTailRectangle.Size() / 2, scale, SpriteEffects.FlipVertically, 0);

			Rectangle newHeadRectangle = headRect;
			if (deathrayWidth != 1f)
			{
				newHeadRectangle.X = newHeadRectangle.X + (int)((headRect.Width / 2) * (1 - deathrayWidth));
				newHeadRectangle.Width = (int)(deathrayWidth * newHeadRectangle.Width);
			}
			spriteBatch.Draw(texture, start + (maxDist/* + step*/) * unit - Main.screenPosition, newHeadRectangle, drawColor, r, newHeadRectangle.Size() / 2, scale, SpriteEffects.FlipVertically, 0);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 unit = projectile.velocity;
			float point = 0f;

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Position(),
				Position() + unit * MaxDistance, 22, ref point);
		}

		bool start = true;
		Vector2 ogPosition;
		public override void AI()
		{
			if (start)
            {
				start = false;
				ogPosition = projectile.Center;
            }

			projectile.position = Position() + projectile.velocity * MoveDistance;

			SetLaserPosition();
			SpawnDusts();
			CastLights();

			projectile.velocity.Normalize();
		}

		public virtual Vector2 Position()
        {
			if (!FollowPosition)
            {
				return ogPosition;
			}
			return Main.player[projectile.owner].Center;
        }

		public virtual void SpawnDusts()
		{

		}

		public virtual void SetLaserPosition()
		{
			if (!projectile.tileCollide)
            {
				MaxDistance = RealMaxDistance;
            }
            else
			{
				for (MaxDistance = MoveDistance; MaxDistance <= RealMaxDistance; MaxDistance += 5f)
				{
					var start = Position() + projectile.velocity * MaxDistance;
					if (Collision.SolidCollision(start, 1, 1))
					{
						MaxDistance -= 5f;
						break;
					}
				}
			}
		}

		public virtual void CastLights()
		{

		}

		public override bool ShouldUpdatePosition() => false;

		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 unit = projectile.velocity;
			Utils.PlotTileLine(projectile.Center, projectile.Center + unit * MaxDistance, (projectile.width + 16) * projectile.scale, DelegateMethods.CutTiles);
		}
	}
}
