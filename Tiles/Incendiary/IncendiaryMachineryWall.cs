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

namespace TerrorbornMod.Tiles.Incendiary
{
    class IncendiaryMachineryWall : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = false;
            drop = ModContent.ItemType<Items.Placeable.Walls.IncendiaryMachineryWall>();
            AddMapEntry(new Color(20, 20, 23));
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
