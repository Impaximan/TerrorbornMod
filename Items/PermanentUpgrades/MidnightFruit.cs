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
            Tooltip.SetDefault("Permanently increases your max mana by 5" +
                "\nCan only be used up to 20 times");
        }
        public override void SetDefaults()
        {
            item.rare = ItemRarityID.Lime;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.consumable = false;
            item.useAnimation = 30;
            item.useTime = 30;
            item.maxStack = 999;
            item.UseSound = SoundID.Item29;
            item.value = Item.sellPrice(0, 1, 0, 0);
        }
        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.MidnightFruit < 20)
            {
                modPlayer.MidnightFruit++;
                player.ManaEffect(5);
                player.statMana += 5;
                item.stack--;
                return true;
            }
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.player[item.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            tooltips.Add(new TooltipLine(mod, "MidnightFruitCount", "Consumed " + modPlayer.MidnightFruit + "/20 available fruit"));
            tooltips.FirstOrDefault(x => x.Name == "MidnightFruitCount" && x.mod == "TerrorbornMod").overrideColor = Color.LimeGreen;
        }
    }
}

