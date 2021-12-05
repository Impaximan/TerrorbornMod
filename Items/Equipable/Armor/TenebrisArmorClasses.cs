using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class TenebrisMask : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 18);
            recipe.AddIngredient(ItemID.HallowedBar, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tenebris Helmet");
            Tooltip.SetDefault("6% increased melee damage" +
                "\n16% increased melee critical strike chance" +
                "\n+15 max life");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 5;
            item.defense = 10;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TenebrisChestplate>() && legs.type == ModContent.ItemType<TenebrisLeggings>();
        }

        int blackoutTime = 0;
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Press the <ArmorAbility> mod hotkey during a voidslip to teleport to the cursor" +
                "\nThis teleport has no cost";

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.VoidBlinkTime > 0 && TerrorbornMod.ArmorAbility.JustPressed)
            {
                if (!Collision.SolidCollision(Main.MouseWorld - player.Size / 2, player.width, player.height))
                {
                    blackoutTime = 7;
                    player.position = Main.MouseWorld - player.Size / 2;
                    ModContent.GetSound("TerrorbornMod/Sounds/Effects/undertalewarning").Play(Main.soundVolume, 0.5f, 0f);
                }
            }

            if (blackoutTime > 0)
            {
                blackoutTime--;
                TerrorbornMod.ScreenDarknessAlpha = 1f;
                if (blackoutTime <= 0)
                {
                    TerrorbornMod.ScreenDarknessAlpha = 0f;
                }
            }
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.meleeDamage += 0.06f;
            player.meleeCrit += 16;
            player.statLifeMax2 += 15;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class TenebrisChestplate : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 18);
            recipe.AddIngredient(ItemID.HallowedBar, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("6% increased melee damage" +
                "\n35% terror from Shriek of Horror" +
                "\n+20 max life");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 24;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 5;
            item.defense = 16;
        }

        public override bool DrawBody()
        {
            return false;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.meleeDamage += 0.06f;
            modPlayer.ShriekTerrorMultiplier *= 1.35f;
            player.statLifeMax2 += 20;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class TenebrisLeggings : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 18);
            recipe.AddIngredient(ItemID.HallowedBar, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tenebris Greaves");
            Tooltip.SetDefault("6% increased melee damage" +
                "\nIncreased movement speed" +
                "\n+15 max life");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 12;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 5;
            item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.meleeDamage += 0.06f;
            player.runAcceleration *= 1.3f;
            player.statLifeMax2 += 15;
        }
    }
}


