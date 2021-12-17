using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Terraria.Utilities;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items
{
    class SkullmoundLockbox : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 30;
            item.height = 32;
            item.rare = ItemRarityID.Yellow;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            List<int> mainItems = new List<int>();
            mainItems.Add(ModContent.ItemType<Items.Equipable.Accessories.Shields.IncendiaryShield>());
            mainItems.Add(ModContent.ItemType<Items.Equipable.Accessories.SpecterLocket>());
            mainItems.Add(ModContent.ItemType<Items.Weapons.Magic.Asphodel>());
            mainItems.Add(ModContent.ItemType<Items.Equipable.Hooks.HellishHook>());

            List<int> bossSummons = new List<int>();
            bossSummons.Add(ItemID.MechanicalEye);
            bossSummons.Add(ItemID.MechanicalSkull);
            bossSummons.Add(ItemID.MechanicalWorm);

            List<int> craftingStations = new List<int>();
            craftingStations.Add(ItemID.MythrilAnvil);
            craftingStations.Add(ItemID.OrichalcumAnvil);
            craftingStations.Add(ItemID.AdamantiteForge);
            craftingStations.Add(ItemID.TitaniumForge);

            List<int> souls = new List<int>();
            souls.Add(ItemID.SoulofFright);
            souls.Add(ItemID.SoulofSight);
            souls.Add(ItemID.SoulofMight);
            souls.Add(ModContent.ItemType<Materials.SoulOfPlight>());

            player.QuickSpawnItem(Main.rand.Next(souls), Main.rand.Next(6, 12));

            player.QuickSpawnItem(ModContent.ItemType<Materials.SkullmoundOre>(), Main.rand.Next(20, 35));
            player.QuickSpawnItem(ModContent.ItemType<Materials.HellbornEssence>(), Main.rand.Next(2, 4));
            player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(15, 25));
        }
    }
}
