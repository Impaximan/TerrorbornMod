using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items
{
    class DeputyBag_hm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Peacekeeper's bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}" +
                "\nContains useful hardmode loot to help you along your journey");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 20;
            Item.height = 32;
            Item.rare = ItemRarityID.LightRed;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.HealingPotion, Main.rand.Next(11, 19));
            player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.GreaterHealingPotion, Main.rand.Next(8, 12));
            player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.GreaterManaPotion, Main.rand.Next(15, 26));

            //ores
            if (Main.rand.Next(2) == 0)
            {
                player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.CobaltOre, Main.rand.Next(35, 60));
            }
            else
            {
                player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.PalladiumOre, Main.rand.Next(35, 60));
            }
            if (Main.rand.Next(2) == 0)
            {
                player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.MythrilOre, Main.rand.Next(35, 60));
            }
            else
            {
                player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.OrichalcumOre, Main.rand.Next(35, 60));
            }
            if (Main.rand.Next(2) == 0)
            {
                player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.TitaniumOre, Main.rand.Next(35, 60));
            }
            else
            {
                player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.AdamantiteOre, Main.rand.Next(35, 60));
            }

            player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.SoulofNight, Main.rand.Next(7, 11));
            player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.SoulofLight, Main.rand.Next(7, 11));
            player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.SoulofFlight, Main.rand.Next(18, 28));
            if (WorldGen.crimson)
            {
                player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.CursedArrow, Main.rand.Next(250, 501));
                player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.CursedBullet, Main.rand.Next(250, 501));
                player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.CursedFlame, Main.rand.Next(5, 11));
                if (Main.rand.Next(4) == 0)
                {
                    int choice = Main.rand.Next(5);
                    if (choice == 0)
                    {
                        player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.DartRifle);
                        player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.CursedDart, 150);
                    }
                    if (choice == 1)
                    {
                        player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.WormHook);
                    }
                    if (choice == 2)
                    {
                        player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.ChainGuillotines);
                    }
                    if (choice == 3)
                    {
                        player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.ClingerStaff);
                    }
                    if (choice == 4)
                    {
                        player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.PutridScent);
                    }
                }
            }
            else
            {
                player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.IchorArrow, Main.rand.Next(250, 501));
                player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.IchorBullet, Main.rand.Next(250, 501));
                player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.Ichor, Main.rand.Next(5, 11));
                if (Main.rand.Next(4) == 0)
                {
                    int choice = Main.rand.Next(5);
                    if (choice == 0)
                    {
                        player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.DartPistol);
                        player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.IchorDart, 150);
                    }
                    if (choice == 1)
                    {
                        player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.TendonHook);
                    }
                    if (choice == 2)
                    {
                        player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.FetidBaghnakhs);
                    }
                    if (choice == 3)
                    {
                        player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.SoulDrain);
                    }
                    if (choice == 4)
                    {
                        player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.FleshKnuckles);
                    }
                }
            }
            if (Main.rand.Next(4) == 0)
            {
                int choice = Main.rand.Next(5);
                if (choice == 0)
                {
                    player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.DaedalusStormbow);
                    player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.HolyArrow, 200);
                }
                if (choice == 1)
                {
                    player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.IlluminantHook);
                }
                if (choice == 2)
                {
                    player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.FlyingKnife);
                }
                if (choice == 3)
                {
                    player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.CrystalVileShard);
                }
                if (choice == 4)
                {
                    player.QuickSpawnItem(player.GetItemSource_OpenItem(Item.type), ItemID.CrystalSerpent);
                }
            }
        }
    }
}
