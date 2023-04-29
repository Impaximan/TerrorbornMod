using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class SpecterLocket : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Saves you from death, bringing you to 25 life and granting 4 seconds of damage immunity instead" +
                "\nYour life regen is also increased while you have said damage immunity" +
                "\n3 minute cooldown"); */
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 2;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.SpecterLocket = true;
        }
    }
}

