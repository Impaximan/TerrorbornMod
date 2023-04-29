using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class TarOfHunger : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Undying Tar");
            // Tooltip.SetDefault("'You think you can feel it moving....'");
        }
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Orange;
            Item.width = 26;
            Item.height = 24;
        }
    }
}
