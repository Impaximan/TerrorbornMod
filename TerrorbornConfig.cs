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

        //[DefaultValue(500f)]
        //[BackgroundColor(40, 55, 70)]
        //[Label("Paragraph width")]
        //[Tooltip("The size of each line on lore items' paragraphs")]
        //[Range(0, 2000)]
        //[Increment(5)]
        //[Slider()]
        //public int loreParagraphWidth;

        [DefaultValue(true)]
        [BackgroundColor(40, 55, 70)]
        [Label("Boss title cards")]
        [Tooltip("Whether or not bosses will have title cards that appear at the top of your screen when spawning")]
        public bool titleCards;

        [DefaultValue(true)]
        [BackgroundColor(40, 55, 70)]
        [Label("Show critical strike damage in tooltips")]
        [Tooltip("Whether or not item tooltips will contain critical strike damage")]
        public bool showCritDamage;

        [DefaultValue(3.5f)]
        [BackgroundColor(40, 55, 70)]
        [Label("Boss title card duration")]
        [Tooltip("How long boss title cards will be visible (in seconds) before dissappearing")]
        [Range(0, 10f)]
        [Increment(0.1f)]
        [Slider()]
        public float titleCardTime;

        [Header("Mechanics")]

        [DefaultValue(true)]
        [BackgroundColor(40, 55, 70)]
        [Label("Other mod weapons can be thrown")]
        [Tooltip("Whether or not weapons from other mods can be affected by this mod's thrown weapon system")]
        public bool thrownAffectsMods;

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
            TerrorbornMod.showCritDamage = showCritDamage;
            //TerrorbornMod.LoreParagraphWidth = loreParagraphWidth;
            TerrorbornMod.titleCardDuration = titleCardTime;
            TerrorbornMod.titleCards = titleCards;
            TerrorbornMod.thrownAffectsMods = thrownAffectsMods;
        }
    }
}
