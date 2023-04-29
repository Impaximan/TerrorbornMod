using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class HexingEssence : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("'The essence of a hexed machine'");
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Pink;
            Item.width = 48;
            Item.height = 42;
        }
    }
}