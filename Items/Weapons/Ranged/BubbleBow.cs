using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class BubbleBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble Bow");
            Tooltip.SetDefault("Converts arrows into bubble arrows\nBubble arrows leave behind a trail of bubbles, and explode into them upon\nhitting enemies");
        }

        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Arrow;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<BubbleArrow>();
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
    }
    class BubbleArrow : ModProjectile
    {
        int BubbleWait = 3;
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            //Projectile.extraUpdates = 100;
            Projectile.timeLeft = 200;
            Projectile.penetrate = 1;
        }
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) - 1.57f;
            BubbleWait--;
            if (BubbleWait <= 0)
            {
                BubbleWait = 3;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, 0), ModContent.ProjectileType<Bubble>(), Projectile.damage / 4, 0, Projectile.owner);
            }
        }
        public override void Kill(int timeLeft)
        {
            if (timeLeft > 0)
            {
                for (int i = 0; i < Main.rand.Next(3, 6); i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - Projectile.velocity, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.ProjectileType<Bubble>(), (int)(Projectile.damage / 3), 0, Projectile.owner);
                }
            }
        }
    }
    class Bubble : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Bubble";
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            //Projectile.extraUpdates = 100;
            Projectile.timeLeft = 60;
            Projectile.penetrate = 1;
            Projectile.hide = false;
        }
        public override void Kill(int timeLeft)
        {
            SoundExtensions.PlaySoundOld(SoundID.Item54, Projectile.position);
        }
    }
}
