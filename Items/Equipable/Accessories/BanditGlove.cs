using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class BanditGlove : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bandit's Glove");
            Tooltip.SetDefault("Hitting enemies causes them to be inflicted with stacking damage over time" +
                "\nThe damage over time will be reset after 5 seconds");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.noMelee = true;
            item.rare = ItemRarityID.Green;
            item.defense = 1;
            item.value = Item.sellPrice(0, 2, 50, 0);
            item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.BanditGlove = true;
        }
    }
}
