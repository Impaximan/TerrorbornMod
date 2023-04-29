using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Ranged.Guns
{
    class TheDoubleBarrel : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Blunderbuss");
            /* Tooltip.SetDefault("Fires many bullets at once" +
                "\nCan be fired as quick as you want for 4 shots before needing to be reloaded" +
                "\nReloads automatically when not firing"); */
        }
        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.17f;
            Item.damage = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 50;
            Item.height = 24;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.crit = 15;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.autoReuse = false;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet;
            Item.holdStyle = ItemHoldStyleID.HoldFront;
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
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
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            useCounter = 35;
            shotsLeft--;
            SoundExtensions.PlaySoundOld(SoundID.Item36, player.position);
            for (int i = 0; i < Main.rand.Next(4, 7); i++)
            {
                Vector2 EditedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.7f, 1.3f);
                Projectile.NewProjectile(source, position, EditedSpeed, type, damage, knockback, player.whoAmI);
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
                    TerrorbornSystem.ScreenShake(1f);
                }
            }
            else
            {
                shotsLeft = 4;
            }
        }
    }
}
