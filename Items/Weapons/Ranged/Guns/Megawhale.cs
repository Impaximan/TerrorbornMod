using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Ranged.Guns
{
    class Megawhale : ModItem
    {
        int baseReuseDelay = 25;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires faster the longer you use it" +
                "\nFires three bullets at once in a fluctuating spread" +
                "\n'The Minishark's gacha addiction'");
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.3f;
            Item.damage = 25;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.width = 112;
            Item.height = 46;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.knockBack = 5;
            Item.UseSound = SoundID.Item11; //TODO: Make this a custom sound
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 0, 25, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet;
            Item.scale = 0.75f;
            Item.channel = true;
            Item.reuseDelay = baseReuseDelay;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, -2);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Megashark, 1)
                .AddIngredient(ModContent.ItemType<Materials.PlasmaliumBar>(), 12)
                .AddIngredient(ModContent.ItemType<Materials.AzuriteBar>(), 12)
                .AddIngredient(ModContent.ItemType<Materials.HellbornEssence>(), 4)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void UpdateInventory(Player player)
        {
            if (!player.channel)
            {
                Item.reuseDelay = baseReuseDelay;
            }
        }

        public override bool CanUseItem(Player player)
        {
            if (Item.reuseDelay > 0)
            {
                Item.reuseDelay--;
            }
            return base.CanUseItem(player);
        }

        int shots = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            shots++;
            float rotationAmount = (float)Math.Sin(shots / 10f) * 25f;
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(rotationAmount)), type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-rotationAmount)), type, damage, knockback, player.whoAmI);
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}