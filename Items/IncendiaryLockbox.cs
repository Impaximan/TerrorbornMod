using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Terraria.Utilities;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items
{
    class IncendiaryLockbox : ModItem
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
            item.rare = 4;
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

            List<int> souls = new List<int>();
            souls.Add(ItemID.SoulofLight);
            souls.Add(ItemID.SoulofNight);
            souls.Add(ItemID.SoulofFlight);

            List<int> ammosAndThrowables = new List<int>();
            ammosAndThrowables.Add(ItemID.IchorArrow);
            ammosAndThrowables.Add(ItemID.CursedArrow);
            ammosAndThrowables.Add(ItemID.IchorBullet);
            ammosAndThrowables.Add(ItemID.CursedBullet);

            List<int> bars = new List<int>();
            bars.Add(ItemID.CobaltBar);
            bars.Add(ItemID.PalladiumBar);
            bars.Add(ItemID.MythrilBar);
            bars.Add(ItemID.OrichalcumBar);
            bars.Add(ItemID.AdamantiteBar);
            bars.Add(ItemID.TitaniumBar);

            List<int> commonPotions = new List<int>();
            commonPotions.Add(ItemID.SpelunkerPotion);
            commonPotions.Add(ModContent.ItemType<Items.Potions.AerodynamicPotion>());
            commonPotions.Add(ItemID.AmmoReservationPotion);
            commonPotions.Add(ItemID.ObsidianSkinPotion);
            commonPotions.Add(ItemID.ArcheryPotion);
            commonPotions.Add(ItemID.MagicPowerPotion);

            List<int> uncommonPotions = new List<int>();
            commonPotions.Add(ModContent.ItemType<Items.Potions.DarkbloodPotion>());
            uncommonPotions.Add(ItemID.InfernoPotion);
            uncommonPotions.Add(ItemID.LifeforcePotion);
            uncommonPotions.Add(ItemID.WrathPotion);
            uncommonPotions.Add(ItemID.RagePotion);

            player.QuickSpawnItem(Main.rand.Next(mainItems));

            if (Main.rand.NextFloat() <= 0.5f) player.QuickSpawnItem(ModContent.ItemType<Items.Placeable.Furniture.IncendiaryAltarItem>());

            player.QuickSpawnItem(Main.rand.Next(souls), Main.rand.Next(6, 12));

            player.QuickSpawnItem(ModContent.ItemType<Materials.IncendiusAlloy>(), Main.rand.Next(18, 35));

            if (Main.rand.NextFloat() <= 0.25f)
            {
                player.QuickSpawnItem(Main.rand.Next(bossSummons));
            }

            if (Main.rand.NextFloat() <= 0.5f)
            {
                player.QuickSpawnItem(Main.rand.Next(bars), Main.rand.Next(5, 15));
            }

            if (Main.rand.NextFloat() <= 0.5f)
            {
                player.QuickSpawnItem(Main.rand.Next(ammosAndThrowables), Main.rand.Next(125, 255));
            }

            if (Main.rand.NextFloat() <= 0.5f)
            {
                player.QuickSpawnItem(ItemID.GreaterHealingPotion, Main.rand.Next(6, 11));
            }

            if (Main.rand.NextFloat() <= 0.666f)
            {
                player.QuickSpawnItem(Main.rand.Next(commonPotions), Main.rand.Next(2, 5));
            }

            if (Main.rand.NextFloat() <= 0.333f)
            {
                player.QuickSpawnItem(Main.rand.Next(commonPotions), Main.rand.Next(2, 4));
            }

            player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(5, 10));
        }
    }
}



