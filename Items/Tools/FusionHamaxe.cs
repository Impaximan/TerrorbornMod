using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Tools
{
    class FusionHamaxe : ModItem
    {
        float terrorRequired = 0.25f;

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Requires " + terrorRequired + "% terror per use" +
                "\nCuts faster than the other lunar hamaxes"); */
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.FusionFragment>(), 14)
                .AddIngredient(ItemID.LunarBar, 12)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 1)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.DamageType = DamageClass.Melee;
            Item.width = 48;
            Item.height = 52;
            Item.useAnimation = 10;
            Item.useTime = 3;
            Item.hammer = 100;
            Item.axe = 150 / 5;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5.5f;
            Item.rare = ItemRarityID.Red;
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



