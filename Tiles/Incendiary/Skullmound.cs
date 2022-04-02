using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Tiles.Incendiary
{
    public class Skullmound : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileBlendAll[Type] = true;
            soundType = SoundID.Tink;
            soundStyle = 1;

            minPick = 210;
            mineResist = 10f;
            drop = ModContent.ItemType<Items.Materials.SkullmoundOre>();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Skullmound");
            AddMapEntry(new Color(157, 71, 64), name);
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
