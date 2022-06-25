using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using TerrorbornMod.TBUtils;

namespace TerrorbornMod.Items.Weapons.Melee
{
    public class Superyova : ModItem
	{
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Materials.NovagoldBar>(), 7)
				.AddTile(TileID.Anvils)
				.Register();
		}

		public override void SetStaticDefaults()
		{
			ItemID.Sets.Yoyo[Item.type] = true;
			ItemID.Sets.GamepadExtraRange[Item.type] = 15;
			ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
			Tooltip.SetDefault("Implodes after hitting an enemy 3 times, pulling other enemies closer to itself");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.width = 26;
			Item.height = 40;
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.shootSpeed = 17f;
			Item.shoot = ModContent.ProjectileType<SuperyovaProjectile>();
			Item.knockBack = 0f;
			Item.damage = 20;
			Item.rare = ItemRarityID.Blue;
			Item.DamageType = DamageClass.Melee;
			Item.channel = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.value = Item.sellPrice(0, 1, 0, 0);
		}

		private static readonly int[] unwantedPrefixes = new int[] { PrefixID.Terrible, PrefixID.Dull, PrefixID.Shameful, PrefixID.Annoying, PrefixID.Broken, PrefixID.Damaged, PrefixID.Shoddy };

		public override bool AllowPrefix(int pre)
		{
			if (Array.IndexOf(unwantedPrefixes, pre) > -1)
			{
				return false;
			}
			return true;
		}
	}
	public class SuperyovaProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 7f;
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 400f;
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 12f;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = 99;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
		}

		int timesUntilImplode = 3;
		int glowSize = 26;
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			glowSize += 3;
			timesUntilImplode--;
			if (timesUntilImplode <= 0)
			{
				SoundExtensions.PlaySoundOld(SoundID.Item14, Projectile.Center);
				TerrorbornSystem.ScreenShake(1.5f);
				Projectile.timeLeft = 0;
				Projectile.damage = 0;
				DustExplosion(Projectile.Center, 30, 25f, 150f);
				foreach (NPC NPC in Main.npc)
				{
					if (!NPC.dontTakeDamage && !NPC.friendly && NPC.active && NPC.knockBackResist != 0f && NPC.Distance(Projectile.Center) <= 500)
					{
						NPC.velocity = Projectile.DirectionFrom(NPC.Center) * 17.5f * NPC.knockBackResist;
					}
				}
			}
		}

        public override bool PreDraw(ref Color lightColor)
        {
			Graphics.DrawGlow_1(Main.spriteBatch, Projectile.Center - Main.screenPosition, glowSize, Color.LightSkyBlue * 0.5f);
            return true;
        }

        public void DustExplosion(Vector2 position, int dustAmount, float minDistance, float maxDistance)
		{
			Vector2 direction = new Vector2(0, 1);
			for (int i = 0; i < dustAmount; i++)
			{
				Vector2 newPos = position + direction.RotatedBy(MathHelper.ToRadians((360f / dustAmount) * i)) * Main.rand.NextFloat(minDistance, maxDistance);
				Dust dust = Dust.NewDustPerfect(newPos, 15);
				dust.scale = 1f;
				dust.velocity = (position - newPos) / 10;
				dust.noGravity = true;
			}
		}
	}
}