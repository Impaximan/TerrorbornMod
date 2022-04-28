using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class PyroclasticShinobiHelmet : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), (int)(18 * TerrorbornMod.IncendiaryAlloyMultiplier))
                .AddIngredient(ModContent.ItemType<Items.Materials.PyroclasticGemstone>(), 8)
                .AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>())
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("6% increasd damage with thrown weapons" +
                "\n12% increased critical strike chance with thrown weapons");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 12;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PyroclasticShinobiBreastplate>() && legs.type == ModContent.ItemType<PyroclasticShinobiGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "6% increased critical strike chance with thrown weapons" +
                "\nDealing a critical hit will cause your next thrown Projectile to be a 'superthrow'" +
                "\nSuperthrown Projectiles will move twice as fast and explode on enemy hits" +
                "\nThe explosion inflicts a random type of fire";

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.PyroclasticShinobiBonus = true;
            player.GetCritChance(DamageClass.Throwing) += 6;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetDamage(DamageClass.Throwing) *= 1.06f;
            player.GetCritChance(DamageClass.Throwing) += 12;
        }

    }

    [AutoloadEquip(EquipType.Body)]
    public class PyroclasticShinobiBreastplate : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), (int)(25 * TerrorbornMod.IncendiaryAlloyMultiplier))
                .AddIngredient(ModContent.ItemType<Items.Materials.PyroclasticGemstone>(), 12)
                .AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>())
                .Register();
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Tooltip.SetDefault("10% increased damage with thrown weapons" +
                "\n25% increased wing flight time");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetDamage(DamageClass.Throwing) *= 1.1f;
            modPlayer.flightTimeMultiplier *= 1.25f;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class PyroclasticShinobiGreaves : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), (int)(17 * TerrorbornMod.IncendiaryAlloyMultiplier))
                .AddIngredient(ModContent.ItemType<Items.Materials.PyroclasticGemstone>(), 4)
                .AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>())
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("6% increased damage with thrown weapons" +
                "\n10% increased item use speed while moving");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 12;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetDamage(DamageClass.Throwing) *= 1.06f;

            if (player.velocity.X != 0)
            {
                player.GetAttackSpeed(DamageClass.Generic) *= 1.1f;
            }
        }
    }
}