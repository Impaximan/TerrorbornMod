using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class PlasmaliumPowerVisor : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.PlasmaliumBar>(), 10)
                .AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 4)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("15% increased damage" +
                "\nIncreased movement speed");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetDamage(DamageClass.Generic) *= 1.15f;
            player.moveSpeed += 0.04f;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 15;
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
            player.GetDamage(DamageClass.Generic) *= 1f + (modPlayer.PlasmaPower / 100f);
        }
    }

    [AutoloadEquip(EquipType.Head)]
    public class PlasmaliumPowerHelmet : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.PlasmaliumBar>(), 10)
                .AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 4)
                .AddTile(TileID.MythrilAnvil)
                .Register();
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
            player.GetDamage(DamageClass.Generic) *= 1.05f;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 22;
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
            player.GetDamage(DamageClass.Generic) *= 1.08f;
            modPlayer.flightTimeMultiplier *= 1.5f;
            player.statLifeMax2 += 35;
            ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesBottomSkin[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.PlasmaliumBar>(), 15)
                .AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 7)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 20;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class PlasmaliumPowerGreaves : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.PlasmaliumBar>(), 8)
                .AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 4)
                .AddTile(TileID.MythrilAnvil)
                .Register();
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
            player.GetCritChance(DamageClass.Generic) += 10;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 14;
        }
    }
}
