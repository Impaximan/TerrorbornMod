using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Tiles
{
    public class MemorialBrick : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            //Main.tileShine[Type] = 1;
            Main.tileLighted[Type] = true;
            soundType = SoundID.Tink;
            soundStyle = 1;
            //Main.soundDig[Type] =  21;

            minPick = int.MaxValue;
            mineResist = int.MaxValue;
            AddMapEntry(new Color(20, 30, 40));
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override bool HasWalkDust()
        {
            return true;
        }

    }
    class MemorialWall : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = false;
            AddMapEntry(new Color(20 / 2, 30 / 2, 40 / 2));
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override void KillWall(int i, int j, ref bool fail)
        {
            fail = true;
        }
    }
    public class MemorialBrickItem : ModItem
    {
        public override string Texture { get { return "Terraria/Item_" + ItemID.GreenBrick; } }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("DEVELOPER TOOL");
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 18;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = mod.TileType("MemorialBrick");
        }
    }
    public class MemorialWallItem : ModItem
    {
        public override string Texture { get { return "Terraria/Item_" + ItemID.GreenBrickWall; } }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("DEVELOPER TOOL");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 18;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createWall = mod.WallType("MemorialWall");
        }
    }
}

