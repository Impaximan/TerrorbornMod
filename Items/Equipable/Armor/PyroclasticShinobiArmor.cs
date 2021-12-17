using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class PyroclasticShinobiHelmet : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), (int)(18 * TerrorbornMod.IncendiaryAlloyMultiplier));
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.PyroclasticGemstone>(), 8);
            recipe.AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("6% increasd damage with thrown weapons" +
                "\n12% increased critical strike chance with thrown weapons");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.defense = 12;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PyroclasticShinobiBreastplate>() && legs.type == ModContent.ItemType<PyroclasticShinobiGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "6% increased critical strike chance with thrown weapons" +
                "\nDealing a critical hit will cause your next thrown projectile to be a 'superthrow'" +
                "\nSuperthrown projectiles will move twice as fast and explode on enemy hits" +
                "\nThe explosion inflicts a random type of fire";

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.PyroclasticShinobiBonus = true;
            player.thrownCrit += 6;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.thrownDamage += 0.06f;
            player.thrownCrit += 12;
        }

    }

    [AutoloadEquip(EquipType.Body)]
    public class PyroclasticShinobiBreastplate : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), (int)(25 * TerrorbornMod.IncendiaryAlloyMultiplier));
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.PyroclasticGemstone>(), 12);
            recipe.AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Tooltip.SetDefault("10% increased damage with thrown weapons" +
                "\n25% increased wing flight time");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 24;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.defense = 18;
        }

        public override bool DrawBody()
        {
            return false;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.thrownDamage += 0.1f;
            modPlayer.flightTimeMultiplier *= 1.25f;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class PyroclasticShinobiGreaves : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), (int)(17 * TerrorbornMod.IncendiaryAlloyMultiplier));
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.PyroclasticGemstone>(), 4);
            recipe.AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("6% increased damage with thrown weapons" +
                "\n10% increased item use speed while moving");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 12;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.thrownDamage += 0.06f;

            if (player.velocity.X != 0)
            {
                modPlayer.allUseSpeed *= 1.1f;
            }
        }
    }
}