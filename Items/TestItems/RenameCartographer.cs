using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items.TestItems
{
    class RenameCartographer : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Sets the terror master's dialogue sequence counter to 0");
        }

        public override void SetDefaults()
        {
            item.rare = -12;
            item.autoReuse = false;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.useTime = 20;
            item.useAnimation = 20;
        }

        public override bool CanUseItem(Player player)
        {
            string name = getCartographerName();
            Main.NewText("Cartographer renamed to " + name + "!");
            TerrorbornWorld.CartographerName = name;
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

