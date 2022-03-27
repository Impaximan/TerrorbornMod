using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.MiscConsumables
{
    class MetalKey : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Consumable" +
                "\nGain access to a rare lavender chest that contains powerful loot");
        }

        public override void SetDefaults()
        {
            item.rare = ItemRarityID.Blue;
            item.maxStack = 1;
        }
    }
}

