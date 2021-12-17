using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Potions
{
    public class StarpowerPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("40 increased max mana");
        }
        public override void SetDefaults()
        {
            item.useTime = 28;
            item.useAnimation = 32;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.maxStack = 30;
            item.consumable = true;
            item.rare = ItemRarityID.Blue;
            item.autoReuse = false;
            item.UseSound = SoundID.Item3;
            item.useTurn = true;
            item.maxStack = 30;
            item.buffType = ModContent.BuffType<Buffs.Starpower>();
            item.buffTime = 18000;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(ItemID.FallenStar, 3);
            //recipe1.AddIngredient(ModContent.ItemType<Materials.NovagoldOre>(), 2);
            recipe1.AddIngredient(ItemID.BottledWater);
            recipe1.AddTile(TileID.Bottles);
            recipe1.SetResult(this);
            recipe1.AddRecipe();
        }
    }
}

