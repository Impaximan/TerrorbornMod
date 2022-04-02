using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TerrorbornMod.Items.Materials;

namespace TerrorbornMod.Items.Potions
{
    public class SinducementPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Killing enemies causes them to explode into blood and turn into a soul that spins around your cursor");
        }
        public override void SetDefaults()
        {
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ItemRarityID.LightRed;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item3;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.buffType = ModContent.BuffType<Buffs.Sinducement>();
            Item.buffTime = 3600 * 4;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BottledWater)
                .AddIngredient(ModContent.ItemType<PyroclasticGemstone>(), 5)
                .AddIngredient(ItemID.AshBlock, 10)
                .AddIngredient(ItemID.Fireblossom)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}

