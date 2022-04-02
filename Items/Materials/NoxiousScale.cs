using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class NoxiousScale : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The scale of a powerful monster, likely one that consumed a large amount of souls at once");
        }
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 25, 0);
            Item.rare = ItemRarityID.Pink;
            Item.width = 18;
            Item.height = 20;
        }
    }
}
