using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.MiscConsumables
{
    class DarkSigil : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Consumable" +
                "\nStarts a blood moon" +
                "\nCan only be used during the night"); */
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.rare = ItemRarityID.Green;
            Item.maxStack = 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ShadowScale, 15)
                .AddTile(TileID.DemonAltar)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.TissueSample, 15)
                .AddTile(TileID.DemonAltar)
                .Register();
        }

        public override bool CanUseItem(Player player)
        {
            if (!Main.bloodMoon && !Main.dayTime)
            {
                Main.NewText("The sky begins to bleed...", Color.FromNonPremultiplied(175, 75, 255, 255));
                Main.bloodMoon = true;
                SoundExtensions.PlaySoundOld(SoundID.Roar, (int)player.Center.X, (int)player.Center.Y, 0);
                Item.stack--;
            }
            else
            {
                return false;
            }
            return base.CanUseItem(player);
        }
    }
}
