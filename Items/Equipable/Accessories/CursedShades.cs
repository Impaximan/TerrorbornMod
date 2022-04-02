using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class CursedShades : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Lens, 3)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Critical hits do 20% more damage than normal" +
                "\n10% increased critical strike chance");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.critDamage += 0.2f;
            player.GetCritChance(DamageClass.Generic) += 10;
        }
    }
}
