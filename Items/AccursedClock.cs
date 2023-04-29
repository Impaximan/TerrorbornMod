using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items
{
    class AccursedClock : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Not consumable" +
                "\nSummons a Hexed Constructor"); */
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 32;
            Item.rare = ItemRarityID.Pink;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            return TerrorbornPlayer.modPlayer(player).ZoneIncendiary && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.HexedConstructor.HexedConstructor>());
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Bosses.HexedConstructor.HexedConstructor>());
            SoundExtensions.PlaySoundOld(SoundID.Roar, player.position, 0);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.IncendiusAlloy>(), 15)
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddIngredient(ItemID.Wire, 25)
                .AddIngredient(ItemID.SoulofFlight, 25)
                .AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>())
                .Register();
        }
    }
}

