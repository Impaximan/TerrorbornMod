﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Tiles.Incendiary
{
    public class IncendiaryMachinery : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            //Main.tileShine[Type] = 1;
            //Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = false;
            soundType = 21;
            soundStyle = 1;
            //Main.soundDig[Type] =  21;

            dustType = DustID.Fire;

            minPick = 150;
            mineResist = 8;
            drop = ModContent.ItemType<Items.Placeable.Blocks.IncendiaryMachineryBlock>();
            ModTranslation name = CreateMapEntryName();
            AddMapEntry(new Color(39, 39, 45));
        }

        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
        {
            dustType = DustID.Fire;
            makeDust = true;
        }

        public override bool HasWalkDust()
        {
            return true;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        //public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        //{
        //    r = 0.1f;
        //    g = 0;
        //    b = 0f;
        //}
    }
}
