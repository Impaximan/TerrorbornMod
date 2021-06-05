using System;
namespace TerrorbornMod.Structures
{
    /// <summary>
    /// A flag used to enumerate different types of structure texture samples in <see cref="GenerationSample"/>
    /// </summary>
    internal enum SamplingKey : byte
    {
        /// <summary>
        /// Tile placement. Color(0, 0, 0) is Null (Tile type gets unchanged). Color(255, 255, 255) is Air (Replaces tile with air).
        /// <para>
        /// Requires sampling values of the type <see cref="ValueTuple"/>{<see cref="byte"/>, <see cref="byte"/>, <see cref="byte"/>, <see cref="int"/>}
        /// </para>
        /// </summary>
        Placement = 0,
        /// <summary>
        /// Sets tiles in the sampling to half bricks
        /// </summary>
        HalfBrick = 1,
        /// <summary>
        /// Sets values in the sampling to down right slopes
        /// </summary>
        SlopeDownRight = 2,
        /// <summary>
        /// Sets tiles in the sampling to down left slopes
        /// </summary>
        SlopeDownLeft = 3,
        /// <summary>
        /// Sets tiles in the sampling to up right slopes
        /// </summary>
        SlopeUpRight = 4,
        /// <summary>
        /// Sets tiles in the sampling to up left slopes
        /// </summary>
        SlopeUpLeft = 5,
        /// <summary>
        /// Sets tiles in the sampling to be filled with water. The amount of liquid given to the tile is determined by the R color of the sample. Setting the G color of the sample to 255 will make the Application process ignore that tile
        /// </summary>
        LiquidWater = 6,
        /// <summary>
        /// Sets tiles in the sampling to be filled with lava. The amount of liquid given to the tile is determined by the R color of the sample. Setting the G color of the sample to 255 will make the Application process ignore that tile
        /// </summary>
        LiquidLava = 7,
        /// <summary>
        /// Sets tiles in the sampling to be filled with honey. The amount of liquid given to the tile is determined by the R color of the sample. Setting the G color of the sample to 255 will make the Application process ignore that tile
        /// </summary>
        LiquidHoney = 8,
        /// <summary>
        /// Wall placement. Color(0, 0, 0) is Null (Wall type gets unchanged). Color(255, 255, 255) is Air (Replaces wall with air).
        /// <para>
        /// Requires sampling values of the type <see cref="ValueTuple"/>{<see cref="byte"/>, <see cref="byte"/>, <see cref="byte"/>, <see cref="int"/>}
        /// </para>
        /// </summary>
        Walls = 9,
        /// <summary>
        /// Frame important tile placement. Color(0, 0, 0) is Null (Tile type gets unchanged)
        /// <para>
        /// Requires sampling values of the type <see cref="ValueTuple"/>{<see cref="byte"/>, <see cref="byte"/>, <see cref="byte"/>, <see cref="int"/>, <see cref="int"/>}
        /// </para>
        /// </summary>
        FrameImportantTiles = 10,
        Count
    }
}
