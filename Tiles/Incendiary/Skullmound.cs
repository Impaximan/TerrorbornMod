using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Tiles.Incendiary
{
    public class Skullmound : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileBlendAll[Type] = true;
            HitSound = SoundID.Tink;

            MinPick = 210;
            MineResist = 10f;
            ItemDrop = ModContent.ItemType<Items.Materials.SkullmoundOre>();
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Skullmound");
            AddMapEntry(new Color(157, 71, 64), name);
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
