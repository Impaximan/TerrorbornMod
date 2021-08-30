using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

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
            item.width = 32;
            item.height = 48;
            item.maxStack = 999;
            item.useTurn = true;
            item.rare = 4;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.consumable = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.createTile = ModContent.TileType<Tiles.Incendiary.IncendiaryRitual>();
        }
    }
}