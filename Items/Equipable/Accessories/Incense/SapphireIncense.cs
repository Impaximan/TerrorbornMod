﻿using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories.Incense
{
    class SapphireIncense : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Sapphire, 5);
            recipe.AddIngredient(ItemID.Bottle);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Decreased enemy defense by 3." +
                "\n2% increased movement speed for every enemy nearby.");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 44;
            item.accessory = true;
            item.noMelee = true;
            item.rare = ItemRarityID.Green;
            item.value = Item.sellPrice(gold: 1, silver: 25);
            item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.armorPenetration += 3;
            int enemyCount = 0;
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.friendly && npc.CanBeChasedBy())
                {
                    enemyCount++;
                }
            }
            player.moveSpeed *= 1 + (0.02f * enemyCount);
        }
    }
}


