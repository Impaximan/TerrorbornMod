using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Potions
{
    public class SuperTerrorPotion : ModItem
    {
        float terrorAmount = 5f;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Tap the Shriek of Horror hotkey while in your inventory to consume" +
                "\nCauses you to gain " + terrorAmount.ToString() + "% terror per nearby enemy when consumed" +
                "\nHas a default cooldown of 10 seconds");
        }

        public override void SetDefaults()
        {
            item.value = Item.sellPrice(0, 0, 30, 0);
            item.consumable = true;
            item.rare = ItemRarityID.Lime;
            item.maxStack = 999;
            item.width = 20;
            item.height = 26;
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.terrorPotionTerror = terrorAmount;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<GreaterTerrorPotion>(), 2);
            recipe.AddIngredient(ModContent.ItemType<Materials.SoulOfPlight>(), 1);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this, 2);
            recipe.AddRecipe();
        }
    }
}


