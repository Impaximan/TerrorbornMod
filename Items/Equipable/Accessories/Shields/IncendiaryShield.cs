using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories.Shields
{
    class IncendiaryShield : ModItem
    {
        int cooldown = 15 * 60;
        float knockback = 7.5f;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(TBUtils.Accessories.GetParryShieldString(cooldown, knockback) + "\nSuccessful parries rest the cooldown instantly");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = ItemRarityID.LightRed;
            item.defense = 8;
            item.knockBack = knockback;
            item.value = Item.sellPrice(0, 5, 0, 0);
            TerrorbornItem.modItem(item).parryShield = true;
        }

        int dashTime = 0;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.parryColor = Color.Red;
            if (modPlayer.JustParried)
            {
                player.ClearBuff(ModContent.BuffType<Buffs.Debuffs.ParryCooldown>());
                modPlayer.ParryCooldown = 5;
            }
            TBUtils.Accessories.UpdateParryShield(cooldown, item, player);
        }
    }
}


