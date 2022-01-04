using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Events;
using Terraria.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.World.Generation;
using Terraria.UI;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class Shrapnade : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.ShellFragments>(), 2);
            recipe.AddIngredient(ItemID.IronBar);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Explodes into multiple bits of shrapnel");
        }

        public override void SetDefaults()
        {
            item.damage = 11;
            item.ranged = true;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 30;
            item.useAnimation = 30;
            item.noUseGraphic = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 10;
            item.value = Item.sellPrice(0, 0, 0, 20);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.noMelee = true;
            item.shootSpeed = 10;
            item.shoot = ModContent.ProjectileType<ShrapnadeProjectile>();
        }
    }

    class ShrapnadeProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/Shrapnade";
        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 18;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.ranged = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 60 * 3;
        }

        public override void AI()
        {
            projectile.rotation += MathHelper.ToRadians(projectile.velocity.X * 4);
            projectile.velocity.Y += 0.2f;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item14, projectile.Center);
            for (int i = 0; i < Main.rand.Next(3, 6); i++)
            {
                float Speed = Main.rand.Next(7, 10);
                Vector2 ProjectileSpeed = MathHelper.ToRadians(Main.rand.Next(361)).ToRotationVector2() * Speed;
                Projectile.NewProjectile(projectile.Center, ProjectileSpeed, ModContent.ProjectileType<Shrapnel>(), (int)(projectile.damage * 0.75f), 0, projectile.owner);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.position.X = projectile.position.X + projectile.velocity.X;
                projectile.velocity.X = -oldVelocity.X * 0.5f;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.position.Y = projectile.position.Y + projectile.velocity.Y;
                projectile.velocity.Y = -oldVelocity.Y * 0.5f;
            }
            return false;
        }
    }

    class Shrapnel : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 16;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.timeLeft = 600;
            projectile.frame = Main.rand.Next(3);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 8;
            height = 8;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override void AI()
        {
            projectile.velocity.Y += 0.2f;

            if (projectile.velocity.X > 0)
            {
                projectile.rotation += MathHelper.ToRadians(projectile.velocity.Length());
            }
            else
            {
                projectile.rotation -= MathHelper.ToRadians(projectile.velocity.Length());
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.position);
            return true;
        }
    }
}
