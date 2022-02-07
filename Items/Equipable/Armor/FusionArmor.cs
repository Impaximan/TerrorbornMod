using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class FusionHelmet : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.FusionFragment>(), 10);
            recipe.AddIngredient(ItemID.LunarBar, 8);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fusion Mask");
            Tooltip.SetDefault("12% increased damage" +
                "\n5% increased critical strike chance" +
                "\nGreatly increased terror gained from shriek of horror");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 28;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.Red;
            item.defense = 15;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<FusionBreastplate>() && legs.type == ModContent.ItemType<FusionLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Gaining and consuming terror restores health" +
                "\nGenerates terror over time while in combat";

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.FusionArmor = true;
            if (modPlayer.inCombat)
            {
                modPlayer.GainTerror(0.75f, true, true);
            }
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.allDamage += 0.12f;
            player.thrownCrit += 5;
            player.magicCrit += 5;
            player.rangedCrit += 5;
            player.meleeCrit += 5;
            modPlayer.ShriekTerrorMultiplier *= 1.35f;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class FusionBreastplate : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.FusionFragment>(), 20);
            recipe.AddIngredient(ItemID.LunarBar, 16);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 2);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("8% increased damage and critical strike chance" +
                "\n15% increased restless damage" +
                "\nIncreased non-charged restless use speed" +
                "\nIncreased item use speed");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 20;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.Red;
            item.defense = 20;
        }

        public override bool DrawBody()
        {
            return false;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.allDamage += 0.08f;
            player.thrownCrit += 8;
            player.magicCrit += 8;
            player.rangedCrit += 8;
            player.meleeCrit += 8;
            modPlayer.restlessNonChargedUseSpeed *= 1.35f;
            modPlayer.restlessDamage += 0.15f;
            modPlayer.allUseSpeed *= 1.06f;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class FusionLeggings : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.FusionFragment>(), 15);
            recipe.AddIngredient(ItemID.LunarBar, 12);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("10% increased damage and critical strike chance" +
                "\nIncreased shriek of horror speed" +
                "\nIncreased item use speed");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.Red;
            item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.allDamage += 0.1f;
            player.thrownCrit += 10;
            player.magicCrit += 10;
            player.rangedCrit += 10;
            player.meleeCrit += 10;
            modPlayer.ShriekSpeed *= 0.6f;
            modPlayer.allUseSpeed *= 1.06f;
        }
    }
}