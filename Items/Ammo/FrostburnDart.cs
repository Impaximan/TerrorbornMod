using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Ammo
{
    class FrostburnDart : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 5;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 22;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 1;
            Item.shootSpeed = 0;
            Item.rare = ItemRarityID.White;
            Item.shoot = ModContent.ProjectileType<FrostburnDartProjectile>();
            Item.ammo = AmmoID.Dart;
        }
        //public override bool HoldItemFrame(Player player)
        //{
        //    player.bodyFrame.Y = 56 * 2;
        //    return true;
        //}
        public override void AddRecipes()
        {
            CreateRecipe(10)
                .AddIngredient(ModContent.ItemType<WoodDart>(), 10)
                .AddIngredient(ItemID.IceBlock, 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
    class FrostburnDartProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Ammo/FrostburnDart";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Flaming dart");
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 22;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 1000;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }
        int DustCooldown = 69;
        public override void AI()
        {
            if (DustCooldown == 69)
            {
                DustCooldown = Main.rand.Next(5, 10);
            }
            Projectile.velocity.Y += 0.1f;
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            DustCooldown--;
            if (DustCooldown <= 0)
            {
                DustCooldown = Main.rand.Next(5, 10);
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135);
                Main.dust[dust].velocity = Projectile.velocity;
                Main.dust[dust].noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.Frostburn, 60 * 3);
            }
        }
        public override void Kill(int timeLeft)
        {
            if (Main.rand.Next(101) <= 20)
            {
                Item.NewItem(Projectile.GetSource_DropAsItem(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, ModContent.ItemType<FrostburnDart>());
            }
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
        }
    }
}

