using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Structures
{
    public static class StructureGenerator
    {
        internal static void GenerateHAShrine(Mod mod, Point point)
        {
            GenerationSample sample = new GenerationSample();
            sample.SetSample((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Structures/HAShrine"));
            sample.SetFlag(SamplingKey.Placement);
            sample.SetPosition(point);
            int tileType = ModContent.TileType<Tiles.MemorialBrick>(); // a random tile type that doesn't have Main.frameImportant flagged
            for (int i = 0; i < Main.maxTileSets; i++) // using a for loop instead of a while loop so that if the 1 in a quadrabilliontrillion chance that you don't get a valid tile after 1,000,000,000,000,000 rolls of Main.rand doesn't lag lul
            {

            }
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, tileType) // use that tile type here
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            sample = new GenerationSample();
            sample.SetSample((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Structures/HAShrine_Walls")); // creates background walls
            sample.SetFlag(SamplingKey.Walls);
            sample.SetPosition(point);
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, ModContent.WallType<Tiles.MemorialWall>())
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            //sample = new GenerationSample();
            //sample.SetSample((Texture2D)ModContent.Request<Texture2D>("Structures/StructureTest_FrameImportant")); // places frame important tiles
            //sample.SetFlag(SamplingKey.FrameImportantTiles);
            //sample.SetPosition(point);
            //sample.SetSamplingValues(
            //    new ValueTuple<byte, byte, byte, int, int>(253, 27, 77, TileID.DemonAltar, 1) // the extra int value is the style of the tile
            //    );
            //TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            //sample = new GenerationSample();
            //sample.SetSample((Texture2D)ModContent.Request<Texture2D>("Structures/StructureTest_Water"));
            //sample.SetFlag(SamplingKey.LiquidWater);
            //sample.SetPosition(point);
            //TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            //sample = new GenerationSample();
            //sample.SetSample((Texture2D)ModContent.Request<Texture2D>("Structures/StructureTest_Lava"));
            //sample.SetFlag(SamplingKey.LiquidLava);
            //sample.SetPosition(point);
            //TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            //sample = new GenerationSample();
            //sample.SetSample((Texture2D)ModContent.Request<Texture2D>("Structures/StructureTest_Honey"));
            //sample.SetFlag(SamplingKey.LiquidHoney);
            //sample.SetPosition(point);
            //TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            //sample = new GenerationSample();
            //sample.SetSample((Texture2D)ModContent.Request<Texture2D>("Structures/StructureTest_HalfBrick"));
            //sample.SetFlag(SamplingKey.HalfBrick);
            //sample.SetPosition(point);
            //TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));
        }

        internal static void GenerateTWShrine(Mod mod, Point point)
        {
            GenerationSample sample = new GenerationSample();
            sample.SetSample((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Structures/TWShrine"));
            sample.SetFlag(SamplingKey.Placement);
            sample.SetPosition(point);
            int tileType = ModContent.TileType<Tiles.MemorialBrick>(); // a random tile type that doesn't have Main.frameImportant flagged
            for (int i = 0; i < Main.maxTileSets; i++) // using a for loop instead of a while loop so that if the 1 in a quadrabilliontrillion chance that you don't get a valid tile after 1,000,000,000,000,000 rolls of Main.rand doesn't lag lul
            {

            }
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, tileType),
                new ValueTuple<byte, byte, byte, int>(0, 255, 0, TileID.Titanium)
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            sample = new GenerationSample();
            sample.SetSample((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Structures/TWShrine_Walls")); // creates background walls
            sample.SetFlag(SamplingKey.Walls);
            sample.SetPosition(point);
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, ModContent.WallType<Tiles.MemorialWall>())
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            //sample = new GenerationSample();
            //sample.SetSample((Texture2D)ModContent.Request<Texture2D>("Structures/StructureTest_FrameImportant")); // places frame important tiles
            //sample.SetFlag(SamplingKey.FrameImportantTiles);
            //sample.SetPosition(point);
            //sample.SetSamplingValues(
            //    new ValueTuple<byte, byte, byte, int, int>(253, 27, 77, TileID.DemonAltar, 1) // the extra int value is the style of the tile
            //    );
            //TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            //sample = new GenerationSample();
            //sample.SetSample((Texture2D)ModContent.Request<Texture2D>("Structures/StructureTest_Water"));
            //sample.SetFlag(SamplingKey.LiquidWater);
            //sample.SetPosition(point);
            //TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            //sample = new GenerationSample();
            //sample.SetSample((Texture2D)ModContent.Request<Texture2D>("Structures/StructureTest_Lava"));
            //sample.SetFlag(SamplingKey.LiquidLava);
            //sample.SetPosition(point);
            //TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            //sample = new GenerationSample();
            //sample.SetSample((Texture2D)ModContent.Request<Texture2D>("Structures/StructureTest_Honey"));
            //sample.SetFlag(SamplingKey.LiquidHoney);
            //sample.SetPosition(point);
            //TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            //sample = new GenerationSample();
            //sample.SetSample((Texture2D)ModContent.Request<Texture2D>("Structures/StructureTest_HalfBrick"));
            //sample.SetFlag(SamplingKey.HalfBrick);
            //sample.SetPosition(point);
            //TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));
        }

        internal static void GenerateVBShrine(Mod mod, Point point)
        {
            GenerationSample sample = new GenerationSample();
            sample.SetSample((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Structures/VBShrine"));
            sample.SetFlag(SamplingKey.Placement);
            sample.SetPosition(point);
            int tileType = ModContent.TileType<Tiles.MemorialBrick>(); // a random tile type that doesn't have Main.frameImportant flagged
            for (int i = 0; i < Main.maxTileSets; i++) // using a for loop instead of a while loop so that if the 1 in a quadrabilliontrillion chance that you don't get a valid tile after 1,000,000,000,000,000 rolls of Main.rand doesn't lag lul
            {

            }
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, tileType),
                new ValueTuple<byte, byte, byte, int>(0, 255, 0, TileID.Platforms),
                new ValueTuple<byte, byte, byte, int>(0, 0, 255, TileID.LivingDemonFire)
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            sample = new GenerationSample();
            sample.SetSample((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Structures/VBShrine_Walls")); // creates background walls
            sample.SetFlag(SamplingKey.Walls);
            sample.SetPosition(point);
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, ModContent.WallType<Tiles.MemorialWall>())
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            //sample = new GenerationSample();
            //sample.SetSample((Texture2D)ModContent.Request<Texture2D>("Structures/StructureTest_FrameImportant")); // places frame important tiles
            //sample.SetFlag(SamplingKey.FrameImportantTiles);
            //sample.SetPosition(point);
            //sample.SetSamplingValues(
            //    new ValueTuple<byte, byte, byte, int, int>(253, 27, 77, TileID.DemonAltar, 1) // the extra int value is the style of the tile
            //    );
            //TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            //sample = new GenerationSample();
            //sample.SetSample((Texture2D)ModContent.Request<Texture2D>("Structures/StructureTest_Water"));
            //sample.SetFlag(SamplingKey.LiquidWater);
            //sample.SetPosition(point);
            //TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            sample = new GenerationSample();
            sample.SetSample((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Structures/VBShrine_Lava"));
            sample.SetFlag(SamplingKey.LiquidLava);
            sample.SetPosition(point);
            TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            //sample = new GenerationSample();
            //sample.SetSample((Texture2D)ModContent.Request<Texture2D>("Structures/StructureTest_Honey"));
            //sample.SetFlag(SamplingKey.LiquidHoney);
            //sample.SetPosition(point);
            //TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            sample = new GenerationSample();
            sample.SetSample((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Structures/VBShrine_HalfBrick"));
            sample.SetFlag(SamplingKey.HalfBrick);
            sample.SetPosition(point);
            TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));
        }

        internal static void GenerateSOHShrine(Mod mod, Point point)
        {
            GenerationSample sample = new GenerationSample();
            sample.SetSample((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Structures/SOHShrine"));
            sample.SetFlag(SamplingKey.Placement);
            sample.SetPosition(point);
            int tileType = ModContent.TileType<Tiles.MemorialBrick>();
            for (int i = 0; i < Main.maxTileSets; i++)
            {

            }
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, tileType),
                new ValueTuple<byte, byte, byte, int>(0, 255, 0, TileID.Platforms),
                new ValueTuple<byte, byte, byte, int>(0, 0, 255, TileID.LivingDemonFire)
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            sample = new GenerationSample();
            sample.SetSample((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Structures/SOHShrine_Walls"));
            sample.SetFlag(SamplingKey.Walls);
            sample.SetPosition(point);
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, ModContent.WallType<Tiles.MemorialWall>())
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            sample = new GenerationSample();
            sample.SetSample((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Structures/SOHShrine_HalfBrick"));
            sample.SetFlag(SamplingKey.HalfBrick);
            sample.SetPosition(point);
            TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            sample = new GenerationSample();
            sample.SetSample((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Structures/SOHShrine_SlopeDownRight"));
            sample.SetFlag(SamplingKey.SlopeDownLeft);
            sample.SetPosition(point);
            TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            sample = new GenerationSample();
            sample.SetSample((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Structures/SOHShrine_SlopeDownLeft"));
            sample.SetFlag(SamplingKey.SlopeDownRight); //Already Tried: Slope Down Left, Slope Down Right, Slope Up Left, Slope Up Right (apparently one of the last 2)
            sample.SetPosition(point);
            TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));
        }

        internal static void GenerateIIArena(Mod mod, Point point)
        {
            GenerationSample sample = new GenerationSample();
            sample.SetSample((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Structures/IIArena"));
            sample.SetFlag(SamplingKey.Placement);
            sample.SetPosition(point);
            int tileType = ModContent.TileType<Tiles.MemorialBrick>();
            for (int i = 0; i < Main.maxTileSets; i++)
            {

            }
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, tileType),
                new ValueTuple<byte, byte, byte, int>(0, 255, 0, TileID.Chain),
                new ValueTuple<byte, byte, byte, int>(0, 0, 255, TileID.SnowBlock),
                new ValueTuple<byte, byte, byte, int>(0, 255, 255, TileID.IceBlock)
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            sample = new GenerationSample();
            sample.SetSample((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Structures/IIArena_Water"));
            sample.SetFlag(SamplingKey.LiquidWater);
            sample.SetPosition(point);
            TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));

            sample = new GenerationSample();
            sample.SetSample((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Structures/IIArena_Walls"));
            sample.SetFlag(SamplingKey.Walls);
            sample.SetPosition(point);
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, ModContent.WallType<Tiles.MemorialWall>()),
                new ValueTuple<byte, byte, byte, int>(0, 255, 0, WallID.IceUnsafe)
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(sample.Apply));
        }
    }
}