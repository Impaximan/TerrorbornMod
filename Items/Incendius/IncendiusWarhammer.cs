using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Incendius
{
    class IncendiusWarhammer : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), (int)(35 * TerrorbornMod.IncendiaryAlloyMultiplier))
                .AddRecipeGroup("cobalt", 20)
                .AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>())
                .Register();
        }
        public override void SetDefaults()
        {
            Item.damage = 33;
            Item.DamageType = DamageClass.Melee;
            Item.width = 64;
            Item.height = 64;
            Item.useTime = 4;
            Item.useAnimation = 17;
            Item.hammer = 75;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 11;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }
    }
}
