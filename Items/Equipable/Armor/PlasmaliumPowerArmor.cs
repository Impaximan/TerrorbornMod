using Terraria;
using System;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class PlasmaliumPowerVisor : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.PlasmaliumBar>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("15% increased damage" +
                "\nIncreased movement speed");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.allDamage *= 1.15f;
            player.moveSpeed += 0.04f;
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.Cyan;
            item.defense = 15;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PlasmaliumPowerBreastplate>() && legs.type == ModContent.ItemType<PlasmaliumPowerGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Not getting hit over time increases your damage, up to a max of 30%" +
                "\nThis bonus is reset upon being hit";

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.PlasmaPower += 1f / 60f;
            if (modPlayer.PlasmaPower >= 30)
            {
                modPlayer.PlasmaPower = 30;
            }
            player.allDamage *= 1f + (modPlayer.PlasmaPower / 100f);
        }
    }

    [AutoloadEquip(EquipType.Head)]
    public class PlasmaliumPowerHelmet : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.PlasmaliumBar>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("+50 max life" +
                "\n5% increased damage");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.statLifeMax2 += 50;
            player.allDamage *= 1.05f;
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 26;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.Cyan;
            item.defense = 22;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PlasmaliumPowerBreastplate>() && legs.type == ModContent.ItemType<PlasmaliumPowerGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Not getting hit over time increases your max life, up to a max of 100" +
                "\nThis bonus is reset upon being hit" +
                "\nIncreased life regen";

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.PlasmaPower += 1f / 60f;
            if (modPlayer.PlasmaPower >= 30)
            {
                modPlayer.PlasmaPower = 30;
            }
            player.statLifeMax2 += (int)(100f * (modPlayer.PlasmaPower / 30));
            player.lifeRegen += 4;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class PlasmaliumPowerBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("7% increased item use speed" +
                "\n8% increased damage" +
                "\nIncreases flight time by 50%" +
                "\n+35 max life");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.allUseSpeed *= 1.07f;
            player.allDamage *= 1.08f;
            modPlayer.flightTimeMultiplier *= 1.5f;
            player.statLifeMax2 += 35;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.PlasmaliumBar>(), 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.Cyan;
            item.defense = 20;
        }

        public override bool DrawBody()
        {
            return false;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class PlasmaliumPowerGreaves : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.PlasmaliumBar>(), 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("10% increased critical strike chance" +
                "\n8% increased item use speed");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.allUseSpeed *= 1.08f;
            player.thrownCrit += 10;
            player.rangedCrit += 10;
            player.magicCrit += 10;
            player.meleeCrit += 10;
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 18;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.Cyan;
            item.defense = 14;
        }
    }
}
