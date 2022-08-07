using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TerrorbornMod.Dreadwind;

namespace TerrorbornMod.Items
{
    class DreadwindTotem : ModItem
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
            DisplayName.SetDefault("Angelic Idol");
            Tooltip.SetDefault("A demonic artifact of human pride disguised in the shape of an angel" +
                "\nSummons the Dreadwind when used in the underworld" +
                "\nNot consumable");
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