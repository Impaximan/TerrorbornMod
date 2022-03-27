using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Ammo
{
    class GemDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Splits into multiple gem fragments on hit");
        }

        public override void SetDefaults()
        {
            item.damage = 6;
            item.ranged = true;
            item.width = 14;
            item.height = 28;
            item.maxStack = 999;
            item.consumable = true;
            item.knockBack = 1;
            item.shootSpeed = 2;
            item.rare = ItemRarityID.Blue;
            item.shoot = ModContent.ProjectileType<GemDartProjectile>();
            item.ammo = AmmoID.Dart;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Amethyst, 1);
            recipe.AddIngredient(ModContent.ItemType<WoodDart>(), 50);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.Topaz, 1);
            recipe2.AddIngredient(ModContent.ItemType<WoodDart>(), 75);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this, 75);
            recipe2.AddRecipe();
            ModRecipe recipe3 = new ModRecipe(mod);
            recipe3.AddIngredient(ItemID.Sapphire, 1);
            recipe3.AddIngredient(ModContent.ItemType<WoodDart>(), 100);
            recipe3.AddTile(TileID.Anvils);
            recipe3.SetResult(this, 100);
            recipe3.AddRecipe();
            ModRecipe recipe4 = new ModRecipe(mod);
            recipe4.AddIngredient(ItemID.Emerald, 1);
            recipe4.AddIngredient(ModContent.ItemType<WoodDart>(), 125);
            recipe4.AddTile(TileID.Anvils);
            recipe4.SetResult(this, 125);
            recipe4.AddRecipe();
            ModRecipe recipe5 = new ModRecipe(mod);
            recipe5.AddIngredient(ItemID.Ruby, 1);
            recipe5.AddIngredient(ModContent.ItemType<WoodDart>(), 150);
            recipe5.AddTile(TileID.Anvils);
            recipe5.SetResult(this, 150);
            recipe5.AddRecipe();
            ModRecipe recipe6 = new ModRecipe(mod);
            recipe6.AddIngredient(ItemID.Diamond, 1);
            recipe6.AddIngredient(ModContent.ItemType<WoodDart>(), 175);
            recipe6.AddTile(TileID.Anvils);
            recipe6.SetResult(this, 175);
            recipe6.AddRecipe();
            ModRecipe recipe7 = new ModRecipe(mod);
            recipe7.AddIngredient(ItemID.Amber, 1);
            recipe7.AddIngredient(ModContent.ItemType<WoodDart>(), 175);
            recipe7.AddTile(TileID.Anvils);
            recipe7.SetResult(this, 175);
            recipe7.AddRecipe();
        }
    }

    class GemDartProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Ammo/GemDart";

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
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 28;
            projectile.ranged = true;
            projectile.timeLeft = 1000;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.extraUpdates = 1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

        bool start = true;
        public override void AI()
        {
            projectile.velocity.Y += 0.03f;
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                float speed = Main.rand.NextFloat(10f, 15f);
                Vector2 velocity = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2() * speed;
                Projectile.NewProjectile(projectile.Center, velocity, ModContent.ProjectileType<GemFragment>(), projectile.damage / 3, projectile.knockBack / 3, projectile.owner);
            }
            Main.PlaySound(SoundID.DD2_WitherBeastHurt, projectile.Center);
        }
    }

    class GemFragment : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.ranged = true;
            projectile.timeLeft = 300;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.extraUpdates = 1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.penetrate = 3;
        }

        public override void AI()
        {
            projectile.velocity.Y += 0.3f;
            projectile.velocity *= 0.97f;
            projectile.rotation += MathHelper.ToRadians(projectile.velocity.X * 4);
        }
    }
}