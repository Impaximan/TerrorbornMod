using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class Crossbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Right click to load in ammo, up to max of 5" +
                "\nLeft click to rapidly fire loaded ammo");
        }
        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            item.damage = 9;
            item.ranged = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.width = 56;
            item.height = 20;
            item.useTime = 8;
            item.useAnimation = 8;
            item.knockBack = 5;
            item.UseSound = SoundID.Item5;
            item.shoot = ProjectileID.PurificationPowder;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.sellPrice(0, 0, 25, 0);
            item.rare = ItemRarityID.Blue;
            item.shootSpeed = 16f;
            item.useAmmo = AmmoID.Arrow;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup(RecipeGroupID.Wood, 20);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 7);
            recipe.AddIngredient(ItemID.Cobweb, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        int shotsLeft = 0;
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                shotsLeft++;
                if (shotsLeft > 5)
                {
                    shotsLeft = 5;
                }
                item.shoot = ProjectileID.None;
                item.autoReuse = true;
                item.reuseDelay = 10;
                item.UseSound = SoundID.Item56;
                CombatText.NewText(player.getRect(), Color.White, shotsLeft, shotsLeft == 5, true);
                return base.CanUseItem(player);
            }

            item.shoot = ProjectileID.PurificationPowder;
            item.autoReuse = true;
            item.reuseDelay = 0;
            item.UseSound = SoundID.Item5;
            return shotsLeft > 0;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                return false;
            }
            shotsLeft--;
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
    }
}

