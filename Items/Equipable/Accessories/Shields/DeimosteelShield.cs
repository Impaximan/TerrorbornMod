using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories.Shields
{
    [AutoloadEquip(EquipType.Shield)]
    class DeimosteelShield : ModItem
    {
        int cooldown = 5 * 60;
        float knockback = 7.5f;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(TBUtils.Accessories.GetParryShieldString(cooldown, knockback) + "\nParrying attacks will also grant you 20% terror");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = 1;
            item.defense = 3;
            item.knockBack = knockback;
            item.value = Item.sellPrice(0, 3, 0, 0);
            TerrorbornItem.modItem(item).parryShield = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.DeimosteelBar>(), 7);
            recipe.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.parryColor = new Color(138, 155, 152);
            if (modPlayer.JustParried)
            {
                modPlayer.GainTerror(20f, false);
            }
            TBUtils.Accessories.UpdateParryShield(cooldown, item, player);
        }
    }
}
