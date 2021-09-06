using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class Asphodel : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrimtaneBar, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            Tooltip.SetDefault("Creates a flower at your cursor that explodes into numerous seeds");
        }

        public override void SetDefaults()
        {
            item.damage = 25;
            item.noMelee = true;
            item.width = 52;
            item.height = 52;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = 5;
            item.knockBack = 2.5f;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item73;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("IncendiaryFlower");
            item.shootSpeed = 5f;
            item.mana = 6;
            item.magic = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;
            speedX = 0;
            speedY = 0;
            return true;
        }
    }

    class IncendiaryFlower : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 46;
            projectile.height = 46;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.alpha = 255;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 20;
        }

        public override void AI()
        {
            projectile.rotation += MathHelper.ToRadians(10);
            //projectile.scale += 0.5f / 45f;
            
            if (projectile.alpha > (int)(255 * 0.25f))
            {
                projectile.alpha -= 15;
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.DD2_FlameburstTowerShot, projectile.Center);
            for (int i = 0; i < Main.rand.Next(4, 7); i++)
            {
                Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                float speed = Main.rand.Next(10, 15);
                int proj = Projectile.NewProjectile(projectile.Center, direction * speed, ModContent.ProjectileType<IncendiarySeed>(), projectile.damage, projectile.knockBack, projectile.owner);
            }
        }
    }

    class IncendiarySeed : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 18;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.hostile = false;
            projectile.timeLeft = 110;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 15;
            height = 15;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override void AI()
        {
            projectile.velocity.Y += 0.2f;
            Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.Fire);
            projectile.rotation = projectile.velocity.ToRotation();
        }
    }
}
