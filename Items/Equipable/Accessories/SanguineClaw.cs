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
            item.width = 32;
            item.height = 34;
            item.accessory = true;
            item.noMelee = true;
            item.rare = ItemRarityID.Green;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.useAnimation = 5;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(ModContent.ItemType<Materials.SanguineFang>(), 10);
            recipe1.AddIngredient(ItemID.TissueSample, 5);
            recipe1.AddTile(TileID.Anvils);
            recipe1.SetResult(this);
            recipe1.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<Materials.SanguineFang>(), 10);
            recipe2.AddIngredient(ItemID.ShadowScale, 5);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            bool bonus = false;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && npc.Distance(player.Center) <= 400)
                {
                    bonus = true;
                    break;
                }
            }
            if (bonus)
            {
                player.rangedCrit += 20;
                player.thrownCrit += 20;
            }
            modPlayer.critDamage += 0.15f;
            player.rangedDamage += 0.12f;
            player.thrownDamage += 0.12f;
        }
    }
}

