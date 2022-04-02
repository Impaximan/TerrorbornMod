using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Placeable.Blocks
{
    public class SmoothDeimostoneBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
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
            Item.createTile = ModContent.TileType<Tiles.SmoothDeimostone>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<DeimostoneBlock>())
                .AddTile(TileID.WorkBenches)
                .Register();
            Recipe recipe2 = CreateRecipe()
                .AddIngredient(this)
                .AddTile(TileID.WorkBenches);
            recipe2.ReplaceResult(ModContent.ItemType<Walls.SmoothDeimostoneWall>(), 4);
            recipe2.Register();
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Walls.SmoothDeimostoneWall>(), 4)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}


