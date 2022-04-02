using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class Shelmet : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.ShellFragments>(), 12)
                .AddRecipeGroup(RecipeGroupID.IronBar, 6)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("9% increased melee critical strike chance" +
                "\nHas a set bonus when paired with copper or tin armor");
        }
        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Melee) += 9;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.defense = 6;
            Item.rare = ItemRarityID.Blue;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (body.type == ItemID.CopperChainmail && legs.type == ItemID.CopperGreaves) || (body.type == ItemID.TinChainmail && legs.type == ItemID.TinGreaves);
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "10% increased melee speed" +
                "\n4 defense";
            player.meleeSpeed += 0.1f;
            player.statDefense += 4;
        }
    }
}
