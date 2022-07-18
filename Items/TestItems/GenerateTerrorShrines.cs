using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items.TestItems
{
    class GenerateTerrorShrines : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override bool IsLoadingEnabled(Mod mod)
        {
            return TerrorbornMod.IsInTestingMode;
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Generates the terror shrines in a pre-made world");
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
            Vector2 ShriekOfHorrorPosition = new Vector2(Main.spawnTileX, Main.spawnTileY);
            float sizeMultiplier = 1;
            if (Main.maxTilesX == 6400)
            {
                sizeMultiplier = 1.5f;
            }
            if (Main.maxTilesX == 8400)
            {
                sizeMultiplier = 2f;
            }
            ShriekOfHorrorPosition.Y += 285 * sizeMultiplier;
            ShriekOfHorrorPosition.X -= 15;

            Structures.StructureGenerator.GenerateSOHShrine(Mod, new Point((int)ShriekOfHorrorPosition.X, (int)ShriekOfHorrorPosition.Y));

            int DungeonDirection = 1;
            if (Main.dungeonX < Main.spawnTileX)
            {
                DungeonDirection = -1;
            }
            Vector2 HorrificAdaptationPosition = new Vector2(Main.spawnTileX + (Main.maxTilesX / 4) * -DungeonDirection, Main.maxTilesY / 2);
            Structures.StructureGenerator.GenerateHAShrine(Mod, new Point((int)HorrificAdaptationPosition.X, (int)HorrificAdaptationPosition.Y));

        //    bool foundIIShrinePosition = false;
        //    while (!foundIIShrinePosition)
        //    {
        //        TerrorbornSystem.IIShrinePosition = new Vector2(WorldGen.genRand.Next(150, Main.maxTilesX - 150), 100);
        //        while (!(Main.tileSolid[Main.tile[(int)TerrorbornSystem.IIShrinePosition.X, (int)TerrorbornSystem.IIShrinePosition.Y].TileType] || TerrorbornSystem.IIShrinePosition.Y >= Main.maxTilesY * 0.75f))
        //        {
        //            TerrorbornSystem.IIShrinePosition.Y++;
        //        }
        //        if (Main.tile[(int)TerrorbornSystem.IIShrinePosition.X, (int)TerrorbornSystem.IIShrinePosition.Y].TileType == TileID.SnowBlock || Main.tile[(int)TerrorbornSystem.IIShrinePosition.X, (int)TerrorbornSystem.IIShrinePosition.Y].TileType == TileID.IceBlock)
        //        {
        //            foundIIShrinePosition = true;
        //            break;
        //        }
        //    }
        //    while (!WorldUtils.Find(TerrorbornSystem.IIShrinePosition.ToPoint(), Searches.Chain(new Searches.Down(1), new GenCondition[]
        //        {
        //new Conditions.IsSolid()
        //        }), out _))
        //    {
        //        TerrorbornSystem.IIShrinePosition.Y++;
        //    }
        //    Structures.StructureGenerator.GenerateIIArena(Mod, new Point((int)TerrorbornSystem.IIShrinePosition.X - 52, (int)TerrorbornSystem.IIShrinePosition.Y - 8));

            TerrorbornSystem.VoidBlink = new Vector2(WorldGen.genRand.Next(50, Main.maxTilesX - 50), Main.maxTilesY * 0.95f);
            Structures.StructureGenerator.GenerateVBShrine(Mod, new Point((int)TerrorbornSystem.VoidBlink.X, (int)TerrorbornSystem.VoidBlink.Y));

            TerrorbornSystem.TerrorWarp = new Vector2(WorldGen.genRand.Next(50, Main.maxTilesX - 50), Main.maxTilesY * 0.66f);
            Structures.StructureGenerator.GenerateTWShrine(Mod, new Point((int)TerrorbornSystem.TerrorWarp.X, (int)TerrorbornSystem.TerrorWarp.Y));
            return base.CanUseItem(player);
        }
    }
}


