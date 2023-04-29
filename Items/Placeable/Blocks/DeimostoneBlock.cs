using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Placeable.Blocks
{
    public class DeimostoneBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Stone infused with terror after many years of intense exposure");
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
            Item.createTile = ModContent.TileType<Tiles.Deimostone>();
        }

        public override void AddRecipes()
        {
            Recipe wall = CreateRecipe()
                .AddIngredient(this)
                .AddTile(TileID.WorkBenches);
            wall.ReplaceResult(ModContent.ItemType<Walls.DeimostoneWall>(), 4);
            wall.Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Walls.DeimostoneWall>(), 4)
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        public override void ExtractinatorUse(int extractinatorBlockType, ref int resultType, ref int resultStack)
        {
            if (Main.rand.NextBool(5))
            {
                resultType = ModContent.ItemType<DarkEnergy>();
                if (Main.rand.NextBool(5))
                {
                    resultStack = 1;
                }
            }
        }
    }
}

