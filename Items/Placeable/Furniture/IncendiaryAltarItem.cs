using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
            Item.width = 44;
            Item.height = 30;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.rare = ItemRarityID.LightRed;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.createTile = ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>();
        }
    }
}