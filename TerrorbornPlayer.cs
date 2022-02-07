using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Effects;
using TerrorbornMod.Abilities;
using TerrorbornMod.ForegroundObjects;
using Terraria.Graphics.Shaders;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Input;
using Extensions;
using TerrorbornMod.TwilightMode;

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
        public float magicUseSpeed = 1f;
        public float rangedUseSpeed = 1f;
        public float toolUseSpeed = 1f;
        public float placeSpeed = 1f;
        public float allUseSpeed = 1f;
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

        #region GlowmaskStuff
        public static readonly PlayerLayer legsGlow = new PlayerLayer("TerrorbornMod", "Legs_Glow", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            if (player.armor[2] != null && player.armor[2].type > Main.maxItemTypes)
            {
                if (player.armor[2] != null && player.armor[2].type > Main.maxItemTypes)
                {
                    Texture2D texture = ModContent.GetTexture(player.armor[2].modItem.Texture + "_Legs_Glow");
                    Vector2 drawPosition = drawInfo.position + new Vector2(-player.bodyFrame.Width / 2 + (player.width / 2), player.height - player.bodyFrame.Height + 10) + player.headPosition + drawInfo.legOrigin;
                    DrawData data = new DrawData(texture, new Vector2((int)(drawPosition.X - Main.screenPosition.X), (int)(drawPosition.Y - Main.screenPosition.Y) - 6), player.legFrame, Color.White * (drawInfo.bodyColor.A / 255f), player.legRotation, drawInfo.legOrigin, 1f, drawInfo.spriteEffects, 0);
                    Main.playerDrawData.Add(data);
                }
            }
        });

        public static readonly PlayerLayer legsGlowVanity = new PlayerLayer("TerrorbornMod", "Legs_Glow", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            int vanityAddend = 10;
            if (player.armor[2 + vanityAddend] != null && player.armor[2 + vanityAddend].type > Main.maxItemTypes)
            {
                if (player.armor[2 + vanityAddend] != null && player.armor[2 + vanityAddend].type > Main.maxItemTypes)
                {
                    Texture2D texture = ModContent.GetTexture(player.armor[2 + vanityAddend].modItem.Texture + "_Legs_Glow");
                    Vector2 drawPosition = drawInfo.position + new Vector2(-player.bodyFrame.Width / 2 + (player.width / 2), player.height - player.bodyFrame.Height + 10) + player.headPosition + drawInfo.legOrigin;
                    DrawData data = new DrawData(texture, new Vector2((int)(drawPosition.X - Main.screenPosition.X), (int)(drawPosition.Y - Main.screenPosition.Y) - 6), player.legFrame, Color.White * (drawInfo.bodyColor.A / 255f), player.legRotation, drawInfo.legOrigin, 1f, drawInfo.spriteEffects, 0);
                    Main.playerDrawData.Add(data);
                }
            }
        });

        public static readonly PlayerLayer armsGlow = new PlayerLayer("TerrorbornMod", "Arms_Glow", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            if (player.armor[1] != null && player.armor[1].type > Main.maxItemTypes)
            {
                if (player.armor[1] != null && player.armor[1].type > Main.maxItemTypes)
                {
                    Texture2D texture = ModContent.GetTexture(player.armor[1].modItem.Texture + "_Arms_Glow");
                    Vector2 drawPosition = drawInfo.position + new Vector2(-player.bodyFrame.Width / 2 + (player.width / 2), player.height - player.bodyFrame.Height + 10) + player.headPosition + drawInfo.bodyOrigin;
                    DrawData data = new DrawData(texture, new Vector2((int)(drawPosition.X - Main.screenPosition.X), (int)(drawPosition.Y - Main.screenPosition.Y) - 6), player.bodyFrame, Color.White * (drawInfo.bodyColor.A / 255f), player.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                    Main.playerDrawData.Add(data);
                }
            }
        });

        public static readonly PlayerLayer armsGlowVanity = new PlayerLayer("TerrorbornMod", "Arms_Glow", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            int vanityAddend = 10;
            if (player.armor[1 + vanityAddend] != null && player.armor[1 + vanityAddend].type > Main.maxItemTypes)
            {
                if (player.armor[1 + vanityAddend] != null && player.armor[1 + vanityAddend].type > Main.maxItemTypes)
                {
                    Texture2D texture = ModContent.GetTexture(player.armor[1 + vanityAddend].modItem.Texture + "_Arms_Glow");
                    Vector2 drawPosition = drawInfo.position + new Vector2(-player.bodyFrame.Width / 2 + (player.width / 2), player.height - player.bodyFrame.Height + 10) + player.headPosition + drawInfo.bodyOrigin;
                    DrawData data = new DrawData(texture, new Vector2((int)(drawPosition.X - Main.screenPosition.X), (int)(drawPosition.Y - Main.screenPosition.Y) - 6), player.bodyFrame, Color.White * (drawInfo.bodyColor.A / 255f), player.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                    Main.playerDrawData.Add(data);
                }
            }
        });

        public static readonly PlayerLayer bodyGlow = new PlayerLayer("TerrorbornMod", "Body_Glow", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            if (player.armor[1] != null && player.armor[1].type > Main.maxItemTypes)
            {
                if (player.armor[1] != null && player.armor[1].type > Main.maxItemTypes)
                {
                    Texture2D texture = ModContent.GetTexture(player.armor[1].modItem.Texture + "_Body_Glow");
                    Vector2 drawPosition = drawInfo.position + new Vector2(-player.bodyFrame.Width / 2 + (player.width / 2), player.height - player.bodyFrame.Height + 10) + player.headPosition + drawInfo.bodyOrigin;
                    DrawData data = new DrawData(texture, new Vector2((int)(drawPosition.X - Main.screenPosition.X), (int)(drawPosition.Y - Main.screenPosition.Y) - 6), player.bodyFrame, Color.White * (drawInfo.bodyColor.A / 255f), player.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                    Main.playerDrawData.Add(data);
                }
            }
        });

        public static readonly PlayerLayer bodyGlowVanity = new PlayerLayer("TerrorbornMod", "Body_Glow", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            int vanityAddend = 10;
            if (player.armor[1 + vanityAddend] != null && player.armor[1 + vanityAddend].type > Main.maxItemTypes)
            {
                if (player.armor[1 + vanityAddend] != null && player.armor[1 + vanityAddend].type > Main.maxItemTypes)
                {
                    Texture2D texture = ModContent.GetTexture(player.armor[1 + vanityAddend].modItem.Texture + "_Body_Glow");
                    Vector2 drawPosition = drawInfo.position + new Vector2(-player.bodyFrame.Width / 2 + (player.width / 2), player.height - player.bodyFrame.Height + 10) + player.headPosition + drawInfo.bodyOrigin;
                    DrawData data = new DrawData(texture, new Vector2((int)(drawPosition.X - Main.screenPosition.X), (int)(drawPosition.Y - Main.screenPosition.Y) - 6), player.bodyFrame, Color.White * (drawInfo.bodyColor.A / 255f), player.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                    Main.playerDrawData.Add(data);
                }
            }
        });

        public static readonly PlayerLayer headGlow = new PlayerLayer("TerrorbornMod", "Head_Glow", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            if (player.armor[0] != null && player.armor[0].type > Main.maxItemTypes)
            {
                if (player.armor[0] != null && player.armor[0].type > Main.maxItemTypes)
                {
                    Texture2D texture = ModContent.GetTexture(player.armor[0].modItem.Texture + "_Head_Glow");
                    Vector2 drawPosition = drawInfo.position + new Vector2(-player.bodyFrame.Width / 2 + (player.width / 2), player.height - player.bodyFrame.Height + 10) + player.headPosition + drawInfo.bodyOrigin;
                    DrawData data = new DrawData(texture, new Vector2((int)(drawPosition.X - Main.screenPosition.X), (int)(drawPosition.Y - Main.screenPosition.Y) - 6), player.bodyFrame, Color.White * (drawInfo.bodyColor.A / 255f), player.headRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                    Main.playerDrawData.Add(data);
                }
            }
        });

        public static readonly PlayerLayer headGlowVanity = new PlayerLayer("TerrorbornMod", "Head_Glow", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            int vanityAddend = 10;
            if (player.armor[0 + vanityAddend] != null && player.armor[0 + vanityAddend].type > Main.maxItemTypes)
            {
                if (player.armor[0 + vanityAddend] != null && player.armor[0 + vanityAddend].type > Main.maxItemTypes)
                {
                    Texture2D texture = ModContent.GetTexture(player.armor[0 + vanityAddend].modItem.Texture + "_Head_Glow");
                    Vector2 drawPosition = drawInfo.position + new Vector2(-player.bodyFrame.Width / 2 + (player.width / 2), player.height - player.bodyFrame.Height + 10) + player.headPosition + drawInfo.bodyOrigin;
                    DrawData data = new DrawData(texture, new Vector2((int)(drawPosition.X - Main.screenPosition.X), (int)(drawPosition.Y - Main.screenPosition.Y) - 6), player.bodyFrame, Color.White * (drawInfo.bodyColor.A / 255f), player.headRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                    Main.playerDrawData.Add(data);
                }
            }
        });
        #endregion


        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int vanityAddend = 10;

            bool didDoVanity = false;

            if (player.armor[2 + vanityAddend] != null && player.armor[2 + vanityAddend].type > Main.maxItemTypes)
            {
                if (player.armor[2 + vanityAddend].modItem.mod.Name == "TerrorbornMod" && ModContent.TextureExists(player.armor[2 + vanityAddend].modItem.Texture + "_Legs_Glow"))
                {
                    didDoVanity = true;
                    int index = layers.FindIndex((pl) => pl.Name == "Legs");
                    layers.Insert(index + 1, legsGlowVanity);
                }
            }

            if (player.armor[2] != null && player.armor[2].type > Main.maxItemTypes && !didDoVanity && player.armor[2 + vanityAddend].IsAir)
            {
                if (player.armor[2].modItem.mod.Name == "TerrorbornMod" && ModContent.TextureExists(player.armor[2].modItem.Texture + "_Legs_Glow"))
                {
                    int index = layers.FindIndex((pl) => pl.Name == "Legs");
                    layers.Insert(index + 1, legsGlow);
                }
            }

            didDoVanity = false;

            if (player.armor[1 + vanityAddend] != null && player.armor[1 + vanityAddend].type > Main.maxItemTypes)
            {
                if (player.armor[1 + vanityAddend].modItem.mod.Name == "TerrorbornMod" && ModContent.TextureExists(player.armor[1 + vanityAddend].modItem.Texture + "_Arms_Glow"))
                {
                    didDoVanity = true;
                    int index = layers.FindIndex((pl) => pl.Name == "Arms");
                    layers.Insert(index + 1, armsGlowVanity);
                }
            }

            if (player.armor[1] != null && player.armor[1].type > Main.maxItemTypes && !didDoVanity && player.armor[1 + vanityAddend].IsAir)
            {
                if (player.armor[1].modItem.mod.Name == "TerrorbornMod" && ModContent.TextureExists(player.armor[1].modItem.Texture + "_Arms_Glow"))
                {
                    int index = layers.FindIndex((pl) => pl.Name == "Arms");
                    layers.Insert(index + 1, armsGlow);
                }
            }

            didDoVanity = false;

            if (player.armor[1 + vanityAddend] != null && player.armor[1 + vanityAddend].type > Main.maxItemTypes)
            {
                if (player.armor[1 + vanityAddend].modItem.mod.Name == "TerrorbornMod" && ModContent.TextureExists(player.armor[1 + vanityAddend].modItem.Texture + "_Body_Glow"))
                {
                    didDoVanity = true;
                    int index = layers.FindIndex((pl) => pl.Name == "Body");
                    layers.Insert(index + 1, bodyGlowVanity);
                }
            }

            if (player.armor[1] != null && player.armor[1].type > Main.maxItemTypes && !didDoVanity && player.armor[1 + vanityAddend].IsAir)
            {
                if (player.armor[1].modItem.mod.Name == "TerrorbornMod" && ModContent.TextureExists(player.armor[1].modItem.Texture + "_Body_Glow"))
                {
                    int index = layers.FindIndex((pl) => pl.Name == "Body");
                    layers.Insert(index + 1, bodyGlow);
                }
            }

            didDoVanity = false;

            if (player.armor[0 + vanityAddend] != null && player.armor[0 + vanityAddend].type > Main.maxItemTypes)
            {
                if (player.armor[0 + vanityAddend].modItem.mod.Name == "TerrorbornMod" && ModContent.TextureExists(player.armor[0 + vanityAddend].modItem.Texture + "_Head_Glow"))
                {
                    didDoVanity = true;
                    int index = layers.FindIndex((pl) => pl.Name == "Head");
                    layers.Insert(index + 1, headGlowVanity);
                }
            }

            if (player.armor[0] != null && player.armor[0].type > Main.maxItemTypes && !didDoVanity && player.armor[0 + vanityAddend].IsAir)
            {
                if (player.armor[0].modItem.mod.Name == "TerrorbornMod" && ModContent.TextureExists(player.armor[0].modItem.Texture + "_Head_Glow"))
                {
                    int index = layers.FindIndex((pl) => pl.Name == "Head");
                    layers.Insert(index + 1, headGlow);
                }
            }
        }

        public void TriggerAbilityAnimation(string name, string description1, string description2, int animationType, string description3 = "You can equip terror abilities by using the 'Open/Close Terror Abilities Menu' hotkey.", int visibilityTime = 600)
        {
            UI.TerrorAbilityUnlock.UnlockUI.abilityUILifetimeCounter = visibilityTime;
            UI.TerrorAbilityUnlock.UnlockUI.abilityUnlockName = name;
            UI.TerrorAbilityUnlock.UnlockUI.abilityUnlockDescription1 = description1;
            UI.TerrorAbilityUnlock.UnlockUI.abilityUnlockDescription2 = description2;
            UI.TerrorAbilityUnlock.UnlockUI.abilityUnlockDescription3 = description3;

            abilityAnimationType = animationType;
            if (animationType == 0) //Shriek of Horror/Default
            {
                abilityAnimationCounter1 = 9;
            }
        }

        public int parryLightTime = 0;
        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (MidShriek && drawInfo.position == player.position && ShriekTime > 0)
            {
                int effectTime = ShriekTime;
                if (effectTime > 120)
                {
                    effectTime = 120;
                }
                TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, player.Center - Main.screenPosition, (int)(TerrorPercent / 2 + 100 + effectTime / 2), Color.Black * (0.75f * (effectTime / 120f)));
            }

            if (drawInfo.position == player.position && parryLightTime > 0)
            {
                parryLightTime--;
                TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, player.Center - Main.screenPosition, (int)(100 + parryLightTime * 2), parryColor * (0.75f * (parryLightTime / 20f)));
            }
        }

        int effectCounter = 60;
        int progress = 0;
        public bool twilight = false;
        public override void UpdateBiomeVisuals()
        {
            if (ZoneIncendiary)
            {
                effectCounter--;
                if (effectCounter <= 0)
                {
                    effectCounter = Main.rand.Next(3, 6);
                    int maxDistance = 1500;
                    ForegroundObject.NewForegroundObject(new Vector2(Main.rand.Next(-maxDistance, maxDistance), Main.rand.Next(-maxDistance, maxDistance)), new IncendiaryFog());
                }
            }

            player.ManageSpecialBiomeVisuals("TerrorbornMod:PrototypeIShader", NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.PrototypeI>()));
            player.ManageSpecialBiomeVisuals("TerrorbornMod:DarknessShader", ZoneDeimostone || ZoneICU);
            player.ManageSpecialBiomeVisuals("TerrorbornMod:BlandnessShader", ZoneICU && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.InfectedIncarnate.InfectedIncarnate>()));
            //player.ManageSpecialBiomeVisuals("TerrorbornMod:IncarnateBoss", NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.InfectedIncarnate.InfectedIncarnate>()));
            player.ManageSpecialBiomeVisuals("TerrorbornMod:ColorlessShader", TimeFreezeTime > 0);

            player.ManageSpecialBiomeVisuals("TerrorbornMod:HexedMirage", HexedMirage);

            player.ManageSpecialBiomeVisuals("TerrorbornMod:TwilightShaderNight", twilight);

            //Filters.Scene["TerrorbornMod:GlitchShader"].GetShader().UseTargetPosition(new Vector2(0f, Main.rand.NextFloat(0f, 1f)));
            //Filters.Scene["TerrorbornMod:GlitchShader"].GetShader().UseIntensity(Main.rand.NextFloat(-0.1f, 0.1f));
            //switch (Main.rand.Next(3))
            //{
            //    case 0:
            //        Filters.Scene["TerrorbornMod:GlitchShader"].GetShader().UseColor(1f, 0f, 0f);
            //        break;
            //    case 1:
            //        Filters.Scene["TerrorbornMod:GlitchShader"].GetShader().UseColor(0f, 1f, 0f);
            //        break;
            //    case 2:
            //        Filters.Scene["TerrorbornMod:GlitchShader"].GetShader().UseColor(0f, 0f, 1f);
            //        break;
            //    default:
            //        Filters.Scene["TerrorbornMod:GlitchShader"].GetShader().UseColor(1f, 0f, 0f);
            //        break;
            //}

            //Filters.Scene["TerrorbornMod:GlitchShader"].GetShader().Update(Main._drawInterfaceGameTime);
            ////Filters.Scene["TerrorbornMod:GlitchShader"].Load();
            //player.ManageSpecialBiomeVisuals("TerrorbornMod:GlitchShader", true);
        }

        public override void UpdateBiomes()
        {
            ZoneDeimostone = TerrorbornWorld.deimostoneTiles > 75;

            Rectangle incendiaryBiomeRect = new Rectangle(0, 0, (int)(Main.maxTilesX / 4f * 16) + 120 * 16, (int)(Main.maxTilesY / 17f * 16) + 120 * 16);
            if (TerrorbornWorld.incendiaryIslandsSide == 1)
            {
                incendiaryBiomeRect = new Rectangle((Main.maxTilesX * 16) - (int)(Main.maxTilesX / 4f * 16) - 120 * 16, 0, (int)(Main.maxTilesX / 4f * 16) + 120 * 16, (int)(Main.maxTilesY / 17f * 16) + 120 * 16);
            }
            ZoneIncendiary = incendiaryBiomeRect.Intersects(player.getRect()) && Main.hardMode;

            ZoneICU = player.Distance(TerrorbornWorld.IIShrinePosition * 16 + new Vector2(0f, 140f) * 8) <= 1200 && player.behindBackWall;
        }

        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            Item compass = new Item();
            compass.SetDefaults(ModContent.ItemType<Items.MysteriousCompass>());
            items.Add(compass);

            if (TerrorbornMod.StartingItems)
            {
                //Extra starting weapons, accessories, etc.

                Item item = new Item();

                //staff
                item = new Item();
                item.SetDefaults(ItemID.AmethystStaff);
                items.Add(item);

                //torch
                item = new Item();
                item.SetDefaults(ItemID.Torch);
                item.stack = 50;
                items.Add(item);

                //torch
                item = new Item();
                item.SetDefaults(ItemID.Glowstick);
                item.stack = 50;
                items.Add(item);

                //bombs
                item = new Item();
                item.SetDefaults(ItemID.Bomb);
                item.stack = 15;
                items.Add(item);

                //ropes
                item = new Item();
                item.SetDefaults(ItemID.Rope);
                item.stack = 200;
                items.Add(item);

                //crown
                item = new Item();
                item.SetDefaults(ItemID.SlimeCrown);
                items.Add(item);

                //mining potions
                item = new Item();
                item.SetDefaults(ItemID.MiningPotion);
                item.stack = 3;
                items.Add(item);

                //spelunker potions
                item = new Item();
                item.SetDefaults(ItemID.SpelunkerPotion);
                item.stack = 3;
                items.Add(item);

                //shackle
                item = new Item();
                item.SetDefaults(ItemID.Hook);
                items.Add(item);
            }

            Item matrix = new Item();
            matrix.SetDefaults(ModContent.ItemType<Items.TwilightMatrix>());
            items.Add(matrix);
        }

        float fusionHealCounter = 0f;
        const float maxFusionHealCounter = 2.5f;
        public void LoseTerror(float amount, bool silent = true, bool perSecond = false, bool smallerText = false)
        {
            if (!(perSecond || silent))
            {
                CombatText.NewText(player.getRect(), Color.FromNonPremultiplied(108, 150, 143, 255), "-" + amount + "%", dot: smallerText);
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
                    player.HealEffect(healAmount);
                    player.statLife += healAmount;
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
                CombatText.NewText(player.getRect(), Color.FromNonPremultiplied(108, 150, 143, 255), amount + "%", dot: smallerText);
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
                    player.HealEffect(healAmount);
                    player.statLife += healAmount;
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
            ShriekOfHorrorMovement = 0f;
            TerrorTonic = false;
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
            rangedUseSpeed = 1f;
            toolUseSpeed = 1f;
            placeSpeed = 1f;
            SangoonBand = false;
            LiesOfNourishment = false;
            magicUseSpeed = 1f;
            allUseSpeed = 1f;
            IncendiaryShield = false;
            Aerodynamic = false;
            Glooped = false;
            PlasmaArmorBonus = false;
            PyroclasticShinobiBonus = false;
            AzuriteArmorBonus = false;
            VampiricPendant = false;
            TerrorPotionCooldown = 60 * 10;
            noAmmoConsumeChance = 0f;

            player.statManaMax2 += 5 * MidnightFruit;

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
                if (player.controlLeft) player.velocity.X = -graniteSparkData.speed;
                else if (player.controlRight) player.velocity.X = graniteSparkData.speed;
                else player.velocity.X = 0;

                if (player.controlUp) player.velocity.Y = -graniteSparkData.speed;
                else if (player.controlDown) player.velocity.Y = graniteSparkData.speed;
                else player.velocity.Y = 0;
            }

            if (astralSpark)
            {
                float speedMultiplier = 1f;

                if (player.controlJump) speedMultiplier = 5f;

                if (player.controlLeft) player.velocity.X = -astralSparkData.speed * speedMultiplier;
                else if (player.controlRight) player.velocity.X = astralSparkData.speed * speedMultiplier;
                else player.velocity.X = 0;

                if (player.controlUp) player.velocity.Y = -astralSparkData.speed * speedMultiplier;
                else if (player.controlDown) player.velocity.Y = astralSparkData.speed * speedMultiplier;
                else player.velocity.Y = 0;
            }

            if (ShriekOfHorrorMovement == 0 && MidShriek)
            {
                player.velocity = Vector2.Zero;
            }

            if (player.HeldItem != null && !player.HeldItem.IsAir && player.HeldItem.useStyle == ItemUseStyleID.SwingThrow && player.controlUseItem)
            {
                if (Main.MouseWorld.X > player.Center.X)
                {
                    player.direction = 1;
                }
                else
                {
                    player.direction = -1;
                }
            }
        }

        public override float UseTimeMultiplier(Item item)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            float finalMult = allUseSpeed;
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
            if (item.magic)
            {
                finalMult *= magicUseSpeed;
            }
            if (item.ranged)
            {
                finalMult *= rangedUseSpeed;
            }
            if (modItem.restless)
            {
                finalMult *= restlessUseSpeed;
                if (modItem.RestlessChargedUp())
                {
                    finalMult *= restlessChargedUseSpeed;
                }
                else
                {
                    finalMult *= restlessNonChargedUseSpeed;
                }
            }
            if (item.useTime <= (int)(1f / (finalMult - 1f)))
            {
                return 1f;
            }
            return finalMult;
        }

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {

        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPCWithProj(proj, target, damage, knockback, crit);
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
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

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            OnHitByAnything(npc, damage, crit);
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            OnHitByAnything(proj, damage, crit);
        }

        public override void UpdateVanityAccessories()
        {
        }

        public override void PostUpdateEquips()
        {
            player.wingTimeMax = (int)(player.wingTimeMax * flightTimeMultiplier);

            if (Aerodynamic)
            {
                player.wingTimeMax *= 2;
            }

            if (TimeFreezeTime > 0)
            {
                allUseSpeed *= 0.5f;
            }

            if (Glooped)
            {
                player.wingTimeMax /= 4;
            }

            if (VoidBlinkTime > 0)
            {
                allUseSpeed *= 1.3f;
                player.accRunSpeed *= 2;
            }

            for (int i = 0; i < player.armor.Length; i++)
            {
                if (i >= player.armor.Length)
                {
                    break;
                }
                else if (player.armor[i] != null && !player.armor[i].IsAir)
                {
                    Item item = player.armor[i];
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

            Vector2 ScreenOffset = new Vector2(Main.rand.NextFloat(-TerrorbornMod.screenShaking, TerrorbornMod.screenShaking), Main.rand.NextFloat(-TerrorbornMod.screenShaking, TerrorbornMod.screenShaking));
            Main.screenPosition += ScreenOffset;
        }

        public void ConsumeTerrorPotion()
        {
            Item currentItem = null;
            float currentTerror = 0f;
            foreach (Item item in player.inventory)
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
                    NPC npc = Main.npc[i];
                    if (!npc.friendly && npc.Distance(player.Center) <= range + (npc.height + npc.width) / 4 && npc.active && npc.type != NPCID.TargetDummy)
                    {
                        GainTerror(modItem.terrorPotionTerror, false, false, true);
                    }
                }

                currentItem.stack--;

                Main.PlaySound(SoundID.Item3, player.Center);
                player.velocity = Vector2.Zero;
                DustCircle(player.Center, 180, range, 63, -5, 3f);

                player.AddBuff(ModContent.BuffType<Buffs.Debuffs.TerrorSickness>(), TerrorPotionCooldown);
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
            bool darkblood = player.HasBuff(ModContent.BuffType<Buffs.Darkblood>());

            if (TerrorbornMod.ShriekOfHorror.JustPressed)
            {
                timeSincePressed = 0;
            }

            if (TerrorbornMod.ShriekOfHorror.Current)
            {
                timeSincePressed++;
            }

            if (TerrorbornMod.ShriekOfHorror.JustReleased && timeSincePressed <= 10 && !player.HasBuff(ModContent.BuffType<Buffs.Debuffs.TerrorSickness>()) && TerrorbornWorld.obtainedShriekOfHorror)
            {
                ConsumeTerrorPotion();
            }

            if (TerrorbornMod.ShriekOfHorror.Current && (player.velocity.Y == 0 || ShriekOfHorrorMovement > 0) && (player.itemTime <= 0 || darkblood) && TimeFreezeTime <= 0 && VoidBlinkTime <= 0 && BlinkDashTime <= 0)
            {
                MidShriek = true;
            }
            if (!TerrorbornMod.ShriekOfHorror.Current || player.dead)
            {
                MidShriek = false;
            }


            if (MidShriek)
            {
                if (!(ShriekOfHorrorMovement == 0))
                {
                    if ((int)player.velocity.Y != 0)
                    {
                        player.position -= player.velocity * (1 - ShriekOfHorrorMovement);
                    }
                    else
                    {
                        player.position.X -= player.velocity.X * (1 - ShriekOfHorrorMovement);
                    }
                }

                if (ShriekCounter > 0)
                {
                    ShriekTime = 0;
                    ShriekCounter--;
                    if (!darkblood)
                    {
                        player.bodyFrame.Y = 6 * player.bodyFrame.Height;
                    }
                }
                else
                {
                    ShriekTime++;
                    Shriek();
                    if (!darkblood)
                    {
                        player.bodyFrame.Y = 5 * player.bodyFrame.Height;
                    }
                    SoundCounter--;
                    if (SoundCounter <= 0)
                    {
                        SoundCounter = 22;
                        Main.PlaySound(SoundID.Item103, player.Center);
                    }
                    Vector2 dustpos = player.Center;
                    dustpos.Y -= 13;
                    dustpos.X -= 3;

                    dustpos.X += player.direction * 1;

                    int dust = Dust.NewDust(dustpos, 0, 0, 54);
                    Main.dust[dust].velocity = player.velocity / 3;
                    TerrorbornMod.ScreenShake(5);
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
                DustExplosion(player.Center, 0, 360, 45 * ShriekRangeMultiplier, 54, 1, true);
            }

            float range = 375 * ShriekRangeMultiplier;
            float NPCsInRange = 0;
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
                if (target.Distance(player.Center) < range && !target.friendly && target.active && target.lifeMax > 5)
                {
                    NPCsInRange++;
                    //target.velocity -= target.DirectionTo(player.Center) * ((range - target.Distance(player.Center)) / 400) * target.knockBackResist * ShriekKnockback;
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
                player.velocity = Vector2.Zero;
                player.velocity.Y -= 1;
                abilityAnimationCounter2--;
                if (abilityAnimationCounter2 <= 0)
                {
                    abilityAnimationCounter1--;
                    if (abilityAnimationCounter1 <= 0)
                    {
                        DustExplosion(player.Center, 0, 360, 40, 66, 1, true);
                        Main.PlaySound(SoundID.Item103, player.Center);
                        TerrorbornMod.ScreenShake(30);
                    }
                    else
                    {
                        Main.PlaySound(SoundID.MaxMana, player.Center);
                        TerrorbornMod.ScreenShake(5);
                        DustExplosion(player.Center, 0, 360, 10, 66, 0.5f, true);
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
                    Main.PlaySound(SoundID.MenuClose);
                    ShowTerrorAbilityMenu = false;
                }
                else
                {
                    Main.PlaySound(SoundID.MenuOpen);
                    ShowTerrorAbilityMenu = true;
                }
            }
        }

        public void SuperBlinkDash()
        {
            BlinkDashTime = 0;
            DustExplosion(player.Center, 0, 25, 45, 162, 2f, true);
            Main.PlaySound(SoundID.Item72, player.Center);
            player.velocity = BlinkDashVelocity * 1.5f;

            blinkDashSpinRotation = 0f;
            blinkSpin = true;
            if (BlinkDashVelocity.X != 0)
            {
                blinkDashDirection = Math.Sign(BlinkDashVelocity.X);
            }
            else
            {
                blinkDashDirection = player.direction;
            }
            Main.PlaySound(SoundID.Item62, player.Center);
            TerrorbornMod.ScreenShake(10f);
        }

        float parryEffectProgress = 0;
        float blinkDashSpinRotation = 0f;
        bool blinkSpin = false;
        int blinkDashDirection = 0;
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
            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && npc != null)
                {
                    if (npc.Distance(player.Center) <= 550 && !npc.friendly && npc.lifeMax > 15)
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
                    NPC npc = deimosChained[i];
                    if (!npc.active)
                    {
                        deimosChained.Remove(npc);
                    }
                }
            }

            if (blinkSpin)
            {
                blinkDashSpinRotation += MathHelper.ToRadians(15f) * blinkDashDirection;
                player.fullRotation = blinkDashSpinRotation;
                player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);
                if (Math.Abs(blinkDashSpinRotation) >= MathHelper.ToRadians(360))
                {
                    blinkSpin = false;
                    player.fullRotation = 0f;
                }
            }

            if (HeadHunterCritCooldown > 0)
            {
                HeadHunterCritCooldown--;
            }

            Main.manaTexture = ModContent.GetTexture("Terraria/Mana");
            if (MidnightFruit == 20)
            {
                Main.manaTexture = ModContent.GetTexture("TerrorbornMod/MidnightMana");
            }

            if (ParryTime > 0)
            {
                player.bodyFrame.Y = 6 * player.bodyFrame.Height;
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
                    Main.PlaySound(SoundID.MaxMana, (int)(player.Center.X), (int)(player.Center.Y), 0);
                    CombatText.NewText(player.getRect(), parryColor, "Parry Recharged", false, true);
                }
            }

            if (TerrorbornWorld.incendiaryRitual)
            {
                player.AddBuff(ModContent.BuffType<Buffs.Debuffs.IncendiaryCurse>(), 2);
            }

            if (GelatinArmorTime > 0)
            {
                GelatinArmorTime--;
                int dust = Dust.NewDust(player.position, player.width, player.height, DustID.t_Slime);
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
                    DustExplosion(player.Center, 0, 25, 15, 27, 1.5f, true);
                    Main.PlaySound(SoundID.Item72, player.Center);
                }
                if (VoidBlinkTime == 60)
                {
                    DustExplosion(player.Center, 0, 25, 7.5f, 27, 1.5f, true);
                    Main.PlaySound(SoundID.MaxMana, player.Center);
                }
                player.immuneAlpha = 255 / 2;
            }

            if (BlinkDashCooldown > 0)
            {
                BlinkDashCooldown--;
            }

            if (BlinkDashTime > 0)
            {
                player.noFallDmg = true;
                BlinkDashCooldown = 30;
                player.velocity = BlinkDashVelocity;
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && npc.Distance(player.Center) <= 100 && !npc.friendly)
                    {
                        npc.AddBuff(BuffID.Confused, 60 * 3);
                    }
                }
                BlinkDashTime--;
                if (BlinkDashTime == 0)
                {
                    DustExplosion(player.Center, 0, 15, 30, 162, 1.5f, true);
                    Main.PlaySound(SoundID.Item72, player.Center);
                    player.velocity /= 2;
                }
                if (BlinkDashTime <= 5 && (TerrorbornMod.PrimaryTerrorAbility.JustPressed || TerrorbornMod.SecondaryTerrorAbility.JustPressed))
                {
                    SuperBlinkDash();
                }
                player.immuneAlpha = 255;
            }
            if (TimeFreezeTime > 0)
            {
                TimeFreezeTime--;
            }

            if (TerrorbornMod.OpenTerrorAbilityMenu.JustPressed)
            {
                if (ShowTerrorAbilityMenu)
                {
                    Main.PlaySound(SoundID.MenuClose);
                    ShowTerrorAbilityMenu = false;
                }
                else
                {
                    Main.PlaySound(SoundID.MenuOpen);
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

            if (primaryAbility.HeldDown() && TerrorPercent >= primaryAbility.Cost() / 60 && primaryAbility.canUse(player) && TerrorbornMod.PrimaryTerrorAbility.Current)
            {
                primaryAbility.OnUse(player);
                LoseTerror(primaryAbility.Cost(), true, true, false);

            }

            if (!primaryAbility.HeldDown() && TerrorPercent >= primaryAbility.Cost() && primaryAbility.canUse(player) && TerrorbornMod.PrimaryTerrorAbility.JustPressed)
            {
                primaryAbility.OnUse(player);
                LoseTerror(primaryAbility.Cost(), true, false, false);
            }

            if (secondaryAbility.HeldDown() && TerrorPercent >= secondaryAbility.Cost() * 1.5f / 60 && secondaryAbility.canUse(player) && TerrorbornMod.SecondaryTerrorAbility.Current)
            {
                secondaryAbility.OnUse(player);
                LoseTerror(secondaryAbility.Cost() * 1.5f, true, true, false);

            }

            if (!secondaryAbility.HeldDown() && TerrorPercent >= secondaryAbility.Cost() * 1.5f && secondaryAbility.canUse(player) && TerrorbornMod.SecondaryTerrorAbility.JustPressed)
            {
                secondaryAbility.OnUse(player);
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

            if (TerrorbornWorld.obtainedShriekOfHorror)
            {
                UpdateShriekOfHorror();
            }
            if (TerrorPercent > 100)
            {
                TerrorPercent = 100;
            }
            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.Dunestock>())) player.buffImmune[BuffID.WindPushed] = true;

            if (TerrorbornMod.quickVirus.JustPressed && player.HasItem(ModContent.ItemType<Items.AstralSpark>()))
            {
                astralSparkData.Transform(player);
            }

            if (TerrorbornMod.quickVirus.JustPressed && player.HasItem(ModContent.ItemType<Items.graniteVirusSpark>()))
            {
                graniteSparkData.Transform(player);
            }

            if (graniteSpark || astralSpark)
            {
                player.wings = 0;
            }

            if (iFrames > 0)
            {
                iFrames--;
                player.immuneAlpha = 125;
            }

            if (TenebrisDashTime > 0)
            {
                TenebrisDashTime--;
                int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 74, 0f, 0f, 100, Scale: 1.5f);
                player.velocity = TenebrisDashVelocity;
            }

            if (!NPC.AnyNPCs(ModContent.NPCType<NPCs.TownNPCs.SkeletonSheriff>()) && CombatPoints > 0)
            {
                CombatPoints = 0;
            }
        }

        public override void OnRespawn(Player player)
        {
            usingPrimary = false;
        }

        public void ActivateParryEffect()
        {
            if (Main.netMode != NetmodeID.Server && !Filters.Scene["ParryShockwave"].IsActive())
            {
                parryEffectProgress = 0;
                Filters.Scene.Activate("ParryShockwave", player.Center).GetShader().UseColor(1, 1, 10).UseTargetPosition(player.Center); //Ripple Count, Ripple Size, Ripple Speed
                CombatText.NewText(player.getRect(), parryColor, "Successful Parry!", true, false);
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if (ParryTime > 0)
            {
                ParryTime = 0;
                damage /= 4;
                JustParried = true;
                Main.PlaySound(SoundID.Item37, player.Center);
                ActivateParryEffect();
            }

            if (GelatinPunishmentDamage > 0)
            {
                damage += GelatinPunishmentDamage;
                GelatinPunishmentDamage = 0;
                Main.PlaySound(SoundID.NPCDeath1, player.Center);
                for (int i = 0; i < 15; i++)
                {
                    int dust = Dust.NewDust(player.position, player.width, player.height, DustID.t_Slime);
                    Main.dust[dust].color = Color.LightBlue;
                    Main.dust[dust].velocity /= 2;
                    Main.dust[dust].alpha = 255 / 2;
                }
            }

            if (GelatinArmorTime > 0)
            {
                GelatinArmorTime = 0;
                GelatinPunishmentDamage = damage / 3;
                Main.PlaySound(SoundID.NPCHit1, player.Center);
                damage = 0;
            }

            if (MidShriek)
            {
                damage = (int)(damage * ShriekOfHorrorExtraDamageMultiplier);
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            if (ParryTime > 0)
            {
                ParryTime = 0;
                damage /= 4;
                JustParried = true;
                Main.PlaySound(SoundID.Item37, player.Center);
                ActivateParryEffect();
            }

            if (GelatinPunishmentDamage > 0)
            {
                damage += GelatinPunishmentDamage;
                GelatinPunishmentDamage = 0;
                Main.PlaySound(SoundID.NPCDeath1, player.Center);
                for (int i = 0; i < 15; i++)
                {
                    int dust = Dust.NewDust(player.position, player.width, player.height, DustID.t_Slime);
                    Main.dust[dust].color = Color.LightBlue;
                    Main.dust[dust].velocity /= 2;
                    Main.dust[dust].alpha = 255 / 2;
                }
            }

            if (GelatinArmorTime > 0)
            {
                GelatinArmorTime = 0;
                GelatinPunishmentDamage = damage / 3;
                Main.PlaySound(SoundID.NPCHit1, player.Center);
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
            if (player.HasBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>()))
            {
                if (player.lifeRegen > 0) player.lifeRegen = 0;
                player.lifeRegen -= 10 + (player.statDefense / 100) * 18;
            }
            if (badLifeRegen > 0)
            {
                if (player.lifeRegen > 0) player.lifeRegen = 0;
                player.lifeRegen -= badLifeRegen;
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (MidShriek)
            {
                int choice = Main.rand.Next(3);
                if (choice == 0)
                {
                    damageSource = PlayerDeathReason.ByCustomReason(player.name + " was overloaded with fear.");
                }
                if (choice == 1)
                {
                    damageSource = PlayerDeathReason.ByCustomReason(player.name + " drained their own life.");
                }
                if (choice == 2)
                {
                    damageSource = PlayerDeathReason.ByCustomReason(player.name + " couldn't handle their power.");
                }
            }

            TerrorPercent = 0f;

            if (SpecterLocket && !player.HasBuff(ModContent.BuffType<Buffs.Debuffs.UnholyCooldown>()))
            {
                CombatText.NewText(player.getRect(), Color.OrangeRed, "Revived!", true);
                Main.PlaySound(SoundID.NPCDeath52, player.Center);
                player.statLife = 25;
                player.HealEffect(25);
                player.AddBuff(ModContent.BuffType<Buffs.IncendiaryRevival>(), 60 * 4);
                player.AddBuff(ModContent.BuffType<Buffs.Debuffs.UnholyCooldown>(), 3600 * 3);
                return false;
            }

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write(CombatPoints);
            packet.Write(EyeOfTheMenace);
            packet.Write(GoldenTooth);
            packet.Write(CoreOfFear);
            packet.Write(AnekronianApple);
            packet.Write(primaryAbilityInt);
            packet.Write(secondaryAbilityInt);
            packet.Send(toWho, fromWho);
        }

        public override void clientClone(ModPlayer clientClone)
        {
            TerrorbornPlayer clone = clientClone as TerrorbornPlayer;
        }

        public override TagCompound Save()
        {
            return new TagCompound {
                {"CombatPoints", CombatPoints},
                {"EyeOfTheMenace", EyeOfTheMenace},
                {"GoldenTooth", GoldenTooth},
                {"CoreOfFear", CoreOfFear},
                {"AnekronianApple", AnekronianApple},
                {"DemonicLense", DemonicLense},
                {"PrimaryAbility", primaryAbilityInt},
                {"SecondaryAbility", secondaryAbilityInt},
                {"unlockedAbilities", unlockedAbilities},
                {"TerrorPercent", TerrorPercent},
                {"MidnightFruit", MidnightFruit},
                {"DarkEnergyStored", DarkEnergyStored}
            };
        }

        public override void Load(TagCompound tag)
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

        public static TerrorbornPlayer modPlayer(Player player)
        {
            return player.GetModPlayer<TerrorbornPlayer>();
        }
    }
}
