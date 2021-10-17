using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class HarpyBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hold JUMP to fall slower");
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
            item.defense = 3;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Feather, 6);
            recipe.AddIngredient(ItemID.Cloud, 20);
            recipe.AddIngredient(ItemID.IronBar, 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.controlJump)
            {
                player.maxFallSpeed /= 5;
                player.noFallDmg = true;
            }
        }
    }
}