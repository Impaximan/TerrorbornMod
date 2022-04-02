using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Placeable.Blocks
{
    public class IncendiaryPipe : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A pipe designed to contain things that would otherwise be uncontainable" +
                "\nJust being near it creeps you out");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.Incendiary.IncendiaryPiping>();
        }
    }
}
