using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class KomusoBaskethelm : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Hay, 30)
                .AddRecipeGroup(RecipeGroupID.IronBar, 8)
                .AddIngredient(ItemID.Silk, 6)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases summon damage by 4%");
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) *= 1.04f;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 6;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<KomusoBreastplate>() && legs.type == ModContent.ItemType<KomusoGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Increases summon damage by 12%";
            player.GetDamage(DamageClass.Summon) *= 1.12f;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class KomusoBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases summon damage by 5%" +
                "\nIncreases your maximum number of minions by 1");
            ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesBottomSkin[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) *= 1.05f;
            player.maxMinions++;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Hay, 25)
                .AddRecipeGroup(RecipeGroupID.IronBar, 13)
                .AddIngredient(ItemID.Silk, 9)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 25;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 7;
        }
    }
    [AutoloadEquip(EquipType.Legs)]
    public class KomusoGreaves : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Hay, 25)
                .AddRecipeGroup(RecipeGroupID.IronBar, 8)
                .AddIngredient(ItemID.Silk, 4)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases summon damage by 3%");
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) *= 1.03f;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 12;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 4;
        }
    }
}