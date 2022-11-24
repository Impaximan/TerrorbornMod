using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace TerrorbornMod.Items.Materials
{
    class TerrorSample : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'It seems to run from you...'");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 8));
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 18;
            Item.height = 26;
            Item.rare = ItemRarityID.Blue;
        }

        Color restlessColor = Color.FromNonPremultiplied(40 * 2, 55 * 2, 70 * 2, 255);
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.Mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.OverrideColor = restlessColor;
                }
            }
        }

        public override void GrabRange(Player player, ref int grabRange)
        {
            grabRange *= 0;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Player player = Main.player[Player.FindClosest(Item.position, Item.width, Item.height)];
            if (Item.Distance(player.Center) <= 75)
            {
                Item.velocity += Item.DirectionTo(player.Center) * -0.135f;
            }
            Item.velocity = Collision.TileCollision(Item.position, Item.velocity, Item.width, Item.height);
        }
    }
}

