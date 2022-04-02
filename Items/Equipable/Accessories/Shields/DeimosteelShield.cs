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
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 3;
            Item.knockBack = knockback;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            TerrorbornItem.modItem(Item).parryShield = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.DeimosteelBar>(), 7)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.parryColor = new Color(138, 155, 152);
            if (modPlayer.JustParried)
            {
                modPlayer.GainTerror(20f, false);
            }
            TBUtils.Accessories.UpdateParryShield(cooldown, Item, player);
        }
    }
}
