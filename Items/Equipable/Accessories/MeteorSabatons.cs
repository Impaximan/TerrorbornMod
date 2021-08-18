using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class MeteorSabatons : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteorite Sabatons");
            Tooltip.SetDefault("Hold DOWN to fall faster");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 24;
            item.accessory = true;
            item.noMelee = true;
            item.rare = 1;
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.useAnimation = 5;
            item.defense = 5;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MeteoriteBar, 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
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