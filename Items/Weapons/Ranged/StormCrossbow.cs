using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class StormCrossbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Right click to load in ammo, up to max of 8" +
                "\nLeft click to rapidly fire loaded ammo" +
                "\nFiring with 8 ammo loaded will release a blast of lightning bolts as well, but consume 3 shots");
        }

        public override void SetDefaults()
        {
            item.reuseDelay = 45;
            item.damage = 43;
            item.ranged = true;
            item.width = 64;
            item.height = 36;
            item.channel = true;
            item.useTime = 6;
            item.useAnimation = 6;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 2;
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = ProjectileID.PurificationPowder;
            item.shootSpeed = 18f;
            item.useAmmo = AmmoID.Arrow;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3f, -3f);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 18);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.NoxiousScale>(), 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        int shotsLeft = 0;
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                shotsLeft++;
                if (shotsLeft > 8)
                {
                    shotsLeft = 8;
                }
                item.shoot = ProjectileID.None;
                item.autoReuse = true;
                item.reuseDelay = 5;
                item.UseSound = SoundID.Item56;
                CombatText.NewText(player.getRect(), Color.White, shotsLeft, shotsLeft == 8, true);
                return base.CanUseItem(player);
            }

            item.shoot = ProjectileID.PurificationPowder;
            item.autoReuse = true;
            item.reuseDelay = 0;
            item.UseSound = SoundID.Item5;
            return shotsLeft > 0;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                return false;
            }
            if (shotsLeft == 8)
            {
                Main.PlaySound(SoundID.Item92, position);
                TerrorbornMod.ScreenShake(3.5f);
                for (int i = 0; i < Main.rand.Next(5, 7); i++)
                {
                    Vector2 velocity = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20)) * Main.rand.NextFloat(2f, 3f);
                    Projectile.NewProjectile(position, velocity, ModContent.ProjectileType<LightningBolt>(), damage, knockBack, player.whoAmI);
                }
                shotsLeft -= 3;
            }
            else
            {
                shotsLeft--;
            }
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override void UpdateInventory(Player player)
        {
            if (!player.channel)
            {
                item.reuseDelay = 45;
            }
        }
    }

    class LightningBolt : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(Color.White) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 8;
            projectile.ranged = true;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.penetrate = -1;
            projectile.extraUpdates = 4;
        }

        bool start = true;
        public override void AI()
        {
            if (start)
            {
                start = false;
                projectile.velocity /= projectile.extraUpdates + 1;
            }
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
        }
    }
}

