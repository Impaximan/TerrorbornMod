﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Tiles
{
    public class Deimostone : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = false;
            HitSound = SoundID.Tink;
            

            MinPick = 0;
            MineResist = 0.5f;
            ItemDrop = ModContent.ItemType<Items.Placeable.Blocks.DeimostoneBlock>();
            AddMapEntry(new Color(40, 55, 70));
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
