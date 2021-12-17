using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

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
            item.width = 24;
            item.height = 24;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.rare = ItemRarityID.LightRed;
            item.value = Item.sellPrice(0, 0, 10, 0);
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.createTile = ModContent.TileType<Tiles.Incendiary.IncendiaryPiping>();
        }
    }
}
