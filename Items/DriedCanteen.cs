using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items
{
    class DriedCanteen : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Not consumable" +
                "\nSummons Dunestock");
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 24;
            Item.rare = ItemRarityID.Orange;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ZoneDesert && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.Dunestock>());
        }
        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Bosses.Dunestock>());
            SoundExtensions.PlaySoundOld(SoundID.Roar, player.position, 0);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Cobweb, 50)
                .AddIngredient(ItemID.AntlionMandible, 15)
                .AddIngredient(ItemID.Silk, 5)
                .AddIngredient(ModContent.ItemType<Materials.TarOfHunger>(), 75)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
