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
            item.maxStack = 999;
            item.consumable = true;
            item.width = 24;
            item.height = 24;
            item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            if (Main.rand.Next(7) == 0)
            {
                player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Vanity.BossMasks.HexedConstructorMask>());
            }
            //player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Accessories.DarkAbdomen>());
            player.QuickSpawnItem(ModContent.ItemType<Items.Materials.HexingEssence>(), Main.rand.Next(15, 20));
            int choice = Main.rand.Next(3);
            if (choice == 0)
            {
                player.QuickSpawnItem(ModContent.ItemType<Items.Weapons.Ranged.MirageBow>());
            }
            else if (choice == 1)
            {
                player.QuickSpawnItem(ModContent.ItemType<Items.Weapons.Melee.IcarusShred>());
            }
            else if (choice == 2)
            {
                player.QuickSpawnItem(ModContent.ItemType<Items.Weapons.Magic.SongOfTime>());
            }

            int armorChoice = Main.rand.Next(3);
            if (armorChoice == 0)
            {
                player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Armor.HexDefenderMask>());
                player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Armor.HexDefenderBreastplate>());
            }
            if (armorChoice == 1)
            {
                player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Armor.HexDefenderGreaves>());
                player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Armor.HexDefenderBreastplate>());
            }
            if (armorChoice == 2)
            {
                player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Armor.HexDefenderMask>());
                player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Armor.HexDefenderGreaves>());
            }
        }

        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.HexedConstructor.HexedConstructor>();
    }
}