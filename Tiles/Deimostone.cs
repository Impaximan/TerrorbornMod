using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Tiles
{
    public class Deimostone : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = false;
            soundType = SoundID.Tink;
            soundStyle = 1;

            minPick = 0;
            mineResist = 0.5f;
            drop = ModContent.ItemType<Items.Placeable.Blocks.DeimostoneBlock>();
            AddMapEntry(new Color(40, 55, 70));
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
