using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class AzuriteBrooch : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.AzuriteBar>(), 8)
                .AddIngredient(ItemID.Diamond, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Gives hitting enemies a chance to create a bouncing azurite shard that does" +
                "\na fraction of the damage the original attack dealt"); */
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Green;
            Item.defense = 2;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.AzuriteBrooch = true;
        }
    }
}