using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class ShellFragments : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 15, 0);
            Item.width = 22;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
        }
    }
}
