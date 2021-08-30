using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace TerrorbornMod.Items.Placeable.Furniture
{
    class IncendiaryAltarItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strange Altar");
            Tooltip.SetDefault("Used to craft with numorous hellish materials");
        }
        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 30;
            item.maxStack = 999;
            item.useTurn = true;
            item.rare = 4;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.consumable = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.createTile = ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>();
        }
    }
}


