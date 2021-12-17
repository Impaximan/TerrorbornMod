using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Tiles
{
    public class Novagold : ModTile
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

            minPick = 40;
            mineResist = 3f;
            drop = ModContent.ItemType<Items.Materials.NovagoldOre>();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Novagold");
            AddMapEntry(new Color(255, 238, 187), name);

            dustType = 15;
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
