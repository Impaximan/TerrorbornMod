using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class IncendiusAlloy : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Incendiary Alloy");
            /* Tooltip.SetDefault("A hellish metal created by a hellish curse" +
                "\nSeek the island in the center of the biome to craft with it"); */
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.Incendiary.IncendiaryAlloyTile>();
        }
    }
}

