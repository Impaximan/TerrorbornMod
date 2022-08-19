using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Structures
{
    public static class StructureGenerator
    {
        internal static void GenerateHAShrine(Mod mod, Point point)
        {
            GenerationSample sample = new GenerationSample();
            sample.SetSample("TerrorbornMod/Structures/HAShrine");
            sample.SetFlag(SamplingKey.Placement);
            sample.SetPosition(point);
            int tileType = ModContent.TileType<Tiles.MemorialBrick>();

            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, tileType)
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(() => sample.Apply()));

            sample = new GenerationSample();
            sample.SetSample("TerrorbornMod/Structures/HAShrine_Walls");
            sample.SetFlag(SamplingKey.Walls);
            sample.SetPosition(point);
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, ModContent.WallType<Tiles.MemorialWall>())
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(() => sample.Apply()));
        }

        internal static void GenerateTWShrine(Mod mod, Point point)
        {
            GenerationSample sample = new GenerationSample();
            sample.SetSample("TerrorbornMod/Structures/TWShrine");
            sample.SetFlag(SamplingKey.Placement);
            sample.SetPosition(point);
            int tileType = ModContent.TileType<Tiles.MemorialBrick>();

            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, tileType),
                new ValueTuple<byte, byte, byte, int>(0, 255, 0, TileID.Titanium)
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(() => sample.Apply()));

            sample = new GenerationSample();
            sample.SetSample("TerrorbornMod/Structures/TWShrine_Walls");
            sample.SetFlag(SamplingKey.Walls);
            sample.SetPosition(point);
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, ModContent.WallType<Tiles.MemorialWall>())
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(() => sample.Apply()));
        }

        internal static void GenerateVBShrine(Mod mod, Point point)
        {
            GenerationSample sample = new GenerationSample();
            sample.SetSample("TerrorbornMod/Structures/VBShrine");
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
            TerrorbornUtils.InvokeOnMainThread(new Action(() => sample.Apply()));

            sample = new GenerationSample();
            sample.SetSample("TerrorbornMod/Structures/VBShrine_Walls");
            sample.SetFlag(SamplingKey.Walls);
            sample.SetPosition(point);
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, ModContent.WallType<Tiles.MemorialWall>())
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(() => sample.Apply()));

            sample = new GenerationSample();
            sample.SetSample("TerrorbornMod/Structures/VBShrine_Lava");
            sample.SetFlag(SamplingKey.LiquidLava);
            sample.SetPosition(point);
            TerrorbornUtils.InvokeOnMainThread(new Action(() => sample.Apply()));

            sample = new GenerationSample();
            sample.SetSample("TerrorbornMod/Structures/VBShrine_HalfBrick");
            sample.SetFlag(SamplingKey.HalfBrick);
            sample.SetPosition(point);
            TerrorbornUtils.InvokeOnMainThread(new Action(() => sample.Apply()));
        }

        internal static void GenerateSOHShrine(Mod mod, Point point)
        {
            GenerationSample sample = new GenerationSample();
            sample.SetSample("TerrorbornMod/Structures/SOHShrine");
            sample.SetFlag(SamplingKey.Placement);
            sample.SetPosition(point);
            int tileType = ModContent.TileType<Tiles.MemorialBrick>();
            for (int i = 0; i < Main.maxTileSets; i++)
            {

            }
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, tileType)
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(() => sample.Apply()));

            sample = new GenerationSample();
            sample.SetSample("TerrorbornMod/Structures/SOHShrine_Walls");
            sample.SetFlag(SamplingKey.Walls);
            sample.SetPosition(point);
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, ModContent.WallType<Tiles.MemorialWall>())
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(() => sample.Apply()));

            sample = new GenerationSample();
            sample.SetSample("TerrorbornMod/Structures/SOHShrine_SlopeDownLeft");
            sample.SetFlag(SamplingKey.SlopeDownLeft);
            sample.SetPosition(point);
            TerrorbornUtils.InvokeOnMainThread(new Action(() => sample.Apply()));
            sample = new GenerationSample();

            sample.SetSample("TerrorbornMod/Structures/SOHShrine_SlopeDownRight");
            sample.SetFlag(SamplingKey.SlopeDownRight);
            sample.SetPosition(point);
            TerrorbornUtils.InvokeOnMainThread(new Action(() => sample.Apply()));

            sample = new GenerationSample();
            sample.SetSample("TerrorbornMod/Structures/SOHShrine_HalfBrick");
            sample.SetFlag(SamplingKey.HalfBrick);
            sample.SetPosition(point);
            TerrorbornUtils.InvokeOnMainThread(new Action(() => sample.Apply()));
        }

        internal static void GenerateIIArena(Mod mod, Point point)
        {
            GenerationSample sample = new GenerationSample();
            sample.SetSample("TerrorbornMod/Structures/IIArena");
            sample.SetFlag(SamplingKey.Placement);
            sample.SetPosition(point);
            int tileType = ModContent.TileType<Tiles.MemorialBrick>();
            for (int i = 0; i < Main.maxTileSets; i++)
            {

            }
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, tileType),
                new ValueTuple<byte, byte, byte, int>(0, 255, 255, TileID.IceBlock),
                new ValueTuple<byte, byte, byte, int>(0, 255, 0, TileID.SnowBlock),
                new ValueTuple<byte, byte, byte, int>(0, 0, 255, TileID.Chain)
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(() => sample.Apply()));

            sample = new GenerationSample();
            sample.SetSample("TerrorbornMod/Structures/IIArena_Walls");
            sample.SetFlag(SamplingKey.Walls);
            sample.SetPosition(point);
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, ModContent.WallType<Tiles.MemorialWall>()),
                new ValueTuple<byte, byte, byte, int>(0, 255, 0, WallID.IceUnsafe)
                );
            TerrorbornUtils.InvokeOnMainThread(new Action(() => sample.Apply()));


            sample = new GenerationSample();
            sample.SetSample("TerrorbornMod/Structures/IIArena_Water");
            sample.SetFlag(SamplingKey.LiquidWater);
            sample.SetPosition(point);
            TerrorbornUtils.InvokeOnMainThread(new Action(() => sample.Apply()));
        }
    }
}