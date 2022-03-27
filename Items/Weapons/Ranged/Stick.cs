using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class Stick : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stick");
            Tooltip.SetDefault("It's just a stick...");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.damage = 5;
            item.ranged = true;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.UseSound = SoundID.Item1;
            item.rare = ItemRarityID.White;
            item.consumable = true;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = false;
            item.width = 30;
            item.height = 30;
            item.shoot = ModContent.ProjectileType<StickThrown>();
            item.shootSpeed = 15f;
            item.knockBack = 2f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup(RecipeGroupID.Wood);
            recipe.SetResult(this, 15);
            recipe.AddRecipe();
        }
    }

    class StickThrown : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/Stick";

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.ranged = true;
            projectile.timeLeft = 300;
            projectile.penetrate = 3;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 15;
            height = 15;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        int timeAlive = 0;
        public override void AI()
        {
            projectile.spriteDirection = Math.Sign(projectile.velocity.X);
            projectile.rotation += projectile.spriteDirection * MathHelper.ToRadians(projectile.velocity.Length() * 1.5f);
            timeAlive++;
            if (timeAlive >= 15)
            {
                projectile.velocity.Y += 0.4f;
                projectile.velocity.X *= 0.95f;
            }
        }
    }
}