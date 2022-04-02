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
            Item.useTime = 16;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ItemRarityID.Green;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item3;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.buffType = ModContent.BuffType<Buffs.BloodFlow>();
            Item.buffTime = 3600 * 3;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Ruby, 1)
                .AddIngredient(ModContent.ItemType<Materials.SanguineFang>(), 3)
                .AddIngredient(ItemID.Deathweed, 1)
                .AddIngredient(ItemID.BottledWater)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}