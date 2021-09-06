using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class SpecterLocket : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Saves you from death, bringing you to 25 life and granting 4 seconds of damage immunity instead" +
                "\nYour life regen is also increased while you have said damage immunity" +
                "\n3 minute cooldown");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.noMelee = true;
            item.rare = 4;
            item.defense = 2;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.SpecterLocket = true;
        }
    }
}

