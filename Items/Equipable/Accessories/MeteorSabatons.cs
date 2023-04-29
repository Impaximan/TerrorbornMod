using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class MeteorSabatons : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Meteorite Sabatons");
            // Tooltip.SetDefault("Hold DOWN to fall faster");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.useAnimation = 5;
            Item.defense = 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MeteoriteBar, 12)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.controlDown)
            {
                player.maxFallSpeed *= 2;
                player.velocity.Y += 0.5f;
            }
        }
    }
}