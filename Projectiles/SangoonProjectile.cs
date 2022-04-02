using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Projectiles
{
    class SangoonProjectile : ModProjectile // Created by Seraph
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sangoon");
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 30;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.timeLeft = 900;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            
        }

        public float AI_State
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float AI_Timer
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }


        public float TargetIndex
        {
            get => Projectile.localAI[1];
            set => Projectile.localAI[1] = value;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(TargetIndex);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            TargetIndex = reader.ReadSingle();
        }

        public static readonly int FindingTarget = 0;
        public static readonly int Attacking = 1;
        public static readonly int AttackCooldown = 2;

        public static readonly float MaxEngagementDistance = 1000;
        public static readonly float MaxAttackDistance = 150;
        public void GradualTurnMovement(Vector2 TargetPosition, float MaxSpeed, float TurnResist)
        {
            float speed = MaxSpeed;
            Vector2 move = TargetPosition -Projectile.Center;
            float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            float turnResistance = TurnResist; //the larger this is, the slower the NPC will turn
            move = (Projectile.velocity * turnResistance + move) / (turnResistance + 1f);
            magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            Projectile.velocity = move;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            NPC target;

            

            AI_Timer++;

            if (TargetIndex >= 0)
            {
                target = Main.npc[(int)TargetIndex];
            }
            else
            {
                target = null;
            }

            if (AI_State == FindingTarget)
            {
                float distance;
                NPC possibleTarget;
                for (int k = 0; k < Main.maxNPCs; k++)
                {
                    possibleTarget = Main.npc[k];
                    distance = (possibleTarget.Center - player.Center).Length();
                    if (distance < MaxEngagementDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && possibleTarget.chaseable && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal)
                    {
                        TargetIndex = k;
                        AI_State = Attacking;


                        //maxDistance = (target.Center - Projectile.Center).Length();
                    }

                }

                if(Vector2.Distance(Projectile.Center, player.Center)> 80f)
                {
                    GradualTurnMovement(player.Center, 5 + (Vector2.Distance(Projectile.Center, player.Center) / 75f), 30 / ((Vector2.Distance(Projectile.Center, player.Center) / 100f) + 1f));
                }
            }

            if (AI_State == Attacking)
            {
                if (target == null || !target.active || target.dontTakeDamage || !target.chaseable|| Vector2.Distance(target.Center, player.Center)>MaxEngagementDistance)
                {
                    AI_State = FindingTarget;
                }
                float distance = Vector2.Distance(target.Center, Projectile.Center);
                if (distance > MaxAttackDistance)
                {
                    GradualTurnMovement(target.Center, 15, 10);
                }
                else
                {
                    Vector2 dash01 = target.Center - Projectile.Center;
                    dash01.Normalize();
                    dash01 *= 24f;
                    Projectile.velocity = dash01;
                    AI_State = AttackCooldown;
                    AI_Timer = 0;
                }

            }

            if(AI_State == AttackCooldown)
            {
                if (target == null || !target.active || target.dontTakeDamage || !target.chaseable || Vector2.Distance(target.Center, player.Center) > MaxEngagementDistance)
                {
                    AI_State = FindingTarget;
                }
                if (AI_Timer >= 15)
                {
                    AI_State = Attacking;
                }
            }

            #region animation

            Projectile.frameCounter++;

            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame > 4)
                {
                    Projectile.frame = 0;
                }
            }

            #endregion


            Projectile.direction = (Projectile.velocity.X <= 0).ToDirectionInt();
            Projectile.spriteDirection = Projectile.direction;
        }

    }
}
