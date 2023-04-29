using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TerrorbornMod.Tiles.Incendiary
{
    class PyroclasticGemstone : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);

            HitSound = SoundID.DD2_WitherBeastHurt;
            ItemDrop = ModContent.ItemType<Items.Materials.PyroclasticGemstone>();

            MineResist = 5f;
            MinPick = 100;

            DustType = 6;

            Main.tileLighted[Type] = true;
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Pyroclastic Gemstone");
            AddMapEntry(new Color(255, 246, 216), name);
        }

        private readonly int animationFrameWidth = 18;
        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            int uniqueAnimationFrame = Main.tileFrame[Type] + i;
            if (i % 2 == 0)
            {
                uniqueAnimationFrame += 3;
            }
            if (i % 3 == 0)
            {
                uniqueAnimationFrame += 3;
            }
            if (i % 4 == 0)
            {
                uniqueAnimationFrame += 3;
            }
            uniqueAnimationFrame = uniqueAnimationFrame % 3;

            frameXOffset = uniqueAnimationFrame * animationFrameWidth;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            float thingy = 3f;
            r = 1f / thingy;
            g = 0.426f / thingy;
            b = 0.384f / thingy;
        }
    }
}
