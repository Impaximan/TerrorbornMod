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
            RecipeGroup.RegisterGroup("fragment", fragment);
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
                    float distanceUntilFade = 0.3f;
                    double dX = (double)(i - x) / (double)width;
                    double dY = (double)(j - y) / (double)width;
                    float c = (float)Math.Sqrt(dX * dX + dY * dY) * 2f;
                    c -= distanceUntilFade;
                    if (c < 0f)
                    {
                        c = 0f;
                    }
                    colors[index] = Color.Lerp(Color.Lerp(Color.Yellow, Color.OrangeRed, c / (float)(1f - distanceUntilFade)), Color.Transparent, c / (float)(1f - distanceUntilFade));
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

        private static string savingFolder = Path.Combine(Main.SavePath, "Mods", "Cache");
        public override void Load()
        {
            Directory.CreateDirectory(savingFolder);
            string path = Path.Combine(savingFolder, "TerrorbornOutput.png");
            //using (Stream stream = File.OpenWrite(path))
            //{
            //    CreateImage(1000, 1000).SaveAsPng(stream, 1000, 1000);
            //}

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

                Ref<Effect> dunestockRef = new Ref<Effect>(GetEffect("Effects/Shaders/DunestockShader"));
                Filters.Scene["TerrorbornMod:DunestockShader"] = new Filter(new ScreenShaderData(dunestockRef, "Dunestock"), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:DunestockShader"].Load();

                Ref<Effect> glitchRef = new Ref<Effect>(GetEffect("Effects/Shaders/GlitchShader"));
                Filters.Scene["TerrorbornMod:GlitchShader"] = new Filter(new ScreenShaderData(glitchRef, "Glitch"), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:GlitchShader"].Load();

                Ref<Effect> colorlessRef = new Ref<Effect>(GetEffect("Effects/Shaders/ColorlessShader"));
                Filters.Scene["TerrorbornMod:ColorlessShader"] = new Filter(new ScreenShaderData(colorlessRef, "Colorless"), EffectPriority.VeryHigh);
                Filters.Scene["TerrorbornMod:ColorlessShader"].Load();
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

        public static Color darkRainColor = Color.FromNonPremultiplied((int)(40f * 0.7f), (int)(55f * 0.7f), (int)(70f * 0.7f), 255);
        public static Color incendiaryColor = Color.FromNonPremultiplied(191, 122, 122, 255) * 0.65f;
        public static Color transitionColor = Color.White;
        public static Color lightningColor = Color.FromNonPremultiplied((int)(209f), (int)(138f), (int)(255f), 255);
        public static float positionForward = 0f;
        public static float positionBackward = 0f;
        public static float positionLightning = 0f;
        public static float transitionTime = 600f;

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

            //if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.PrototypeI>()))
            //{
            //    changingToBossColor = true;
            //    bossColor = Color.LightGreen;
            //}

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

        public override void PostUpdateEverything()
        {
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
                yabhb.Call("RegisterHealthBar", ModContent.NPCType<NPCs.Bosses.Sangrune>());
                yabhb.Call("RegisterHealthBarMini", ModContent.NPCType<NPCs.TerrorRain.FrightcrawlerHead>());
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
                bossChecklist.Call("AddBossWithInfo", "Tidal Titan", 3.5f, (Func<bool>)(() => TerrorbornWorld.downedTidalTitan), "Kill a mysterious crab, which rarely spawns in the ocean biome during the night. Despawns if it sinks back into the water (a layer of platforms over the ocean is recommended). Note: doesn't despawn when it becomes day");
                bossChecklist.Call("AddBossWithInfo", "Dunestock", 5.5f, (Func<bool>)(() => TerrorbornWorld.downedDunestock), "Use a [i:" + ModContent.ItemType<Items.DriedCanteen>() + "] in the desert.");
                bossChecklist.Call("AddBossWithInfo", "Shadowcrawler", 9.5f, (Func<bool>)(() => TerrorbornWorld.downedShadowcrawler), "Use a [i:" + ModContent.ItemType<Items.RadioactiveSpiderFood>() + "] during the night.");
                bossChecklist.Call("AddBossWithInfo", "Prototype I", 11.35f, (Func<bool>)(() => TerrorbornWorld.downedPrototypeI), "Use a [i:" + ModContent.ItemType<Items.PlasmaCore>() + "] during the night.");
                bossChecklist.Call("AddMiniBossWithInfo", "Sangrune", 3.25f, (Func<bool>)(() => TerrorbornWorld.downedSangrune), "Spawns during a blood moon after the eater of worlds/brain of cthulhu have been defeated.");
                bossChecklist.Call("AddMiniBossWithInfo", "Sangrune (hardmode)", 7.5f, (Func<bool>)(() => TerrorbornWorld.downedSangrune2), "Re-fight Sangrune after the Wall of Flesh has been defeated.");
                bossChecklist.Call("AddBossWithInfo", "Hexed Constructor", 7.9f, (Func<bool>)(() => TerrorbornWorld.downedIncendiaryBoss), "Use an [i:" + ModContent.ItemType<Items.AccursedClock>() + "] in the Sisyphean Islands biome. The boss will enrage if you leave the biome.");
                bossChecklist.Call("AddMiniBossWithInfo", "Undying Spirit", 6.05f, (Func<bool>)(() => TerrorbornWorld.downedUndyingSpirit), "A strange eratic ghost that 'died' long ago. Spawns occasionally in the corruption: be wary.");
                bossChecklist.Call("AddEventWithInfo", "???", -5f, (Func<bool>)(() => TerrorbornWorld.obtainedShriekOfHorror), "Follow the [i:" + ModContent.ItemType<Items.MysteriousCompass>() + "]'s guidance");
                bossChecklist.Call("AddEventWithInfo", "Astraphobia", 6.06f, (Func<bool>)(() => TerrorbornWorld.downedTerrorRain), "Has a chance to occur instead of rain. Can be manually summoned by using a [i:" + ModContent.ItemType<Items.MiscConsumables.BrainStorm>() + "] during rain.");
                bossChecklist.Call("AddMiniBossWithInfo", "Frightcrawler", 6.07f, (Func<bool>)(() => TerrorbornWorld.downedFrightcrawler), "Spawns during the Astraphobia event (see above).");
            }
        }
    }
}
