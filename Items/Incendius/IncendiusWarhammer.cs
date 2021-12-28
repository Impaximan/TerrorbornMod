using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Incendius
{
    class IncendiusWarhammer : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), (int)(35 * TerrorbornMod.IncendiaryAlloyMultiplier));
            recipe.AddRecipeGroup("cobalt", 20);
            recipe.AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetDefaults()
        {
            item.damage = 33;
            item.melee = true;
            item.width = 64;
            item.height = 64;
            item.useTime = 4;
            item.useAnimation = 17;
            item.hammer = 75;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 11;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
    }
}
