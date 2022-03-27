using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrorbornMod.Items
{
    class PlasmaCore : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofNight, 15);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 30);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.HexingEssence>(), 3);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.HellbornEssence>(), 3);
            recipe.AddIngredient(ItemID.Ectoplasm, 20);
            recipe.AddIngredient(ItemID.MartianConduitPlating, 100);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Midnight Energy Core");
            Tooltip.SetDefault("Calls forth the first prototype, a failure to create control" +
                "\nNot consumable");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 14));
        }
        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.value = 0;
            item.rare = ItemRarityID.Yellow;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.consumable = false;
            item.useAnimation = 30;
            item.useTime = 30;
            item.noUseGraphic = true;
        }
        public override bool CanUseItem(Player player)
        {
            return !Main.dayTime && !NPC.AnyNPCs(mod.NPCType("PrototypeI"));
        }
        public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("PrototypeI"));
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }
    }
}
