using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 26;
            Item.height = 32;
            Item.rare = ItemRarityID.Orange;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                int type = 0;
                switch (Main.rand.Next(6))
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
                    case 5:
                        type = ModContent.ItemType<Potions.AdrenalinePotion>();
                        break;
                    default:
                        break;
                }

                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), type, Main.rand.Next(2, 4));
            }
        }
    }
}

