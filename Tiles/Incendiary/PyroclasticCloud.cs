using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Tiles.Incendiary
{
    public class PyroclasticCloud : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = false;
            //Main.tileShine[Type] = 1;
            Main.tileLighted[Type] = false;
            Main.tileNoSunLight[Type] = false;
            Main.tileSpelunker[Type] = false;
            SoundType = SoundID.Dig;
            SoundStyle = 1;
            //Main.soundDig[Type] =  21;

            DustType = 6;

            MinPick = 0;
            ItemDrop = ModContent.ItemType<Items.Placeable.Blocks.PyroclasticCloudBlock>(); ;
            MineResist = 3;
            AddMapEntry(new Color(255, 246, 216));
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

        //public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        //{
        //    r = 0.1f;
        //    g = 0;
        //    b = 0f;
        //}
    }
}