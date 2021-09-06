using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TerrorbornMod.Items.Equipable.Hooks
{
	class HellishHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Has very long range and homes into the cursor, but returns to you slowly");
		}

		public override void SetDefaults()
		{
			item.noUseGraphic = true;
			item.damage = 0;
			item.knockBack = 7f;
			item.useStyle = 5;
			item.shootSpeed = 30f;
			item.width = 44;
			item.height = 38;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 20;
			item.useTime = 20;
			item.rare = 4;
			item.noMelee = true;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.shoot = ModContent.ProjectileType<HellishHookProjectile>();
		}
	}

	class HellishHookProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.width = 36;
			projectile.height = 36;
			projectile.aiStyle = 7;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft = 600;
		}

		//public override bool? CanUseGrapple(Player player)
		//{
		//	int hooksOut = 0;
		//	for (int l = 0; l < 1000; l++)
		//	{
		//		if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == projectile.type)
		//		{
		//			hooksOut++;
		//		}
		//	}
		//	if (hooksOut > 0)
		//	{
		//		return false;
		//	}
		//	return true;
		//}

		bool start = false;
		public override void PostAI()
		{
			projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(Main.MouseWorld).ToRotation(), MathHelper.ToRadians(1f * (projectile.velocity.Length() / 20))).ToRotationVector2() * projectile.velocity.Length();
			Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.Fire, Vector2.Zero);
			dust.noGravity = true;
		}

		public override bool? SingleGrappleHook(Player player)
		{
			return true;
		}
		public override float GrappleRange()
		{
			return 1000f;
		}

		public override void NumGrappleHooks(Player player, ref int numHooks)
		{
			numHooks = 1;
		}

		public override void GrappleRetreatSpeed(Player player, ref float speed)
		{
			speed = 10f;
		}

		public override void GrapplePullSpeed(Player player, ref float speed)
		{
			speed = 15f;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 18;
			height = 18;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough);
		}

		public override void GrappleTargetPoint(Player player, ref float grappleX, ref float grappleY)
		{
			Vector2 dirToPlayer = projectile.DirectionTo(player.Center);
			float hangDist = 50f;
			grappleX += dirToPlayer.X * hangDist;
			grappleY += dirToPlayer.Y * hangDist;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 originPoint = Main.player[projectile.owner].Center;
			Vector2 center = projectile.Center;
			Vector2 distToProj = originPoint - projectile.Center;
			float projRotation = distToProj.ToRotation() - 1.57f;
			float distance = distToProj.Length();
			Texture2D texture = ModContent.GetTexture("TerrorbornMod/Items/Equipable/Hooks/HellishHookChain");

			while (distance > texture.Height && !float.IsNaN(distance))
			{
				distToProj.Normalize();
				distToProj *= texture.Height;
				center += distToProj;
				distToProj = originPoint - center;
				distance = distToProj.Length();


				//Draw chain
				spriteBatch.Draw(texture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
					new Rectangle(0, 0, texture.Width, texture.Height), Color.White, projRotation,
					new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
			}

			Texture2D texture2 = Main.projectileTexture[projectile.type];
			Vector2 position = projectile.Center - Main.screenPosition;
			Main.spriteBatch.Draw(texture2, new Rectangle((int)position.X, (int)position.Y, texture2.Width, texture2.Height), new Rectangle(0, 0, texture2.Width, texture2.Height), projectile.GetAlpha(Color.White), projectile.rotation - MathHelper.ToRadians(90), new Vector2(texture2.Width / 2, texture2.Height / 2), SpriteEffects.None, 0);
			return false;
		}
	}
}

