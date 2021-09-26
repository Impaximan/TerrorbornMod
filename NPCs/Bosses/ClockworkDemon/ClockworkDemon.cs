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
using Terraria.Utilities;


namespace TerrorbornMod.NPCs.Bosses.ClockworkDemon
{
    class ClockworkDemon : ModNPC
    {
        public override bool CheckActive()
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 10;
        }

        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 68;
            npc.height = 152;
            npc.damage = 60;
            npc.HitSound = SoundID.NPCHit41;
            npc.defense = 20;
            npc.DeathSound = SoundID.NPCDeath39;
            npc.boss = true;
            npc.frame.Height = 254;
            npc.lifeMax = 23000;
            npc.knockBackResist = 0;
            npc.aiStyle = -1;
            npc.alpha = 255;
            npc.dontTakeDamage = true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            Main.PlaySound(SoundID.NPCHit54, npc.Center);
        }

        public void MoveTowardsPosition(Vector2 position, float speed, float maxSpeed, float dragMultiplier = 0.98f)
        {
            npc.velocity += npc.DirectionTo(position) * speed;

            if (npc.velocity.Length() > maxSpeed)
            {
                npc.velocity = npc.velocity.ToRotation().ToRotationVector2() * maxSpeed;
            }

            npc.velocity *= dragMultiplier;
        }

        bool doingMelee = false;
        int baseDamage;
        bool start = true;
        int AIPhase = 0;
        int phase = 1;
        Player player;
        public void SetStats()
        {
            npc.TargetClosest(false);
            if (doingMelee)
            {
                npc.damage = baseDamage;
            }
            else
            {
                npc.damage = 0;
            }
            player = Main.player[npc.target];
            if (player.Center.X > npc.Center.X)
            {
                npc.spriteDirection = -1;
            }
            else
            {
                npc.spriteDirection = 1;
            }
        }

        public void OnStart()
        {
            baseDamage = npc.damage;
            transitioning = true;
            attackCounter = 10;
            attackWait = 10;
            attackDirection = MathHelper.ToRadians(-90);
        }

        bool phaseStart;
        List<int> NextAttacks = new List<int>();
        int LastAttack = -1;
        float secondPhaseHealth = 0.5f;
        bool transitioning = true;
        int attackWait;
        int attackWait2;
        int attackCounter;
        float attackDirection = 0f;

        public void DecideNextAttack()
        {
            if (NextAttacks.Count <= 0)
            {
                WeightedRandom<int> listOfAttacks = new WeightedRandom<int>();
                listOfAttacks.Add(0);
                listOfAttacks.Add(1);
                listOfAttacks.Add(2);
                listOfAttacks.Add(3);
                if (phase >= 2)
                {
                    listOfAttacks.Add(4);
                }
                listOfAttacks.Add(5);
                for (int i = 0; i < listOfAttacks.elements.Count; i++)
                {
                    int choice = listOfAttacks.Get();
                    while (NextAttacks.Contains(choice) || (choice == LastAttack && NextAttacks.Count == 0))
                    {
                        choice = listOfAttacks.Get();
                    }
                    NextAttacks.Add(choice);
                }
            }
            if (phase == 1 && npc.life <= secondPhaseHealth * npc.lifeMax)
            {

            }
            else
            {
                AIPhase = NextAttacks[0];
                LastAttack = AIPhase;
                NextAttacks.RemoveAt(0);
            }
            phaseStart = true;
        }

        public override void AI()
        {
            if (start)
            {
                OnStart();
                start = false;
            }

            SetStats();

            if (transitioning)
            {
                if (attackCounter >= 0)
                {
                    attackWait--;
                    if (attackWait <= 0)
                    {
                        attackWait = 10;
                        attackCounter--;
                        float speed = 25f;
                        attackDirection += MathHelper.ToRadians(360 / 10);
                        Vector2 velocity = attackDirection.ToRotationVector2() * -speed;
                        float distance = 2000;
                        Vector2 position = player.Center + (attackDirection.ToRotationVector2() * distance);
                        Main.PlaySound(SoundID.Item33, position);
                        Projectile projectile = Main.projectile[Projectile.NewProjectile(position, velocity, ModContent.ProjectileType<Projectiles.HellbornLaser>(), 80 / 4, 0)];
                    }

                    if (attackCounter <= 0)
                    {
                        npc.position = player.Center + new Vector2(0, -500) - npc.Size / 2;
                        npc.dontTakeDamage = false;
                    }
                }
                else if (npc.alpha > 0)
                {
                    npc.alpha -= 5;
                    MoveTowardsPosition(player.Center, 1f, 10f);
                }
                else
                {
                    npc.velocity *= 0.93f;
                }
            }

        }
    }
}
