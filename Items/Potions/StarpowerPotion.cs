using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Potions
{
    public class StarpowerPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("40 increased max mana");
        }
        public override void SetDefaults()
        {
            Item.useTime = 28;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item3;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.buffType = ModContent.BuffType<Buffs.Starpower>();
            Item.buffTime = 18000;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FallenStar, 3)
                .AddIngredient(ItemID.BottledWater)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}

