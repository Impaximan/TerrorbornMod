﻿using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class ThunderShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A fragment of one of the souls released by killing the Wall of Flesh" +
                "\nWhen torn apart, the soul seems to have gained an electric charge");
        }
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Pink;
            Item.width = 18;
            Item.height = 18;
        }
    }
}

