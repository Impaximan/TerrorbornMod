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
    //class TerrorbornConfig : ModConfig
    //{
    //    public override ConfigScope Mode => ConfigScope.ServerSide;

    //    public static TerrorbornConfig Instance;

    //    [Header("Soul Charge Meter")]

    //    [DefaultValue(80)]
    //    [Range(0, 100)]
    //    [Increment(1)]
    //    [Label("Soul charge meter X")]
    //    [Tooltip("The meter's horizontal position on the screen\nWritten as a percent to the right from the left side of the screen")]
    //    [Slider]
    //    public int soulChargeMeterX;

    //    [DefaultValue(2)]
    //    [Range(0, 100)]
    //    [Increment(1)]
    //    [Label("Soul charge meter Y")]
    //    [Tooltip("The meter's vertical position on the screen\nWritten as a percent down from the top of the screen")]
    //    [Slider]
    //    public int soulChargeMeterY;

    //    [Label("Always show soul charge")]
    //    [Tooltip("Whether or not the soul charge meter is always visible\nToggle off to make it only visible when you have more than 0 soul charge")]
    //    [DefaultValue(false)]
    //    public bool alwaysShowSoulChargeMeter;

    //    [DefaultValue(1)]
    //    [Range(0.5f, 3f)]
    //    [Increment(0.05f)]
    //    [Label("Soul charge meter scale")]
    //    [Tooltip("How large the meter is, one being its normal size")]
    //    [Slider]
    //    public float soulChargeMeterScale;

    //    [Label("Full meter sound effect")]
    //    [Tooltip("Whether or not a sound will be played when you reach 100 soul charge")]
    //    [DefaultValue(true)]
    //    public bool fullMeterSoundEffect;




    //    public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
    //    {
    //        return true;
    //    }

    //    public override void OnLoaded()
    //    {
    //        //alwaysShowSoulChargeMeter = false;
    //        //soulChargeMeterX = 0.8f;
    //        //soulChargeMeterY = 0.02f;
    //        //soulChargeMeterScale = 10;
    //    }

    //    public override void OnChanged()
    //    {
    //        TerrorbornMod.soulChargeMeterPos = new Vector2(Main.screenWidth * ((float)soulChargeMeterX / 100f), Main.screenHeight * ((float)soulChargeMeterY / 100f));
    //        TerrorbornMod.alwaysShowSoulCharge = alwaysShowSoulChargeMeter;
    //        TerrorbornMod.soulChargeMeterScale = soulChargeMeterScale;
    //        TerrorbornMod.fullMeterSound = fullMeterSoundEffect;
    //    }
    //}
}
