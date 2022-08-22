using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ReLogic.Content;
using Terraria.GameContent.UI;
using Microsoft.Xna.Framework.Audio;
using TerrorbornMod.NPCs.Bosses;
using TerrorbornMod.NPCs.Bosses.InfectedIncarnate;
using TerrorbornMod.NPCs.Bosses.TidalTitan;
using TerrorbornMod.NPCs.Bosses.HexedConstructor;

namespace TerrorbornMod
{
    class TerrorbornMod : Mod
    {
        public const bool IsInTestingMode = false;

        public const float IncendiaryAlloyMultiplier = 0.5f;

        public static float TerrorMeterX = 0.5f;
        public static float TerrorMeterY = 0.06f;

        public static float TwilightMeterX = 0.5f;
        public static float TwilightMeterY = 0.13f;

        public static ModKeybind ArmorAbility;
        public static ModKeybind quickVirus;
        public static ModKeybind ShriekOfHorror;
        public static ModKeybind PrimaryTerrorAbility;
        public static ModKeybind SecondaryTerrorAbility;
        public static ModKeybind OpenTerrorAbilityMenu;
        public static ModKeybind Parry;

        public static int CombatTokenCustomCurrencyId;

        public static bool StartingItems = true;

        public static float LoreParagraphWidth = 500f;

        public static float ScreenDarknessAlpha = 0f;

        public static float titleCardDuration = 3.5f;
        public static bool titleCards = true;

        public static float defeatMessageDuration = 3.5f;
        public static bool defeatMessages = true;

        public static bool thrownAffectsMods = true;
        public static bool showCritDamage = true;

        public static string TerrorMeterStyle = "Default";
        public static bool TerrorMeterText = true;

        public static bool showWingStats = true;

        public static bool showNoUseSpeed = true;

        public static bool InstantDeathProtectionEnabled = true;

        public static Texture2D SoHShrineText;

        //public override void UpdateMusic(ref int music, ref MusicPriority priority)
        //{
        //    if (!Main.gameMenu)
        //    {
        //        Player player = Main.LocalPlayer;
        //        TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

        //        if (modPlayer.ZoneDeimostone)
        //        {
        //            music = GetSoundSlot(SoundType.Music, "Sounds/Music/CreepyCaverns");
        //            priority = MusicPriority.BiomeMedium;
        //        }

        //        if (modPlayer.ZoneIncendiary)
        //        {
        //            music = GetSoundSlot(SoundType.Music, "Sounds/Music/IncendiaryIslands");
        //            priority = MusicPriority.Environment;
        //        }

        //        if (modPlayer.ZoneICU)
        //        {
        //            music = GetSoundSlot(SoundType.Music, "Sounds/Music/ICU");
        //            priority = MusicPriority.Environment;
        //        }

        //        if (TerrorbornSystem.terrorRain && Main.raining && player.ZoneRain)
        //        {
        //            music = GetSoundSlot(SoundType.Music, "Sounds/Music/DarkRain");
        //            priority = MusicPriority.Event;
        //        }
        //    }
        //}

        public TerrorbornMod()
        {
            //Utils.DrawBorderString()
        }

        public virtual void SetColors(ref Color[] colors, int width, int height)
        {
            FastNoiseLite noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

            int centerX = width / 2;
            int centerY = height / 2;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    Vector2 center = new Vector2(centerX, centerY);
                    Vector2 position = new Vector2(i, j);
                    float rotation = (position - center).ToRotation();
                    float distance = Vector2.Distance(position, center);

                    int index = j * width + i;
                    float noiseValue = (noise.GetNoise(rotation * 100f, ((float)Math.Sin(rotation) + 5f) * 20f) + 1f) / 2f;
                    float glowValue = (1f - (distance / (width / 2f)));
                    float value = MathHelper.Lerp(glowValue, noiseValue, distance / (width / 2f)) * glowValue;

                    colors[index] = new Color(value, value, value, value);
                }
            }
        }

        public virtual Texture2D CreateImage(int width, int height)
        {
            var graphics = Main.instance.GraphicsDevice;
            Color[] colors = new Color[width * height];
            Texture2D output = new Texture2D(graphics, width, height, false, SurfaceFormat.Color);
            SetColors(ref colors, width, height);
            output.SetData(colors);
            return output;
        }

        public static List<int> GoldenChestLore = new List<int>();
        private static string savingFolder = Path.Combine(Main.SavePath, "Mods", "Cache");

        public static Texture2D DreadwindTexture;

        public override void Load()
        {
            GoldenChestLore.Clear();

            //Utils.TerrorbornUtils.InvokeOnMainThread(() =>
            //{
            //    Directory.CreateDirectory(savingFolder);
            //    string path = Path.Combine(savingFolder, "Shine.png");
            //    using (Stream stream = File.OpenWrite(path))
            //    {
            //        CreateImage(1000, 1000).SaveAsPng(stream, 1000, 1000);
            //    }
            //});
            

            Utils.Detours.Initialize();

            ArmorAbility = KeybindLoader.RegisterKeybind(this, "ArmorAbility", "Z");
            ShriekOfHorror = KeybindLoader.RegisterKeybind(this, "Shriek of Horror", "Q");
            PrimaryTerrorAbility = KeybindLoader.RegisterKeybind(this, "Primary Terror Ability", "F");
            SecondaryTerrorAbility = KeybindLoader.RegisterKeybind(this, "Secondary Terror Ability", "X");
            quickVirus = KeybindLoader.RegisterKeybind(this, "Quick Spark", "T");
            OpenTerrorAbilityMenu = KeybindLoader.RegisterKeybind(this, "Open/Close Terror Ability Menu", "P");
            Parry = KeybindLoader.RegisterKeybind(this, "Parry", "Mouse4");
            CombatTokenCustomCurrencyId = CustomCurrencyManager.RegisterCurrency(new CombatTokenCurrency(ModContent.ItemType<CombatToken>(), 999L));

            DreadwindTexture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/Wind", AssetRequestMode.ImmediateLoad);

            ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/RiveterSound", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/RiveterDrawSound", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/CoolerMachineGun", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/HexedConstructorDeath", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/Gunfire1", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/PrototypeIBeat", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/PrototypeIExplosion", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/PrototypeIRoar", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/ThunderAmbience", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/TTSplash", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/undertalewarning", AssetRequestMode.ImmediateLoad);
            ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/BossDefeatedSlam", AssetRequestMode.ImmediateLoad);

            ModContent.Request<Texture2D>("TerrorbornMod/MainMenuForeground1", AssetRequestMode.ImmediateLoad);
            ModContent.Request<Texture2D>("TerrorbornMod/WhitePixel", AssetRequestMode.ImmediateLoad);

            string prefix = "TerrorbornMod/Structures/";
            ModContent.Request<Texture2D>(prefix + "SOHShrine", AssetRequestMode.ImmediateLoad);
            ModContent.Request<Texture2D>(prefix + "SOHShrine_HalfBrick", AssetRequestMode.ImmediateLoad);
            ModContent.Request<Texture2D>(prefix + "SOHShrine_SlopeDownLeft", AssetRequestMode.ImmediateLoad);
            ModContent.Request<Texture2D>(prefix + "SOHShrine_SlopeDownRight", AssetRequestMode.ImmediateLoad);
            ModContent.Request<Texture2D>(prefix + "SOHShrine_Walls", AssetRequestMode.ImmediateLoad);

            ModContent.Request<Texture2D>(prefix + "HAShrine", AssetRequestMode.ImmediateLoad);
            ModContent.Request<Texture2D>(prefix + "HAShrine_Walls", AssetRequestMode.ImmediateLoad);

            ModContent.Request<Texture2D>(prefix + "IIArena", AssetRequestMode.ImmediateLoad);
            ModContent.Request<Texture2D>(prefix + "IIArena_Walls", AssetRequestMode.ImmediateLoad);
            ModContent.Request<Texture2D>(prefix + "IIArena_Water", AssetRequestMode.ImmediateLoad);

            ModContent.Request<Texture2D>(prefix + "TWShrine", AssetRequestMode.ImmediateLoad);
            ModContent.Request<Texture2D>(prefix + "TWShrine_Walls", AssetRequestMode.ImmediateLoad);

            ModContent.Request<Texture2D>(prefix + "VBShrine", AssetRequestMode.ImmediateLoad);
            ModContent.Request<Texture2D>(prefix + "VBShrine_Walls", AssetRequestMode.ImmediateLoad);
            ModContent.Request<Texture2D>(prefix + "VBShrine_Lava", AssetRequestMode.ImmediateLoad);
            ModContent.Request<Texture2D>(prefix + "VBShrine_HalfBrick", AssetRequestMode.ImmediateLoad);

            if (Main.netMode != NetmodeID.Server)
            {
                Ref<Effect> screenRef = new Ref<Effect>((Effect)ModContent.Request<Effect>("TerrorbornMod/Effects/ShockwaveEffect", AssetRequestMode.ImmediateLoad));
                Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["Shockwave"].Load();

                Filters.Scene["ParryShockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["ParryShockwave"].Load();

                Ref<Effect> prototypeRef = new Ref<Effect>((Effect)ModContent.Request<Effect>("TerrorbornMod/Effects/PrototypeIShader", AssetRequestMode.ImmediateLoad));
                Filters.Scene["TerrorbornMod:PrototypeIShader"] = new Filter(new ScreenShaderData(prototypeRef, "PrototypeI"), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:PrototypeIShader"].Load();

                Ref<Effect> darknessRef = new Ref<Effect>(ModContent.Request<Effect>("TerrorbornMod/Effects/DarknessShader", AssetRequestMode.ImmediateLoad).Value);
                Filters.Scene["TerrorbornMod:DarknessShader"] = new Filter(new ScreenShaderData(darknessRef, "Darkness"), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:DarknessShader"].Load();

                Ref<Effect> incarnateRef = new Ref<Effect>((Effect)ModContent.Request<Effect>("TerrorbornMod/Effects/IncarnateBoss", AssetRequestMode.ImmediateLoad));
                Filters.Scene["TerrorbornMod:IncarnateBoss"] = new Filter(new ScreenShaderData(incarnateRef, "IncarnateBoss"), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:IncarnateBoss"].Load();

                Ref<Effect> glitchRef = new Ref<Effect>((Effect)ModContent.Request<Effect>("TerrorbornMod/Effects/GlitchShader", AssetRequestMode.ImmediateLoad));
                Filters.Scene["TerrorbornMod:GlitchShader"] = new Filter(new ScreenShaderData(glitchRef, "Glitch"), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:GlitchShader"].Load();

                Ref<Effect> colorlessRef = new Ref<Effect>((Effect)ModContent.Request<Effect>("TerrorbornMod/Effects/ColorlessShader", AssetRequestMode.ImmediateLoad));
                Filters.Scene["TerrorbornMod:ColorlessShader"] = new Filter(new ScreenShaderData(colorlessRef, "Colorless"), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:ColorlessShader"].Load();

                Filters.Scene["TerrorbornMod:BlandnessShader"] = new Filter(new ScreenShaderData(colorlessRef, "Colorless").UseOpacity(0.5f), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:BlandnessShader"].Load();

                Ref<Effect> hexedRef = new Ref<Effect>((Effect)ModContent.Request<Effect>("TerrorbornMod/Effects/HexedMirage", AssetRequestMode.ImmediateLoad));
                Filters.Scene["TerrorbornMod:HexedMirage"] = new Filter(new ScreenShaderData(hexedRef, "HexedMirage"), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:HexedMirage"].Load();

                Ref<Effect> twilightRef = new Ref<Effect>((Effect)ModContent.Request<Effect>("TerrorbornMod/Effects/TwilightShaderNight", AssetRequestMode.ImmediateLoad));
                Filters.Scene["TerrorbornMod:TwilightShaderNight"] = new Filter(new ScreenShaderData(twilightRef, "TwilightShaderNight"), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:TwilightShaderNight"].Load();
            }
        }

        public override void Unload()
        {
            Utils.Detours.Unload();
            //Main.manaTexture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Mana");

            //ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/RiveterDrawSound").Dispose();
            //ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/CoolerMachineGun").Dispose();
            //ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/HexedConstructorDeath").Dispose();
            //ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/Gunfire1").Dispose();
            //ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/PrototypeIBeat").Dispose();
            //ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/PrototypeIExplosion").Dispose();
            //ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/PrototypeIRoar").Dispose();
            //ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/ThunderAmbience").Dispose();
            //ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/TTSplash").Dispose();
            //ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/undertalewarning").Dispose();

            //ModContent.Request<Texture2D>("TerrorbornMod/MainMenuForeground1").Dispose();
            //ModContent.Request<Texture2D>("TerrorbornMod/WhitePixel").Dispose();

            //ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/Wind").Dispose();
        }

        public static Vector2 screenFollowPosition;
        public static float maxScreenLerp;
        public static float currentScreenLerp;
        public static int screenTransitionTime;
        public static int screenFollowTime;
        public static void SetScreenToPosition(int duration, int transitionTime, Vector2 position, float maxLerp = 1f)
        {
            screenFollowTime = duration;
            screenTransitionTime = transitionTime;
            currentScreenLerp = 0f;
            maxScreenLerp = maxLerp;
            screenFollowPosition = position;
        }

        public static void DrawTextureEasy(Vector2 position, Texture2D texture)
        {
            Main.spriteBatch.Begin();
            Main.spriteBatch.Draw(texture, position - Main.screenPosition, null, Color.White, 0f, texture.Size() / 2f, 10f, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
        }

        public override void PostSetupContent()
        {
            ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist);
            if (bossChecklist != null)
            {
                bossChecklist.Call("AddBoss", //Which method is being used
                    this, //Mod instance
                    "Infected Incarnate", //Boss name
                    ModContent.NPCType<InfectedIncarnate>(), //Boss ID
                    1.5f, //Progression placement
                    (Func<bool>)(() => TerrorbornSystem.downedInfectedIncarnate), //Downed bool
                    () => true, //Availability (ie make this false if you want to hide the checklist entry)
                    new List<int>(), // Collectibles
                    new List<int>(), // Summoning items
                    "Once you've obtained Shriek of Horror, find a strange chamber, the entrance to which is found in the snow biome. In the chamber use Shriek of Horror, and your foe will awake.", //Spawn description
                    "And so the battle ended, both incarnates a shadow of their former selves...", //Despawn message, DELETE THIS FOR EVENTS
                    //PUT DRAWCODE FOR THE ENTRY HERE
                    (SpriteBatch sb, Rectangle rect, Color color) => {
                        Texture2D texture = ModContent.Request<Texture2D>("TerrorbornMod/BossChecklist/InfectedIncarnate").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    });

                bossChecklist.Call("AddBoss", //Which method is being used
                    this, //Mod instance
                    "Mysterious Crab", //Boss name
                    ModContent.NPCType<MysteriousCrab>(), //Boss ID
                    3.5f, //Progression placement
                    () => TerrorbornSystem.downedMysteriousCrab, //Downed bool
                    () => true, //Availability (ie make this false if you want to hide the checklist entry)
                    new List<int>(), // Collectibles
                    new List<int>()
                    {
                        { ModContent.ItemType<Items.LunarRitual>() }
                    }, // Summoning items
                    "Spawns in the ocean during the night. Legends say that it's choice food for some mighty aquatic animals...", //Spawn description
                    "Well, there goes the crab.", //Despawn message, DELETE THIS FOR EVENTS
                    //PUT DRAWCODE FOR THE ENTRY HERE
                    (SpriteBatch sb, Rectangle rect, Color color) => {
                        Texture2D texture = ModContent.Request<Texture2D>("TerrorbornMod/BossChecklist/MysteriousCrab").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    });

                bossChecklist.Call("AddBoss", //Which method is being used
                    this, //Mod instance
                    "Azuredire", //Boss name
                    ModContent.NPCType<TidalTitan>(), //Boss ID
                    3.501f, //Progression placement
                    () => TerrorbornSystem.downedTidalTitan, //Downed bool
                    () => TerrorbornSystem.downedMysteriousCrab, //Availability (ie make this false if you want to hide the checklist entry)
                    new List<int>(), // Collectibles
                    new List<int>()
                    {
                        { ModContent.ItemType<Items.LunarRitual>() }
                    }, // Summoning items
                    "Kill a mysterious crab (see the previous entry)", //Spawn description
                    "The mighty aquatic beast returns to its slumber in the depths.", //Despawn message, DELETE THIS FOR EVENTS
                                                  //PUT DRAWCODE FOR THE ENTRY HERE
                    (SpriteBatch sb, Rectangle rect, Color color) => {
                        Texture2D texture = ModContent.Request<Texture2D>("TerrorbornMod/BossChecklist/TidalTitan").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    }); 
                
                bossChecklist.Call("AddBoss", //Which method is being used
                    this, //Mod instance
                    "Dunestock", //Boss name
                    ModContent.NPCType<Dunestock>(), //Boss ID
                    6.5f, //Progression placement
                    () => TerrorbornSystem.downedDunestock, //Downed bool
                    () => true, //Availability (ie make this false if you want to hide the checklist entry)
                    new List<int>(), // Collectibles
                    new List<int>()
                    {
                        { ModContent.ItemType<Items.DriedCanteen>() }
                    }, // Summoning items
                    "Use a [i/s1: " + ModContent.ItemType<Items.DriedCanteen>() + "] anywhere at any time in the desert.", //Spawn description
                    "The starving abomination flies off, looking forward to its next feast.", //Despawn message, DELETE THIS FOR EVENTS
                                                                                      //PUT DRAWCODE FOR THE ENTRY HERE
                    (SpriteBatch sb, Rectangle rect, Color color) => {
                        Texture2D texture = ModContent.Request<Texture2D>("TerrorbornMod/BossChecklist/Dunestock").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    });

                bossChecklist.Call("AddBoss", //Which method is being used
                    this, //Mod instance
                    "Hexed Constructor", //Boss name
                    ModContent.NPCType<HexedConstructor>(), //Boss ID
                    9.5f, //Progression placement
                    () => TerrorbornSystem.downedIncendiaryBoss, //Downed bool
                    () => true, //Availability (ie make this false if you want to hide the checklist entry)
                    new List<int>(), // Collectibles
                    new List<int>()
                    {
                        { ModContent.ItemType<Items.AccursedClock>() }
                    }, // Summoning items
                    "Use an [i/s1: " + ModContent.ItemType<Items.AccursedClock>() + "] within the Sisyphean Islands biome at any time.", //Spawn description
                    "The possessed machine hovers away, its clock still ticking.", //Despawn message, DELETE THIS FOR EVENTS
                                                                                              //PUT DRAWCODE FOR THE ENTRY HERE
                    (SpriteBatch sb, Rectangle rect, Color color) => {
                        Texture2D texture = ModContent.Request<Texture2D>("TerrorbornMod/BossChecklist/HexedConstructor").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    });
            }
        }

        //public override void PostSetupContent()
        //{
        //    Mod yabhb = ModLoader.GetMod("FKBossHealthBar");
        //    if (yabhb != null)
        //    {
        //        yabhb.Call("RegisterHealthBarMini", ModContent.NPCType<NPCs.TerrorRain.FrightcrawlerHead>());
        //        yabhb.Call("RegisterHealthBarMini", ModContent.NPCType<NPCs.SlateBanshee>());
        //        yabhb.Call("RegisterHealthBar", ModContent.NPCType<NPCs.Minibosses.DreadAngel>());
        //    }

        //    Mod fargos = ModLoader.GetMod("Fargowiltas");
        //    if (fargos != null)
        //    {
        //        fargos.Call("AddEventSummon", 1f, "TerrorbornMod", "BrainStorm", (Func<bool>)(() => TerrorbornSystem.downedTerrorRain), Item.buyPrice(0, 15, 0, 0));
        //        fargos.Call("AddSummon", 3.5f, "TerrorbornMod", "LunarRitual", (Func<bool>)(() => TerrorbornSystem.downedTidalTitan), Item.buyPrice(0, 15, 0, 0));
        //        fargos.Call("AddSummon", 5.5f, "TerrorbornMod", "DriedCanteen", (Func<bool>)(() => TerrorbornSystem.downedDunestock), Item.buyPrice(0, 25, 0, 0));
        //        fargos.Call("AddSummon", 9.5f, "TerrorbornMod", "RadioactiveSpiderFood", (Func<bool>)(() => TerrorbornSystem.downedShadowcrawler), Item.buyPrice(0, 15, 0, 0));
        //        fargos.Call("AddSummon", 11.35f, "TerrorbornMod", "PlasmaCore", (Func<bool>)(() => TerrorbornSystem.downedPrototypeI), Item.buyPrice(0, 50, 0, 0));
        //    }

        //    Mod bossChecklist = ModLoader.GetMod("BossChecklist");
        //    if (bossChecklist != null)
        //    {
        //        bossChecklist.Call("AddBossWithInfo", "Infected Incarnate", 1.5f, (Func<bool>)(() => TerrorbornSystem.downedInfectedIncarnate), "Once you've obtained Shriek of Horror, find a strange chamber, the entrance to which is found in the snow biome. In the chamber use Shriek of Horror, and your foe will awake.");
        //        bossChecklist.Call("AddBossWithInfo", "Tidal Titan", 3.5f, (Func<bool>)(() => TerrorbornSystem.downedTidalTitan), "Kill a mysterious crab, which occassionally spawns in the ocean biome during the night.");
        //        bossChecklist.Call("AddBossWithInfo", "Dunestock", 5.5f, (Func<bool>)(() => TerrorbornSystem.downedDunestock), "Use a [i:" + ModContent.ItemType<Items.DriedCanteen>() + "] in the desert.");
        //        bossChecklist.Call("AddBossWithInfo", "Shadowcrawler", 9.5f, (Func<bool>)(() => TerrorbornSystem.downedShadowcrawler), "Use a [i:" + ModContent.ItemType<Items.RadioactiveSpiderFood>() + "] during the night.");
        //        bossChecklist.Call("AddBossWithInfo", "Prototype I", 11.35f, (Func<bool>)(() => TerrorbornSystem.downedPrototypeI), "Use a [i:" + ModContent.ItemType<Items.PlasmaCore>() + "] during the night.");
        //        bossChecklist.Call("AddBossWithInfo", "Hexed Constructor", 7.9f, (Func<bool>)(() => TerrorbornSystem.downedIncendiaryBoss), "Use an [i:" + ModContent.ItemType<Items.AccursedClock>() + "] in the Sisyphean Islands biome. The boss will enrage if you leave the biome.");
        //        bossChecklist.Call("AddMiniBossWithInfo", "Undying Spirit", 6.05f, (Func<bool>)(() => TerrorbornSystem.downedUndyingSpirit), "A strange eratic ghost that 'died' long ago. Spawns occasionally in the corruption: be wary.");
        //        bossChecklist.Call("AddEventWithInfo", "???", -5f, (Func<bool>)(() => TerrorbornSystem.obtainedShriekOfHorror), "Follow the [i:" + ModContent.ItemType<Items.MysteriousCompass>() + "]'s guidance");
        //        bossChecklist.Call("AddMiniBossWithInfo", "Slate Banshee", 0.5f, (Func<bool>)(() => TerrorbornSystem.downedSlateBanshee), "Spawns occasionally in deimostone caves after you've obtained Shriek of Horror. Has an increased spawn chance if you're wearing a [i:" + ModContent.ItemType<Items.Equipable.Accessories.DeimosteelCharm>() + "].");
        //        bossChecklist.Call("AddEventWithInfo", "Astraphobia", 6.06f, (Func<bool>)(() => TerrorbornSystem.downedTerrorRain), "Has a chance to occur instead of rain. Can be manually summoned by using a [i:" + ModContent.ItemType<Items.MiscConsumables.BrainStorm>() + "] during rain.");
        //        bossChecklist.Call("AddMiniBossWithInfo", "Frightcrawler", 6.07f, (Func<bool>)(() => TerrorbornSystem.downedFrightcrawler), "Spawns during the Astraphobia event (see above).");
        //        bossChecklist.Call("AddMiniBossWithInfo", "Dread Angel", 15.05f, (Func<bool>)(() => TerrorbornSystem.downedDreadAngel), "Spawns in the Sisyphean Islands biome after Moon Lord has been defeated.");
        //        bossChecklist.Call("AddEventWithInfo", "Dreadwind", 16f, (Func<bool>)(() => TerrorbornSystem.downedDreadwind), "Not written yet.");
        //        bossChecklist.Call("AddBossWithInfo", "Phobos", 16.01f, (Func<bool>)(() => TerrorbornSystem.downedPhobos), "Spawns at the end of the Dreadwind event.");
        //    }
        //}
    }
}