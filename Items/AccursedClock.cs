using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items
{
    class AccursedClock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Not consumable" +
                "\nSummons a Hexed Constructor");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 32;
            item.rare = 5;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = 4;
            item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            return TerrorbornPlayer.modPlayer(player).ZoneIncendiary && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.HexedConstructor.HexedConstructor>());
        }

        public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Bosses.HexedConstructor.HexedConstructor>());
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.IncendiusAlloy>(), 15);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddIngredient(ItemID.Wire, 25);
            recipe.AddIngredient(ItemID.SoulofFlight, 25);
            recipe.AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>());
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}

