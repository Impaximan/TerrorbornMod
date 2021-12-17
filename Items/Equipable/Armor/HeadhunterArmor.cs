using Terraria;
using System;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class HeadhunterHelmet : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 8);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(ItemID.GlowingMushroom, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("25% chance to not consume ammo" +
                "\n15% increased ranged critical strike chance" +
                "\n10% increased ranged damage");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.rangedCrit += 15;
            modPlayer.noAmmoConsumeChance += 0.25f;
            player.rangedDamage += 0.1f;
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = ItemRarityID.Yellow;
            item.defense = 20;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<HeadhunterBreastplate>() && legs.type == ModContent.ItemType<HeadhunterGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Attacking enemies gradually increases your critical strike chance" +
                            "\nOnce this bonus reaches 30%, it will reset and grant you the 'Headhunter's Frenzy' buff" +
                            "\nHeadhunter's Frenzy increases your movement and item use speed, as well as your critical hit damage" +
                            "\nEntering a frenzy will also heal you";
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.HeadHunter = true;
            modPlayer.HeadhunterClass = 2;

            player.magicCrit += modPlayer.HeadHunterCritBonus;
            player.rangedCrit += modPlayer.HeadHunterCritBonus;
            player.thrownCrit += modPlayer.HeadHunterCritBonus;
            player.meleeCrit += modPlayer.HeadHunterCritBonus;
        }
    }

    [AutoloadEquip(EquipType.Head)]
    public class HeadhunterCrown : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 8);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(ItemID.BeetleHusk, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Melee speed increased by 15%" +
                "\n15% increased melee critical strike chance" +
                "\n10% increased melee damage");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.meleeCrit += 15;
            player.meleeSpeed += 0.15f;
            player.meleeSpeed += 0.1f;
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 26;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = ItemRarityID.Yellow;
            item.defense = 28;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<HeadhunterBreastplate>() && legs.type == ModContent.ItemType<HeadhunterGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Attacking enemies gradually increases your critical strike chance" +
                            "\nOnce this bonus reaches 30%, it will reset and grant you the 'Headhunter's Frenzy' buff" +
                            "\nHeadhunter's Frenzy increases your movement and item use speed, as well as your critical hit damage" +
                            "\nEntering a frenzy will also heal you";
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.HeadHunter = true;
            modPlayer.HeadhunterClass = 1;

            player.magicCrit += modPlayer.HeadHunterCritBonus;
            player.rangedCrit += modPlayer.HeadHunterCritBonus;
            player.thrownCrit += modPlayer.HeadHunterCritBonus;
            player.meleeCrit += modPlayer.HeadHunterCritBonus;
        }
    }

    [AutoloadEquip(EquipType.Head)]
    public class HeadhunterHood : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 8);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(ItemID.Ectoplasm, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Max mana increased by 100" +
                "\n15% increased magic critical strike chance" +
                "\n10% increased magic damage");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.magicCrit += 15;
            player.statManaMax2 += 100;
            player.magicDamage += 0.1f;
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 22;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = ItemRarityID.Yellow;
            item.defense = 12;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<HeadhunterBreastplate>() && legs.type == ModContent.ItemType<HeadhunterGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Attacking enemies gradually increases your critical strike chance" +
                            "\nOnce this bonus reaches 30%, it will reset and grant you the 'Headhunter's Frenzy' buff" +
                            "\nHeadhunter's Frenzy increases your movement and item use speed, as well as your critical hit damage" +
                            "\nEntering a frenzy will also heal you";
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.HeadHunter = true;
            modPlayer.HeadhunterClass = 0;

            player.magicCrit += modPlayer.HeadHunterCritBonus;
            player.rangedCrit += modPlayer.HeadHunterCritBonus;
            player.thrownCrit += modPlayer.HeadHunterCritBonus;
            player.meleeCrit += modPlayer.HeadHunterCritBonus;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class HeadhunterBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases item/weapon use speed by 12%" +
                "\nIncreases flight time by 50%" +
                "\n50% increased Shriek of Horror range");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.allUseSpeed += 0.12f;
            modPlayer.flightTimeMultiplier *= 1.5f;
            modPlayer.ShriekRangeMultiplier *= 1.5f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 12);
            recipe.AddIngredient(ItemID.SoulofFlight, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = ItemRarityID.Yellow;
            item.defense = 18;
        }

        public override bool DrawBody()
        {
            return false;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class HeadhunterGreaves : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 8);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increased maximum movement speed" +
                "\n8% increased damage");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.allDamage += 0.08f;
            player.accRunSpeed *= 1.25f;
            player.maxRunSpeed *= 1.25f;
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = ItemRarityID.Yellow;
            item.defense = 12;
        }
    }

    public class HeadhunterFrenzy : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Headhunter's Frenzy");
            Description.SetDefault("You have been sent into a state of chaos!");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.moveSpeed += 0.5f;
            modPlayer.allUseSpeed *= 1.3f;
            modPlayer.critDamage *= 1.3f;
        }
    }
}
