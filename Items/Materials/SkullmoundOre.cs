using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class SkullmoundOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("'You mind screams as you touch it'");
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.Incendiary.Skullmound>();
        }
    }
}


