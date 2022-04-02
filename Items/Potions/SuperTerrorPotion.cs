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
            Item.value = Item.sellPrice(0, 0, 30, 0);
            Item.consumable = true;
            Item.rare = ItemRarityID.Lime;
            Item.maxStack = 999;
            Item.width = 20;
            Item.height = 26;
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.terrorPotionTerror = terrorAmount;
        }

        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ModContent.ItemType<GreaterTerrorPotion>(), 2)
                .AddIngredient(ModContent.ItemType<Materials.SoulOfPlight>(), 1)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}


