using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class ShellFragments : ModItem
    {
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.value = Item.sellPrice(0, 0, 15, 0);
            item.width = 22;
            item.height = 20;
            item.rare = ItemRarityID.Blue;
        }
    }
}
