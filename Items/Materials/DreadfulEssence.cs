using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class DreadfulEssence : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("'The essence of an angel'");
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ModContent.RarityType<Rarities.Golden>();
            Item.width = 48;
            Item.height = 42;
        }
    }
}