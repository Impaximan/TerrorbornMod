using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.TestItems
{
    class RenameCartographer : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";

        public override bool IsLoadingEnabled(Mod mod)
        {
            return TerrorbornMod.IsInTestingMode;
        }

        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Sets the terror master's dialogue sequence counter to 0");
        }

        public override void SetDefaults()
        {
            Item.rare = -12;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
        }

        public override bool CanUseItem(Player player)
        {
            string name = getCartographerName();
            Main.NewText("Cartographer renamed to " + name + "!");
            TerrorbornSystem.CartographerName = name;
            return true;
        }

        public string getCartographerName()
        {
            switch (WorldGen.genRand.Next(7))
            {
                case 0:
                    return "Lupo";
                case 1:
                    return "Albert";
                case 2:
                    return "Cata";
                case 3:
                    return "Cornifer";
                case 4:
                    return "Abraham";
                case 5:
                    return "Gerardus";
                case 6:
                    return "Arthur";
                default:
                    return "David";
            }
        }
    }
}

