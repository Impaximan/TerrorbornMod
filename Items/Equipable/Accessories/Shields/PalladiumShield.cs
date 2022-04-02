using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories.Shields
{
    [AutoloadEquip(EquipType.Shield)]
    class PalladiumShield : ModItem
    {
        int cooldown = 4 * 60;
        float knockback = 5f;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(TBUtils.Accessories.GetParryShieldString(cooldown, knockback) + "\nParrying attacks will also temporarily grant you increased life regen");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.defense = 5;
            Item.knockBack = knockback;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            TerrorbornItem.modItem(Item).parryShield = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PalladiumBar, 12)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.parryColor = new Color(255, 89, 41);
            if (modPlayer.JustParried)
            {
                player.AddBuff(BuffID.RapidHealing, 60 * 6);
            }
            TBUtils.Accessories.UpdateParryShield(cooldown, Item, player);
        }
    }
}

