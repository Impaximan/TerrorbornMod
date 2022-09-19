using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.TreasureBags
{
    class II_TreasureBag : ModItem
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
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Vanity.BossMasks.UnkindledAnekronianMask>());
            }
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.TerrorTonic>());
            switch (Main.rand.Next(3))
            {
                case 0:
                    player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Weapons.Melee.Swords.NighEndSaber>());
                    player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Weapons.Magic.Infectalanche>());
                    break;
                case 1:
                    player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Weapons.Ranged.Thrown.GraveNeedle>());
                    player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Weapons.Magic.Infectalanche>());
                    break;
                case 2:
                    player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Weapons.Ranged.Thrown.GraveNeedle>());
                    player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Weapons.Melee.Swords.NighEndSaber>());
                    break;
            }

            int armorChoice = Main.rand.Next(3);
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Armor.SilentHelmet>());
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Armor.SilentBreastplate>());
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Armor.SilentGreaves>());
        }

        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.InfectedIncarnate.InfectedIncarnate>();
    }
}
