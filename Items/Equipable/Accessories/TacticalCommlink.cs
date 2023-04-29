using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class TacticalCommlink : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Hitting enemies with ranged weapons has a chance to cause a rocket to fall from above" +
                "\n10% increased ranged damage"); */
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.TacticalCommlink = true;
            player.GetDamage(DamageClass.Ranged) *= 1.1f;
        }
    }
}
