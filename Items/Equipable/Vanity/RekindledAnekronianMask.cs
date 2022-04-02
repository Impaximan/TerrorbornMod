using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class RekindledAnekronianMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Lime;
            Item.vanity = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BossMasks.UnkindledAnekronianMask>())
                .AddIngredient(ModContent.ItemType<Materials.SoulOfPlight>(), 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}