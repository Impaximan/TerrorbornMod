﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Events;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace TerrorbornMod.Items
{
    class DustwindRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Use to start a sandstorm" +
                "\nIf a sandstorm is already happening, it will end the sandstorm");
        }
        public override void SetDefaults()
        {
            Item.expert = true;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.UseSound = SoundID.Item117;
            Item.value = Item.sellPrice(0, 5, 0, 0);
        }
        public override bool CanUseItem(Player player)
        {
            if (Sandstorm.Happening)
            {
                Main.NewText("The desert winds calm down...", Color.FromNonPremultiplied(175, 75, 255, 255));
                Sandstorm.TimeLeft = 1;
            }
            else
            {
                Main.NewText("A sandstorm has begun!", Color.FromNonPremultiplied(175, 75, 255, 255));
                Sandstorm.TimeLeft = 3600 * 10;
                typeof(Sandstorm).GetMethod("StartSandstorm", BindingFlags.Static | BindingFlags.NonPublic).Invoke((object)null, (object[])null);
            }
            return base.CanUseItem(player);
        }
    }
}



