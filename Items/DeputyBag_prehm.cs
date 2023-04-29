using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items
{
    class DeputyBag_prehm : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Deputy's bag");
            /* Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}" +
                "\nContains useful pre-hardmode loot to help you along your journey"); */
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 20;
            Item.height = 32;
            Item.rare = ItemRarityID.Green;
        }

        public override bool CanRightClick()
        {
            return true;
        }
        
        public override void RightClick(Player player)
        {
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.LesserHealingPotion, Main.rand.Next(11, 19));
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.HealingPotion, Main.rand.Next(8, 12));
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.WoodenArrow, Main.rand.Next(250, 501));
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.MusketBall, Main.rand.Next(150, 301));
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.SilverBullet, Main.rand.Next(50, 150));
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.ManaPotion, Main.rand.Next(15, 26));

            //ores
            if (Main.rand.NextBool(2))
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.CopperOre, Main.rand.Next(35, 60));
            }
            else
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.TinOre, Main.rand.Next(35, 60));
            }
            if (Main.rand.NextBool(2))
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.IronOre, Main.rand.Next(35, 60));
            }
            else
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.LeadOre, Main.rand.Next(35, 60));
            }
            if (Main.rand.NextBool(2))
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.SilverOre, Main.rand.Next(35, 60));
            }
            else
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.TungstenOre, Main.rand.Next(35, 60));
            }
            if (Main.rand.NextBool(2))
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.GoldOre, Main.rand.Next(35, 60));
            }
            else
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.PlatinumOre, Main.rand.Next(35, 60));
            }

            if (WorldGen.crimson)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.DemoniteOre, Main.rand.Next(35, 60));
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.ShadowScale, Main.rand.Next(5, 11));
                if (Main.rand.NextBool(4))
                {
                    int choice = Main.rand.Next(5);
                    if (choice == 0)
                    {
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.Musket);
                    }
                    if (choice == 1)
                    {
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.ShadowOrb);
                    }
                    if (choice == 2)
                    {
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.Vilethorn);
                    }
                    if (choice == 3)
                    {
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.BallOHurt);
                    }
                    if (choice == 4)
                    {
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.BandofStarpower);
                    }
                }
            }
            else
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.CrimtaneOre, Main.rand.Next(35, 60));
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.TissueSample, Main.rand.Next(5, 11));
                if (Main.rand.NextBool(4))
                {
                    int choice = Main.rand.Next(5);
                    if (choice == 0)
                    {
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.TheUndertaker);
                    }
                    if (choice == 1)
                    {
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.CrimsonHeart);
                    }
                    if (choice == 2)
                    {
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.PanicNecklace);
                    }
                    if (choice == 3)
                    {
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.CrimsonRod);
                    }
                    if (choice == 4)
                    {
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.TheRottedFork);
                    }
                }
            }
        }
    }
}
