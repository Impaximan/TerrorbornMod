using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Ammo
{
    class TungstenDart : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 28;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 1;
            Item.shootSpeed = 3;
            Item.rare = ItemRarityID.White;
            Item.shoot = ModContent.ProjectileType<TungstenDartProjectile>();
            Item.ammo = AmmoID.Dart;
        }
        //public override bool HoldItemFrame(Player player)
        //{
        //    player.bodyFrame.Y = 56 * 2;
        //    return true;
        //}
        public override void AddRecipes()
        {
            CreateRecipe(111)
                .AddIngredient(ItemID.TungstenBar)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    class TungstenDartProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Ammo/TungstenDart";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tungsten dart");
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 1000;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.05f;
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
        }
        public override void Kill(int timeLeft)
        {
            if (Main.rand.Next(101) <= 20)
            {
                Item.NewItem(Projectile.GetItemSource_DropAsItem(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, ModContent.ItemType<TungstenDart>());
            }
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        }
    }
}


