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

namespace TerrorbornMod
{
    class TerrorbornMod : Mod
    {
        public const float IncendiaryAlloyMultiplier = 0.5f;

        internal UserInterface terrorMeterInterface;
        internal TerrorMeterUI terrorMeterUI;

        internal UserInterface unlockInterface;
        internal UnlockUI unlockUI;

        internal UserInterface titleCardInterface;
        internal TitleCardUI titleCardUI;

        internal UserInterface terrorMenuInterface;
        internal TerrorAbilityMenu terrorAbilityMenu;

        public static ModHotKey ArmorAbility;
        public static ModHotKey quickVirus;
        public static ModHotKey ShriekOfHorror;
        public static ModHotKey PrimaryTerrorAbility;
        public static ModHotKey SecondaryTerrorAbility;
        public static ModHotKey OpenTerrorAbilityMenu;
        public static ModHotKey Parry;

        public const int foregroundObjectsCount = 500;
        public static ForegroundObject[] foregroundObjects = new ForegroundObject[foregroundObjectsCount];

        public static int CombatTokenCustomCurrencyId;

        public static float screenShaking = 0f;

        public static bool StartingItems = true;

        public static float LoreParagraphWidth = 500f;

        public static float ScreenDarknessAlpha = 0f;

        public static float titleCardDuration = 3.5f;
        public static bool titleCards = true;

        public static bool thrownAffectsMods = true;
        public static bool showCritDamage = true;

        public static string TerrorMeterStyle = "Default";
        public static bool TerrorMeterText = true;

        public static bool showNoUseSpeed = true;

        public static void ScreenShake(float Intensity)
        {
            if (screenShaking < Intensity)
            {
                screenShaking = Intensity;
            }
        }

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (!Main.gameMenu)
            {
                Player player = Main.LocalPlayer;
                TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

                if (modPlayer.ZoneDeimostone)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/CreepyCaverns");
                    priority = MusicPriority.BiomeMedium;
                }

                if (modPlayer.ZoneIncendiary)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/IncendiaryIslands");
                    priority = MusicPriority.Environment;
                }

                if (modPlayer.ZoneICU)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/ICU");
                    priority = MusicPriority.Environment;
                }

                if (TerrorbornWorld.terrorRain && Main.raining && player.ZoneRain)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/DarkRain");
                    priority = MusicPriority.Event;
                }
            }
        }

        public TerrorbornMod()
        {
            //Utils.DrawBorderString()
        }

        string BugString()
        {
            return "Any Bug";
        }

        string MythrilString()
        {
            return "Any Mythril Bar";
        }

        string FragmentString()
        {
            return "Any Lunar Fragment";
        }

        string CobaltString()
        {
            return "Any Cobalt Bar";
        }
        public override void PreSaveAndQuit()
        {
            Main.manaTexture = ModContent.GetTexture("Terraria/Mana");
        }

        public override void AddRecipeGroups()
        {
            //Any Bug
            RecipeGroup bugs = new RecipeGroup(new Func<string>(BugString));
            bugs.ValidItems.Add(ItemID.JuliaButterfly);
            bugs.ValidItems.Add(ItemID.MonarchButterfly);
            bugs.ValidItems.Add(ItemID.PurpleEmperorButterfly);
            bugs.ValidItems.Add(ItemID.RedAdmiralButterfly);
            bugs.ValidItems.Add(ItemID.SulphurButterfly);
            bugs.ValidItems.Add(ItemID.TreeNymphButterfly);
            bugs.ValidItems.Add(ItemID.UlyssesButterfly);
            bugs.ValidItems.Add(ItemID.ZebraSwallowtailButterfly);
            bugs.ValidItems.Add(ItemID.Firefly);
            bugs.ValidItems.Add(ItemID.Buggy);
            bugs.ValidItems.Add(ItemID.Grasshopper);
            bugs.ValidItems.Add(ItemID.Grubby);
            bugs.ValidItems.Add(ItemID.LightningBug);
            RecipeGroup.RegisterGroup("bugs", bugs);

            //Any Mythril Bar
            RecipeGroup mythril = new RecipeGroup(new Func<string>(MythrilString));
            mythril.ValidItems.Add(ItemID.MythrilBar);
            mythril.ValidItems.Add(ItemID.OrichalcumBar);
            RecipeGroup.RegisterGroup("mythril", mythril);

            //Any Lunar Fragment
            RecipeGroup fragment = new RecipeGroup(new Func<string>(FragmentString));
            fragment.ValidItems.Add(ItemID.FragmentSolar);
            fragment.ValidItems.Add(ItemID.FragmentNebula);
            fragment.ValidItems.Add(ItemID.FragmentStardust);
            fragment.ValidItems.Add(ItemID.FragmentVortex);
            fragment.ValidItems.Add(ModContent.ItemType<Items.Materials.FusionFragment>());
            RecipeGroup.RegisterGroup("fragment", fragment);

            RecipeGroup cobalt = new RecipeGroup(new Func<string>(CobaltString));
            cobalt.ValidItems.Add(ItemID.CobaltBar);
            cobalt.ValidItems.Add(ItemID.PalladiumBar);
            RecipeGroup.RegisterGroup("cobalt", cobalt);
        }


        public virtual void SetColors(ref Color[] colors, int width, int height)
        {
            int x = width / 2;
            int y = height / 2;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    int index = j * width + i;
                    float value = 0f;
                    if (Math.Abs(i - x) <= MathHelper.Lerp(0f, width / 2, (float)j / (float)height))
                    {
                        value = MathHelper.Lerp(1f, 0f, (float)j / (float)height);
                    }
                    colors[index] = new Color(value, value, value, value);
                    //double dX = (double)(i - x) / (double)width * 2;
                    //double dY = (double)(j - y) / (double)height * 2;
                    //float c = Math.Abs((float)Math.Sqrt(dX * dX + dY * dY) - 0.85f) / 0.15f;
                    //if (c <= 0)
                    //{
                    //    colors[index] = new Color(1f, 1f, 1f, 1f);
                    //}
                    //else
                    //{
                    //    float value = 1f - (float)c;
                    //    colors[index] = new Color(value, value, value, value);
                    //}
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
        public override void Load()
        {
            GoldenChestLore.Clear();
            Directory.CreateDirectory(savingFolder);
            string path = Path.Combine(savingFolder, "TerrorbornOutput.png");
            using (Stream stream = File.OpenWrite(path))
            {
                CreateImage(200, 1000).SaveAsPng(stream, 200, 1000);
            }

            TBUtils.Detours.Initialize();

            ArmorAbility = RegisterHotKey("ArmorAbility", "Z");
            ShriekOfHorror = RegisterHotKey("Shriek of Horror", "Q");
            PrimaryTerrorAbility = RegisterHotKey("Primary Terror Ability", "F");
            SecondaryTerrorAbility = RegisterHotKey("Secondary Terror Ability", "X");
            quickVirus = RegisterHotKey("Quick Spark", "T");
            OpenTerrorAbilityMenu = RegisterHotKey("Open/Close Terror Ability Menu", "P");
            Parry = RegisterHotKey("Parry", "Mouse4");
            CombatTokenCustomCurrencyId = CustomCurrencyManager.RegisterCurrency(new CombatTokenCurrency(ItemType("CombatToken"), 999L));

            if (Main.netMode != NetmodeID.Server)
            {
                Ref<Effect> screenRef = new Ref<Effect>(GetEffect("Effects/ShockwaveEffect"));
                Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["Shockwave"].Load();

                Filters.Scene["ParryShockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["ParryShockwave"].Load();

                Ref<Effect> prototypeRef = new Ref<Effect>(GetEffect("Effects/Shaders/PrototypeIShader"));
                Filters.Scene["TerrorbornMod:PrototypeIShader"] = new Filter(new ScreenShaderData(prototypeRef, "PrototypeI"), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:PrototypeIShader"].Load();

                Ref<Effect> darknessRef = new Ref<Effect>(GetEffect("Effects/Shaders/DarknessShader"));
                Filters.Scene["TerrorbornMod:DarknessShader"] = new Filter(new ScreenShaderData(darknessRef, "Darkness"), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:DarknessShader"].Load();

                Ref<Effect> incarnateRef = new Ref<Effect>(GetEffect("Effects/Shaders/IncarnateBoss"));
                Filters.Scene["TerrorbornMod:IncarnateBoss"] = new Filter(new ScreenShaderData(incarnateRef, "IncarnateBoss"), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:IncarnateBoss"].Load();

                Ref<Effect> glitchRef = new Ref<Effect>(GetEffect("Effects/Shaders/GlitchShader"));
                Filters.Scene["TerrorbornMod:GlitchShader"] = new Filter(new ScreenShaderData(glitchRef, "Glitch"), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:GlitchShader"].Load();

                Ref<Effect> colorlessRef = new Ref<Effect>(GetEffect("Effects/Shaders/ColorlessShader"));
                Filters.Scene["TerrorbornMod:ColorlessShader"] = new Filter(new ScreenShaderData(colorlessRef, "Colorless"), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:ColorlessShader"].Load();

                Filters.Scene["TerrorbornMod:BlandnessShader"] = new Filter(new ScreenShaderData(colorlessRef, "Colorless").UseOpacity(0.5f), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:BlandnessShader"].Load();

                Ref<Effect> hexedRef = new Ref<Effect>(GetEffect("Effects/Shaders/HexedMirage"));
                Filters.Scene["TerrorbornMod:HexedMirage"] = new Filter(new ScreenShaderData(hexedRef, "HexedMirage"), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:HexedMirage"].Load();

                Ref<Effect> twilightRef = new Ref<Effect>(GetEffect("Effects/Shaders/TwilightShaderNight"));
                Filters.Scene["TerrorbornMod:TwilightShaderNight"] = new Filter(new ScreenShaderData(twilightRef, "TwilightShaderNight"), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:TwilightShaderNight"].Load();
            }

            if (!Main.dedServ)
            {
                terrorMeterInterface = new UserInterface();
                unlockInterface = new UserInterface();
                terrorMenuInterface = new UserInterface();
                titleCardInterface = new UserInterface();

                terrorMeterUI = new TerrorMeterUI();
                terrorMeterUI.Activate();
                
                unlockUI = new UnlockUI();
                unlockUI.Activate();

                terrorAbilityMenu = new TerrorAbilityMenu();
                terrorAbilityMenu.Activate();

                titleCardUI = new TitleCardUI();
                titleCardUI.Activate();
            }
        }

        public override void Unload()
        {
            TBUtils.Detours.Unload();
            Main.rainTexture = ModContent.GetTexture("Terraria/Rain");
            Main.manaTexture = ModContent.GetTexture("Terraria/Mana");
        }

        private GameTime _lastUpdateUiGameTime;
        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (terrorMeterInterface?.CurrentState != null)
            {
                terrorMeterInterface.Update(gameTime);
            }
            if (titleCardInterface?.CurrentState != null)
            {
                titleCardInterface.Update(gameTime);
            }
            if (unlockInterface?.CurrentState != null)
            {
                unlockInterface.Update(gameTime);
            }
            if (terrorMenuInterface?.CurrentState != null)
            {
                terrorMenuInterface.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "TerrorbornMod: terrorMeterInterface",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && terrorMeterInterface?.CurrentState != null)
                        {
                            terrorMeterInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                       InterfaceScaleType.UI));

                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "TerrorbornMod: unlockInterface",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && unlockInterface?.CurrentState != null)
                        {
                            unlockInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                       InterfaceScaleType.UI));

                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "TerrorbornMod: terrorMenuInterface",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && terrorMenuInterface?.CurrentState != null)
                        {
                            terrorMenuInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                       InterfaceScaleType.UI));

                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "TerrorbornMod: titleCardInterface",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && titleCardInterface?.CurrentState != null)
                        {
                            titleCardInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                       InterfaceScaleType.UI));
            }
        }

        public static void TerrorThunder()
        {
            positionLightning = 1f;
            //transitionColor = Color.FromNonPremultiplied((int)(209f), (int)(138f), (int)(255f), 255);
            ScreenShake(10);
            ModContent.GetSound("TerrorbornMod/Sounds/Effects/ThunderAmbience").Play(Main.ambientVolume, Main.rand.NextFloat(-0.25f, 0.25f), Main.rand.NextFloat(-0.3f, 0.3f));
        }

        public static void p1Thunder()
        {
            if (timeSinceLightning > 10)
            {
                ModContent.GetSound("TerrorbornMod/Sounds/Effects/ThunderAmbience").Play(Main.soundVolume * 0.75f, Main.rand.NextFloat(0.75f, 1f), Main.rand.NextFloat(-0.3f, 0.3f));
            }
            timeSinceLightning = 0;
            positionLightningP1 = 1f;
            //transitionColor = Color.FromNonPremultiplied((int)(209f), (int)(138f), (int)(255f), 255);
            ScreenShake(15);
        }

        public static Color darkRainColor = Color.FromNonPremultiplied((int)(40f * 0.7f), (int)(55f * 0.7f), (int)(70f * 0.7f), 255);
        public static Color incendiaryColor = Color.FromNonPremultiplied(191, 122, 122, 255);
        public static Color transitionColor = Color.White;
        public static Color lightningColor = Color.FromNonPremultiplied((int)(209f), (int)(138f), (int)(255f), 255);
        public static Color p1LightningColor = Color.FromNonPremultiplied((int)(143f), (int)(255f), (int)(191f), 255);
        public static float positionForward = 0f;
        public static float positionBackward = 0f;
        public static float positionLightning = 0f;
        public static float transitionTime = 600f;
        public static float positionLightningP1 = 0f;

        public static Color bossColor;
        public static float bossColorLerp = 0f;

        public override void ModifyLightingBrightness(ref float scale)
        {
            if (TerrorbornWorld.terrorRain && Main.raining && Main.LocalPlayer.ZoneRain && !Main.dayTime)
            {
                scale *= 0.92f;
            }
        }

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            Player player = Main.LocalPlayer;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (TerrorbornWorld.terrorRain && Main.raining)
            {
                positionBackward = 0f;
                if (positionForward < 1f)
                {
                    positionForward += 1f / transitionTime;
                }
                else
                {
                    positionForward = 1f;
                }
                transitionColor = Color.Lerp(transitionColor, darkRainColor, positionForward);
                tileColor = tileColor.MultiplyRGBA(transitionColor);
                backgroundColor = backgroundColor.MultiplyRGBA(transitionColor);
                if (positionLightning > 0f)
                {
                    positionLightning -= 1f / 30f;
                    backgroundColor = Color.Lerp(backgroundColor, lightningColor, positionLightning);
                }
            }
            else if (transitionColor != Color.White)
            {
                positionForward = 0f;
                if (positionBackward < 1f)
                {
                    positionBackward += 1f / transitionTime;
                }
                else
                {
                    positionBackward = 1f;
                }
                transitionColor = Color.Lerp(transitionColor, Color.White, positionBackward);
                tileColor = tileColor.MultiplyRGBA(transitionColor);
                backgroundColor = backgroundColor.MultiplyRGBA(transitionColor);
            }

            if (!Main.gameMenu && modPlayer.ZoneIncendiary)
            {
                positionBackward = 0f;
                if (positionForward < 1f)
                {
                    positionForward += 1f / transitionTime;
                }
                else
                {
                    positionForward = 1f;
                }
                transitionColor = Color.Lerp(transitionColor, incendiaryColor, positionForward);
                tileColor = tileColor.MultiplyRGBA(transitionColor);
                backgroundColor = backgroundColor.MultiplyRGBA(transitionColor);
                if (positionLightning > 0f)
                {
                    positionLightning -= 1f / 30f;
                    backgroundColor = Color.Lerp(backgroundColor, lightningColor, positionLightning);
                }
            }
            else if (transitionColor != Color.White)
            {
                positionForward = 0f;
                if (positionBackward < 1f)
                {
                    positionBackward += 1f / transitionTime;
                }
                else
                {
                    positionBackward = 1f;
                }
                transitionColor = Color.Lerp(transitionColor, Color.White, positionBackward);
                tileColor = tileColor.MultiplyRGBA(transitionColor);
                backgroundColor = backgroundColor.MultiplyRGBA(transitionColor);
            }

            bool changingToBossColor = false;

            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.PrototypeI.PrototypeI>()))
            {
                changingToBossColor = true;
                bossColor = Color.LightBlue;
            }

            if (changingToBossColor)
            {
                if (bossColorLerp < 1f)
                {
                    bossColorLerp += 1f / 60f;
                }
            }
            else
            {
                if (bossColorLerp > 0f)
                {
                    bossColorLerp -= 1f / 60f;
                }
            }

            backgroundColor = backgroundColor.MultiplyRGBA(Color.Lerp(Color.White, bossColor, bossColorLerp));

            if (positionLightningP1 > 0f)
            {
                positionLightningP1 -= 1f / 30f;
                backgroundColor = Color.Lerp(backgroundColor, p1LightningColor, positionLightningP1);
            }

            //if (!Main.gameMenu)
            //{
            //    Main.NewText(bossColorLerp);
            //}
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

        public static void UpdateForegroundObjects()
        {
            foreach(ForegroundObject fObject in foregroundObjects)
            {
                if (fObject != null)
                {
                    fObject.Update();
                }
            }
        }

        static int timeSinceLightning = 0;
        public override void PostUpdateEverything()
        {
            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.PrototypeI.PrototypeI>()))
            {
                timeSinceLightning++;
            }
            TerrorbornUtils.Update();

            UpdateForegroundObjects();

            ShowUI();

            screenShaking *= 0.95f;
            if ((int)Math.Round(screenShaking) == 0)
            {
                screenShaking = 0;
            }
        }

        internal void ShowUI()
        {
            unlockInterface?.SetState(unlockUI);
            terrorMenuInterface?.SetState(terrorAbilityMenu);
            terrorMeterInterface?.SetState(terrorMeterUI);
            titleCardInterface?.SetState(titleCardUI);
        }

        internal void HideUI()
        {
            unlockInterface?.SetState(null);
            terrorMenuInterface?.SetState(null);
            terrorMeterInterface?.SetState(null);
            titleCardInterface?.SetState(null);
        }

        public static void DrawTextureEasy(Vector2 position, Texture2D texture)
        {
            Main.spriteBatch.Begin();
            Main.spriteBatch.Draw(texture, position - Main.screenPosition, null, Color.White, 0f, texture.Size() / 2f, 10f, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
        }

        public override void PostSetupContent()
        {
            Mod yabhb = ModLoader.GetMod("FKBossHealthBar");
            if (yabhb != null)
            {
                yabhb.Call("RegisterHealthBarMini", ModContent.NPCType<NPCs.TerrorRain.FrightcrawlerHead>());
                yabhb.Call("RegisterHealthBarMini", ModContent.NPCType<NPCs.SlateBanshee>());
                yabhb.Call("RegisterHealthBar", ModContent.NPCType<NPCs.Minibosses.DreadAngel>());
            }

            Mod fargos = ModLoader.GetMod("Fargowiltas");
            if (fargos != null)
            {
                fargos.Call("AddEventSummon", 1f, "TerrorbornMod", "BrainStorm", (Func<bool>)(() => TerrorbornWorld.downedTerrorRain), Item.buyPrice(0, 15, 0, 0));
                fargos.Call("AddSummon", 3.5f, "TerrorbornMod", "LunarRitual", (Func<bool>)(() => TerrorbornWorld.downedTidalTitan), Item.buyPrice(0, 15, 0, 0));
                fargos.Call("AddSummon", 5.5f, "TerrorbornMod", "DriedCanteen", (Func<bool>)(() => TerrorbornWorld.downedDunestock), Item.buyPrice(0, 25, 0, 0));
                fargos.Call("AddSummon", 9.5f, "TerrorbornMod", "RadioactiveSpiderFood", (Func<bool>)(() => TerrorbornWorld.downedShadowcrawler), Item.buyPrice(0, 15, 0, 0));
                fargos.Call("AddSummon", 11.35f, "TerrorbornMod", "PlasmaCore", (Func<bool>)(() => TerrorbornWorld.downedPrototypeI), Item.buyPrice(0, 50, 0, 0));
            }

            Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            if (bossChecklist != null)
            {
                bossChecklist.Call("AddBossWithInfo", "Infected Incarnate", 1.5f, (Func<bool>)(() => TerrorbornWorld.downedInfectedIncarnate), "Once you've obtained Shriek of Horror, find a strange chamber, the entrance to which is found in the snow biome. In the chamber use Shriek of Horror, and your foe will awake.");
                bossChecklist.Call("AddBossWithInfo", "Tidal Titan", 3.5f, (Func<bool>)(() => TerrorbornWorld.downedTidalTitan), "Kill a mysterious crab, which occassionally spawns in the ocean biome during the night.");
                bossChecklist.Call("AddBossWithInfo", "Dunestock", 5.5f, (Func<bool>)(() => TerrorbornWorld.downedDunestock), "Use a [i:" + ModContent.ItemType<Items.DriedCanteen>() + "] in the desert.");
                bossChecklist.Call("AddBossWithInfo", "Shadowcrawler", 9.5f, (Func<bool>)(() => TerrorbornWorld.downedShadowcrawler), "Use a [i:" + ModContent.ItemType<Items.RadioactiveSpiderFood>() + "] during the night.");
                bossChecklist.Call("AddBossWithInfo", "Prototype I", 11.35f, (Func<bool>)(() => TerrorbornWorld.downedPrototypeI), "Use a [i:" + ModContent.ItemType<Items.PlasmaCore>() + "] during the night.");
                bossChecklist.Call("AddBossWithInfo", "Hexed Constructor", 7.9f, (Func<bool>)(() => TerrorbornWorld.downedIncendiaryBoss), "Use an [i:" + ModContent.ItemType<Items.AccursedClock>() + "] in the Sisyphean Islands biome. The boss will enrage if you leave the biome.");
                bossChecklist.Call("AddMiniBossWithInfo", "Undying Spirit", 6.05f, (Func<bool>)(() => TerrorbornWorld.downedUndyingSpirit), "A strange eratic ghost that 'died' long ago. Spawns occasionally in the corruption: be wary.");
                bossChecklist.Call("AddEventWithInfo", "???", -5f, (Func<bool>)(() => TerrorbornWorld.obtainedShriekOfHorror), "Follow the [i:" + ModContent.ItemType<Items.MysteriousCompass>() + "]'s guidance");
                bossChecklist.Call("AddMiniBossWithInfo", "Slate Banshee", 0.5f, (Func<bool>)(() => TerrorbornWorld.downedSlateBanshee), "Spawns occasionally in deimostone caves after you've obtained Shriek of Horror. Has an increased spawn chance if you're wearing a [i:" + ModContent.ItemType<Items.Equipable.Accessories.DeimosteelCharm>() + "].");
                bossChecklist.Call("AddEventWithInfo", "Astraphobia", 6.06f, (Func<bool>)(() => TerrorbornWorld.downedTerrorRain), "Has a chance to occur instead of rain. Can be manually summoned by using a [i:" + ModContent.ItemType<Items.MiscConsumables.BrainStorm>() + "] during rain.");
                bossChecklist.Call("AddMiniBossWithInfo", "Frightcrawler", 6.07f, (Func<bool>)(() => TerrorbornWorld.downedFrightcrawler), "Spawns during the Astraphobia event (see above).");
                bossChecklist.Call("AddMiniBossWithInfo", "Dread Angel", 15.05f, (Func<bool>)(() => TerrorbornWorld.downedDreadAngel), "Spawns in the Sisyphean Islands biome after Moon Lord has been defeated.");
                bossChecklist.Call("AddEventWithInfo", "Dreadwind", 16f, (Func<bool>)(() => TerrorbornWorld.downedDreadwind), "Not written yet.");
                bossChecklist.Call("AddBossWithInfo", "Uriel", 16.01f, (Func<bool>)(() => TerrorbornWorld.downedUriel), "Spawns at the end of the Dreadwind event.");
            }
        }
    }
}