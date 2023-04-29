using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.MiscConsumables
{
    class MetalKey : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Consumable" +
                "\nGain access to a rare lavender chest that contains powerful loot"); */
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 1;
        }
    }
}

