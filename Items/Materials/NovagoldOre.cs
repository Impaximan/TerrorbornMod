using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    public class NovagoldOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Gold created by a powerful supernova, that somehow managed to make its way here");
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 18;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.Novagold>();
            Item.value = Item.sellPrice(0, 0, 4, 0);
            Item.rare = ItemRarityID.Blue;
        }
    }
}