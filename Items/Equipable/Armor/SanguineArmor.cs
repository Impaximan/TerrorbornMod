using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SanguineHood : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.SanguineFang>(), 6)
                .AddIngredient(ItemID.SoulofNight, 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases item/weapon use speed by 4%" +
                "\nIncreases critical strike chance by 5%" +
                "\nIncreases shriek of horror's use speed");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetAttackSpeed(DamageClass.Generic) += 0.04f;
            player.GetCritChance(DamageClass.Generic) += 5;
            modPlayer.ShriekSpeed *= 0.7f;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Pink;
            Item.defense = 11;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<SanguineBreastplate>() && legs.type == ModContent.ItemType<SanguineGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "The healing orbs from necromantic curse return to you much faster" +
                "\nThe Projectile from necromantic curse homes into enemies and can hit multiple enemies";
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.SanguineSetBonus = true;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class SanguineBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases item/weapon use speed by 6%" +
                "\nIncreases critical strike chance by 4%" +
                "\nIncreases the amount of terror obtained from Shriek of horror by 30%" +
                "\nIncrease armor penetration by 12");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetAttackSpeed(DamageClass.Generic) += 0.06f;
            modPlayer.ShriekTerrorMultiplier *= 1.3f;
            player.GetCritChance(DamageClass.Generic) += 4;
            player.GetArmorPenetration(DamageClass.Generic) += 12;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.SanguineFang>(), 6)
                .AddIngredient(ItemID.SoulofNight, 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 25;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Pink;
            Item.defense = 19;
        }
    }
    [AutoloadEquip(EquipType.Legs)]
    public class SanguineGreaves : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.SanguineFang>(), 6)
                .AddIngredient(ItemID.SoulofNight, 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases item/weapon use speed by 4%" +
                "\nIncreases critical strike chance by 4%" +
                "\nIncreases life regen");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetAttackSpeed(DamageClass.Generic) += 0.04f;
            player.lifeRegen += 2;
            player.GetCritChance(DamageClass.Generic) += 4;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 12;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Pink;
            Item.defense = 8;
        }
    }
}