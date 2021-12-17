using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Potions
{
    public class BloodFlowPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increased life regen while moving");
        }
        public override void SetDefaults()
        {
            item.useTime = 16;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.maxStack = 30;
            item.consumable = true;
            item.rare = ItemRarityID.Green;
            item.autoReuse = false;
            item.UseSound = SoundID.Item3;
            item.useTurn = true;
            item.maxStack = 30;
            item.buffType = ModContent.BuffType<Buffs.BloodFlow>();
            item.buffTime = 3600 * 3;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(ItemID.Ruby, 1);
            recipe1.AddIngredient(ModContent.ItemType<Materials.SanguineFang>(), 3);
            recipe1.AddIngredient(ItemID.Deathweed, 1);
            recipe1.AddIngredient(ItemID.BottledWater);
            recipe1.AddTile(TileID.Bottles);
            recipe1.SetResult(this);
            recipe1.AddRecipe();
        }
    }
}