using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Tools
{
    class FusionPickaxe : ModItem
    {
        float terrorRequired = 0.25f;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Requires " + terrorRequired + "% terror per use" +
                "\nMines faster than the other lunar pickaxes");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.FusionFragment>(), 12);
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetDefaults()
        {
            item.damage = 80;
            item.melee = true;
            item.width = 44;
            item.height = 44;
            item.useAnimation = 6;
            item.useTime = 2;
            item.pick = 225;
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


