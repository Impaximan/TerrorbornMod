using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories.Incense
{
    class AmethystIncense : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Amethyst, 2);
            recipe.AddIngredient(ModContent.ItemType<Materials.TerrorSample>());
            recipe.AddIngredient(ItemID.Bottle);
            recipe.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Makes your terror meter purple");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.noMelee = true;
            item.rare = ItemRarityID.Green;
            item.value = Item.sellPrice(gold: 1, silver: 25);
            item.vanity = true;
            TerrorbornItem.modItem(item).meterColor = Color.MediumPurple;
        }
    }
}
