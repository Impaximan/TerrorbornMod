using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace TerrorbornMod.Items.TreasureBags
{
    class DS_TreasureBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Treasure Bag");
            // Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
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
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Vanity.BossMasks.DunestockMask>());
            }

            int item1;
            WeightedRandom<int> itemlist = new WeightedRandom<int>();
            itemlist.Add(ModContent.ItemType<Items.Dunestock.NeedleClawStaff>());
            itemlist.Add(ModContent.ItemType<Items.Dunestock.Dunesting>());
            itemlist.Add(ModContent.ItemType<Items.Dunestock.HungryWhirlwind>());
            item1 = itemlist.Get();
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), item1);
            int item2 = itemlist.Get();
            while (item2 == item1)
            {
                item2 = itemlist.Get();
            }
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), item2);

            itemlist = new WeightedRandom<int>();
            itemlist.Add(ModContent.ItemType<Equipable.Accessories.DryScarf>());
            itemlist.Add(ModContent.ItemType<Equipable.Accessories.AntlionShell>());
            itemlist.Add(ModContent.ItemType<Equipable.Accessories.AntlionClaw>());
            itemlist.Add(ModContent.ItemType<Equipable.Accessories.Wings.AntlionWings>(), 1.5f);
            item1 = itemlist.Get();
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), item1);
            item2 = itemlist.Get();
            while (item2 == item1)
            {
                item2 = itemlist.Get();
            }
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), item2);
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Equipable.Accessories.CloakOfTheWind>());
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<DustwindRod>());
        }

        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.Dunestock>();
    }
}


