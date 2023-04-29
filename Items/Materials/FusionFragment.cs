using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class FusionFragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("'The resentment of an aging star shrieks from this fragment'");
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 1, 5, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.width = 22;
            Item.height = 22;
        }
    }
}

