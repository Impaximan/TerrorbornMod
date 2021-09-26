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
using Terraria.ModLoader.Config;
using System.ComponentModel;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using TerrorbornMod;
using Terraria.Map;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using TerrorbornMod.UI.TerrorMeter;
using TerrorbornMod.UI.TerrorAbilityUnlock;

namespace TerrorbornMod
{
    [BackgroundColor(40 / 2, 55 / 2, 70 / 2, (int)(255f * 0.75f))]
    class TerrorbornConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public static TerrorbornConfig Instance;

        [Header("QoL features")]

        [DefaultValue(true)]
        [BackgroundColor(40, 55, 70)]
        [Label("Starting Items")]
        [Tooltip("Whether or not the player will spawn with extra items to make early game a more enjoyable experience")]
        public bool startingItems;

        [Header("UI")]

        [DefaultValue(500f)]
        [BackgroundColor(40, 55, 70)]
        [Label("Paragraph width")]
        [Tooltip("The size of each line on lore items' paragraphs")]
        [Range(0, 2000)]
        [Increment(5)]
        [Slider()]
        public int loreParagraphWidth;

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
        {
            return true;
        }

        public override void OnLoaded()
        {
            //alwaysShowSoulChargeMeter = false;
            //soulChargeMeterX = 0.8f;
            //soulChargeMeterY = 0.02f;
            //soulChargeMeterScale = 10;
        }

        public override void OnChanged()
        {
            TerrorbornMod.StartingItems = startingItems;
            TerrorbornMod.LoreParagraphWidth = loreParagraphWidth;
        }
    }
}
