using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Placeable.Furniture
{
    class IncendiaryRitual : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Incendiary Ritual");
            Tooltip.SetDefault("Greatly increases spawnrates while nearby");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 48;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.rare = ItemRarityID.LightRed;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.createTile = ModContent.TileType<Tiles.Incendiary.IncendiaryRitual>();
        }
    }
}