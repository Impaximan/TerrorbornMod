using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using ReLogic.Graphics;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.ModLoader.IO;
using TerrorbornMod.Abilities;
using Terraria.Graphics.Shaders;
using TerrorbornMod.ForegroundObjects;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Input;
using Extensions;

namespace TerrorbornMod.TBUtils
{
    class Accessories
    {
        public static string GetParryShieldString(int parryCooldown, float knockback)
        {
            string knockbackText = "Insane";

            if (knockback <= 11f)
            {
                knockbackText = "Extremely strong";
            }

            if (knockback <= 9f)
            {
                knockbackText = "Very strong";
            }

            if (knockback <= 7f)
            {
                knockbackText = "Strong";
            }

            if (knockback <= 6f)
            {
                knockbackText = "Average";
            }

            if (knockback <= 4f)
            {
                knockbackText = "Weak";
            }

            if (knockback <= 3f)
            {
                knockbackText = "Very weak";
            }

            if (knockback <= 1.5f)
            {
                knockbackText = "Extremely weak";
            }

            if (knockback <= 0f)
            {
                knockbackText = "No";
            }

            return knockbackText + " knockback" + 
                "\nPress the 'Parry' mod hotkey right before an attack hits you to do a parry" +
                "\nParrying attacks has a " + parryCooldown / 60 + " second cooldown" +
                "\nParried attacks will do 75% less damage and knock away enemies";
        }

        public static void UpdateParryShield(int parryCooldown, Item item, Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (TerrorbornMod.Parry.JustPressed && modPlayer.ParryCooldown <= 0)
            {
                modPlayer.ParryTime = 15;
                modPlayer.ParryCooldown = parryCooldown;
                player.AddBuff(ModContent.BuffType<Buffs.Debuffs.ParryCooldown>(), parryCooldown);
                modPlayer.parryLightTime = 20;
                Main.PlaySound(SoundID.Item8, player.Center);
            }

            if (modPlayer.JustParried)
            {
                foreach (NPC npc in Main.npc)
                {
                    if (npc.Distance(player.Center) <= 200 && !npc.friendly && npc.knockBackResist > 0f)
                    {
                        npc.velocity = npc.DirectionFrom(player.Center) * npc.knockBackResist * item.knockBack * 2f;
                    }
                }
                modPlayer.iFrames = 60;
                TerrorbornMod.ScreenShake(5f);
                modPlayer.JustParried = false;
            }
        }

        static bool fullyCharged = false;
        public static void UpdateBurstJump(int chargeUpTime, int effectTime, Item item, Player player, Vector2 velocityRight, Color textColor, Terraria.Audio.LegacySoundStyle sound)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (player.controlUp)
            {
                modPlayer.BurstJumpChargingTime++;
                if (modPlayer.BurstJumpChargingTime > chargeUpTime)
                {
                    if (!fullyCharged)
                    {
                        CombatText.NewText(player.getRect(), textColor, item.Name + " charged...", true, false);
                        fullyCharged = true;
                        Main.PlaySound(SoundID.Item37, player.Center);
                    }
                    player.armorEffectDrawOutlines = true;
                }
            }
            else
            {
                modPlayer.BurstJumpChargingTime = 0;
                if (fullyCharged)
                {
                    fullyCharged = false;
                    Vector2 velocity = velocityRight;
                    player.direction = 1;
                    if (Main.MouseWorld.X < player.Center.X)
                    {
                        velocity.X *= -1;
                        player.direction = -1;
                    }
                    player.velocity = velocity;
                    modPlayer.BurstJumpTime = effectTime;
                    modPlayer.JustBurstJumped = true;
                    CombatText.NewText(player.getRect(), textColor, "..." + item.Name + " activated!", true, false);
                    Main.PlaySound(sound, player.Center);
                    TerrorbornMod.ScreenShake(5);
                    player.fallStart = (int)player.position.Y;
                    player.jumpAgainSandstorm = true;
                    player.jumpAgainSail = true;
                    player.jumpAgainFart = true;
                    player.jumpAgainCloud = true;
                    player.jumpAgainBlizzard = true;
                }
            }

            if (modPlayer.BurstJumpTime > 0)
            {
                modPlayer.BurstJumpTime--;
            }
        }

        public static string GetBurstJumpString(int chargeUpTime)
        {
            return "Hold UP for " + chargeUpTime / 60f + " seconds to charge up a burst jump" +
                "\nOnce fully charged, release UP to launch yourself left or right" +
                "\nThe direction of the launch depends on the position of your cursor";
        }
    }
}
