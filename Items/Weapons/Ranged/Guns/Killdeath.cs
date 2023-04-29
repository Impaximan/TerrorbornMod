using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Ranged.Guns
{
    class Killdeath : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("THE KILLDEATH");
            /* Tooltip.SetDefault("Fires 120 bullets per second, only consuming 60 per second" +
                "\nConverts all bullets to high velocity bullets" +
                "\n[c/FF1919:Wings? What for?]"); */
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-30, 0);
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.3f;
            Item.damage = 1;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.width = 158;
            Item.height = 64;
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.knockBack = 15f;
            Item.UseSound = new SoundStyle("TerrorbornMod/Sounds/Effects/CoolerMachineGun");
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 25, 0, 0);
            Item.rare = ModContent.RarityType<Rarities.Twilight>();
            Item.shootSpeed = 25f;
            Item.useAmmo = AmmoID.Bullet;
            Item.scale = 1f;
            Item.channel = true;
            Item.reuseDelay = 10;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type = ProjectileID.BulletHighVelocity;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (Item.reuseDelay <= 0)
            {
                TerrorbornSystem.ScreenShake(25f);
                Projectile.NewProjectile(source, position + velocity / 2, velocity, type, damage, knockback, player.whoAmI);
                velocity.Normalize();
                player.velocity -= velocity * 0.5f;
                player.maxFallSpeed = 500f;
                player.noFallDmg = true;
            }
            else
            {
                TerrorbornSystem.ScreenShake(3f);
                Item.reuseDelay--;
            }
            return false;
        }

        public override void UpdateInventory(Player player)
        {
            if (!player.controlUseItem)
            {
                Item.reuseDelay = 10;
            }
        }
    }
}
