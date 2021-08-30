using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace TerrorbornMod.Items.Placeable.Blocks
{
    public class KindlingSoilBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("If you listen closely, you can hear... wispers?");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.rare = 4;
            item.value = Item.sellPrice(0, 0, 10, 0);
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = ModContent.TileType<Tiles.Incendiary.KindlingSoil>();
        }
    }
}
