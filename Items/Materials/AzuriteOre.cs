using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace TerrorbornMod.Items.Materials
{
    public class AzuriteOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'Water condensed by the constantly shifting world, merged into the other minerals'");
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
            item.createTile = mod.TileType("Azurite");
            item.value = Item.sellPrice(0, 0, 8, 0);
        }
    }
}
