using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class TarOfHunger : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Undying Tar");
            Tooltip.SetDefault("'You think you can feel it moving....'");
        }
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.value = Item.sellPrice(0, 0, 10, 0);
            item.rare = ItemRarityID.Orange;
            item.width = 26;
            item.height = 24;
        }
    }
}
