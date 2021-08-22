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
    class GenerateIncendiaryIsland : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Generates an incendiary sky island" +
                "\nRight click to change the type");
        }

        public override void SetDefaults()
        {
            item.rare = -12;
            item.autoReuse = false;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.useTime = 20;
            item.useAnimation = 20;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        const int typeCount = 2;
        int currentType = 0;
        //0 = normal
        //1 = main
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                currentType++;
                if (currentType >= typeCount)
                {
                    currentType = 0;
                }

                if (currentType == 0)
                {
                    Main.NewText("Type: Normal");
                }

                if (currentType == 1)
                {
                    Main.NewText("Type: Main");
                }
            }
            else
            {
                if (currentType == 0)
                {
                    TerrorbornWorld.GenerateIncendiaryIsland_Normal((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16, Main.rand.NextFloat(1f, 2f));
                }

                if (currentType == 1)
                {
                    TerrorbornWorld.GenerateIncendiaryIsland_Main((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16, Main.rand.NextFloat(1f, 1.2f));
                }
            }
            return base.CanUseItem(player);
        }
    }
}
