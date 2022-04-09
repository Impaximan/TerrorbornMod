using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace TerrorbornMod.Tiles
{
    class MidnightFruit : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);

            SoundType = SoundID.Trackable;
            SoundStyle = 165;

            MineResist = 5f;
            MinPick = 100;

            DustType = 61;

            Main.tileLighted[Type] = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Midnight Fruit");
            AddMapEntry(Color.LimeGreen, name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 32, ModContent.ItemType<Items.PermanentUpgrades.MidnightFruit>());
        }

        private readonly int animationFrameWidth = 18 * 2;
        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            int uniqueAnimationFrame = Main.tileFrame[Type] + i;
            if (Main.tile[i, j].TileFrameX == 0 || Main.tile[i, j].TileFrameX == 18 * 2 || Main.tile[i, j].TileFrameX == 18 * 4)
            {
                uniqueAnimationFrame++;
            }
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
            r = 0f / thingy;
            g = 1f / thingy;
            b = 0f / thingy;
        }
    }
}
