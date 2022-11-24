using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class GobletOfLifeblood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goblet Of Lifeblood");
            Tooltip.SetDefault("The first hit on an enemy will always be critical" +
                "\nKilling enemies has an increased chance to Drop hearts" +
                "\nDealing crits heals you for 1 hp" + 
                "\nIncreases life regen");
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 36;
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 4, silver: 25);
            Item.useAnimation = 5;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.SanguineFang>(4)
                .AddIngredient(ModContent.ItemType<Items.Equipable.Accessories.SangoonBand>())
                .AddIngredient(ModContent.ItemType<Items.Equipable.Accessories.TheLiesOfNourishment>())
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.lifeRegen += 3;
            modPlayer.SangoonBand = true;
            modPlayer.LiesOfNourishment = true;
        }
    }
}
