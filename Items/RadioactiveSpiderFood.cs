using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items
{
    class RadioactiveSpiderFood : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("bugs", 5)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddIngredient(ItemID.SoulofSight, 5)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        int Shadowcrawler = ModContent.NPCType<NPCs.Bosses.Shadowcrawler>();
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons an ancient foe of Anekronyx");
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 26;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Lime;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
            Item.useAnimation = 30;
            Item.useTime = 30;
        }
        public override bool CanUseItem(Player player)
        {
            return !Main.dayTime && !NPC.AnyNPCs(Shadowcrawler);
        }
        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, Shadowcrawler);
            SoundExtensions.PlaySoundOld(SoundID.Roar, player.position, 0);
            TerrorbornSystem.ScreenShake(50);
            return true;
        }
    }
}
