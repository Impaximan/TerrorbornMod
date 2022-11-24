using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Placeable.Walls
{
    public class DeimostoneWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Stone infused with terror after many years of intense exposure");
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
            Item.useTime = 7;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createWall = ModContent.WallType<Tiles.DeimostoneWallTile>();
        }
    }
}


