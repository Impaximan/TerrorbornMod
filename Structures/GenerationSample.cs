using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace TerrorbornMod.Structures
{
    internal class GenerationSample
    {
        private Texture2D Sample { get; set; }

        private SamplingKey Flag { get; set; }

        private Point WorldPosition { get; set; }

        object[] flagValues = new object[(byte)SamplingKey.Count][];

        public void SetSample(Texture2D texture)
        {
            Sample = texture;
        }

        public void SetFlag(SamplingKey flag)
        {
            Flag = flag;
        }

        public void SetPosition(Point point)
        {
            WorldPosition = point;
        }

        public void SetSamplingValues(params object[] value)
        {
            flagValues = value;
        }

        private static readonly ValueTuple<byte, byte, byte, int> airColor = (255, 255, 255, -1);

        private static readonly ValueTuple<byte, byte, byte, int> nullColor = (0, 0, 0, -1);

        /// <summary>
        /// Applys the sample to the world using the data that has been given
        /// </summary>
        /// <param name="p"></param>
        /// <param name="types"></param>
        public void Apply(bool mute = false, bool forced = true)
        {
            int width = Sample.Width;
            int height = Sample.Height;
            Color[] arr = new Color[width * height];
            Sample.GetData(arr);
            switch (Flag)
            {
                case SamplingKey.Placement:
                {
                    ValueTuple<byte, byte, byte, int>[] tileValues = new ValueTuple<byte, byte, byte, int>[flagValues.Length];
                    for (int i = 0; i < flagValues.Length; i++)
                    {
                        tileValues[i] = (ValueTuple<byte, byte, byte, int>)flagValues[i];
                    }
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            Color color = arr[i + j * width];
                            for (int k = 0; k < tileValues.Length; k++)
                            {
                                if (color.R == tileValues[k].Item1 && color.G == tileValues[k].Item2 && color.B == tileValues[k].Item3)
                                {
                                    WorldGen.PlaceTile(WorldPosition.X + i, WorldPosition.Y + j, tileValues[k].Item4, mute, forced);
                                    Framing.GetTileSafely(WorldPosition.X + i, WorldPosition.Y + j).slope(Tile.Type_Solid);
                                }
                                else if (color.R == airColor.Item1 && color.G == airColor.Item2 && color.B == airColor.Item3)
                                {
                                    Framing.GetTileSafely(WorldPosition.X + i, WorldPosition.Y + j).active(active: false);
                                }
                            }
                        }
                    }
                }
                break;

                case SamplingKey.Walls:
                {
                    ValueTuple<byte, byte, byte, int>[] wallValues = new ValueTuple<byte, byte, byte, int>[flagValues.Length];
                    for (int i = 0; i < flagValues.Length; i++)
                    {
                        wallValues[i] = (ValueTuple<byte, byte, byte, int>)flagValues[i];
                    }
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            Color color = arr[i + j * width];
                            for (int k = 0; k < wallValues.Length; k++)
                            {
                                if (color.R == wallValues[k].Item1 && color.G == wallValues[k].Item2 && color.B == wallValues[k].Item3)
                                {
                                    int x = WorldPosition.X + i;
                                    int y = WorldPosition.Y + j;
                                    if (Framing.GetTileSafely(x, y).wall > 0)
                                    {
                                        Main.tile[x, y].wall = (ushort)wallValues[k].Item4;
                                    }
                                    else
                                    {
                                        WorldGen.PlaceWall(x, y, wallValues[k].Item4, mute);
                                    }
                                }
                                else if (color.R == airColor.Item1 && color.G == airColor.Item2 && color.B == airColor.Item3)
                                {
                                    Framing.GetTileSafely(WorldPosition.X + i, WorldPosition.Y + j).wall = 0;
                                }
                            }
                        }
                    }
                }
                break;

                case SamplingKey.FrameImportantTiles:
                {
                    ValueTuple<byte, byte, byte, int, int>[] tileValues = new ValueTuple<byte, byte, byte, int, int>[flagValues.Length];
                    for (int i = 0; i < flagValues.Length; i++)
                    {
                        tileValues[i] = (ValueTuple<byte, byte, byte, int, int>)flagValues[i];
                    }
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            Color color = arr[i + j * width];
                            for (int k = 0; k < tileValues.Length; k++)
                            {
                                if (color.R == tileValues[k].Item1 && color.G == tileValues[k].Item2 && color.B == tileValues[k].Item3)
                                {
                                    WorldGen.PlaceTile(WorldPosition.X + i, WorldPosition.Y + j, tileValues[k].Item4, mute, forced, -1, tileValues[k].Item5);
                                }
                            }
                        }
                    }
                }
                break;

                case SamplingKey.LiquidHoney:
                case SamplingKey.LiquidLava:
                case SamplingKey.LiquidWater:
                {
                    int liquidType = Flag == SamplingKey.LiquidLava ? Tile.Liquid_Lava : Flag == SamplingKey.LiquidHoney ? Tile.Liquid_Honey : Tile.Liquid_Water;
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            Color color = arr[i + j * width];
                            if (color.G != byte.MaxValue)
                            {
                                Tile tile = Framing.GetTileSafely(WorldPosition.X + i, WorldPosition.Y + j);
                                tile.liquid = color.R;
                                tile.liquidType(liquidType: liquidType);
                            }
                        }
                    }
                }
                break;

                case SamplingKey.SlopeUpLeft:
                case SamplingKey.SlopeUpRight:
                case SamplingKey.SlopeDownLeft:
                case SamplingKey.SlopeDownRight:
                {
                    byte slopeType;
                    switch (Flag)
                    {
                        case SamplingKey.SlopeUpLeft:
                        slopeType = Tile.Type_SlopeUpLeft - 1;
                        break;

                        case SamplingKey.SlopeUpRight:
                        slopeType = Tile.Type_SlopeUpRight - 1;
                        break;

                        case SamplingKey.SlopeDownLeft:
                        slopeType = Tile.Type_SlopeDownLeft - 1;
                        break;

                        default:                     
                        slopeType = Tile.Type_SlopeDownRight - 1;
                        break;

                    }
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            Color color = arr[i + j * width];
                            if (color.R != 0)
                            {
                                Framing.GetTileSafely(WorldPosition.X + i, WorldPosition.Y + j).slope(slope: slopeType);
                            }
                        }
                    }
                }
                break;

                case SamplingKey.HalfBrick:
                {
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            Color color = arr[i + j * width];
                            if (color.R != 0)
                            {
                                Framing.GetTileSafely(WorldPosition.X + i, WorldPosition.Y + j).halfBrick(halfBrick: true);
                            }
                        }
                    }
                }
                break;
            }
        }
    }
}