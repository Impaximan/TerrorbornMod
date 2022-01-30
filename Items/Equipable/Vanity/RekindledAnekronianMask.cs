using Terraria;
using System;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class RekindledAnekronianMask : ModItem
    {
        public override void SetStaticDefaults()
        {

        }

        public override bool DrawHead()
        {
            return true;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
        }

        public override void SetDefaults()
        {
            item.rare = ItemRarityID.Lime;
            item.vanity = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<BossMasks.UnkindledAnekronianMask>());
            recipe.AddIngredient(ModContent.ItemType<Materials.SoulOfPlight>(), 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}