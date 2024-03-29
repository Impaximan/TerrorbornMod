﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Ranged.DartWeapons
{
    class GoldBlowpipe : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Uses darts as ammo" +
                "\nRight click to fire slower, but cause your darts to move quicker and deal slightly more damage");
        }
        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 38;
            Item.height = 12;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = 101;
            Item.knockBack = 0.6f;
            Item.value = 0;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item63;
            Item.autoReuse = true;
            Item.shootSpeed = 11.25f;
            Item.useAmmo = AmmoID.Dart;
        }
        Vector2 offset = new Vector2(0, 1);
        public override void UseItemFrame(Player player)
        {
            player.bodyFrame.Y = 2 * player.bodyFrame.Height;
            player.itemLocation = player.Center + offset;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.reuseDelay = 10;
            }
            else
            {
                Item.reuseDelay = 0;
            }
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                velocity.X *= 2;
                velocity.Y *= 2;
                damage = (int)(damage * 1.05f);
            }
            player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation();
            if (player.direction == -1)
            {
                player.itemRotation += MathHelper.ToRadians(180);
            }
            position = player.Center + offset;
            position.Y -= 3;
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override bool? UseItem(Player player)
        {
            player.bodyFrame.Y = 56 * 2;
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GoldBar, 7)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}


