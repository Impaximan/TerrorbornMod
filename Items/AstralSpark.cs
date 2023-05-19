using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items
{
    class AstralSpark : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GraniteVirusSpark>())
                .AddIngredient(ModContent.ItemType<Materials.NoxiousScale>(), 15)
                .AddIngredient(ModContent.ItemType<Materials.ThunderShard>(), 25)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Astral Spark");
            Tooltip.SetDefault("Turns you into a precise astral spark for 5 seconds" +
                "\nWhile transformed, you can't use items" +
                "\nYou can also press the 'Quick Spark' hotkey to transform as long as the item is in your inventory" +
                "\nWhile transformed, hold JUMP to move at insanely fast speed" +
                "\n20 second cooldown");
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Pink;
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.value = Item.sellPrice(0, 5, 0, 0);
        }

        public override bool CanUseItem(Player player)
        {
            astralSparkData.Transform(player);
            return base.CanUseItem(player);
        }
    }
}

