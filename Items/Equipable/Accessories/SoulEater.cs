using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class SoulEater : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Causes killing enemies to grant you terror" +
                "\nYou will also gain terror for every 7.5% of a boss's health");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.noMelee = true;
            item.rare = ItemRarityID.Pink;
            item.defense = 2;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.SoulEater = true;
        }
    }
}
