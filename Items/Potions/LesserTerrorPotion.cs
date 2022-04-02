using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Potions
{
    public class LesserTerrorPotion : ModItem
    {
        float terrorAmount = 2f;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Tap the Shriek of Horror hotkey while in your inventory to consume" +
                "\nCauses you to gain " + terrorAmount.ToString() + "% terror per nearby enemy when consumed" +
                "\nHas a default cooldown of 10 seconds");
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(0, 0, 0, 60);
            Item.consumable = true;
            Item.rare = ItemRarityID.White;
            Item.maxStack = 999;
            Item.width = 20;
            Item.height = 26;
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.terrorPotionTerror = terrorAmount;
        }

        public override void AddRecipes()
        {
            CreateRecipe(3)
                .AddIngredient(ModContent.ItemType<Materials.TerrorSample>())
                .AddIngredient(ItemID.Bottle, 3)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}