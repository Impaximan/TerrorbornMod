using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TerrorbornMod.Dreadwind.Waves;
using Terraria.DataStructures;
using TerrorbornMod.Dreadwind.NPCs;
using System;

namespace TerrorbornMod.Dreadwind
{
    class DreadwindSystem : ModSystem
    {
        public static bool DreadwindActive = false;
        public static Color DreadwindTargetColor;
        public static Color CurrentDreadwindColor;

        public override void OnWorldLoad()
        {
            DreadwindActive = false;
        }

        public static List<DreadwindWave> upcomingWaves = new();
        public static List<int> extraEnemies = new();
        public static DreadwindWave currentWave = null;
        public static int waveNumber = 0;
        public static void StartDreadwind()
        {
            FrightRaining = false;
            waveNumber = 0;
            upcomingWaves.Clear();
            extraEnemies.Clear();
            upcomingWaves.Add(new WaveOfLight());
            upcomingWaves.Add(new WaveOfNight());
            upcomingWaves.Add(new WaveOfFlight());
            upcomingWaves.Add(new WaveOfMight());
            upcomingWaves.Add(new WaveOfSight());
            upcomingWaves.Add(new WaveOfFright());
            upcomingWaves.Add(new WaveOfPlight());
            DreadwindActive = true;
            StartNextWave();
            CurrentDreadwindColor = Color.Black;
            reaper_HealthLeft = 1f;
            reaper_AIPhase = -1;
            reaper_SideMult = 1;
            reaper_ShouldDespawn = false;

        }

        public const int DreadwindLargeDamage = 200;
        public const int DreadwindMidDamage = 150;
        public const int DreadwindLowDamage = 100;

        public static void StartNextWave()
        {
            FrightRaining = false;
            if (upcomingWaves.Count == 0)
            {
                FinishDreadwind(true);
                return;
            }
            waveNumber++;
            currentWave = upcomingWaves[0];
            upcomingWaves.RemoveAt(0);
            Main.NewText("Wave " + waveNumber + ": " + currentWave, currentWave.WaveColor);
            DreadwindTargetColor = currentWave.WaveColor;
            currentWave.InitializeWave(Main.LocalPlayer);
        }


        public static void FinishDreadwind(bool victory)
        {
            FrightRaining = false;
            DreadwindActive = false;
            Main.NewText("The Dreadwind has subsided...", Color.LightGreen);
        }

        //Purgatory Reaper AI stuff
        public static int reaper_AIPhase;
        public static int reaper_NextAIPhase;
        public static float reaper_HealthLeft = 1f;
        public static int reaper_SideMult = 1;
        public static bool reaper_ShouldDespawn = false;

        public static List<int> reaper_NextAttacksList = new List<int>();
        public static void Reaper_DecideNextAttack(int currentAIPhase)
        {
            if (reaper_NextAttacksList.Count == 0)
            {
                int count = 3;
                while (reaper_NextAttacksList.Count < count)
                {
                    int attack = Main.rand.Next(count);
                    while (reaper_NextAttacksList.Contains(attack) || (reaper_NextAttacksList.Count == 0 && attack == currentAIPhase))
                    {
                        attack = Main.rand.Next(count);
                    }
                    reaper_NextAttacksList.Add(attack);
                }
                //while (reaper_AIPhase == currentAIPhase)
                //{
                //    reaper_AIPhase = Main.rand.Next(count);
                //}
            }
            reaper_NextAIPhase = reaper_NextAttacksList[0];
            reaper_NextAttacksList.RemoveAt(0);
        }

        int endTimer = 0;
        int nextWaveTimer = 0;
        public static float HesperusTelegraphRotation = 0f;
        public static float FrightArenaWidth = 0f;
        public static float FrightArenaX = 0f;
        public static bool FrightRaining = false;
        public const float FrightArenaMaxWidth = 4500f;
        int rainCounter = 0;
        public override void PostUpdateEverything()
        {
            if (!DreadwindActive)
            {
                return;
            }

            Player player = Main.LocalPlayer;

            if (NPC.AnyNPCs(ModContent.NPCType<PurgatoryReaper>()))
            {
                reaper_AIPhase = reaper_NextAIPhase;

                int count = 0;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i] != null)
                    {
                        NPC nPC = Main.npc[i];
                        if (nPC.type == ModContent.NPCType<PurgatoryReaper>() && nPC.active)
                        {
                            count++;
                            if (((float)nPC.life / nPC.lifeMax) < reaper_HealthLeft)
                            {
                                reaper_HealthLeft = (float)nPC.life / nPC.lifeMax;
                            }
                        }
                    }
                }
                if (count == 1)
                {
                    reaper_ShouldDespawn = true;
                }
            }

            HesperusTelegraphRotation += MathHelper.ToRadians(1f);
            HesperusTelegraphRotation = MathHelper.WrapAngle(HesperusTelegraphRotation);

            CurrentDreadwindColor = Color.Lerp(CurrentDreadwindColor, DreadwindTargetColor, 0.1f);

            if (!player.ZoneUnderworldHeight || player.dead)
            {
                endTimer++;
                if (endTimer > 60 * 2)
                {
                    FinishDreadwind(false);
                }
            }
            else
            {
                endTimer = 0;
            }

            if (FrightRaining)
            {
                if (FrightArenaWidth > 0f)
                {
                    float secondsToLive = 120f;
                    if (Main.masterMode)
                    {
                        secondsToLive *= 1.5f;
                    }
                    if (TerrorbornSystem.TwilightMode)
                    {
                        secondsToLive *= 1.3f;
                    }
                    FrightArenaWidth -= FrightArenaMaxWidth / (60f * secondsToLive);
                }

                float extraWidth = 0;
                if (Math.Abs(player.Center.X - FrightArenaX) > FrightArenaWidth / 2f) extraWidth += Math.Abs(player.Center.X - FrightArenaX) - FrightArenaWidth / 2f;

                int side = 1;
                if (Main.rand.NextBool()) side = -1;
                Vector2 position = new Vector2((FrightArenaWidth / 2f + extraWidth) * side + FrightArenaX + Main.rand.NextFloat(Main.screenWidth * 1.5f) * side, Main.screenPosition.Y - 120);
                Vector2 velocity = new Vector2(-5, 35);
                Projectile.NewProjectile(new EntitySource_WorldEvent("DreadwindRain"), position, velocity, ModContent.ProjectileType<Projectiles.DreadRain>(), DreadwindLargeDamage / 4, 0f);

                rainCounter++;
                if (rainCounter > 5)
                {
                    rainCounter = 0;
                    side = 1;
                    if (Main.rand.NextBool()) side = -1;
                    position = new Vector2((FrightArenaWidth / 2f + extraWidth) * side + FrightArenaX, Main.screenPosition.Y - 120);
                    velocity = new Vector2(-5, 35);
                    Projectile.NewProjectile(new EntitySource_WorldEvent("DreadwindRain"), position, velocity, ModContent.ProjectileType<Projectiles.DreadRain>(), DreadwindLargeDamage / 4, 0f);
                }

            }

            for (int i = 0; i < currentWave.requiredKills.Count; i++)
            {
                if (i >= currentWave.requiredKills.Count)
                {
                    break;
                }
                if (!NPC.AnyNPCs(currentWave.requiredKills[i]))
                {
                    currentWave.requiredKills.RemoveAt(i);
                }
            }

            if (currentWave.requiredKills.Count <= 0)
            {
                nextWaveTimer++;
                if (nextWaveTimer >= 60 && !player.dead)
                {
                    StartNextWave();
                    nextWaveTimer = 0;
                }
            }
        }
    }

    class DreadwindWave
    {
        public virtual Color WaveColor => Color.White;

        public virtual string WaveName => "Default Name";

        public override string ToString()
        {
            return WaveName;
        }

        public virtual void InitializeWave(Player player)
        {
            
        }

        public List<int> requiredKills = new List<int>();
        public int SpawnEnemy(int type, Vector2 position)
        {
            if (!requiredKills.Contains(type))
            {
                requiredKills.Add(type);
            }
            return NPC.NewNPC(new EntitySource_Misc("Dreadwind"), (int)position.X, (int)position.Y, type);
        }
    }
}
