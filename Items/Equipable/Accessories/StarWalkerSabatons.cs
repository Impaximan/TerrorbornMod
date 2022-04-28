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
            Item.width = 34;
            Item.height = 26;
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.defense = 8;
            Item.useAnimation = 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AeroSabatons>())
                .AddIngredient(ItemID.LightningBoots)
                .AddRecipeGroup("fragment", 4)
                .AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 6)
                .AddIngredient(ModContent.ItemType<Materials.PlasmaliumBar>(), 6)
                .AddIngredient(ModContent.ItemType<Materials.NovagoldBar>(), 6)
                .AddIngredient(ItemID.HallowedBar, 6)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AeroSabatons>())
                .AddIngredient(ItemID.FrostsparkBoots)
                .AddRecipeGroup("fragment", 4)
                .AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 6)
                .AddIngredient(ModContent.ItemType<Materials.PlasmaliumBar>(), 6)
                .AddIngredient(ModContent.ItemType<Materials.NovagoldBar>(), 6)
                .AddIngredient(ItemID.HallowedBar, 6)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AeroSabatons>())
                .AddIngredient(ItemID.TerrasparkBoots)
                .AddRecipeGroup("fragment", 4)
                .AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 6)
                .AddIngredient(ModContent.ItemType<Materials.PlasmaliumBar>(), 6)
                .AddIngredient(ModContent.ItemType<Materials.NovagoldBar>(), 6)
                .AddIngredient(ItemID.HallowedBar, 6)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetAttackSpeed(DamageClass.Generic) *= 1.1f;

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


