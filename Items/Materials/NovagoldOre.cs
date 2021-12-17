using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace TerrorbornMod.Items.Materials
{
    public class NovagoldOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Gold created by a powerful supernova, that somehow managed to make its way here");
            ItemID.Sets.ExtractinatorMode[item.type] = item.type;
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 18;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.createTile = ModContent.TileType<Tiles.Novagold>();
            item.value = Item.sellPrice(0, 0, 4, 0);
            item.rare = ItemRarityID.Blue;
        }
    }
}