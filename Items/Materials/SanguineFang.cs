﻿using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class SanguineFang : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sanguine fang");
            Tooltip.SetDefault("The fang of a vampiric foe");
        }
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.value = Item.sellPrice(0, 0, 15, 0);
            item.rare = ItemRarityID.Green;
            item.width = 28;
            item.height = 24;
        }
    }
}
