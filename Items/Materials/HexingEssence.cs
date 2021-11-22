using Terraria.ID;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class HexingEssence : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'The essence of a hexed machine'");
            ItemID.Sets.ItemNoGravity[item.type] = true;
            ItemID.Sets.ItemIconPulse[item.type] = true;
        }
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.value = Item.sellPrice(0, 0, 75, 0);
            item.rare = 5;
            item.width = 48;
            item.height = 42;
        }
    }
}
