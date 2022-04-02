using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class PyroclasticGemstone : ModItem
    {
        public override void SetStaticDefaults()
        {

        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(0, 0, 15, 0);
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.Incendiary.PyroclasticGemstone>();
        }
    }
}
