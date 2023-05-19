using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Tools
{
    class Hellscaper : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 8)
                .AddIngredient(ItemID.SoulofNight, 3)
                .AddIngredient(ItemID.SoulofLight, 3)
                .AddIngredient(ItemID.SoulofFlight, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Right click to use as a hammer");
        }

        public override void SetDefaults()
        {
            Item.damage = 32;
            Item.DamageType = DamageClass.Melee;
            Item.width = 56;
            Item.height = 54;
            Item.useAnimation = 15;
            Item.useTime = 5;
            Item.axe = 175 / 5;
            Item.pick = 210;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.autoReuse = false;
                Item.axe = 0;
                Item.hammer = 100;
                Item.pick = 0;
            }
            else
            {
                Item.autoReuse = true;
                Item.axe = 175 / 5;
                Item.hammer = 0;
                Item.pick = 210;
            }
            return base.CanUseItem(player);
        }
    }
}


