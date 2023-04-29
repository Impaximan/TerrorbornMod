using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TerrorbornMod.Items.PermanentUpgrades
{
    class MidnightFruit : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Permanently increases your max mana by 5" +
                "\nCan only be used up to 20 times"); */
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Lime;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.maxStack = 9999;
            Item.UseSound = SoundID.Item29;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }
        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.MidnightFruit < 20)
            {
                modPlayer.MidnightFruit++;
                player.ManaEffect(5);
                player.statMana += 5;
                Item.stack--;
                return true;
            }
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            tooltips.Add(new TooltipLine(Mod, "MidnightFruitCount", "Consumed " + modPlayer.MidnightFruit + "/20 available fruit"));
            tooltips.FirstOrDefault(x => x.Name == "MidnightFruitCount" && x.Mod == "TerrorbornMod").OverrideColor = Color.LimeGreen;
        }
    }
}

