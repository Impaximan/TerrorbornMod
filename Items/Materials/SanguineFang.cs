using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class SanguineFang : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sanguine fang");
            Tooltip.SetDefault("The fang of a vampiric foe");
        }
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 15, 0);
            Item.rare = ItemRarityID.Green;
            Item.width = 28;
            Item.height = 24;
        }
    }
}
