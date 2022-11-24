using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Tiles.Incendiary
{
    public class IncendiaryPiping : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            //Main.tileShine[Type] = 1;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = true;
            HitSound = SoundID.Tink;
            
            //Main.soundDig[Type] =  21;

            DustType = 6;

            MinPick = 100;
            MineResist = 10;
            ItemDrop = ModContent.ItemType<Items.Placeable.Blocks.IncendiaryPipe>();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Strange Piping");
            AddMapEntry(new Color(255, 176, 142), name);
        }

        public override void WalkDust(ref int DustType, ref bool makeDust, ref Color color)
        {
            DustType = 6;
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

