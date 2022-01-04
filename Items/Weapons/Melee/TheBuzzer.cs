using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Melee
{
    class TheBuzzer : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BeeWax, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Leaves a trail of bees when thrown");
        }

        public override void SetDefaults()
        {
            item.damage = 22;
            item.width = 70;
            item.height = 74;
            item.useTime = 22;
            item.useAnimation = 22;
            item.rare = ItemRarityID.Orange;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 3f;
            item.UseSound = SoundID.Item1;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.shootSpeed = 35f;
            item.shoot = ModContent.ProjectileType<TheBuzzer_projectile>();
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.maxStack = 1;
            item.melee = true;
            item.noMelee = true;
        }
    }

    class TheBuzzer_projectile : ModProjectile
    {
        public override string Texture { get { return "TerrorbornMod/Items/Weapons/Melee/TheBuzzer"; } }

        public override void SetDefaults()
        {
            projectile.melee = true;
            projectile.width = 70;
            projectile.height = 74;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 8;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 20;
            height = 20;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.position);

            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.position.X = projectile.position.X + projectile.velocity.X;
                projectile.velocity.X = -oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.position.Y = projectile.position.Y + projectile.velocity.Y;
                projectile.velocity.Y = -oldVelocity.Y;
            }

            TimeUntilReturn = 0;
            return false;
        }

        int TimeUntilReturn = 25;
        int projectileCounter = 5;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            projectile.spriteDirection = player.direction * -1;
            projectile.rotation += 0.5f * player.direction;
            if (TimeUntilReturn <= 0)
            {
                projectile.tileCollide = false;
                Vector2 targetPosition = Main.player[projectile.owner].Center;
                float speed = 35f;
                projectile.velocity = projectile.DirectionTo(player.Center) * speed;
                if (Main.player[projectile.owner].Distance(projectile.Center) <= speed)
                {
                    projectile.active = false;
                }
            }
            else
            {
                TimeUntilReturn--;
                projectileCounter--;
                if (projectileCounter <= 0)
                {
                    projectileCounter = 5;
                    int proj = Projectile.NewProjectile(projectile.Center, projectile.velocity / 10, ProjectileID.Bee, projectile.damage / 2, 0f, projectile.owner);
                    Main.projectile[proj].melee = true;
                    Main.projectile[proj].usesIDStaticNPCImmunity = true;
                    Main.projectile[proj].idStaticNPCHitCooldown = 7;
                }
            }
        }
    }
}
