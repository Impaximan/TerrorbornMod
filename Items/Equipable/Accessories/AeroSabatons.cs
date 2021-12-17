using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class AeroSabatons : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aero Sabatons");
            Tooltip.SetDefault("Hold JUMP to fall slower" +
                "\nHold DOWN to fall faster" +
                "\n50% increased wing flight time" +
                "\nIncreased jump speed" +
                "\n10% increased item use speed" +
                "\nIncreased manueverability while airborne");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 24;
            item.accessory = true;
            item.noMelee = true;
            item.rare = ItemRarityID.Pink;
            item.value = Item.sellPrice(0, 3, 50, 0);
            item.defense = 8;
            item.useAnimation = 5;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<HarpyBoots>());
            recipe.AddIngredient(ModContent.ItemType<MeteorSabatons>());
            recipe.AddIngredient(ModContent.ItemType<BoostRelic>());
            recipe.AddIngredient(ModContent.ItemType<Materials.ThunderShard>(), 20);
            recipe.AddIngredient(ModContent.ItemType<Materials.NoxiousScale>(), 13);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.allUseSpeed *= 1.1f;

            if (player.controlJump)
            {
                player.maxFallSpeed /= 3;
                player.noFallDmg = true;
            }

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

            return;
            player.accRunSpeed = 9f;
            player.runSoundDelay = (int)(player.runSoundDelay * 0.75f);
            if (player.velocity.Y == 0)
            {
                player.accRunSpeed *= 1.2f;
                player.runAcceleration += 0.15f;
            }
        }
    }
}

