using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories.Incense
{
    class HeartCrystalIncense : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Heart Incense");
            /* Tooltip.SetDefault("25% increased maximum HP" +
                "\nBeing above your normal max HP causes you to take light damage over time"); */
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 38;
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 0, 80, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.statLifeMax2 += (int)(player.statLifeMax * 0.25f);
            if (player.statLife > player.statLifeMax)
            {
                modPlayer.badLifeRegen = 5;
            }
        }
    }
}


