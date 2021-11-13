using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories.Shields
{
    [AutoloadEquip(EquipType.Shield)]
    class BronzeBuckler : ModItem
    {
        int cooldown = 5 * 60;
        float knockback = 8f;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(TBUtils.Accessories.GetParryShieldString(cooldown, knockback));
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = 1;
            item.defense = 4;
            item.knockBack = knockback;
            item.value = Item.sellPrice(0, 3, 0, 0);
            TerrorbornItem.modItem(item).parryShield = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.parryColor = Color.SaddleBrown;
            TBUtils.Accessories.UpdateParryShield(cooldown, item, player);
        }
    }
}

