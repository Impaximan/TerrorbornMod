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
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using TerrorbornMod.UI.TerrorMeter;
using TerrorbornMod.UI.TerrorAbilityUnlock;

namespace TerrorbornMod
{
    class TerrorbornMod : Mod
    {

        internal UserInterface terrorMeterInterface;
        internal TerrorMeterUI terrorMeterUI;

        internal UserInterface unlockInterface;
        internal UnlockUI unlockUI;

        internal UserInterface terrorMenueInterface;
        internal TerrorAbilityMenu terrorAbilityMenue;

        public static ModHotKey ArmorAbility;
        public static ModHotKey quickVirus;
        public static ModHotKey ShriekOfHorror;
        public static ModHotKey PrimaryTerrorAbility;
        public static ModHotKey SecondaryTerrorAbility;
        public static ModHotKey OpenTerrorAbilityMenue;

        public static int CombatTokenCustomCurrencyId;

        public static float screenShaking = 0f;

        public static Vector2 soulChargeMeterPos;
        public static bool alwaysShowSoulCharge;
        public static bool fullMeterSound;
        public static float soulChargeMeterScale;

        public static void ScreenShake(float Intensity)
        {
            screenShaking = Intensity;
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
        public override void AddRecipeGroups()
        {
            //Any Bug
            RecipeGroup bugs = new RecipeGroup(new System.Func<string>(BugString));
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
            RecipeGroup mythril = new RecipeGroup(new System.Func<string>(MythrilString));
            mythril.ValidItems.Add(ItemID.MythrilBar);
            mythril.ValidItems.Add(ItemID.OrichalcumBar);
            RecipeGroup.RegisterGroup("mythril", mythril);
        }
        public override void Load()
        {
            ArmorAbility = RegisterHotKey("ArmorAbility", "z");
            ShriekOfHorror = RegisterHotKey("Shriek of Horror", "F");
            PrimaryTerrorAbility = RegisterHotKey("Primary Terror Ability", "F");
            SecondaryTerrorAbility = RegisterHotKey("Secondary Terror Ability", "F");
            quickVirus = RegisterHotKey("Quick Virus", "z");
            OpenTerrorAbilityMenue = RegisterHotKey("Open/Close Terror Ability Menue", "z");
            CombatTokenCustomCurrencyId = CustomCurrencyManager.RegisterCurrency(new CombatTokenCurrency(ItemType("CombatToken"), 999L));

            if (Main.netMode != NetmodeID.Server)
            {
                Ref<Effect> screenRef = new Ref<Effect>(GetEffect("Effects/ShockwaveEffect")); // The path to the compiled shader file.
                Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["Shockwave"].Load();
            }
            if (!Main.dedServ)
            {
                terrorMeterInterface = new UserInterface();
                unlockInterface = new UserInterface();
                terrorMenueInterface = new UserInterface();

                terrorMeterUI = new TerrorMeterUI();
                terrorMeterUI.Activate();
                
                unlockUI = new UnlockUI();
                unlockUI.Activate();

                terrorAbilityMenue = new TerrorAbilityMenu();
                terrorAbilityMenue.Activate();
            }
        }
        private GameTime _lastUpdateUiGameTime;
        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (terrorMeterInterface?.CurrentState != null)
            {
                terrorMeterInterface.Update(gameTime);
            }
            if (unlockInterface?.CurrentState != null)
            {
                unlockInterface.Update(gameTime);
            }
            if (terrorMenueInterface?.CurrentState != null)
            {
                terrorMenueInterface.Update(gameTime);
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
                    "TerrorbornMod: terrorMenueInterface",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && terrorMenueInterface?.CurrentState != null)
                        {
                            terrorMenueInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                       InterfaceScaleType.UI));
            }
        }
        public override void PostUpdateEverything()
        {
            TerrorbornUtils.Update();

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
            terrorMenueInterface?.SetState(terrorAbilityMenue);
            terrorMeterInterface?.SetState(terrorMeterUI);
        }

        internal void HideUI()
        {
            unlockInterface?.SetState(null);
            terrorMenueInterface?.SetState(null);
            terrorMeterInterface?.SetState(null);
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
            }

            Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            if (bossChecklist != null)
            {
                bossChecklist.Call("AddBossWithInfo", "Tidal Titan", 3.5f, (Func<bool>)(() => TerrorbornWorld.downedTidalTitan), "Kill a mysterious crab, which rarely spawns in the ocean biome during the night. Despawns if it sinks back into the water (a layer of platforms over the ocean is recommended). Note: doesn't despawn when it becomes day");
                bossChecklist.Call("AddBossWithInfo", "Dunestock", 5.5f, (Func<bool>)(() => TerrorbornWorld.downedDunestock), "Use a [i:" + ModContent.ItemType<Items.DriedCanteen>() + "] in the desert.");
                bossChecklist.Call("AddBossWithInfo", "Shadowcrawler", 9.5f, (Func<bool>)(() => TerrorbornWorld.downedNightcrawler), "Use a [i:" + ModContent.ItemType<Items.RadioactiveSpiderFood>() + "] during the night.");
                bossChecklist.Call("AddBossWithInfo", "Prototype I", 11.35f, (Func<bool>)(() => TerrorbornWorld.downedPrototypeI), "Use a [i:" + ModContent.ItemType<Items.PlasmaCore>() + "] during the night.");
                bossChecklist.Call("AddMiniBossWithInfo", "Sangrune", 3.25f, (Func<bool>)(() => TerrorbornWorld.downedSangrune), "Spawns during a blood moon after the eater of worlds/brain of cthulhu have been defeated.");
                bossChecklist.Call("AddMiniBossWithInfo", "Sangrune (hardmode)", 7.5f, (Func<bool>)(() => TerrorbornWorld.downedSangrune2), "Re-fight Sangrune after the Wall of Flesh has been defeated.");
                bossChecklist.Call("AddMiniBossWithInfo", "Undying Spirit", 6.05f, (Func<bool>)(() => TerrorbornWorld.downedUndyingSpirit), "A strange eratic ghost that 'died' long ago. Spawns occasionally in the corruption: be wary.");
                bossChecklist.Call("AddEventWithInfo", "???", -5f, (Func<bool>)(() => TerrorbornWorld.obtainedShriekOfHorror), "Follow the [i:" + ModContent.ItemType<Items.MysteriousCompass>() + "]'s guidance");
            }
        }
    }
}
