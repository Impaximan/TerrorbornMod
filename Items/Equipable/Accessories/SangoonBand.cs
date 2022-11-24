using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class SangoonBand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sangoon Band");
            Tooltip.SetDefault("Increases life regen" +
                "\nDealing crits heals you for 1 hp");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 34;
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.useAnimation = 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.SanguineFang>(10)
                .AddIngredient(ItemID.TissueSample, 5)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe()
                .AddIngredient<Materials.SanguineFang>(10)
                .AddIngredient(ItemID.ShadowScale, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.lifeRegen += 3;
            modPlayer.SangoonBand = true;
        }
    }
}
