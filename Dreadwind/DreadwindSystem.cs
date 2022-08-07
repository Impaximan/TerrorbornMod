using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TerrorbornMod.Dreadwind.Waves;
using Terraria.DataStructures;
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
        }

        public const int DreadwindLargeDamage = 200;
        public const int DreadwindMidDamage = 150;
        public const int DreadwindLowDamage = 100;

        public static void StartNextWave()
        {
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
            DreadwindActive = false;
            Main.NewText("The Dreadwind has subsided...", Color.LightGreen);
        }

        int endTimer = 0;
        int nextWaveTimer = 0;
        public static float HesperusTelegraphRotation = 0f;
        public override void PostUpdateEverything()
        {
            if (!DreadwindActive)
            {
                return;
            }
            Player player = Main.LocalPlayer;

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
