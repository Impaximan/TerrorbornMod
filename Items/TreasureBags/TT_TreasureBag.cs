using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.TreasureBags
{
    class TT_TreasureBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            if (Main.rand.NextBool(7))
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Vanity.BossMasks.TidalTitanMask>());
            }
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Accessories.LunarHeart>());
            int choice = Main.rand.Next(3);
            if (choice == 0)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Weapons.Ranged.Jawvelin>(), 750);
            }
            else if (choice == 1)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Weapons.Summons.Whips.AzuretoothWhip>());
            }
            else if (choice == 2)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Weapons.Magic.BubbleBlaster>());
            }
        }

        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.TidalTitan.TidalTitan>();
    }
}

