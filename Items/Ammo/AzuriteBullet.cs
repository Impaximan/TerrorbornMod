using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Ammo
{
    class AzuriteBullet : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 8;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 0, 5);
            Item.shootSpeed = 20;
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<AzuriteBulletProjectile>();
            Item.ammo = AmmoID.Bullet;
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A high speed bullet that has a 20% chance to rain azurite shards on hit foes\nThe bullet's speed is unaffected by water");
        }

        public override void AddRecipes()
        {
            CreateRecipe(111)
                .AddIngredient<Materials.AzuriteBar>()
                .AddIngredient(ItemID.MusketBall, 111)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    class AzuriteBulletProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Azurite Bullet");
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 14;
            height = 14;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(101) <= 20)
            {
                Vector2 position = new Vector2(target.position.X + Main.rand.Next(-200, 200), target.position.Y - Main.rand.Next(600, 800));
                float speed = 5f;
                Vector2 velocity = target.DirectionFrom(position) * speed;

                int proj = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<Projectiles.AzuriteShard>(), Projectile.damage / 2, 0, Main.myPlayer, 0f, 0f);
                Main.projectile[proj].DamageType = DamageClass.Ranged;
            }
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
        }

        public override void Kill(int timeLeft)
        {
            if (timeLeft > 0)
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
                for (int i = 0; i < Main.rand.Next(2, 4); i++)
                {
                    int Num54 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 33, 0, 0, Scale: 1f, newColor: Color.Blue);
                    Main.dust[Num54].noGravity = true;
                    Main.dust[Num54].velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(0.9f, 1.1f);
                }
            }
        }
    }
}