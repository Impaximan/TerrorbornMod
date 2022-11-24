using Terraria.ID;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class SoulOfPlight : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul of Plight");
            Tooltip.SetDefault("'The essence of an ancient menace'");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 1, 5, 0);
            Item.rare = ItemRarityID.Pink;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.Green.ToVector3() * Main.essScale);
        }
    }
}
