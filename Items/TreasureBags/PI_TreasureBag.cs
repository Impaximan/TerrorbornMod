using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.TreasureBags
{
    class PI_TreasureBag : ModItem
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
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.Equipable.Vanity.BossMasks.PrototypeIMask>());
            }
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Equipable.Accessories.MidnightPlasmaGlobe>());
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Materials.PlasmaliumBar>(), Main.rand.Next(18, 25));
            int choice = Main.rand.Next(3);
            if (choice == 0)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.PrototypeI.PlasmaScepter>());
            }
            if (choice == 1)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.PrototypeI.PlasmoditeShotgun>());
            }
            if (choice == 2)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Items.PrototypeI.PlasmaticVortex>());
            }
        }

    }
}



