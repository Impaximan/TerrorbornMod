using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.TreasureBags
{
    class HC_TreasureBag : ModItem
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
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Vanity.BossMasks.HexedConstructorMask>());
            }
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Accessories.ConstructorsDestructors>());
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Materials.HexingEssence>(), Main.rand.Next(15, 20));
            int choice = Main.rand.Next(3);
            if (choice == 0)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Weapons.Ranged.Bows.MirageBow>());
            }
            else if (choice == 1)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Weapons.Melee.Swords.IcarusShred>());
            }
            else if (choice == 2)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Weapons.Magic.SongOfTime>());
            }

            int armorChoice = Main.rand.Next(3);
            if (armorChoice == 0)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Armor.HexDefenderMask>());
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Armor.HexDefenderBreastplate>());
            }
            if (armorChoice == 1)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Armor.HexDefenderGreaves>());
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Armor.HexDefenderBreastplate>());
            }
            if (armorChoice == 2)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Armor.HexDefenderMask>());
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Armor.HexDefenderGreaves>());
            }
        }

        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.HexedConstructor.HexedConstructor>();
    }
}