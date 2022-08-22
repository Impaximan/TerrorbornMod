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
            Tooltip.SetDefault(Utils.Accessories.GetParryShieldString(cooldown, knockback));
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 4;
            Item.knockBack = knockback;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            TerrorbornItem.modItem(Item).parryShield = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.parryColor = Color.SaddleBrown;
            Utils.Accessories.UpdateParryShield(cooldown, Item, player);
        }
    }
}

