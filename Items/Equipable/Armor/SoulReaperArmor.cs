using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SoulReaperMask : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 15)
                .AddIngredient(ModContent.ItemType<Items.Materials.NoxiousScale>(), 10)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("+10 max life" +
                "\n10% increased restless weapon damage" +
                "\n5% increase to all damage");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 24;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.defense = 12;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<SoulReaperBreastplate>() && legs.type == ModContent.ItemType<FusionLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Killing enemies or taking away 7.5% of a boss's health causes them to Drop a thunder soul" +
                "\nPicking up a thunder soul grants you the Soul Maniac buff for 5 seconds, increasing restless weapon stats" +
                "\n10% increased critical strike chance" +
                "\n+10 max life";
            player.statLifeMax2 += 10;
            player.GetCritChance(DamageClass.Generic) += 10;

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.SoulReaperArmorBonus = true;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.statLifeMax2 += 10;
            modPlayer.restlessDamage += 0.10f;
            player.GetDamage(DamageClass.Generic) *= 1.05f;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class SoulReaperBreastplate : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 24)
                .AddIngredient(ModContent.ItemType<Items.Materials.NoxiousScale>(), 16)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("+20 max life" +
                "\n30% increased restless weapon use speed while not fully charged" +
                "\n5% increase to all damage");
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.statLifeMax2 += 20;
            modPlayer.restlessNonChargedUseSpeed *= 1.3f;
            player.GetDamage(DamageClass.Generic) *= 1.05f;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class SoulReaperGreaves : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 12)
                .AddIngredient(ModContent.ItemType<Items.Materials.NoxiousScale>(), 8)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("+10 max life" +
                "\n10% increased restless weapon use speed while fully charged" +
                "\n5% increase to all damage");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.statLifeMax2 += 10;
            modPlayer.restlessChargedUseSpeed *= 1.1f;
            player.GetDamage(DamageClass.Generic) *= 1.05f;
        }
    }

    class ThunderSoul : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
        }

        public override bool OnPickup(Player player)
        {
            player.AddBuff(ModContent.BuffType<Buffs.SoulManiac>(), 60 * 5);
            SoundExtensions.PlaySoundOld(SoundID.Item4, player.Center);
            CombatText.NewText(player.getRect(), Color.FromNonPremultiplied(108, 150, 143, 255), "Soul Maniac!");
            return false;
        }
    }
}
