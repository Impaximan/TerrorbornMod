using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.World.Generation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items
{
    class CrackedTimeChime : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Changes the time from day to night, or vice versa" +
                "\nRight click to instead move time forward");
        }

        public override void SetDefaults()
        {
            item.rare = ItemRarityID.Pink;
            item.autoReuse = false;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.useTime = 20;
            item.useAnimation = 20;
            item.width = 20;
            item.height = 26;
            item.UseSound = SoundID.Item67;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.autoReuse = true;
                Main.time += 1200;
                item.UseSound = SoundID.Item28;
            }
            else
            {
                item.UseSound = SoundID.Item67;
                item.autoReuse = false;
                Main.dayTime = !Main.dayTime;
                Main.time = 0;
                TerrorbornMod.ScreenShake(10f);
            }
            return base.CanUseItem(player);
        }
    }
}
