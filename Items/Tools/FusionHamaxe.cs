using Microsoft.Xna.Framework;
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
            Tooltip.SetDefault("Requires " + terrorRequired + "% terror per use" +
                "\nCuts faster than the other lunar hamaxes");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.FusionFragment>(), 14);
            recipe.AddIngredient(ItemID.LunarBar, 12);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetDefaults()
        {
            item.damage = 60;
            item.melee = true;
            item.width = 48;
            item.height = 52;
            item.useAnimation = 10;
            item.useTime = 3;
            item.hammer = 100;
            item.axe = 150 / 5;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 5.5f;
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
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



