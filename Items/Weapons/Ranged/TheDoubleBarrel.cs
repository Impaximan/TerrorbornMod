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
            DisplayName.SetDefault("The Blunderbuss");
            Tooltip.SetDefault("Fires many bullets at once" +
                "\nCan be fired as quick as you want for 4 shots before needing to be reloaded" +
                "\nReloads automatically when not firing");
        }
        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.critDamageMult = 1.17f;
            item.damage = 15;
            item.ranged = true;
            item.noMelee = true;
            item.width = 50;
            item.height = 24;
            item.useTime = 14;
            item.useAnimation = 14;
            item.shoot = ProjectileID.PurificationPowder;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 5;
            item.crit = 15;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.Pink;
            item.autoReuse = false;
            item.shootSpeed = 16f;
            item.useAmmo = AmmoID.Bullet;
            item.holdStyle = ItemHoldStyleID.HoldingOut;
        }

        public override void HoldStyle(Player player)
        {
            player.itemLocation = player.position + new Vector2(-15, 10);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        int useCounter = 0;
        int shotsLeft = 0;
        public override bool CanUseItem(Player player)
        {
            return shotsLeft > 0;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            useCounter = 35;
            shotsLeft--;
            Main.PlaySound(SoundID.Item36, player.position);
            for (int i = 0; i < Main.rand.Next(4, 7); i++)
            {
                Vector2 EditedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.7f, 1.3f);
                Projectile.NewProjectile(position, EditedSpeed, type, damage, knockBack, item.owner);
            }
            return false;
        }

        public override void UpdateInventory(Player player)
        {
            if (useCounter > 0)
            {
                useCounter--;
                if (useCounter <= 0)
                {
                    CombatText.NewText(player.getRect(), Color.White, "Reloaded");
                    TerrorbornMod.ScreenShake(1f);
                }
            }
            else
            {
                shotsLeft = 4;
            }
        }
    }
}
