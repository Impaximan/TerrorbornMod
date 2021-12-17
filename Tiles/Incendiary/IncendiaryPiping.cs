using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Tiles.Incendiary
{
    public class IncendiaryPiping : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            //Main.tileShine[Type] = 1;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = true;
            soundType = SoundID.Tink;
            soundStyle = 1;
            //Main.soundDig[Type] =  21;

            dustType = DustID.Fire;

            minPick = 100;
            mineResist = 10;
            drop = ModContent.ItemType<Items.Placeable.Blocks.IncendiaryPipe>();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Strange Piping");
            AddMapEntry(new Color(255, 176, 142), name);
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

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.1f;
            g = 0;
            b = 0f;
        }
    }
}

