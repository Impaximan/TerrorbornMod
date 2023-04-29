using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Tools
{
    class DeimosteelAxe : ModItem
    {
        float terrorRequired = 1f;

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Requires " + terrorRequired + "% terror per use" +
                "\nCuts insanely fast"); */
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.DeimosteelBar>(), 8)
                .AddRecipeGroup(RecipeGroupID.Wood, 3)
                .AddIngredient(ModContent.ItemType<Materials.TerrorSample>())
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();
        }

        public override void SetDefaults()
        {
            Item.damage = 4;
            Item.DamageType = DamageClass.Melee;
            Item.width = 64;
            Item.height = 64;
            Item.useAnimation = 15;
            Item.useTime = 5;
            Item.axe = 60 / 5;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.TerrorPercent >= terrorRequired)
            {
                modPlayer.LoseTerror(terrorRequired, false);
                return base.CanUseItem(player);
            }
            return false;
        }
    }
}

