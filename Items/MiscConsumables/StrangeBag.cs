using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Terraria.Utilities;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.MiscConsumables
{
    class StrangeBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 26;
            item.height = 32;
            item.rare = 3;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            for (int i = 0; i < Main.rand.Next(2, 3); i++)
            {
                int type = 0;
                switch (Main.rand.Next(5))
                {
                    case 0:
                        type = ModContent.ItemType<Potions.AerodynamicPotion>();
                        break;
                    case 1:
                        type = ModContent.ItemType<Potions.BloodFlowPotion>();
                        break;
                    case 2:
                        type = ModContent.ItemType<Potions.DarkbloodPotion>();
                        break;
                    case 3:
                        type = ModContent.ItemType<Potions.StarpowerPotion>();
                        break;
                    case 4:
                        type = ModContent.ItemType<Potions.VampirismPotion>();
                        break;
                    default:
                        break;
                }

                player.QuickSpawnItem(type, Main.rand.Next(2, 4));
            }
        }
    }
}

