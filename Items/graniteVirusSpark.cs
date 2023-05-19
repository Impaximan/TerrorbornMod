using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items
{
    class GraniteVirusSpark : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Granite, 65)
                .AddIngredient(ItemID.Wire, 175)
                .AddIngredient(ItemID.Actuator, 50)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Granite Virus");
            Tooltip.SetDefault("Turns you into a precise granite spark for 5 seconds\n" +
                "While transformed, you can't use items and you'll take 50% more damage\n" +
                "You can also press the 'Quick Spark' hotkey to transform as long as the item is in your inventory\n" +
                "30 second cooldown");
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Orange;
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.value = Item.sellPrice(0, 5, 0, 0);
        }

        public override bool? UseItem(Player player)
        {
            astralSparkData.Transform(player);
            return base.UseItem(player);
        }
    }
}
