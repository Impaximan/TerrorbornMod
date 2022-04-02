using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Tiles
{
    public class SmoothDeimostone : ModTile
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
            drop = ModContent.ItemType<Items.Placeable.Blocks.SmoothDeimostoneBlock>();
            AddMapEntry(new Color(40, 55, 70));
        }
    }
}

