using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class TorturersTalisman : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.TorturedEssence>(), 5)
                .AddIngredient(ItemID.LunarBar, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Killing an enemy gives a 15 second boost to life regen" +
                "\nAbsurdly increased life regen while not in combat"); */
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.useAnimation = 12;
            Item.defense = 12;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.TorturersTalisman = true;
            if (!modPlayer.inCombat)
            {
                player.lifeRegen += 100;
            }
        }
    }
}
