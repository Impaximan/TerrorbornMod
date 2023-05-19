using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Ammo
{
    class GrapeshotBullet : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 0, 5);
            Item.shootSpeed = 20;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<GrapeshotBulletSpawn>();
            Item.ammo = AmmoID.Bullet;
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires multiple bullets at once");
        }


        public override void AddRecipes()
        {
            CreateRecipe(111)
                .AddIngredient(ModContent.ItemType<Materials.SoulOfPlight>())
                .AddIngredient(ItemID.EmptyBullet)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    class GrapeshotBulletSpawn : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Ammo/GrapeshotBullet";

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 1;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 velocity = Projectile.velocity;
                velocity *= Main.rand.NextFloat(0.8f, 1.2f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity.RotatedByRandom(MathHelper.ToRadians(15)), ModContent.ProjectileType<GrapeshotBulletProjectile>(), (int)(Projectile.damage * 0.45f), Projectile.knockBack, Projectile.owner);
            }
        }
    }

    class GrapeshotBulletProjectile : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 14;
            height = 14;
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 position = Projectile.Center - Main.screenPosition;
            position.Y += 4;
            Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, Projectile.width, Projectile.height), new Rectangle(0, 0, Projectile.width, Projectile.height), Color.White, Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2), SpriteEffects.None, 0);
            return false;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
            Projectile.hide = false;
        }

        public override void Kill(int timeLeft)
        {
            if (timeLeft > 0)
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
            }
        }
    }
}