using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class FusionHelmet : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.FusionFragment>(), 10)
                .AddIngredient(ItemID.LunarBar, 8)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 1)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
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
            Item.width = 26;
            Item.height = 28;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.defense = 15;
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
            player.GetDamage(DamageClass.Generic) += 0.12f;
            player.GetCritChance(DamageClass.Generic) += 5;
            modPlayer.ShriekTerrorMultiplier *= 1.35f;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class FusionBreastplate : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.FusionFragment>(), 20)
                .AddIngredient(ItemID.LunarBar, 16)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 2)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("8% increased damage and critical strike chance" +
                "\n15% increased restless damage" +
                "\nIncreased non-charged restless use speed" +
                "\nIncreased item use speed");
            ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesBottomSkin[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetDamage(DamageClass.Generic) += 0.08f;
            player.GetCritChance(DamageClass.Generic) += 8;
            modPlayer.restlessNonChargedUseSpeed *= 1.35f;
            modPlayer.restlessDamage += 0.15f;
            player.GetAttackSpeed(DamageClass.Generic) *= 1.06f;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class FusionLeggings : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.FusionFragment>(), 15)
                .AddIngredient(ItemID.LunarBar, 12)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 1)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("10% increased damage and critical strike chance" +
                "\nIncreased shriek of horror speed" +
                "\nIncreased item use speed");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetDamage(DamageClass.Generic) += 0.1f;
            player.GetCritChance(DamageClass.Generic) += 10;
            modPlayer.ShriekSpeed *= 0.6f;
            player.GetAttackSpeed(DamageClass.Generic) *= 1.06f;
        }
    }
}