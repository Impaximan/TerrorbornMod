using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Tools
{
    class Hellscaper : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 8);
            recipe.AddIngredient(ItemID.SoulofNight, 3);
            recipe.AddIngredient(ItemID.SoulofLight, 3);
            recipe.AddIngredient(ItemID.SoulofFlight, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Right click to use as a hammer");
        }

        public override void SetDefaults()
        {
            item.damage = 32;
            item.melee = true;
            item.width = 56;
            item.height = 54;
            item.useAnimation = 15;
            item.useTime = 5;
            item.axe = 175 / 5;
            item.pick = 210;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.autoReuse = false;
                item.axe = 0;
                item.hammer = 100;
                item.pick = 0;
            }
            else
            {
                item.autoReuse = true;
                item.axe = 175 / 5;
                item.hammer = 0;
                item.pick = 210;
            }
            return base.CanUseItem(player);
        }
    }
}


