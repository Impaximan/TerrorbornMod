using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class TacticalCommlink : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hitting enemies with ranged weapons has a chance to cause a rocket to fall from above" +
                "\n10% increased ranged damage");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.noMelee = true;
            item.rare = ItemRarityID.Yellow;
            item.value = Item.sellPrice(0, 8, 0, 0);
            item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.TacticalCommlink = true;
            player.rangedDamage += 0.1f;
        }
    }
}
