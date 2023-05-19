using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class SanguineClaw : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases critical damage by 15%" +
                "\n20% increased thrown and ranged critical strike chance while close to an enemy" +
                "\n12% increased thrown and ranged damage");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 34;
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.useAnimation = 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.SanguineFang>(10)
                .AddIngredient(ItemID.TissueSample, 5)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe()
                .AddIngredient<Materials.SanguineFang>(10)
                .AddIngredient(ItemID.ShadowScale, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            bool bonus = false;
            foreach (NPC NPC in Main.npc)
            {
                if (NPC.active && !NPC.friendly && NPC.lifeMax > 5 && NPC.Distance(player.Center) <= 400)
                {
                    bonus = true;
                    break;
                }
            }
            if (bonus)
            {
                player.GetCritChance(DamageClass.Ranged) += 20;
                player.GetCritChance(DamageClass.Throwing) += 20;
            }
            modPlayer.critDamage += 0.15f;
            player.GetDamage(DamageClass.Ranged) *= 1.1f;
            player.GetDamage(DamageClass.Throwing) *= 1.1f;
        }
    }
}

