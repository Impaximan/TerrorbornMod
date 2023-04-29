using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class HermesQuill : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hermes' Quill");
            /* Tooltip.SetDefault("Allows you to use Shriek of Horror quicker" +
                "\nAllows you to move at 50% speed while using Shriek of Horror, but doing so will hurt you 25% more than usual"); */
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 34;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 5, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ShriekSpeed *= 0.65f;
            modPlayer.ShriekOfHorrorMovement += 0.5f;
            modPlayer.ShriekOfHorrorExtraDamageMultiplier *= 1.25f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HermesFeather>())
                .AddIngredient(ModContent.ItemType<DarkQuill>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
