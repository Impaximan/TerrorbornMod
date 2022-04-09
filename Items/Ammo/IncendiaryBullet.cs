using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Ammo
{
    class IncendiaryBullet : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 0, 20);
            Item.shootSpeed = 4;
            Item.rare = ItemRarityID.LightRed;
            Item.shoot = ModContent.ProjectileType<IncendiaryBulletProjectile>();
            Item.ammo = AmmoID.Bullet;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Incendiary Round");
            Tooltip.SetDefault("Fires at incredibly high speeds" +
                "\nLeaves a cloud of fiery gas behind on critical hits" +
                "\nHeals you for 1 HP on crits if you are under 250 HP");
        }
        //public override bool HoldItemFrame(Player player)
        //{
        //    player.bodyFrame.Y = 56 * 2;
        //    return true;
        //}
        public override void AddRecipes()
        {
            CreateRecipe(222)
                .AddIngredient<Materials.IncendiusAlloy>(2)
                .AddRecipeGroup("cobalt")
                .AddTile<Tiles.Incendiary.IncendiaryAltar>()
                .Register();
        }
    }
    class IncendiaryBulletProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.extraUpdates = 5;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 14;
            height = 14;
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 60 * 10);
            if (crit)
            {
                Projectile.NewProjectile(Projectile.GetProjectileSource_OnHit(target, Projectile.whoAmI), Projectile.Center, target.velocity, ModContent.ProjectileType<Incendius.FlameCloud>(), Projectile.damage / 5, 0, Projectile.owner);

                Player player = Main.player[Projectile.owner];
                if (player.statLife <= 250)
                {
                    player.HealEffect(1);
                    player.statLife++;
                }
            }
        }

        int dustCooldown = 4;
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
            dustCooldown--;
            if (dustCooldown <= 0)
            {
                dustCooldown = 4;
                Dust dust = Dust.NewDustPerfect(Projectile.Center, 6);
                dust.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (timeLeft > 0)
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            }
        }
    }
}