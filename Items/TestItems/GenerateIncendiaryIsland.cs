﻿using System;
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

        const int typeCount = 6;
        int currentType = 0;
        //0 = normal
        //1 = main
        //2 = mechanical
        //3 = skullmound
        //4 = ritual
        //5 = chest
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

                if (currentType == 2)
                {
                    Main.NewText("Type: Mechanical");
                }

                if (currentType == 3)
                {
                    Main.NewText("Type: Skullmound");
                }

                if (currentType == 4)
                {
                    Main.NewText("Type: Ritual");
                }

                if (currentType == 5)
                {
                    Main.NewText("Type: Chest");
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

                if (currentType == 2)
                {
                    TerrorbornWorld.GenerateIncendiaryIsland_Mechanical((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16, Main.rand.NextFloat(1.5f, 2.25f));
                }

                if (currentType == 3)
                {
                    TerrorbornWorld.GenerateIncendiaryIsland_Skullmound((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16, Main.rand.NextFloat(1.5f, 2.25f));
                }

                if (currentType == 4)
                {
                    TerrorbornWorld.GenerateIncendiaryIsland_Ritual((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16, Main.rand.NextFloat(1.5f, 2.25f));
                }

                if (currentType == 5)
                {
                    TerrorbornWorld.GenerateIncendiaryIsland_Chest((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16, Main.rand.NextFloat(1.5f, 2.25f));
                }
            }
            return base.CanUseItem(player);
        }
    }

    class GenerateIncendiaryBiome : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Generates the incendiary biome on a random side of the world");
        }

        public override void SetDefaults()
        {
            item.rare = -12;
            item.autoReuse = false;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.useTime = 40;
            item.useAnimation = 40;
        }

        const int typeCount = 3;
        int currentType = 0;
        //0 = normal
        //1 = main
        //2 = mechanical
        public override bool CanUseItem(Player player)
        {
            TerrorbornWorld.GenerateIncendiaryBiome(density: 1.5f);
            return base.CanUseItem(player);
        }
    }
}
