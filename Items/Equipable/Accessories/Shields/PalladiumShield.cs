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
            item.accessory = true;
            item.rare = 3;
            item.defense = 5;
            item.knockBack = knockback;
            item.value = Item.sellPrice(0, 3, 0, 0);
            TerrorbornItem.modItem(item).parryShield = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.parryColor = new Color(255, 89, 41);
            if (modPlayer.JustParried)
            {
                player.AddBuff(BuffID.RapidHealing, 60 * 6);
            }
            TBUtils.Accessories.UpdateParryShield(cooldown, item, player);
        }
    }
}

