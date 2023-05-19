using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Tiles
{
    public class Novagold : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = true;
            HitSound = SoundID.Tink;
            

            MinPick = 40;
            MineResist = 3f;
            //ItemDrop = ModContent.ItemType<Items.Materials.NovagoldOre>();
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Novagold");
            AddMapEntry(new Color(255, 238, 187), name);

            DustType = 15;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            float multiplier = 0.001f;
            r = 207 * multiplier;
            g = 253 * multiplier;
            b = 255 * multiplier;
        }
    }
}
