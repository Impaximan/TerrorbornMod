using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Ranged.Crossbows
{
    class Crossbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Right click to load in ammo, up to max of 5" +
                "\nLeft click to rapidly fire loaded ammo");
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }
        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            Item.damage = 9;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.width = 56;
            Item.height = 20;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.knockBack = 5;
            Item.UseSound = SoundID.Item5;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 0, 25, 0);
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.Wood, 20)
                .AddRecipeGroup(RecipeGroupID.IronBar, 7)
                .AddIngredient(ItemID.Cobweb, 15)
                .AddTile(TileID.Anvils)
                .Register();
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
                Item.shoot = ProjectileID.None;
                Item.autoReuse = true;
                Item.reuseDelay = 10;
                Item.UseSound = SoundID.Item56;
                CombatText.NewText(player.getRect(), Color.White, shotsLeft, shotsLeft == 5, true);
                return base.CanUseItem(player);
            }

            Item.shoot = ProjectileID.PurificationPowder;
            Item.autoReuse = true;
            Item.reuseDelay = 0;
            Item.UseSound = SoundID.Item5;
            return shotsLeft > 0;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                return false;
            }
            shotsLeft--;
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}

