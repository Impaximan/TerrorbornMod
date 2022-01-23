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
                player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Vanity.BossMasks.UnkindledAnekronianMask>());
            }
            player.QuickSpawnItem(ModContent.ItemType<Items.TerrorTonic>());
            switch (Main.rand.Next(2))
            {
                case 0:
                    player.QuickSpawnItem(ModContent.ItemType<Items.Weapons.Melee.NighEndSaber>());
                    break;
                case 1:
                    player.QuickSpawnItem(ModContent.ItemType<Items.Weapons.Magic.Infectalanche>());
                    break;
            }

            int armorChoice = Main.rand.Next(3);
            player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Armor.SilentHelmet>());
            player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Armor.SilentBreastplate>());
            player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Armor.SilentGreaves>());
        }

        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.InfectedIncarnate.InfectedIncarnate>();
    }
}
