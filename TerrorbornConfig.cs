﻿using Terraria.ModLoader.Config;
using System.ComponentModel;

namespace TerrorbornMod
{
    [BackgroundColor(40 / 2, 55 / 2, 70 / 2, (int)(255f * 0.75f))]
    class TerrorbornConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public static TerrorbornConfig Instance;

        [Header("QoL features")]

        [DefaultValue(true)]
        [BackgroundColor(40, 55, 70)]
        [Label("Starting Items")]
        [Tooltip("Whether or not the player will spawn with extra items to make early game a more enjoyable experience")]
        public bool startingItems;

        [DefaultValue(true)]
        [BackgroundColor(40, 55, 70)]
        [Label("Show wing stats")]
        [Tooltip("Whether or not Terrorborn will show specific stats automatically from wings" +
            "\nIt is recommended you disable this if you have other mods enabled that do the same thing")]
        public bool showWingStats;

        [DefaultValue(true)]
        [BackgroundColor(40, 55, 70)]
        [Label("Instant Death Protection")]
        [Tooltip("Whether or not the 'Instant Death Protection' mechanic is enabled" +
            "\nWhile enabled, you cannot die from a hit if you are over 90% of your max hp" +
            "\nInstead, you would get increased iframes and be set to 10% hp" +
            "\nNote that this is never enabled when either Skeletron or the Empress of Light is nearby and it's day time" +
            "\nDoes not affect the dungeon guardian either")]
        public bool enableInstantDeathProtection;

        [Header("UI")]

        [DefaultValue(750)]
        [BackgroundColor(40, 55, 70)]
        [Label("Paragraph width")]
        [Tooltip("The size of each line on lore items' paragraphs")]
        [Range(500, 1500)]
        [ReloadRequired]
        [Increment(5)]
        [Slider()]
        public int loreParagraphWidth;

        [DefaultValue(true)]
        [BackgroundColor(40, 55, 70)]
        [Label("Boss title cards")]
        [Tooltip("Whether or not bosses will have title cards that appear at the top of your screen when spawning")]
        public bool titleCards;

        [DefaultValue(3.5f)]
        [BackgroundColor(40, 55, 70)]
        [Label("Boss title card duration")]
        [Tooltip("How long boss title cards will be visible (in seconds) before dissappearing")]
        [Range(1f, 10f)]
        [Increment(0.1f)]
        [Slider()]
        public float titleCardTime;

        [DefaultValue(true)]
        [BackgroundColor(40, 55, 70)]
        [Label("Boss slain UI")]
        [Tooltip("Whether or not killing a boss for the first time will give you a special UI event")]
        public bool defeatMessages;

        [DefaultValue(5.5f)]
        [BackgroundColor(40, 55, 70)]
        [Label("Boss slain UI duration")]
        [Tooltip("How long the boss slain UI lasts before disappearing")]
        [Range(1f, 10f)]
        [Increment(0.1f)]
        [Slider()]
        public float defeatMessageTime;

        [DefaultValue(true)]
        [BackgroundColor(40, 55, 70)]
        [Label("Show critical strike damage in tooltips")]
        [Tooltip("Whether or not item tooltips will contain critical strike damage")]
        public bool showCritDamage;

        [DefaultValue(true)]
        [BackgroundColor(40, 55, 70)]
        [Label("No use speed modifiers tooltip")]
        [Tooltip("Whether or not item tooltips can contain text saying 'Unaffected by external item use speed modifiers' to indicate that this is the case")]
        public bool showNoUseSpeed;

        [BackgroundColor(40, 55, 70)]
        [Label("Terror meter style")]
        [Tooltip("How the terror meter looks")]
        [DefaultValue("Default")]
        [Slider()]
        [OptionStrings(new string[5]{
            "Default",
            "Filled In",
            "Legacy",
            "Text Only (Smooth)",
            "Text Only (Instant)"
        })]
        public string TerrorMeterStyle;

        [DefaultValue(true)]
        [BackgroundColor(40, 55, 70)]
        [Label("Terror meter text")]
        [Tooltip("Whether or not the terror meter will display how much terror you have in text")]
        public bool TerrorMeterText;

        [DefaultValue(0.5f)]
        [BackgroundColor(40, 55, 70)]
        [Label("Terror meter X")]
        [Tooltip("The X position of the terror meter on your screen")]
        [Slider]
        [Increment(0.005f)]
        [Range(0f, 1f)]
        public float TerrorMeterX;

        [DefaultValue(0.06f)]
        [BackgroundColor(40, 55, 70)]
        [Label("Terror meter Y")]
        [Tooltip("The Y position of the terror meter on your screen")]
        [Slider]
        [Increment(0.005f)]
        [Range(0f, 1f)]
        public float TerrorMeterY;

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
            TerrorbornMod.LoreParagraphWidth = loreParagraphWidth;
            TerrorbornMod.titleCardDuration = titleCardTime;
            TerrorbornMod.titleCards = titleCards;
            TerrorbornMod.thrownAffectsMods = thrownAffectsMods;
            TerrorbornMod.TerrorMeterStyle = TerrorMeterStyle;
            TerrorbornMod.TerrorMeterText = TerrorMeterText;
            TerrorbornMod.showNoUseSpeed = showNoUseSpeed;
            TerrorbornMod.TerrorMeterX = TerrorMeterX;
            TerrorbornMod.TerrorMeterY = TerrorMeterY;
            TerrorbornMod.showWingStats = showWingStats;
            TerrorbornMod.InstantDeathProtectionEnabled = enableInstantDeathProtection;
            TerrorbornMod.defeatMessages = defeatMessages;
            TerrorbornMod.defeatMessageDuration = defeatMessageTime;
        }
    }

    [BackgroundColor(135 / 5, 59 / 5, 212 / 5, (int)(255f * 0.75f))]
    class TwilightModeConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("UI")]
        [DefaultValue(0.5f)]
        [BackgroundColor(210, 92, 255)]
        [Label("Twilight meter X")]
        [Tooltip("The X position of the twilight meter on your screen")]
        [Slider]
        [Increment(0.005f)]
        [Range(0f, 1f)]
        public float TwilightMeterX;

        [DefaultValue(0.13f)]
        [BackgroundColor(210, 92, 255)]
        [Label("Terror meter Y")]
        [Tooltip("The Y position of the twilight meter on your screen")]
        [Slider]
        [Increment(0.005f)]
        [Range(0f, 1f)]
        public float TwilightMeterY;

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
        {
            return true;
        }

        public override void OnChanged()
        {
            TerrorbornMod.TwilightMeterX = TwilightMeterX;
            TerrorbornMod.TwilightMeterY = TwilightMeterY;
        }
    }
}
