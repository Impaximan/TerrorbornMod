using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Tiles
{
    public class DeimosteelOre : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = true;
            soundType = SoundID.Tink;
            soundStyle = 1;

            minPick = 0;
            mineResist = 1f;
            drop = ModContent.ItemType<Items.Placeable.Blocks.DeimosteelOreItem>();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Deimosteel");
            AddMapEntry(new Color(138, 155, 152), name);
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}

