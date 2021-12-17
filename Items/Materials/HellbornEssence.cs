using Terraria.ID;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class HellbornEssence : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'The essence of a horrific creature'");
            ItemID.Sets.ItemNoGravity[item.type] = true;
            ItemID.Sets.ItemIconPulse[item.type] = true;
        }
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.value = Item.sellPrice(0, 1, 5, 0);
            item.rare = ItemRarityID.Yellow;
            item.width = 26;
            item.height = 34;
        }
    }
}
