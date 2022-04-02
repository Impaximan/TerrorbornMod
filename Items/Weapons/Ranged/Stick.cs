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
            Item.maxStack = 999;
            Item.damage = 5;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.White;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.width = 30;
            Item.height = 30;
            Item.shoot = ModContent.ProjectileType<StickThrown>();
            Item.shootSpeed = 15f;
            Item.knockBack = 2f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(15)
                .AddRecipeGroup(RecipeGroupID.Wood)
                .Register();
        }
    }

    class StickThrown : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/Stick";

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 15;
            height = 15;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        int timeAlive = 0;
        public override void AI()
        {
            Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
            Projectile.rotation += Projectile.spriteDirection * MathHelper.ToRadians(Projectile.velocity.Length() * 1.5f);
            timeAlive++;
            if (timeAlive >= 15)
            {
                Projectile.velocity.Y += 0.4f;
                Projectile.velocity.X *= 0.95f;
            }
        }
    }
}