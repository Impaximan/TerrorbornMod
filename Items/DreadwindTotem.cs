using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.Dreadwind;

namespace TerrorbornMod.Items
{
    class DreadwindTotem : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofLight, 15)
                .AddIngredient(ItemID.SoulofNight, 15)
                .AddIngredient(ItemID.SoulofFlight, 15)
                .AddIngredient(ItemID.SoulofMight, 15)
                .AddIngredient(ItemID.SoulofSight, 15)
                .AddIngredient(ItemID.SoulofFright, 15)
                .AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 15)
                .AddIngredient(ModContent.ItemType<Items.Materials.HexingEssence>(), 8)
                .AddIngredient(ModContent.ItemType<Items.Materials.HellbornEssence>(), 8)
                .AddIngredient(ModContent.ItemType<Items.Materials.DreadfulEssence>(), 8)
                .AddIngredient(ModContent.ItemType<Items.Materials.TorturedEssence>(), 8)
                .AddIngredient(ItemID.LunarBar, 10)
                .AddIngredient(ItemID.LihzahrdBrick, 30)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Angelic Idol");
            /* Tooltip.SetDefault("A demonic artifact of human pride disguised in the shape of an angel" +
                "\nSummons the Dreadwind when used in the underworld" +
                "\nNot consumable"); */
        }

        public override void SetDefaults()
        {
            Item.width = 38;
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
            return player.ZoneUnderworldHeight && !DreadwindSystem.DreadwindActive;
        }

        public override bool? UseItem(Player player)
        {
            DreadwindSystem.StartDreadwind();
            SoundExtensions.PlaySoundOld(SoundID.Roar, player.position, 0);
            return true;
        }
    }
}