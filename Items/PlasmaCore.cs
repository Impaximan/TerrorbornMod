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
            CreateRecipe()
                .AddIngredient(ItemID.SoulofNight, 15)
                .AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 30)
                .AddIngredient(ModContent.ItemType<Items.Materials.HexingEssence>(), 3)
                .AddIngredient(ModContent.ItemType<Items.Materials.HellbornEssence>(), 3)
                .AddIngredient(ItemID.Ectoplasm, 20)
                .AddIngredient(ItemID.MartianConduitPlating, 100)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Midnight Energy Core");
            Tooltip.SetDefault("Calls forth the first prototype, a failure to create control" +
                "\nNot consumable");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 14));
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.value = 0;
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.noUseGraphic = true;
        }
        public override bool CanUseItem(Player player)
        {
            return !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.PrototypeI.PrototypeI>());
        }
        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Bosses.PrototypeI.PrototypeI>());
            SoundExtensions.PlaySoundOld(SoundID.Roar, player.position, 0);
            return true;
        }
    }
}
