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
                "\n5% increased item use speed" +
                "\nIncreased manueverability while airborne");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 3, 50, 0);
            Item.defense = 8;
            Item.useAnimation = 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HarpyBoots>())
                .AddIngredient(ModContent.ItemType<MeteorSabatons>())
                .AddIngredient(ModContent.ItemType<BoostRelic>())
                .AddIngredient(ModContent.ItemType<Materials.ThunderShard>(), 20)
                .AddIngredient(ModContent.ItemType<Materials.NoxiousScale>(), 13)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetAttackSpeed(DamageClass.Generic) *= 1.05f;

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
        }
    }
}

