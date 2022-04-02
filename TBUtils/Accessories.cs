using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, player.Center);
            }

            if (modPlayer.JustParried)
            {
                foreach (NPC NPC in Main.npc)
                {
                    if (NPC.Distance(player.Center) <= 200 && !NPC.friendly && NPC.knockBackResist > 0f)
                    {
                        NPC.velocity = NPC.DirectionFrom(player.Center) * NPC.knockBackResist * Item.knockBack * 2f;
                    }
                }
                modPlayer.iFrames = 60;
                TerrorbornSystem.ScreenShake(5f);
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
                        CombatText.NewText(player.getRect(), textColor, Item.Name + " charged...", true, false);
                        fullyCharged = true;
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item37, player.Center);
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
                    CombatText.NewText(player.getRect(), textColor, "..." + Item.Name + " activated!", true, false);
                    Terraria.Audio.SoundEngine.PlaySound(sound, player.Center);
                    TerrorbornSystem.ScreenShake(5);
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
