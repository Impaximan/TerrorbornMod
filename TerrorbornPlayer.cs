using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Effects;
using TerrorbornMod.Abilities;
using Terraria.Audio;
using Microsoft.Xna.Framework.Audio;

namespace TerrorbornMod
{
    class TerrorbornPlayer : ModPlayer
    {
        public bool graniteSpark = false;
        public bool astralSpark = false;

        public int CombatPoints = 0;
        
        //Terror things
        public float TerrorPercent = 0;
        public float ShriekOfHorrorMovement = 0;
        public bool MidShriek = false;
        public float ShriekSpeed = 1;
        public float ShriekKnockback = 1;
        public float ShriekOfHorrorExtraDamageMultiplier = 1.5f;
        public float ShriekTerrorMultiplier = 1f;
        public float ShriekPain = 1f;
        public float ShriekRangeMultiplier = 1f;
        public int ShriekTime = 0;
        public bool ShowTerrorAbilityMenu = false;
        public int primaryAbilityInt = 0;
        public int secondaryAbilityInt = 0;
        public AbilityInfo primaryAbility = new None();
        public AbilityInfo secondaryAbility = new None();
        public Color terrorMeterColor = Color.White;

        int abilityAnimationCounter1 = 0;
        int abilityAnimationCounter2 = 0;
        int abilityAnimationType = 0;

        public int TenebrisDashTime = 0;
        public Vector2 TenebrisDashVelocity = new Vector2(0, 0);
        public int iFrames = 120;

        //Extra generic stats
        public float toolUseSpeed = 1f;
        public float placeSpeed = 1f;
        public bool canUseItems = true;
        public float critDamage = 1f;
        public float flightTimeMultiplier = 1f;
        public float noAmmoConsumeChance = 0f;
        public int badLifeRegen = 0;
        public bool inCombat = false;
        public int combatTime = 0;

        //Restless stats
        public float restlessDamage = 1f;
        public float restlessKnockback = 1f;
        public float restlessUseSpeed = 1f;
        public float restlessChargedUseSpeed = 1f;
        public float restlessNonChargedUseSpeed = 1f;
        public List<NPC> deimosChained = new List<NPC>();

        //Accessory/equipment fields
        public float NatureSpiritRotation = 0f;
        public bool ConstructorsDestructors = false;
        public bool CaneOfCurses = false;
        public bool SpecterLocket = false;
        public float PlasmaPower = 0f;
        public bool HeadHunter = false;
        public int HeadHunterCritCooldown = 0;
        public int HeadHunterCritBonus = 0;
        public int HeadhunterClass = 0; //0 for magic, 1 for melee, 2 for ranger, and 3 for summoner
        public bool IncendiaryShield = false;
        public bool DeimosteelCharm = false;
        public bool HexDefender = false;
        public bool MysteriousCompass = false;
        public bool AzuriteBrooch = false;
        public bool TacticalCommlink = false;
        public bool ShadowAmulet = false;
        public bool SoulEater = false;
        public bool PrismalCore = false;
        public bool cloakOfTheWind = false;
        public bool TorturersTalisman = false;
        public bool PlasmaArmorBonus = false;
        public bool VampiricPendant = false;
        public bool PyroclasticShinobiBonus = false;
        public bool SuperthrowNext = false;
        public bool SoulReaperArmorBonus = false;
        public bool Aerodynamic = false;
        public bool Glooped = false;
        public bool SanguineSetBonus;
        public bool AzuriteArmorBonus = false;
        public bool SangoonBand = false;
        public int SangoonBandCooldown = 0;
        public bool FusionArmor = false;
        public bool LiesOfNourishment = false;
        public bool IntimidationAura = false;
        public bool AntlionShell = false;
        public bool TideSpirit = false;
        public bool TidalShellArmorBonus = false;
        public bool HorrificCharm = false;
        public int BurstJumpTime = 0;
        public int BurstJumpChargingTime = 0;
        public bool JustBurstJumped = false;
        public bool JustParried = false;
        public int ParryCooldown = 0;
        public int ParryTime = 0;
        public Color parryColor = Color.White;
        public bool BanditGlove = false;
        public bool SilentArmor = false;
        public bool TerrorTonic = false;
        public int DarkEnergyStored = 0;
        public int HiddenInstinctTime = 0;

        //Permanent Upgrades
        public bool EyeOfTheMenace = false;
        public bool GoldenTooth = false;
        public bool CoreOfFear = false;
        public bool AnekronianApple = false;
        public bool DemonicLense = false;
        public int MidnightFruit = 0;

        //Terror ability fields
        public int VoidBlinkTime = 0;
        public int BlinkDashTime = 0;
        public int BlinkDashCooldown = 0;
        public Vector2 BlinkDashVelocity = Vector2.Zero;
        public int GelatinArmorTime = 0;
        public int GelatinPunishmentDamage = 0;
        public IList<int> unlockedAbilities = new List<int>();
        public int TimeFreezeTime = 0;
        public int TerrorPotionCooldown = 60 * 10;

        //Biome fields
        public bool ZoneDeimostone;
        public bool ZoneIncendiary;
        public bool ZoneICU;

        //Misc stuff
        public int terrorDrainCounter = 0;
        public bool HexedMirage = false;
        public bool TwilightMatrix = false;
        public int currentThrownCritState = -1;
        public bool InsantDeathProtection = false;

        //Twilight Mode
        public int TwilightHPCap = 400;
        public float TwilightPower = 0f;
        public float TwilightPowerMultiplier = 1f;
        public bool InTwilightOverload = false;

        #region GlowmaskStuff
        //public static readonly PlayerLayer legsGlow = new PlayerLayer("TerrorbornMod", "Legs_Glow", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        //{
        //    Player Player = drawInfo.drawPlayer;
        //    if (Player.armor[2] != null && Player.armor[2].type > Main.maxItemTypes)
        //    {
        //        if (Player.armor[2] != null && Player.armor[2].type > Main.maxItemTypes)
        //        {
        //            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Player.armor[2].modItem.Texture + "_Legs_Glow");
        //            Vector2 drawPosition = drawInfo.position + new Vector2(-Player.bodyFrame.Width / 2 + (Player.width / 2), Player.height - Player.bodyFrame.Height + 10) + Player.headPosition + drawInfo.legOrigin;
        //            DrawData data = new DrawData(texture, new Vector2((int)(drawPosition.X - Main.screenPosition.X), (int)(drawPosition.Y - Main.screenPosition.Y) - 6), Player.legFrame, Color.White * (drawInfo.bodyColor.A / 255f), Player.legRotation, drawInfo.legOrigin, 1f, drawInfo.spriteEffects, 0);
        //            Main.PlayerDrawData.Add(data);
        //        }
        //    }
        //});

        //public static readonly PlayerLayer legsGlowVanity = new PlayerLayer("TerrorbornMod", "Legs_Glow", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        //{
        //    Player Player = drawInfo.drawPlayer;
        //    int vanityAddend = 10;
        //    if (Player.armor[2 + vanityAddend] != null && Player.armor[2 + vanityAddend].type > Main.maxItemTypes)
        //    {
        //        if (Player.armor[2 + vanityAddend] != null && Player.armor[2 + vanityAddend].type > Main.maxItemTypes)
        //        {
        //            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Player.armor[2 + vanityAddend].modItem.Texture + "_Legs_Glow");
        //            Vector2 drawPosition = drawInfo.position + new Vector2(-Player.bodyFrame.Width / 2 + (Player.width / 2), Player.height - Player.bodyFrame.Height + 10) + Player.headPosition + drawInfo.legOrigin;
        //            DrawData data = new DrawData(texture, new Vector2((int)(drawPosition.X - Main.screenPosition.X), (int)(drawPosition.Y - Main.screenPosition.Y) - 6), Player.legFrame, Color.White * (drawInfo.bodyColor.A / 255f), Player.legRotation, drawInfo.legOrigin, 1f, drawInfo.spriteEffects, 0);
        //            Main.PlayerDrawData.Add(data);
        //        }
        //    }
        //});

        //public static readonly PlayerLayer armsGlow = new PlayerLayer("TerrorbornMod", "Arms_Glow", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        //{
        //    Player Player = drawInfo.drawPlayer;
        //    if (Player.armor[1] != null && Player.armor[1].type > Main.maxItemTypes)
        //    {
        //        if (Player.armor[1] != null && Player.armor[1].type > Main.maxItemTypes)
        //        {
        //            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Player.armor[1].modItem.Texture + "_Arms_Glow");
        //            Vector2 drawPosition = drawInfo.position + new Vector2(-Player.bodyFrame.Width / 2 + (Player.width / 2), Player.height - Player.bodyFrame.Height + 10) + Player.headPosition + drawInfo.bodyOrigin;
        //            DrawData data = new DrawData(texture, new Vector2((int)(drawPosition.X - Main.screenPosition.X), (int)(drawPosition.Y - Main.screenPosition.Y) - 6), Player.bodyFrame, Color.White * (drawInfo.bodyColor.A / 255f), Player.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
        //            Main.PlayerDrawData.Add(data);
        //        }
        //    }
        //});

        //public static readonly PlayerLayer armsGlowVanity = new PlayerLayer("TerrorbornMod", "Arms_Glow", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        //{
        //    Player Player = drawInfo.drawPlayer;
        //    int vanityAddend = 10;
        //    if (Player.armor[1 + vanityAddend] != null && Player.armor[1 + vanityAddend].type > Main.maxItemTypes)
        //    {
        //        if (Player.armor[1 + vanityAddend] != null && Player.armor[1 + vanityAddend].type > Main.maxItemTypes)
        //        {
        //            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Player.armor[1 + vanityAddend].modItem.Texture + "_Arms_Glow");
        //            Vector2 drawPosition = drawInfo.position + new Vector2(-Player.bodyFrame.Width / 2 + (Player.width / 2), Player.height - Player.bodyFrame.Height + 10) + Player.headPosition + drawInfo.bodyOrigin;
        //            DrawData data = new DrawData(texture, new Vector2((int)(drawPosition.X - Main.screenPosition.X), (int)(drawPosition.Y - Main.screenPosition.Y) - 6), Player.bodyFrame, Color.White * (drawInfo.bodyColor.A / 255f), Player.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
        //            Main.PlayerDrawData.Add(data);
        //        }
        //    }
        //});

        //public static readonly PlayerLayer bodyGlow = new PlayerLayer("TerrorbornMod", "Body_Glow", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        //{
        //    Player Player = drawInfo.drawPlayer;
        //    if (Player.armor[1] != null && Player.armor[1].type > Main.maxItemTypes)
        //    {
        //        if (Player.armor[1] != null && Player.armor[1].type > Main.maxItemTypes)
        //        {
        //            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Player.armor[1].modItem.Texture + "_Body_Glow");
        //            Vector2 drawPosition = drawInfo.position + new Vector2(-Player.bodyFrame.Width / 2 + (Player.width / 2), Player.height - Player.bodyFrame.Height + 10) + Player.headPosition + drawInfo.bodyOrigin;
        //            DrawData data = new DrawData(texture, new Vector2((int)(drawPosition.X - Main.screenPosition.X), (int)(drawPosition.Y - Main.screenPosition.Y) - 6), Player.bodyFrame, Color.White * (drawInfo.bodyColor.A / 255f), Player.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
        //            Main.PlayerDrawData.Add(data);
        //        }
        //    }
        //});

        //public static readonly PlayerLayer bodyGlowVanity = new PlayerLayer("TerrorbornMod", "Body_Glow", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        //{
        //    Player Player = drawInfo.drawPlayer;
        //    int vanityAddend = 10;
        //    if (Player.armor[1 + vanityAddend] != null && Player.armor[1 + vanityAddend].type > Main.maxItemTypes)
        //    {
        //        if (Player.armor[1 + vanityAddend] != null && Player.armor[1 + vanityAddend].type > Main.maxItemTypes)
        //        {
        //            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Player.armor[1 + vanityAddend].modItem.Texture + "_Body_Glow");
        //            Vector2 drawPosition = drawInfo.position + new Vector2(-Player.bodyFrame.Width / 2 + (Player.width / 2), Player.height - Player.bodyFrame.Height + 10) + Player.headPosition + drawInfo.bodyOrigin;
        //            DrawData data = new DrawData(texture, new Vector2((int)(drawPosition.X - Main.screenPosition.X), (int)(drawPosition.Y - Main.screenPosition.Y) - 6), Player.bodyFrame, Color.White * (drawInfo.bodyColor.A / 255f), Player.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
        //            Main.PlayerDrawData.Add(data);
        //        }
        //    }
        //});

        //public static readonly PlayerLayer headGlow = new PlayerLayer("TerrorbornMod", "Head_Glow", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        //{
        //    Player Player = drawInfo.drawPlayer;
        //    if (Player.armor[0] != null && Player.armor[0].type > Main.maxItemTypes)
        //    {
        //        if (Player.armor[0] != null && Player.armor[0].type > Main.maxItemTypes)
        //        {
        //            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Player.armor[0].modItem.Texture + "_Head_Glow");
        //            Vector2 drawPosition = drawInfo.position + new Vector2(-Player.bodyFrame.Width / 2 + (Player.width / 2), Player.height - Player.bodyFrame.Height + 10) + Player.headPosition + drawInfo.bodyOrigin;
        //            DrawData data = new DrawData(texture, new Vector2((int)(drawPosition.X - Main.screenPosition.X), (int)(drawPosition.Y - Main.screenPosition.Y) - 6), Player.bodyFrame, Color.White * (drawInfo.bodyColor.A / 255f), Player.headRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
        //            Main.PlayerDrawData.Add(data);
        //        }
        //    }
        //});

        //public static readonly playerdraw headGlowVanity = new PlayerLayer("TerrorbornMod", "Head_Glow", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        //{
        //    Player Player = drawInfo.drawPlayer;
        //    int vanityAddend = 10;
        //    if (Player.armor[0 + vanityAddend] != null && Player.armor[0 + vanityAddend].type > Main.maxItemTypes)
        //    {
        //        if (Player.armor[0 + vanityAddend] != null && Player.armor[0 + vanityAddend].type > Main.maxItemTypes)
        //        {
        //            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Player.armor[0 + vanityAddend].modItem.Texture + "_Head_Glow");
        //            Vector2 drawPosition = drawInfo.position + new Vector2(-Player.bodyFrame.Width / 2 + (Player.width / 2), Player.height - Player.bodyFrame.Height + 10) + Player.headPosition + drawInfo.bodyOrigin;
        //            DrawData data = new DrawData(texture, new Vector2((int)(drawPosition.X - Main.screenPosition.X), (int)(drawPosition.Y - Main.screenPosition.Y) - 6), Player.bodyFrame, Color.White * (drawInfo.bodyColor.A / 255f), Player.headRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
        //            Main.PlayerDrawData.Add(data);
        //        }
        //    }
        //});
        //#endregion

        //public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        //{
        //    int vanityAddend = 10;
        //    bool didDoVanity = false;

        //    if (Player.armor[2 + vanityAddend] != null && Player.armor[2 + vanityAddend].type > Main.maxItemTypes)
        //    {
        //        if (Player.armor[2 + vanityAddend].modItem.mod.Name == "TerrorbornMod" && ModContent.TextureExists(Player.armor[2 + vanityAddend].modItem.Texture + "_Legs_Glow"))
        //        {
        //            didDoVanity = true;
        //            int index = layers.FindIndex((pl) => pl.Name == "Legs");
        //            layers.Insert(index + 1, legsGlowVanity);
        //        }
        //    }

        //    if (Player.armor[2] != null && Player.armor[2].type > Main.maxItemTypes && !didDoVanity && Player.armor[2 + vanityAddend].IsAir)
        //    {
        //        if (Player.armor[2].modItem.mod.Name == "TerrorbornMod" && ModContent.TextureExists(Player.armor[2].modItem.Texture + "_Legs_Glow"))
        //        {
        //            int index = layers.FindIndex((pl) => pl.Name == "Legs");
        //            layers.Insert(index + 1, legsGlow);
        //        }
        //    }

        //    didDoVanity = false;

        //    if (Player.armor[1 + vanityAddend] != null && Player.armor[1 + vanityAddend].type > Main.maxItemTypes)
        //    {
        //        if (Player.armor[1 + vanityAddend].modItem.mod.Name == "TerrorbornMod" && ModContent.TextureExists(Player.armor[1 + vanityAddend].modItem.Texture + "_Arms_Glow"))
        //        {
        //            didDoVanity = true;
        //            int index = layers.FindIndex((pl) => pl.Name == "Arms");
        //            layers.Insert(index + 1, armsGlowVanity);
        //        }
        //    }

        //    if (Player.armor[1] != null && Player.armor[1].type > Main.maxItemTypes && !didDoVanity && Player.armor[1 + vanityAddend].IsAir)
        //    {
        //        if (Player.armor[1].modItem.mod.Name == "TerrorbornMod" && ModContent.TextureExists(Player.armor[1].modItem.Texture + "_Arms_Glow"))
        //        {
        //            int index = layers.FindIndex((pl) => pl.Name == "Arms");
        //            layers.Insert(index + 1, armsGlow);
        //        }
        //    }

        //    didDoVanity = false;

        //    if (Player.armor[1 + vanityAddend] != null && Player.armor[1 + vanityAddend].type > Main.maxItemTypes)
        //    {
        //        if (Player.armor[1 + vanityAddend].modItem.mod.Name == "TerrorbornMod" && ModContent.TextureExists(Player.armor[1 + vanityAddend].modItem.Texture + "_Body_Glow"))
        //        {
        //            didDoVanity = true;
        //            int index = layers.FindIndex((pl) => pl.Name == "Body");
        //            layers.Insert(index + 1, bodyGlowVanity);
        //        }
        //    }

        //    if (Player.armor[1] != null && Player.armor[1].type > Main.maxItemTypes && !didDoVanity && Player.armor[1 + vanityAddend].IsAir)
        //    {
        //        if (Player.armor[1].modItem.mod.Name == "TerrorbornMod" && ModContent.TextureExists(Player.armor[1].modItem.Texture + "_Body_Glow"))
        //        {
        //            int index = layers.FindIndex((pl) => pl.Name == "Body");
        //            layers.Insert(index + 1, bodyGlow);
        //        }
        //    }

        //    didDoVanity = false;

        //    if (Player.armor[0 + vanityAddend] != null && Player.armor[0 + vanityAddend].type > Main.maxItemTypes)
        //    {
        //        if (Player.armor[0 + vanityAddend].modItem.mod.Name == "TerrorbornMod" && ModContent.TextureExists(Player.armor[0 + vanityAddend].modItem.Texture + "_Head_Glow"))
        //        {
        //            didDoVanity = true;
        //            int index = layers.FindIndex((pl) => pl.Name == "Head");
        //            layers.Insert(index + 1, headGlowVanity);
        //        }
        //    }

        //    if (Player.armor[0] != null && Player.armor[0].type > Main.maxItemTypes && !didDoVanity && Player.armor[0 + vanityAddend].IsAir)
        //    {
        //        if (Player.armor[0].modItem.mod.Name == "TerrorbornMod" && ModContent.TextureExists(Player.armor[0].modItem.Texture + "_Head_Glow"))
        //        {
        //            int index = layers.FindIndex((pl) => pl.Name == "Head");
        //            layers.Insert(index + 1, headGlow);
        //        }
        //    }
        //}
        //public override void ModifyDrawLayers(List<PlayerLayer> layers)
        //{
        //}
        #endregion

        public void TriggerAbilityAnimation(string name, string description1, string description2, int AnimationType, string description3 = "You can equip terror abilities by using the 'Open/Close Terror Abilities Menu' hotkey.", int visibilityTime = 600)
        {
            UI.TerrorAbilityUnlock.UnlockUI.abilityUILifetimeCounter = visibilityTime;
            UI.TerrorAbilityUnlock.UnlockUI.abilityUnlockName = name;
            UI.TerrorAbilityUnlock.UnlockUI.abilityUnlockDescription1 = description1;
            UI.TerrorAbilityUnlock.UnlockUI.abilityUnlockDescription2 = description2;
            UI.TerrorAbilityUnlock.UnlockUI.abilityUnlockDescription3 = description3;

            abilityAnimationType = AnimationType;
            if (AnimationType == 0) //Shriek of Horror/Default
            {
                abilityAnimationCounter1 = 9;
            }
        }

        public int parryLightTime = 0;
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (MidShriek && drawInfo.Position == Player.position && ShriekTime > 0)
            {
                int effectTime = ShriekTime;
                if (effectTime > 120)
                {
                    effectTime = 120;
                }
                TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, Player.Center - Main.screenPosition, (int)(TerrorPercent / 2 + 100 + effectTime / 2), Color.Black * (0.75f * (effectTime / 120f)));
            }

            if (drawInfo.Position == Player.position && parryLightTime > 0)
            {
                parryLightTime--;
                TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, Player.Center - Main.screenPosition, (int)(100 + parryLightTime * 2), parryColor * (0.75f * (parryLightTime / 20f)));
            }
        }

        //int effectCounter = 60;
        //int progress = 0;
        public bool twilight = false;
        //public override void UpdateBiomeVisuals()
        //{
        //    if (ZoneIncendiary)
        //    {
        //        effectCounter--;
        //        if (effectCounter <= 0)
        //        {
        //            effectCounter = Main.rand.Next(3, 6);
        //            int maxDistance = 1500;
        //            ForegroundObject.NewForegroundObject(new Vector2(Main.rand.Next(-maxDistance, maxDistance), Main.rand.Next(-maxDistance, maxDistance)), new IncendiaryFog());
        //        }
        //    }

        //    Player.ManageSpecialBiomeVisuals("TerrorbornMod:PrototypeIShader", NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.PrototypeI.PrototypeI>()));
        //    Player.ManageSpecialBiomeVisuals("TerrorbornMod:DarknessShader", ZoneDeimostone || ZoneICU);
        //    Player.ManageSpecialBiomeVisuals("TerrorbornMod:BlandnessShader", ZoneICU && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.InfectedIncarnate.InfectedIncarnate>()));
        //    //Player.ManageSpecialBiomeVisuals("TerrorbornMod:IncarnateBoss", NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.InfectedIncarnate.InfectedIncarnate>()));
        //    Player.ManageSpecialBiomeVisuals("TerrorbornMod:ColorlessShader", TimeFreezeTime > 0);
        //    Player.ManageSpecialBiomeVisuals("TerrorbornMod:TwilightShaderNight", InTwilightOverload);

        //    Player.ManageSpecialBiomeVisuals("TerrorbornMod:HexedMirage", HexedMirage);

        //    //Filters.Scene["TerrorbornMod:GlitchShader"].GetShader().UseTargetPosition(new Vector2(0f, Main.rand.NextFloat(0f, 1f)));
        //    //Filters.Scene["TerrorbornMod:GlitchShader"].GetShader().UseIntensity(Main.rand.NextFloat(-0.1f, 0.1f));
        //    //switch (Main.rand.Next(3))
        //    //{
        //    //    case 0:
        //    //        Filters.Scene["TerrorbornMod:GlitchShader"].GetShader().UseColor(1f, 0f, 0f);
        //    //        break;
        //    //    case 1:
        //    //        Filters.Scene["TerrorbornMod:GlitchShader"].GetShader().UseColor(0f, 1f, 0f);
        //    //        break;
        //    //    case 2:
        //    //        Filters.Scene["TerrorbornMod:GlitchShader"].GetShader().UseColor(0f, 0f, 1f);
        //    //        break;
        //    //    default:
        //    //        Filters.Scene["TerrorbornMod:GlitchShader"].GetShader().UseColor(1f, 0f, 0f);
        //    //        break;
        //    //}

        //    //Filters.Scene["TerrorbornMod:GlitchShader"].GetShader().Update(Main._drawInterfaceGameTime);
        //    ////Filters.Scene["TerrorbornMod:GlitchShader"].Load();
        //    //Player.ManageSpecialBiomeVisuals("TerrorbornMod:GlitchShader", true);
        //}

        //public override void UpdateBiomes()
        //{
        //    ZoneDeimostone = TerrorbornSystem.deimostoneTiles > 75;

        //    Rectangle incendiaryBiomeRect = new Rectangle(0, 0, (int)(Main.maxTilesX / 4f * 16) + 120 * 16, (int)(Main.maxTilesY / 17f * 16) + 120 * 16);
        //    if (TerrorbornSystem.incendiaryIslandsSide == 1)
        //    {
        //        incendiaryBiomeRect = new Rectangle((Main.maxTilesX * 16) - (int)(Main.maxTilesX / 4f * 16) - 120 * 16, 0, (int)(Main.maxTilesX / 4f * 16) + 120 * 16, (int)(Main.maxTilesY / 17f * 16) + 120 * 16);
        //    }
        //    ZoneIncendiary = incendiaryBiomeRect.Intersects(Player.getRect()) && Main.hardMode;

        //    ZoneICU = Player.Distance(TerrorbornSystem.IIShrinePosition * 16 + new Vector2(0f, 140f) * 8) <= 1200 && Player.behindBackWall;
        //}

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            if (mediumCoreDeath)
            {
                return base.AddStartingItems(mediumCoreDeath);
            }
            else if (TerrorbornMod.StartingItems)
            {
                return new[]
                {
                    new Item(ModContent.ItemType<Items.MysteriousCompass>()),
                    new Item(ModContent.ItemType<Items.Weapons.Ranged.Stick>(), 150),
                    new Item(ItemID.Torch, 15),
                    new Item(ItemID.Glowstick, 10),
                    new Item(ItemID.Bomb, 5),
                    new Item(ItemID.Rope, 200),
                    new Item(ItemID.SlimeCrown),
                    new Item(ItemID.SpelunkerPotion, 2),
                    new Item(ItemID.MiningPotion, 2),
                    new Item(ItemID.Hook),
                    new Item(ModContent.ItemType<Items.TwilightMatrix>())

                };
            }
            else
            {
                return new[]
                {
                    new Item(ModContent.ItemType<Items.MysteriousCompass>()),
                    new Item(ModContent.ItemType<Items.TwilightMatrix>())

                };
            }
        }

        float fusionHealCounter = 0f;
        const float maxFusionHealCounter = 2.5f;
        float twilightHealCounter = 0f;
        const float maxTwilightHealCounter = 15f;
        public void LoseTerror(float amount, bool silent = true, bool perSecond = false, bool smallerText = false)
        {
            if (!(perSecond || silent))
            {
                CombatText.NewText(Player.getRect(), Color.FromNonPremultiplied(108, 150, 143, 255), "-" + amount + "%", dot: smallerText);
            }

            if (FusionArmor)
            {
                if (perSecond)
                {
                    fusionHealCounter += amount / 60f;
                }
                else
                {
                    fusionHealCounter += amount;
                }

                int healAmount = 0;
                while (fusionHealCounter >= maxFusionHealCounter)
                {
                    fusionHealCounter -= maxFusionHealCounter;
                    healAmount++;
                }

                if (healAmount > 0)
                {
                    Player.HealEffect(healAmount);
                    Player.statLife += healAmount;
                }
            }

            if (TerrorbornSystem.TwilightMode && !perSecond)
            {
                twilightHealCounter += amount;

                int healAmount = 0;
                float actualMaxHeal = maxTwilightHealCounter;
                if (Player.lifeRegen != 0) actualMaxHeal *= 1f / Player.lifeRegen;
                while (twilightHealCounter >= actualMaxHeal)
                {
                    twilightHealCounter -= actualMaxHeal;
                    healAmount++;
                }

                if (healAmount > 0)
                {
                    Player.HealEffect(healAmount);
                    Player.statLife += healAmount;
                }

                if (!InTwilightOverload)
                {
                    TwilightPower += amount / 65f * TwilightPowerMultiplier;
                }
            }

            if (perSecond)
            {
                TerrorPercent -= amount / 60f;
            }
            else
            {
                TerrorPercent -= amount;
            }

            if (TerrorPercent < 0f)
            {
                TerrorPercent = 0f;
            }
        }

        public void GainTerror(float amount, bool silent = true, bool perSecond = false, bool smallerText = false)
        {
            if (!(perSecond || silent))
            {
                CombatText.NewText(Player.getRect(), Color.FromNonPremultiplied(108, 150, 143, 255), amount + "%", dot: smallerText);
            }

            if (FusionArmor)
            {
                if (perSecond)
                {
                    fusionHealCounter += amount / 60f;
                }
                else
                {
                    fusionHealCounter += amount;
                }

                int healAmount = 0;
                while (fusionHealCounter >= maxFusionHealCounter)
                {
                    fusionHealCounter -= maxFusionHealCounter;
                    healAmount++;
                }

                if (healAmount > 0)
                {
                    Player.HealEffect(healAmount);
                    Player.statLife += healAmount;
                }
            }

            if (perSecond)
            {
                TerrorPercent += amount / 60f;
            }
            else
            {
                TerrorPercent += amount;
            }

            if (TerrorPercent > 100f)
            {
                TerrorPercent = 100f;
            }
        }

        public override void ResetEffects()
        {
            terrorMeterColor = Color.White;
            TwilightMatrix = false;
            HeadHunter = false;
            flightTimeMultiplier = 1f;
            DeimosteelCharm = false;
            restlessDamage = 1f;
            restlessKnockback = 1f;
            SilentArmor = false;
            restlessUseSpeed = 1f;
            restlessChargedUseSpeed = 1f;
            restlessNonChargedUseSpeed = 1f;
            critDamage = 1f;
            HorrificCharm = false;
            TideSpirit = false;
            TidalShellArmorBonus = false;
            twilight = false;
            badLifeRegen = 0;
            SoulReaperArmorBonus = false;
            MysteriousCompass = false;
            ShadowAmulet = false;
            IntimidationAura = false;
            SanguineSetBonus = false;
            HexedMirage = false;
            BanditGlove = false;
            AntlionShell = false;
            PrismalCore = false;
            SoulEater = false;
            AzuriteBrooch = false;
            HexDefender = false;
            TacticalCommlink = false;
            astralSpark = false;
            FusionArmor = false;
            JustBurstJumped = false;
            graniteSpark = false;
            TwilightPowerMultiplier = 1f;
            ShriekOfHorrorMovement = 0f;
            CaneOfCurses = false;
            TerrorTonic = false;
            InsantDeathProtection = Player.statLife > Player.statLifeMax2 * 0.9f;
            if (CoreOfFear)
            {
                ShriekOfHorrorMovement += 0.3f;
            }
            if (AnekronianApple)
            {
                ShriekOfHorrorMovement += 0.2f;
            }
            ShriekSpeed = 1;
            if (DemonicLense)
            {
                ShriekSpeed = 0.5f;
            }
            ShriekKnockback = 1;
            ShriekOfHorrorExtraDamageMultiplier = 2f;
            ShriekTerrorMultiplier = 1f;
            if (EyeOfTheMenace)
            {
                ShriekTerrorMultiplier += 0.5f;
            }
            ShriekPain = 1f;
            ShriekRangeMultiplier = 1f;
            if (GoldenTooth)
            {
                ShriekRangeMultiplier += 1 / 3;
            }
            toolUseSpeed = 1f;
            placeSpeed = 1f;
            SangoonBand = false;
            LiesOfNourishment = false;
            IncendiaryShield = false;
            Aerodynamic = false;
            TorturersTalisman = false;
            Glooped = false;
            PlasmaArmorBonus = false;
            PyroclasticShinobiBonus = false;
            AzuriteArmorBonus = false;
            VampiricPendant = false;
            ConstructorsDestructors = false;
            TerrorPotionCooldown = 60 * 10;
            noAmmoConsumeChance = 0f;

            Player.statManaMax2 += 5 * MidnightFruit;

            Vector2 screenCenter = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2) * Main.UIScale;
            Rectangle mainRect = new Rectangle((int)screenCenter.X - 400, (int)screenCenter.Y - 400, 800, 300);
            Rectangle mouseRectangle = new Rectangle((int)Main.mouseX, (int)Main.mouseY, 1, 1);
            if (mainRect.Intersects(mouseRectangle) && ShowTerrorAbilityMenu)
            {
                canUseItems = false;
            }
            else
            {
                canUseItems = true;
            }
        }

        public override void PreUpdateMovement()
        {
            if (graniteSpark)
            {
                if (Player.controlLeft) Player.velocity.X = -GraniteSparkData.speed;
                else if (Player.controlRight) Player.velocity.X = GraniteSparkData.speed;
                else Player.velocity.X = 0;

                if (Player.controlUp) Player.velocity.Y = -GraniteSparkData.speed;
                else if (Player.controlDown) Player.velocity.Y = GraniteSparkData.speed;
                else Player.velocity.Y = 0;
            }

            if (astralSpark)
            {
                float speedMultiplier = 1f;

                if (Player.controlJump) speedMultiplier = 5f;

                if (Player.controlLeft) Player.velocity.X = -astralSparkData.speed * speedMultiplier;
                else if (Player.controlRight) Player.velocity.X = astralSparkData.speed * speedMultiplier;
                else Player.velocity.X = 0;

                if (Player.controlUp) Player.velocity.Y = -astralSparkData.speed * speedMultiplier;
                else if (Player.controlDown) Player.velocity.Y = astralSparkData.speed * speedMultiplier;
                else Player.velocity.Y = 0;
            }

            if (ShriekOfHorrorMovement == 0 && MidShriek)
            {
                Player.velocity = Vector2.Zero;
            }

            if (Player.HeldItem != null && !Player.HeldItem.IsAir && Player.HeldItem.useStyle == ItemUseStyleID.Swing && Player.controlUseItem && Player.HeldItem.createTile == -1 && Player.HeldItem.createWall == -1 && Player.HeldItem.shoot == ProjectileID.None)
            {
                if (Main.MouseWorld.X > Player.Center.X)
                {
                    Player.direction = 1;
                }
                else
                {
                    Player.direction = -1;
                }
            }
        }

        public override float UseTimeMultiplier(Item item)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            float finalMult = 1f;
            if (item.pick > 0 || item.axe > 0 || item.hammer > 0)
            {
                finalMult *= toolUseSpeed;
            }
            Item dummyItem = new Item();
            dummyItem.SetDefaults(ItemID.UglySweater);
            if (item.createTile != dummyItem.createTile || item.createWall != dummyItem.createWall)
            {
                finalMult *= placeSpeed;
            }
            //if (item.useTime <= 5)
            //{
            //    return 1f;
            //}
            return 1f / finalMult;
        }

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {

        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (CaneOfCurses)
            {
                CaneOfCursesEffect(target);
            }
        }

        public void CaneOfCursesEffect(NPC target)
        {
            if (Main.rand.NextBool(5))
            {
                int buffType = 0;
                switch (Main.rand.Next(5))
                {
                    case 0:
                        buffType = BuffID.OnFire;
                        break;
                    case 1:
                        buffType = BuffID.Poisoned;
                        break;
                    case 2:
                        buffType = BuffID.Confused;
                        break;
                    case 4:
                        buffType = BuffID.Frostburn;
                        break;
                    default:
                        break;
                }
                target.AddBuff(buffType, (int)(60 * Main.rand.NextFloat(2f, 5f)));
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (CaneOfCurses)
            {
                CaneOfCursesEffect(target);
            }
            if (combatTime < 300)
            {
                combatTime = 300;
            }
        }

        public void OnHitByAnything(Entity entity, int damage, bool crit)
        {
            if (combatTime < 600)
            {
                combatTime = 600;
            }

            if (IntimidationAura)
            {
                TerrorPercent -= TerrorPercent / 4;
                if (TerrorPercent < 0)
                {
                    TerrorPercent = 0;
                }
            }

            if (SilentArmor)
            {
                TerrorPercent = 0f;
            }

            if (PlasmaPower > 0f)
            {
                PlasmaPower = 0f;
            }
        }

        public override void OnHitByNPC(NPC NPC, int damage, bool crit)
        {
            OnHitByAnything(NPC, damage, crit);
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            OnHitByAnything(proj, damage, crit);
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (InTwilightOverload)
            {
                damage = (int)(damage * 1.75f);
                if (Main.masterMode)
                {
                    damage = (int)(damage * 1.5f);
                }
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (InTwilightOverload)
            {
                damage = (int)(damage * 1.75f);
                if (Main.masterMode)
                {
                    damage = (int)(damage * 1.5f);
                }
            }
        }

        public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
            if (TerrorbornSystem.TwilightMode)
            {
                TwilightHPCap += healValue;
            }
        }

        public override void PostUpdateEquips()
        {
            if (HiddenInstinctTime > 0)
            {
                HiddenInstinctTime--;
                Player.pickSpeed *= 0.3f;
                Player.AddBuff(BuffID.Spelunker, 2);
                Player.AddBuff(BuffID.Dangersense, 2);
                Player.AddBuff(BuffID.Hunter, 2);
                Player.moveSpeed *= 1.25f;
                Player.jumpSpeedBoost = 2f;
                Lighting.AddLight(Player.position, 1f, 1.2f, 1.2f);
            }

            if (TerrorbornSystem.TwilightMode)
            {
                Player.luck += 1f;
            }

            Player.wingTimeMax = (int)(Player.wingTimeMax * flightTimeMultiplier);

            if (Aerodynamic)
            {
                Player.wingTimeMax *= 2;
            }

            if (TimeFreezeTime > 0)
            {
                Player.GetAttackSpeed(DamageClass.Generic) *= 0.5f;
            }

            if (Glooped)
            {
                Player.wingTimeMax /= 4;
            }

            if (VoidBlinkTime > 0)
            {
                Player.GetAttackSpeed(DamageClass.Generic) *= 1.3f;
                Player.accRunSpeed *= 2;
            }

            if (TerrorbornSystem.TwilightMode && TwilightPower > 0f)
            {
                Player.lifeRegen += (int)MathHelper.Lerp(1f, 5f, TwilightPower);
            }

            if (InTwilightOverload)
            {
                Player.GetAttackSpeed(DamageClass.Generic) *= 1.3f;
                Player.accRunSpeed *= 2;
                Player.jumpSpeedBoost += 3f;
                if (Main.masterMode)
                {
                    Player.GetAttackSpeed(DamageClass.Generic) *= 1.3f;
                    Player.lifeRegen += 7;
                }
            }

            for (int i = 0; i < Player.armor.Length; i++)
            {
                if (i >= Player.armor.Length)
                {
                    break;
                }
                else if (Player.armor[i] != null && !Player.armor[i].IsAir)
                {
                    Item item = Player.armor[i];
                    TerrorbornItem modItem = TerrorbornItem.modItem(item);
                    if (modItem.meterColor != Color.White)
                    {
                        terrorMeterColor = modItem.meterColor;
                    }
                }
            }
        }

        public override void ModifyScreenPosition()
        {
            if (TerrorbornMod.screenFollowTime > 0)
            {
                if (TerrorbornMod.currentScreenLerp < TerrorbornMod.maxScreenLerp)
                {
                    TerrorbornMod.currentScreenLerp += 1f / TerrorbornMod.screenTransitionTime;
                    if (TerrorbornMod.currentScreenLerp > TerrorbornMod.maxScreenLerp)
                    {
                        TerrorbornMod.currentScreenLerp = TerrorbornMod.maxScreenLerp;
                    }
                }
                else if (!NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.InfectedIncarnate.InfectedIncarnate>()))
                {
                    TerrorbornMod.screenFollowTime--;
                }
            }
            else if (TerrorbornMod.currentScreenLerp > 0)
            {
                TerrorbornMod.currentScreenLerp -= 1f / TerrorbornMod.screenTransitionTime;
                if (TerrorbornMod.currentScreenLerp < 0)
                {
                    TerrorbornMod.currentScreenLerp = 0;
                }
            }

            Main.screenPosition = Vector2.Lerp(Main.screenPosition, TerrorbornMod.screenFollowPosition - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), TerrorbornMod.currentScreenLerp);

            Vector2 ScreenOffset = new Vector2(Main.rand.NextFloat(-TerrorbornSystem.screenShaking, TerrorbornSystem.screenShaking), Main.rand.NextFloat(-TerrorbornSystem.screenShaking, TerrorbornSystem.screenShaking));
            Main.screenPosition += ScreenOffset;
        }

        public void ConsumeTerrorPotion()
        {
            Item currentItem = null;
            float currentTerror = 0f;
            foreach (Item item in Player.inventory)
            {
                if (item != null)
                {
                    if (!item.IsAir)
                    {
                        TerrorbornItem modItem = TerrorbornItem.modItem(item);
                        if (modItem.terrorPotionTerror > currentTerror)
                        {
                            currentTerror = modItem.terrorPotionTerror;
                            currentItem = item;
                        }
                    }
                }
            }

            if (currentItem != null)
            {
                float range = 300;
                TerrorbornItem modItem = TerrorbornItem.modItem(currentItem);

                for (int i = 0; i < 200; i++)
                {
                    NPC NPC = Main.npc[i];
                    if (!NPC.friendly && NPC.Distance(Player.Center) <= range + (NPC.height + NPC.width) / 4 && NPC.active && NPC.type != NPCID.TargetDummy)
                    {
                        GainTerror(modItem.terrorPotionTerror, false, false, true);
                    }
                }

                currentItem.stack--;

                SoundExtensions.PlaySoundOld(SoundID.Item3, Player.Center);
                Player.velocity = Vector2.Zero;
                DustCircle(Player.Center, 180, range, 63, -5, 3f);

                Player.AddBuff(ModContent.BuffType<Buffs.Debuffs.TerrorSickness>(), TerrorPotionCooldown);
            }
        }

        public void DustCircle(Vector2 position, int Dusts, float Radius, int DustType, float DustSpeed, float DustScale = 1f) //Thanks to seraph for this code
        {
            float currentAngle = Main.rand.Next(360);
            for (int i = 0; i < Dusts; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Dusts) * i) + currentAngle));
                direction.X *= Radius;
                direction.Y *= Radius;

                Dust dust = Dust.NewDustPerfect(position + direction, DustType, (direction / Radius) * DustSpeed, 0, default(Color), DustScale);
                dust.noGravity = true;
                dust.noLight = true;
                dust.alpha = 125;
            }
        }

        int ShriekCounter = 0;
        int SoundCounter = 0;
        int timeSincePressed = 0;
        public void UpdateShriekOfHorror()
        {
            bool darkblood = Player.HasBuff(ModContent.BuffType<Buffs.Darkblood>());

            if (TerrorbornMod.ShriekOfHorror.JustPressed)
            {
                timeSincePressed = 0;
            }

            if (TerrorbornMod.ShriekOfHorror.Current)
            {
                timeSincePressed++;
            }

            if (TerrorbornMod.ShriekOfHorror.JustReleased && timeSincePressed <= 10 && !Player.HasBuff(ModContent.BuffType<Buffs.Debuffs.TerrorSickness>()) && TerrorbornSystem.obtainedShriekOfHorror)
            {
                ConsumeTerrorPotion();
            }

            if (TerrorbornMod.ShriekOfHorror.Current && (Player.velocity.Y == 0 || ShriekOfHorrorMovement > 0) && (Player.itemTime <= 0 || darkblood) && TimeFreezeTime <= 0 && VoidBlinkTime <= 0 && BlinkDashTime <= 0)
            {
                MidShriek = true;
            }
            if (!TerrorbornMod.ShriekOfHorror.Current || Player.dead)
            {
                MidShriek = false;
            }


            if (MidShriek)
            {
                if (!(ShriekOfHorrorMovement == 0))
                {
                    if ((int)Player.velocity.Y != 0)
                    {
                        Player.position -= Player.velocity * (1 - ShriekOfHorrorMovement);
                    }
                    else
                    {
                        Player.position.X -= Player.velocity.X * (1 - ShriekOfHorrorMovement);
                    }
                }

                if (ShriekCounter > 0)
                {
                    ShriekTime = 0;
                    ShriekCounter--;
                    if (!darkblood)
                    {
                        Player.bodyFrame.Y = 6 * Player.bodyFrame.Height;
                    }
                }
                else
                {
                    ShriekTime++;
                    Shriek();
                    if (!darkblood)
                    {
                        Player.bodyFrame.Y = 5 * Player.bodyFrame.Height;
                    }
                    SoundCounter--;
                    if (SoundCounter <= 0)
                    {
                        SoundCounter = 22;
                        SoundExtensions.PlaySoundOld(SoundID.Item103, Player.Center);
                    }
                    Vector2 dustpos = Player.Center;
                    dustpos.Y -= 13;
                    dustpos.X -= 3;

                    dustpos.X += Player.direction * 1;

                    int dust = Dust.NewDust(dustpos, 0, 0, 54);
                    Main.dust[dust].velocity = Player.velocity / 3;
                    TerrorbornSystem.ScreenShake(5);
                }
            }
            else
            {
                SoundCounter = 0;
                ShriekCounter = (int)(80 * ShriekSpeed);
                ShriekTime = 0;
            }

            if (TimeFreezeTime > 0 || VoidBlinkTime > 0 || BlinkDashTime > 0)
            {
                SoundCounter = 0;
                ShriekCounter = (int)(80 * ShriekSpeed);
                ShriekTime = 0;
            }
        }

        int dustCounter = 15;
        public void Shriek()
        {
            dustCounter--;
            if (dustCounter <= 0)
            {
                dustCounter = 15;
                DustExplosion(Player.Center, 0, 360, 45 * ShriekRangeMultiplier, 54, 1, true);
            }

            float range = 375 * ShriekRangeMultiplier;
            float NPCsInRange = 0;
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
                if (target.Distance(Player.Center) < range && !target.friendly && target.active && target.lifeMax > 5)
                {
                    NPCsInRange++;
                    //target.velocity -= target.DirectionTo(Player.Center) * ((range - target.Distance(Player.Center)) / 400) * target.knockBackResist * ShriekKnockback;
                }
            }
            if (NPCsInRange > 0)
            {
                TerrorPercent = TerrorPercent + 12f / 60f * ShriekTerrorMultiplier;
                GainTerror(12f * ShriekTerrorMultiplier, true, true);
            }
        }

        void UpdateAbilityAnimations()
        {
            if (abilityAnimationType == 0)
            {
                Player.velocity = Vector2.Zero;
                Player.velocity.Y -= 1;
                abilityAnimationCounter2--;
                if (abilityAnimationCounter2 <= 0)
                {
                    abilityAnimationCounter1--;
                    if (abilityAnimationCounter1 <= 0)
                    {
                        DustExplosion(Player.Center, 0, 360, 40, 66, 1, true);
                        SoundExtensions.PlaySoundOld(SoundID.Item103, Player.Center);
                        TerrorbornSystem.ScreenShake(30);
                    }
                    else
                    {
                        SoundExtensions.PlaySoundOld(SoundID.MaxMana, Player.Center);
                        TerrorbornSystem.ScreenShake(5);
                        DustExplosion(Player.Center, 0, 360, 10, 66, 0.5f, true);
                        abilityAnimationCounter2 = 4;
                    }
                }
            }
        }

        int dashTimer = 0;
        int downCounter = 60;
        bool downCounterCounter = false;

        bool usingPrimary;
        bool usingSecondary;
        public override void UpdateAutopause()
        {
            if (TerrorbornMod.OpenTerrorAbilityMenu.JustPressed)
            {
                if (ShowTerrorAbilityMenu)
                {
                    SoundExtensions.PlaySoundOld(SoundID.MenuClose);
                    ShowTerrorAbilityMenu = false;
                }
                else
                {
                    SoundExtensions.PlaySoundOld(SoundID.MenuOpen);
                    ShowTerrorAbilityMenu = true;
                }
            }
        }

        public void SuperBlinkDash()
        {
            BlinkDashTime = 0;
            DustExplosion(Player.Center, 0, 25, 45, 162, 2f, true);
            SoundExtensions.PlaySoundOld(SoundID.Item72, Player.Center);
            Player.velocity = BlinkDashVelocity * 1.5f;

            blinkDashSpinRotation = 0f;
            blinkSpin = true;
            if (BlinkDashVelocity.X != 0)
            {
                blinkDashDirection = Math.Sign(BlinkDashVelocity.X);
            }
            else
            {
                blinkDashDirection = Player.direction;
            }
            SoundExtensions.PlaySoundOld(SoundID.Item62, Player.Center);
            TerrorbornSystem.ScreenShake(10f);
        }

        public void TwilightOverload()
        {
            InTwilightOverload = true;
            bleepWait = 0;
            bleepsLeft = 3;
            SoundExtensions.PlaySoundOld(SoundID.Zombie104, (int)Player.Center.X, (int)Player.Center.Y, 104, 1, 2f);
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (TerrorbornSystem.TwilightMode && TerrorPercent <= 3f)
            {
                damage.Scale(0.75f);
            }
        }

        float parryEffectProgress = 0;
        float blinkDashSpinRotation = 0f;
        bool blinkSpin = false;
        int blinkDashDirection = 0;
        int bleepWait = 0;
        int bleepsLeft = 0;
        public override void PostUpdate()
        {
            if (combatTime > 0)
            {
                combatTime--;
                inCombat = true;
            }
            else
            {
                inCombat = false;
            }

            if (TerrorbornSystem.TwilightMode)
            {
                if (inCombat)
                {
                    LoseTerror(0.5f, true, true);
                }

                if (TerrorPercent <= 3f)
                {
                    if (Player.statLife < TwilightHPCap)
                    {
                        TwilightHPCap = Player.statLife;
                    }
                    if (Player.statLife > TwilightHPCap)
                    {
                        Player.statLife = TwilightHPCap;
                    }
                    Player.statDefense /= 2;
                    Player.AddBuff(ModContent.BuffType<Buffs.Debuffs.TwilightWarning>(), 2);
                }
                else
                {
                    TwilightHPCap = Player.statLifeMax2;
                }

                if (TwilightPower > 1f)
                {
                    TwilightOverload();
                    TwilightPower = 1f;
                }

                if (TwilightPower > 0f)
                {
                    if (InTwilightOverload)
                    {
                        TwilightPower -= 1f / (10f * 60f);
                        GainTerror(3f, true, true, false);
                    }
                    else
                    {
                        TwilightPower -= 1f / (60f * 60f);
                    }
                }

                if (TwilightPower < 0f)
                {
                    TwilightPower = 0f;
                    InTwilightOverload = false;
                }
            }

            if (InTwilightOverload)
            {
                TerrorbornSystem.ScreenShake(1.5f);
                bleepWait--;
                if (bleepWait <= 0)
                {
                    ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/undertalewarning").Value.Play(Main.soundVolume, 0f, 0f);
                    bleepsLeft--;
                    if (bleepsLeft <= 0)
                    {
                        bleepsLeft = 3;
                        bleepWait = 30;
                    }
                    else
                    {
                        bleepWait = 6;
                    }
                }
            }

            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC NPC = Main.npc[i];
                if (NPC.active && NPC != null)
                {
                    if (NPC.Distance(Player.Center) <= 550 && !NPC.friendly && NPC.lifeMax > 15)
                    {
                        if (combatTime < 600)
                        {
                            combatTime = 600;
                        }
                    }
                }
            }

            if (deimosChained.Count > 0)
            {
                for (int i = 0; i < deimosChained.Count; i++)
                {
                    if (i > deimosChained.Count || deimosChained[i] == null)
                    {
                        break;
                    }
                    NPC NPC = deimosChained[i];
                    if (!NPC.active)
                    {
                        deimosChained.Remove(NPC);
                    }
                }
            }

            if (blinkSpin)
            {
                blinkDashSpinRotation += MathHelper.ToRadians(15f) * blinkDashDirection;
                Player.fullRotation = blinkDashSpinRotation;
                Player.fullRotationOrigin = new Vector2(Player.width / 2, Player.height / 2);
                if (Math.Abs(blinkDashSpinRotation) >= MathHelper.ToRadians(360))
                {
                    blinkSpin = false;
                    Player.fullRotation = 0f;
                }
            }

            if (HeadHunterCritCooldown > 0)
            {
                HeadHunterCritCooldown--;
            }

            //Main.manaTexture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Mana");
            //if (MidnightFruit == 20)
            //{
            //    Main.manaTexture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/MidnightMana");
            //}

            if (ParryTime > 0)
            {
                Player.bodyFrame.Y = 6 * Player.bodyFrame.Height;
                canUseItems = false;
                ParryTime--;
            }


            if (Main.netMode != NetmodeID.Server && Filters.Scene["ParryShockwave"].IsActive())
            {
                parryEffectProgress += 1f / 60f;
                Filters.Scene["ParryShockwave"].GetShader().UseProgress(parryEffectProgress).UseOpacity(100 * (1 - parryEffectProgress / 3f));
                if (parryEffectProgress > 0.5f)
                {
                    Filters.Scene["ParryShockwave"].Deactivate();
                }
            }

            if (ParryCooldown > 0)
            {
                ParryCooldown--;
                if (ParryCooldown <= 0)
                {
                    SoundExtensions.PlaySoundOld(SoundID.MaxMana, (int)(Player.Center.X), (int)(Player.Center.Y), 0);
                    CombatText.NewText(Player.getRect(), parryColor, "Parry Recharged", false, true);
                }
            }

            if (TerrorbornSystem.incendiaryRitual)
            {
                Player.AddBuff(ModContent.BuffType<Buffs.Debuffs.IncendiaryCurse>(), 2);
            }

            if (GelatinArmorTime > 0)
            {
                GelatinArmorTime--;
                int dust = Dust.NewDust(Player.position, Player.width, Player.height, DustID.t_Slime);
                Main.dust[dust].color = Color.LightBlue;
                Main.dust[dust].velocity /= 2;
                Main.dust[dust].alpha = 255 / 2;
            }

            if (terrorDrainCounter > 0)
            {
                terrorDrainCounter--;
                TerrorPercent += 1f / 60f;
            }

            if (VoidBlinkTime > 0)
            {
                VoidBlinkTime--;
                if (VoidBlinkTime == 0)
                {
                    DustExplosion(Player.Center, 0, 25, 15, 27, 1.5f, true);
                    SoundExtensions.PlaySoundOld(SoundID.Item72, Player.Center);
                }
                if (VoidBlinkTime == 60)
                {
                    DustExplosion(Player.Center, 0, 25, 7.5f, 27, 1.5f, true);
                    SoundExtensions.PlaySoundOld(SoundID.MaxMana, Player.Center);
                }
                Player.immuneAlpha = 255 / 2;
            }

            if (BlinkDashCooldown > 0)
            {
                BlinkDashCooldown--;
            }

            if (BlinkDashTime > 0)
            {
                Player.noFallDmg = true;
                BlinkDashCooldown = 30;
                Player.velocity = BlinkDashVelocity;
                foreach (NPC NPC in Main.npc)
                {
                    if (NPC.active && NPC.Distance(Player.Center) <= 100 && !NPC.friendly)
                    {
                        NPC.AddBuff(BuffID.Confused, 60 * 3);
                    }
                }
                BlinkDashTime--;
                if (BlinkDashTime == 0)
                {
                    DustExplosion(Player.Center, 0, 15, 30, 162, 1.5f, true);
                    SoundExtensions.PlaySoundOld(SoundID.Item72, Player.Center);
                    Player.velocity /= 2;
                }
                if (BlinkDashTime <= 5 && (TerrorbornMod.PrimaryTerrorAbility.JustPressed || TerrorbornMod.SecondaryTerrorAbility.JustPressed))
                {
                    SuperBlinkDash();
                }
                Player.immuneAlpha = 255;
            }
            if (TimeFreezeTime > 0)
            {
                TimeFreezeTime--;
            }

            if (TerrorbornMod.OpenTerrorAbilityMenu.JustPressed)
            {
                if (ShowTerrorAbilityMenu)
                {
                    SoundExtensions.PlaySoundOld(SoundID.MenuClose);
                    ShowTerrorAbilityMenu = false;
                }
                else
                {
                    SoundExtensions.PlaySoundOld(SoundID.MenuOpen);
                    ShowTerrorAbilityMenu = true;
                }
            }

            primaryAbilityInt = TerrorbornUtils.abilityToInt(primaryAbility);
            secondaryAbilityInt = TerrorbornUtils.abilityToInt(secondaryAbility);

            if (TerrorbornMod.PrimaryTerrorAbility.JustPressed)
            {
                usingPrimary = true;
            }
            if (TerrorbornMod.PrimaryTerrorAbility.JustReleased)
            {
                usingPrimary = false;
            }
            if (TerrorbornMod.SecondaryTerrorAbility.JustPressed)
            {
                usingSecondary = true;
            }
            if (TerrorbornMod.SecondaryTerrorAbility.JustReleased)
            {
                usingSecondary = false;
            }

            if (primaryAbility.HeldDown() && TerrorPercent >= primaryAbility.Cost() / 60 && primaryAbility.canUse(Player) && TerrorbornMod.PrimaryTerrorAbility.Current)
            {
                primaryAbility.OnUse(Player);
                LoseTerror(primaryAbility.Cost(), true, true, false);

            }

            if (!primaryAbility.HeldDown() && TerrorPercent >= primaryAbility.Cost() && primaryAbility.canUse(Player) && TerrorbornMod.PrimaryTerrorAbility.JustPressed)
            {
                primaryAbility.OnUse(Player);
                LoseTerror(primaryAbility.Cost(), true, false, false);
            }

            if (secondaryAbility.HeldDown() && TerrorPercent >= secondaryAbility.Cost() * 1.5f / 60 && secondaryAbility.canUse(Player) && TerrorbornMod.SecondaryTerrorAbility.Current)
            {
                secondaryAbility.OnUse(Player);
                LoseTerror(secondaryAbility.Cost() * 1.5f, true, true, false);

            }

            if (!secondaryAbility.HeldDown() && TerrorPercent >= secondaryAbility.Cost() * 1.5f && secondaryAbility.canUse(Player) && TerrorbornMod.SecondaryTerrorAbility.JustPressed)
            {
                secondaryAbility.OnUse(Player);
                LoseTerror(secondaryAbility.Cost() * 1.5f, true, false, false);
            }

            if (SangoonBandCooldown > 0)
            {
                SangoonBandCooldown--;
            }

            if (abilityAnimationCounter1 > 0)
            {
                UpdateAbilityAnimations();
            }

            if (TerrorbornSystem.obtainedShriekOfHorror)
            {
                UpdateShriekOfHorror();
            }
            if (TerrorPercent > 100)
            {
                TerrorPercent = 100;
            }
            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.Dunestock>())) Player.buffImmune[BuffID.WindPushed] = true;

            if (TerrorbornMod.quickVirus.JustPressed && Player.HasItem(ModContent.ItemType<Items.AstralSpark>()))
            {
                astralSparkData.Transform(Player);
            }

            if (TerrorbornMod.quickVirus.JustPressed && Player.HasItem(ModContent.ItemType<Items.GraniteVirusSpark>()))
            {
                GraniteSparkData.Transform(Player);
            }

            if (graniteSpark || astralSpark)
            {
                Player.wings = 0;
            }

            if (iFrames > 0)
            {
                iFrames--;
                Player.immuneAlpha = 125;
            }

            if (TenebrisDashTime > 0)
            {
                TenebrisDashTime--;
                int dust = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, 74, 0f, 0f, 100, Scale: 1.5f);
                Player.velocity = TenebrisDashVelocity;
            }

            if (!NPC.AnyNPCs(ModContent.NPCType<NPCs.TownNPCs.SkeletonSheriff>()) && CombatPoints > 0)
            {
                CombatPoints = 0;
            }
        }

        public override void OnRespawn(Player Player)
        {
            usingPrimary = false;
            combatTime = 0;
            inCombat = false;
            if (TerrorbornSystem.TwilightMode)
            {
                TwilightHPCap = Player.statLifeMax2;
                Player.statLife = Player.statLifeMax2;
                TerrorPercent = 50f;
            }
        }

        public void ActivateParryEffect()
        {
            if (Main.netMode != NetmodeID.Server && !Filters.Scene["ParryShockwave"].IsActive())
            {
                parryEffectProgress = 0;
                Filters.Scene.Activate("ParryShockwave", Player.Center).GetShader().UseColor(1, 1, 10).UseTargetPosition(Player.Center); //Ripple Count, Ripple Size, Ripple Speed
                CombatText.NewText(Player.getRect(), parryColor, "Successful Parry!", true, false);
            }
        }

        public override void ModifyHitByNPC(NPC NPC, ref int damage, ref bool crit)
        {
            if (TerrorbornSystem.TwilightMode)
            {
                damage = (int)(damage * 1.5f);
            }

            if (ParryTime > 0)
            {
                ParryTime = 0;
                damage /= 4;
                JustParried = true;
                SoundExtensions.PlaySoundOld(SoundID.Item37, Player.Center);
                ActivateParryEffect();
            }

            if (GelatinPunishmentDamage > 0)
            {
                damage += GelatinPunishmentDamage;
                GelatinPunishmentDamage = 0;
                SoundExtensions.PlaySoundOld(SoundID.NPCDeath1, Player.Center);
                for (int i = 0; i < 15; i++)
                {
                    int dust = Dust.NewDust(Player.position, Player.width, Player.height, DustID.t_Slime);
                    Main.dust[dust].color = Color.LightBlue;
                    Main.dust[dust].velocity /= 2;
                    Main.dust[dust].alpha = 255 / 2;
                }
            }

            if (GelatinArmorTime > 0)
            {
                GelatinArmorTime = 0;
                GelatinPunishmentDamage = damage / 3;
                SoundExtensions.PlaySoundOld(SoundID.NPCHit1, Player.Center);
                damage = 0;
            }

            if (MidShriek)
            {
                damage = (int)(damage * ShriekOfHorrorExtraDamageMultiplier);
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            if (TerrorbornSystem.TwilightMode)
            {
                damage = (int)(damage * 1.35f);
            }

            if (ParryTime > 0)
            {
                ParryTime = 0;
                damage /= 4;
                JustParried = true;
                SoundExtensions.PlaySoundOld(SoundID.Item37, Player.Center);
                ActivateParryEffect();
            }

            if (GelatinPunishmentDamage > 0)
            {
                damage += GelatinPunishmentDamage;
                GelatinPunishmentDamage = 0;
                SoundExtensions.PlaySoundOld(SoundID.NPCDeath1, Player.Center);
                for (int i = 0; i < 15; i++)
                {
                    int dust = Dust.NewDust(Player.position, Player.width, Player.height, DustID.t_Slime);
                    Main.dust[dust].color = Color.LightBlue;
                    Main.dust[dust].velocity /= 2;
                    Main.dust[dust].alpha = 255 / 2;
                }
            }

            if (GelatinArmorTime > 0)
            {
                GelatinArmorTime = 0;
                GelatinPunishmentDamage = damage / 3;
                SoundExtensions.PlaySoundOld(SoundID.NPCHit1, Player.Center);
                damage = 0;
            }

            if (MidShriek)
            {
                damage = (int)(damage * ShriekOfHorrorExtraDamageMultiplier);
            }
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (iFrames > 0 || VoidBlinkTime > 0 || BlinkDashTime > 0)
            {
                return false;
            }
            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
        }

        public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
        {
            float currentAngle = Main.rand.Next(360);

            //if(Main.netMode!=1){
            for (int i = 0; i < Streams; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
                direction.X *= DustSpeed;
                direction.Y *= DustSpeed;

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }

        public override void UpdateBadLifeRegen()
        {
            if (Player.HasBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>()))
            {
                if (Player.lifeRegen > 0) Player.lifeRegen = 0;
                Player.lifeRegen -= 10 + (int)((Player.statDefense / 100f) * 18f);
            }
            if (badLifeRegen > 0)
            {
                if (Player.lifeRegen > 0) Player.lifeRegen = 0;
                Player.lifeRegen -= badLifeRegen;
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            bool disableInstantDeathProtection = false;
            if (NPC.AnyNPCs(NPCID.HallowBoss) && Main.dayTime) disableInstantDeathProtection = true;
            if (NPC.AnyNPCs(NPCID.DungeonGuardian)) disableInstantDeathProtection = true;
            if (NPC.AnyNPCs(NPCID.SkeletronHead) && Main.dayTime) disableInstantDeathProtection = true;
            if (NPC.AnyNPCs(NPCID.SkeletronPrime) && Main.dayTime) disableInstantDeathProtection = true;
            if (InsantDeathProtection && TerrorbornMod.InstantDeathProtectionEnabled && !disableInstantDeathProtection)
            {
                Player.statLife = (int)(Player.statLifeMax2 * 0.1f);
                iFrames = 120;
                SoundStyle style = SoundID.PlayerKilled;
                style.Pitch = 0.5f;
                SoundEngine.PlaySound(style, Player.Center);
                return false;
            }

            if (SpecterLocket && !Player.HasBuff(ModContent.BuffType<Buffs.Debuffs.UnholyCooldown>()))
            {
                CombatText.NewText(Player.getRect(), Color.OrangeRed, "Revived!", true);
                SoundExtensions.PlaySoundOld(SoundID.NPCDeath52, Player.Center);
                Player.statLife = 25;
                Player.HealEffect(25);
                Player.AddBuff(ModContent.BuffType<Buffs.IncendiaryRevival>(), 60 * 4);
                Player.AddBuff(ModContent.BuffType<Buffs.Debuffs.UnholyCooldown>(), 3600 * 3);
                return false;
            }

            if (MidShriek)
            {
                int choice = Main.rand.Next(3);
                if (choice == 0)
                {
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was overloaded with fear.");
                }
                if (choice == 1)
                {
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + " drained their own life.");
                }
                if (choice == 2)
                {
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + " couldn't handle their power.");
                }
            }

            TerrorPercent = 0f;

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("CombatPoints", CombatPoints);
            tag.Add("EyeOfTheMenace", EyeOfTheMenace);
            tag.Add("GoldenTooth", GoldenTooth);
            tag.Add("CoreOfFear", CoreOfFear);
            tag.Add("AnekronianApple", AnekronianApple);
            tag.Add("DemonicLense", DemonicLense);
            tag.Add("PrimaryAbility", primaryAbilityInt);
            tag.Add("SecondaryAbility", secondaryAbilityInt);
            tag.Add("unlockedAbilities", unlockedAbilities);
            tag.Add("TerrorPercent", TerrorPercent);
            tag.Add("MidnightFruit", MidnightFruit);
            tag.Add("DarkEnergyStored", DarkEnergyStored);
        }

        public override void LoadData(TagCompound tag)
        {
            CombatPoints = tag.GetInt("CombatPoints");
            EyeOfTheMenace = tag.GetBool("EyeOfTheMenace");
            GoldenTooth = tag.GetBool("GoldenTooth");
            CoreOfFear = tag.GetBool("CoreOfFear");
            DemonicLense = tag.GetBool("DemonicLense");
            AnekronianApple = tag.GetBool("AnekronianApple");
            primaryAbilityInt = tag.GetInt("PrimaryAbility");
            secondaryAbilityInt = tag.GetInt("SecondaryAbility");
            unlockedAbilities = tag.GetList<int>("unlockedAbilities");
            TerrorPercent = tag.GetFloat("TerrorPercent");
            MidnightFruit = tag.GetInt("MidnightFruit");
            DarkEnergyStored = tag.GetInt("DarkEnergyStored");

            primaryAbility = TerrorbornUtils.intToAbility(primaryAbilityInt);
            secondaryAbility = TerrorbornUtils.intToAbility(secondaryAbilityInt);
        }

        public static TerrorbornPlayer modPlayer(Player Player)
        {
            return Player.GetModPlayer<TerrorbornPlayer>();
        }
    }
}
