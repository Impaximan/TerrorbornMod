using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class HarpyBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hold JUMP to fall slower");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.useAnimation = 5;
            Item.defense = 3;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Feather, 6)
                .AddIngredient(ItemID.Cloud, 20)
                .AddRecipeGroup(RecipeGroupID.IronBar, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.controlJump)
            {
                player.maxFallSpeed /= 5;
                player.noFallDmg = true;
            }
        }
    }
}