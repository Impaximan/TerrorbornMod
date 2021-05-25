using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class TheDoubleBarrel : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires two shotgun blasts of bullets in a row");
        }
        public override void SetDefaults()
        {
            item.damage = 22;
            item.ranged = true;
            item.noMelee = true;
            item.width = 60;
            item.height = 22;
            item.useTime = 8;
            item.reuseDelay = 40;
            item.shoot = 10;
            item.useAnimation = 16;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 4;
            item.autoReuse = true;
            item.shootSpeed = 16f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < Main.rand.Next(4, 7); i++)
            {
                Main.PlaySound(SoundID.Item36, player.position);
                Vector2 EditedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(13)) * Main.rand.NextFloat(0.7f, 1.3f);
                Projectile.NewProjectile(position, EditedSpeed, type, damage, knockBack, item.owner);
            }
            return false;
        }
    }
}
