using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Potions
{
    public class AerodynamicPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Grants immunity to heavy wind, and increases movement speed" +
                "\nIncreases wing flight time by 100%"); */
        }
        public override void SetDefaults()
        {
            Item.useTime = 20;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item3;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.buffType = ModContent.BuffType<Buffs.aerodynamic>();
            Item.buffTime = 18000;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.AzuriteOre>(3)
                .AddIngredient(ItemID.Blinkroot)
                .AddIngredient(ItemID.Feather, 2)
                .AddIngredient(ItemID.Cloud, 3)
                .AddIngredient(ItemID.BottledWater)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}
