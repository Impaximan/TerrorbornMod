using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.TreasureBags
{
    class SC_TreasureBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Treasure Bag");
            // Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
            ItemID.Sets.BossBag[Type] = true;
            ItemID.Sets.PreHardmodeLikeBossBag[Type] = false;
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

        public override void RightClick(Player player)
        {
            if (Main.rand.NextBool(7))
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Vanity.BossMasks.ShadowcrawlerMask>());
            }
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Accessories.DarkAbdomen>());
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Materials.SoulOfPlight>(), Main.rand.Next(25, 41));
            int choice = Main.rand.Next(3);
            if (choice == 0)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Shadowcrawler.BladeOfShade>());
            }
            else if (choice == 1)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Shadowcrawler.Nightbrood>());
            }
            else if (choice == 2)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Shadowcrawler.BoiledBarrageWand>());
            }
            int armorChoice = Main.rand.Next(3);
            if (armorChoice == 0)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Armor.TenebrisMask>());
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Armor.TenebrisChestplate>());
            }
            if (armorChoice == 1)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Armor.TenebrisLeggings>());
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Armor.TenebrisChestplate>());
            }
            if (armorChoice == 2)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Armor.TenebrisMask>());
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Armor.TenebrisLeggings>());
            }
        }
    }
}


