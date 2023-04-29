using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class HeadhunterHelmet : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 8)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.GlowingMushroom, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("25% chance to not consume ammo" +
                "\n15% increased ranged critical strike chance" +
                "\n10% increased ranged damage"); */
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetCritChance(DamageClass.Ranged) += 15;
            modPlayer.noAmmoConsumeChance += 0.25f;
            player.GetDamage(DamageClass.Ranged) *= 1.1f;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 20;
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

            player.GetCritChance(DamageClass.Generic) += modPlayer.HeadHunterCritBonus;
        }
    }

    [AutoloadEquip(EquipType.Head)]
    public class HeadhunterCrown : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 8)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.BeetleHusk, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Melee speed increased by 15%" +
                "\n15% increased melee critical strike chance" +
                "\n10% increased melee damage"); */
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetCritChance(DamageClass.Melee) += 15;
            player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
            player.GetDamage(DamageClass.Melee) *= 1.1f;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 28;
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

            player.GetCritChance(DamageClass.Generic) += modPlayer.HeadHunterCritBonus;
        }
    }

    [AutoloadEquip(EquipType.Head)]
    public class HeadhunterHood : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 8)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.Ectoplasm, 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Max mana increased by 100" +
                "\n15% increased magic critical strike chance" +
                "\n10% increased magic damage"); */
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetCritChance(DamageClass.Magic) += 15;
            player.statManaMax2 += 100;
            player.GetDamage(DamageClass.Magic) *= 1.1f;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 22;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 12;
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

            player.GetCritChance(DamageClass.Generic) += modPlayer.HeadHunterCritBonus;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class HeadhunterBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Increases item/weapon use speed by 9%" +
                "\nIncreases flight time by 50%" +
                "\n50% increased Shriek of Horror range"); */
            ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesBottomSkin[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetAttackSpeed(DamageClass.Generic) += 0.09f;
            modPlayer.flightTimeMultiplier *= 1.5f;
            modPlayer.ShriekRangeMultiplier *= 1.5f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 12)
                .AddIngredient(ItemID.SoulofFlight, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();

        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 18;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class HeadhunterGreaves : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 8)
                .AddIngredient(ItemID.SoulofLight, 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Increased maximum movement speed" +
                "\n8% increased damage"); */
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetDamage(DamageClass.Generic) += 0.08f;
            player.accRunSpeed *= 1.25f;
            player.maxRunSpeed *= 1.25f;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 12;
        }
    }

    public class HeadhunterFrenzy : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Headhunter's Frenzy");
            // Description.SetDefault("You have been sent into a state of chaos!");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.moveSpeed += 0.5f;
            player.GetAttackSpeed(DamageClass.Generic) *= 1.3f;
            modPlayer.critDamage *= 1.3f;
        }
    }
}
