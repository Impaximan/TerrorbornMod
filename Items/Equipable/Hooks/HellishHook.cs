using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items.Equipable.Hooks
{
    class HellishHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Has very long range and homes into the cursor, but returns to you slowly");
		}

		public override void SetDefaults()
		{
			Item.noUseGraphic = true;
			Item.damage = 0;
			Item.knockBack = 7f;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 30f;
			Item.width = 44;
			Item.height = 38;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.rare = ItemRarityID.LightRed;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.shoot = ModContent.ProjectileType<HellishHookProjectile>();
		}
	}

	class HellishHookProjectile : ModProjectile
	{
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.SingleGrappleHook[Type] = true;
        }

        public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.width = 36;
			Projectile.height = 36;
			Projectile.aiStyle = 7;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 600;
		}

		//public override bool? CanUseGrapple(Player player)
		//{
		//	int hooksOut = 0;
		//	for (int l = 0; l < 1000; l++)
		//	{
		//		if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == Projectile.type)
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
			Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(Main.MouseWorld).ToRotation(), MathHelper.ToRadians(1f * (Projectile.velocity.Length() / 20))).ToRotationVector2() * Projectile.velocity.Length();
			Dust dust = Dust.NewDustPerfect(Projectile.Center, 6, Vector2.Zero);
			dust.noGravity = true;
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

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 18;
			height = 18;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void GrappleTargetPoint(Player player, ref float grappleX, ref float grappleY)
		{
			Vector2 dirToPlayer = Projectile.DirectionTo(player.Center);
			float hangDist = 50f;
			grappleX += dirToPlayer.X * hangDist;
			grappleY += dirToPlayer.Y * hangDist;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 originPoint = Main.player[Projectile.owner].Center;
			Vector2 center = Projectile.Center;
			Vector2 distToProj = originPoint - Projectile.Center;
			float projRotation = distToProj.ToRotation() - 1.57f;
			float distance = distToProj.Length();
			Texture2D texture = ModContent.Request<Texture2D>("TerrorbornMod/Items/Equipable/Hooks/HellishHookChain").Value;

			while (distance > texture.Height && !float.IsNaN(distance))
			{
				distToProj.Normalize();
				distToProj *= texture.Height;
				center += distToProj;
				distToProj = originPoint - center;
				distance = distToProj.Length();


				//Draw chain
				Main.spriteBatch.Draw(texture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
					new Rectangle(0, 0, texture.Width, texture.Height), Color.White, projRotation,
					new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
			}

			Texture2D texture2 = ModContent.Request<Texture2D>(Texture).Value;
			Vector2 position = Projectile.Center - Main.screenPosition;
			Main.spriteBatch.Draw(texture2, new Rectangle((int)position.X, (int)position.Y, texture2.Width, texture2.Height), new Rectangle(0, 0, texture2.Width, texture2.Height), Projectile.GetAlpha(Color.White), Projectile.rotation - MathHelper.ToRadians(90), new Vector2(texture2.Width / 2, texture2.Height / 2), SpriteEffects.None, 0);
			return false;
		}
	}
}

