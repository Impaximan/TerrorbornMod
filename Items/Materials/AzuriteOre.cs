using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    public class AzuriteOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("'Water condensed by the constantly shifting world, merged into the other minerals'");
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
            Item.createTile = ModContent.TileType<Tiles.Azurite>();
            Item.value = Item.sellPrice(0, 0, 8, 0);
        }
    }
}
