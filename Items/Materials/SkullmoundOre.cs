using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class SkullmoundOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'You mind screams as you touch it'");
        }
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.rare = ItemRarityID.Yellow;
            item.value = Item.sellPrice(0, 0, 20, 0);
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.createTile = ModContent.TileType<Tiles.Incendiary.Skullmound>();
        }
    }
}


