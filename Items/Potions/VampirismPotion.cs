using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Potions
{
    public class VampirismPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increased chance to get hearts and dark energy from enemies that are close to you");
        }
        public override void SetDefaults()
        {
            item.useTime = 32;
            item.useAnimation = 26;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.maxStack = 30;
            item.consumable = true;
            item.rare = ItemRarityID.Green;
            item.autoReuse = false;
            item.UseSound = SoundID.Item3;
            item.useTurn = true;
            item.maxStack = 30;
            item.buffType = ModContent.BuffType<Buffs.Vampirism>();
            item.buffTime = 3600 * 5;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(ItemID.VilePowder, 10);
            recipe1.AddIngredient(ModContent.ItemType<Materials.SanguineFang>(), 3);
            recipe1.AddIngredient(ItemID.BottledWater);
            recipe1.AddTile(TileID.Bottles);
            recipe1.SetResult(this);
            recipe1.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.ViciousPowder, 10);
            recipe2.AddIngredient(ModContent.ItemType<Materials.SanguineFang>(), 3);
            recipe2.AddIngredient(ItemID.BottledWater);
            recipe2.AddTile(TileID.Bottles);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
    }
}
