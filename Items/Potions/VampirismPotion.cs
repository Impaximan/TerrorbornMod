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
            Item.useTime = 32;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ItemRarityID.Green;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item3;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.buffType = ModContent.BuffType<Buffs.Vampirism>();
            Item.buffTime = 3600 * 5;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.VilePowder, 10)
                .AddIngredient(ModContent.ItemType<Materials.SanguineFang>(), 3)
                .AddIngredient(ItemID.BottledWater)
                .AddTile(TileID.Bottles)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.ViciousPowder, 10)
                .AddIngredient(ModContent.ItemType<Materials.SanguineFang>(), 3)
                .AddIngredient(ItemID.BottledWater)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}
