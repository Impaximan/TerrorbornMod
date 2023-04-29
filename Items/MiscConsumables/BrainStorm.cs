using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.MiscConsumables
{
    class BrainStorm : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Consumable" +
                "\nStarts the astraphobia event, or ends it if it's already going on" +
                "\nCan only be used during rain"); */
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
                .AddIngredient(ItemID.SoulofNight, 3)
                .AddIngredient(ItemID.SoulofLight, 3)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 2)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();

        }

        public override bool CanUseItem(Player player)
        {
            if (Main.raining)
            {
                if (TerrorbornSystem.terrorRain)
                {
                    Main.NewText("The sky begins to brighten up again...", Color.FromNonPremultiplied(40 * 2, 55 * 2, 70 * 2, 255));
                    TerrorbornSystem.terrorRain = false;
                    SoundExtensions.PlaySoundOld(SoundID.Roar, (int)player.Center.X, (int)player.Center.Y, 0);
                }
                else
                {
                    Main.NewText("Dark rain begins to fall from the sky!", Color.FromNonPremultiplied(40 * 2, 55 * 2, 70 * 2, 255));
                    TerrorbornSystem.terrorRain = true;
                    SoundExtensions.PlaySoundOld(SoundID.Roar, (int)player.Center.X, (int)player.Center.Y, 0);
                }
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
