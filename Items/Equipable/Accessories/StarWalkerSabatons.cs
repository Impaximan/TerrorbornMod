using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class StarWalkerSabatons : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starwalker Sabatons");
            Tooltip.SetDefault("Hold DOWN to fall faster" +
                "\n50% increased wing flight time" +
                "\nIncreased jump and movement speed" +
                "\n10% increased item use speed" +
                "\nAllows you to walk on water an lava" +
                "\nThe wearer can run INSANELY fast");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 26;
            item.accessory = true;
            item.noMelee = true;
            item.rare = ItemRarityID.Red;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.defense = 8;
            item.useAnimation = 5;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<AeroSabatons>());
            recipe.AddIngredient(ItemID.LightningBoots);
            recipe.AddRecipeGroup("fragment", 4);
            recipe.AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 6);
            recipe.AddIngredient(ModContent.ItemType<Materials.PlasmaliumBar>(), 6);
            recipe.AddIngredient(ModContent.ItemType<Materials.NovagoldBar>(), 6);
            recipe.AddIngredient(ItemID.HallowedBar, 6);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<AeroSabatons>());
            recipe2.AddIngredient(ItemID.FrostsparkBoots);
            recipe2.AddRecipeGroup("fragment", 4);
            recipe2.AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 6);
            recipe2.AddIngredient(ModContent.ItemType<Materials.PlasmaliumBar>(), 6);
            recipe2.AddIngredient(ModContent.ItemType<Materials.NovagoldBar>(), 6);
            recipe2.AddIngredient(ItemID.HallowedBar, 6);
            recipe2.AddTile(TileID.TinkerersWorkbench);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.allUseSpeed *= 1.1f;

            if (player.controlDown)
            {
                player.maxFallSpeed *= 2;
                player.velocity.Y += 0.5f;
            }

            player.jumpSpeedBoost += 2f;

            modPlayer.flightTimeMultiplier *= 1.5f;

            if (player.velocity.Y != 0)
            {
                player.runAcceleration += 0.2f;
            }

            player.waterWalk2 = true;

            player.accRunSpeed = 10f;
            player.runSoundDelay = (int)(player.runSoundDelay * 0.75f);
            if (player.velocity.Y == 0)
            {
                player.accRunSpeed *= 1.1f;
                player.runAcceleration += 0.25f;
            }
        }
    }
}


