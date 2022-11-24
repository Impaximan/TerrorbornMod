using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod
{
    class TerrorbornRecipes : ModSystem
    {
        public override void AddRecipeGroups()
        {
            RecipeGroup bugs = new RecipeGroup(new Func<string>(BugString),
                ItemID.JuliaButterfly,
                ItemID.MonarchButterfly,
                ItemID.PurpleEmperorButterfly,
                ItemID.RedAdmiralButterfly,
                ItemID.SulphurButterfly,
                ItemID.TreeNymphButterfly,
                ItemID.UlyssesButterfly,
                ItemID.ZebraSwallowtailButterfly,
                ItemID.Firefly,
                ItemID.Buggy,
                ItemID.Grasshopper,
                ItemID.Grubby,
                ItemID.LightningBug);
            RecipeGroup.RegisterGroup("bugs", bugs);

            //Any Mythril Bar
            RecipeGroup mythril = new RecipeGroup(new Func<string>(MythrilString),
                ItemID.MythrilBar,
                ItemID.OrichalcumBar);
            RecipeGroup.RegisterGroup("mythril", mythril);

            //Any Lunar Fragment
            RecipeGroup fragment = new RecipeGroup(new Func<string>(FragmentString),
                ItemID.FragmentSolar,
                ItemID.FragmentNebula,
                ItemID.FragmentStardust,
                ItemID.FragmentVortex,
                ModContent.ItemType<Items.Materials.FusionFragment>());
            RecipeGroup.RegisterGroup("fragment", fragment);

            RecipeGroup cobalt = new RecipeGroup(new Func<string>(CobaltString),
                ItemID.CobaltBar,
                ItemID.PalladiumBar);
            RecipeGroup.RegisterGroup("cobalt", cobalt);
        }

        string BugString()
        {
            return "Any Bug";
        }

        string MythrilString()
        {
            return "Any Mythril Bar";
        }

        string FragmentString()
        {
            return "Any Lunar Fragment";
        }

        string CobaltString()
        {
            return "Any Cobalt Bar";
        }

        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.TryGetIngredient(ItemID.CopperShortsword, out Item ingredient))
                {
                    if (ingredient != null)
                    {
                        ingredient.SetDefaults(ModContent.ItemType<Items.Materials.DreadfulEssence>());
                        ingredient.stack = 15;
                    }
                }
            }
        }
    }
}
