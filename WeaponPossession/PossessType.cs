using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.World.Generation;
using Terraria.ModLoader;
using Terraria.UI;
using TerrorbornMod;
using Terraria.Map;
using TerrorbornMod.ForegroundObjects;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using TerrorbornMod.Effects.Shaders;
using TerrorbornMod.UI.TerrorMeter;
using TerrorbornMod.UI.TerrorAbilityUnlock;
using TerrorbornMod.UI.TitleCard;

namespace TerrorbornMod.WeaponPossession
{
    public static class PossessType
    {
        public static int None = 0;
        public static int Light = 1;
        public static int Night = 2;
        public static int Flight = 3;
        public static int Fright = 4;
        public static int Might = 5;
        public static int Sight = 6;
        public static int Plight = 7;

        public static string ToString(int type)
        {
            switch (type)
            {
                case 1:
                    return "Light";
                case 2:
                    return "Night";
                case 3:
                    return "Flight";
                case 4:
                    return "Fright";
                case 5:
                    return "Might";
                case 6:
                    return "Sight";
                case 7:
                    return "Plight";
                default:
                    return "None";
            }
        }

        public static int ToItemType(int type)
        {
            switch (type)
            {
                case 1:
                    return ItemID.SoulofLight;
                case 2:
                    return ItemID.SoulofNight;
                case 3:
                    return ItemID.SoulofFlight;
                case 4:
                    return ItemID.SoulofFright;
                case 5:
                    return ItemID.SoulofMight;
                case 6:
                    return ItemID.SoulofSight;
                case 7:
                    return ModContent.ItemType<Items.Materials.SoulOfPlight>();
                default:
                    return ItemID.DirtBlock;
            }
        }

        public static Color ToColor(int type)
        {
            switch (type)
            {
                case 1:
                    return Color.HotPink;
                case 2:
                    return Color.MediumPurple;
                case 3:
                    return Color.Cyan;
                case 4:
                    return Color.OrangeRed;
                case 5:
                    return Color.RoyalBlue;
                case 6:
                    return Color.LightGreen;
                case 7:
                    return Color.LimeGreen;
                default:
                    return Color.White;
            }
        }
    }
}
