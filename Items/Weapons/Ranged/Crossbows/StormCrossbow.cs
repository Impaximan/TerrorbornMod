using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Ranged.Crossbows
{
    class StormCrossbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Right click to load in ammo, up to max of 8" +
                "\nLeft click to rapidly fire loaded ammo" +
                "\nFiring with 8 ammo loaded will release a blast of lightning bolts as well, but consume 3 shots"); */
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.reuseDelay = 45;
            Item.damage = 32;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 64;
            Item.height = 36;
            Item.channel = true;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 18f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3f, -3f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.ThunderShard>(), 18)
                .AddIngredient(ModContent.ItemType<Materials.NoxiousScale>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
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
                Item.shoot = ProjectileID.None;
                Item.autoReuse = true;
                Item.reuseDelay = 5;
                Item.UseSound = SoundID.Item56;
                CombatText.NewText(player.getRect(), Color.White, shotsLeft, shotsLeft == 8, true);
                return base.CanUseItem(player);
            }

            Item.shoot = ProjectileID.PurificationPowder;
            Item.autoReuse = true;
            Item.reuseDelay = 0;
            Item.UseSound = SoundID.Item5;
            return shotsLeft > 0;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                return false;
            }
            if (shotsLeft == 8)
            {
                SoundExtensions.PlaySoundOld(SoundID.Item92, position);
                TerrorbornSystem.ScreenShake(3.5f);
                for (int i = 0; i < Main.rand.Next(5, 7); i++)
                {
                    velocity = velocity.RotatedByRandom(MathHelper.ToRadians(20)) * Main.rand.NextFloat(2f, 3f);
                    Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<LightningBolt>(), damage, knockback, player.whoAmI);
                }
                shotsLeft -= 3;
            }
            else
            {
                shotsLeft--;
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void UpdateInventory(Player player)
        {
            if (!player.channel)
            {
                Item.reuseDelay = 45;
            }
        }
    }

    class LightningBolt : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.White) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 8;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 4;
        }

        bool start = true;
        public override void AI()
        {
            if (start)
            {
                start = false;
                Projectile.velocity /= Projectile.extraUpdates + 1;
            }
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
        }
    }
}

