using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerrorbornMod.TBUtils;

namespace TerrorbornMod.Items.Weapons.Melee
{
	public class Superyova : ModItem
	{
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Materials.NovagoldBar>(), 7);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void SetStaticDefaults()
		{
			ItemID.Sets.Yoyo[item.type] = true;
			ItemID.Sets.GamepadExtraRange[item.type] = 15;
			ItemID.Sets.GamepadSmartQuickReach[item.type] = true;
			Tooltip.SetDefault("Implodes after hitting an enemy 3 times, pulling other enemies closer to itself");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.width = 26;
			item.height = 40;
			item.useAnimation = 25;
			item.useTime = 25;
			item.shootSpeed = 17f;
			item.shoot = ModContent.ProjectileType<SuperyovaProjectile>();
			item.knockBack = 0f;
			item.damage = 20;
			item.rare = ItemRarityID.Blue;
			item.melee = true;
			item.channel = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.value = Item.sellPrice(0, 1, 0, 0);
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
			ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 7f;
			ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 400f;
			ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 12f;
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = 99;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
		}

		int timesUntilImplode = 3;
		int glowSize = 26;
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			glowSize += 3;
			timesUntilImplode--;
			if (timesUntilImplode <= 0)
			{
				Main.PlaySound(SoundID.Item14, projectile.Center);
				TerrorbornMod.ScreenShake(1.5f);
				projectile.timeLeft = 0;
				projectile.damage = 0;
				DustExplosion(projectile.Center, 30, 25f, 150f);
				foreach (NPC npc in Main.npc)
				{
					if (!npc.dontTakeDamage && !npc.friendly && npc.active && npc.knockBackResist != 0f && npc.Distance(projectile.Center) <= 500)
					{
						npc.velocity = projectile.DirectionFrom(npc.Center) * 17.5f * npc.knockBackResist;
					}
				}
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			Graphics.DrawGlow_1(spriteBatch, projectile.Center - Main.screenPosition, glowSize, Color.LightSkyBlue * 0.5f);
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