using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace TerrorbornMod
{
    class TerrorbornProjectile : GlobalProjectile
    {
        bool Start = true;
        int crystalWait = 10;
        public override bool InstancePerEntity => true;

        public bool VeinBurster = false;
        public bool ContaminatedMarine = false;

        public bool Shadowflame = false;

        public bool fromSplit = false;

        public override bool CanHitPlayer(Projectile projectile, Player target)
        {
            TerrorbornPlayer player = TerrorbornPlayer.modPlayer(target);
            if (player.iFrames > 0 || player.VoidBlinkTime > 0)
            {
                return false;
            }
            return base.CanHitPlayer(projectile, target);
        }

        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type == ProjectileID.JestersArrow || projectile.type == ProjectileID.UnholyArrow)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = -1;
            }
            if (projectile.type == ProjectileID.MeteorShot)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 15;
            }
        }

        public static TerrorbornProjectile modProjectile(Projectile projectile)
        {
            return projectile.GetGlobalProjectile<TerrorbornProjectile>();
        }

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (crit && modPlayer.SangoonBand && target.type != NPCID.TargetDummy)
            {
                if (modPlayer.SangoonBandCooldown <= 0)
                {
                    player.HealEffect(1);
                    player.statLife += 1;
                    modPlayer.SangoonBandCooldown = 20;
                }
            }

            if (VeinBurster)
            {
                Main.PlaySound(SoundID.NPCDeath21, target.Center);
                for (int i = 0; i < Main.rand.Next(3, 5); i++)
                {
                    Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                    float speed = Main.rand.Next(25, 35);
                    int proj = Projectile.NewProjectile(target.Center, direction * speed, ModContent.ProjectileType<Projectiles.VeinBurst>(), damage, 0f, player.whoAmI);
                    Main.projectile[proj].ranged = true;
                }
            }

            if (ContaminatedMarine)
            {
                Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode, target.Center);
                for (int i = 0; i < Main.rand.Next(7, 9); i++)
                {
                    Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                    float speed = Main.rand.Next(25, 35);
                    Projectile.NewProjectile(target.Center, direction * speed, ModContent.ProjectileType<Items.Weapons.Restless.NightmareBoilRanged>(), damage, 0f, player.whoAmI);
                }
            }

            if (Shadowflame)
            {
                target.AddBuff(BuffID.ShadowFlame, 60 * 3);
            }

            if (modPlayer.TacticalCommlink && projectile.ranged && Main.rand.NextFloat() <= .20f)
            {
                Vector2 position = new Vector2(target.Center.X, target.position.Y - 750);
                position.X += Main.rand.Next(-150, 150);
                Vector2 direction = target.DirectionFrom(position);
                float speed = 30f;
                Projectile newProj = Main.projectile[Projectile.NewProjectile(position, direction * speed, ProjectileID.RocketI, damage, 2f, projectile.owner)];
                newProj.localNPCHitCooldown = -1;
                newProj.usesLocalNPCImmunity = true;
                newProj.tileCollide = false;
                newProj.timeLeft = 60 * 3;
            }

            if (modPlayer.ShadowAmulet && Main.rand.NextFloat() <= .35f && projectile.type != ModContent.ProjectileType<Items.Equipable.Accessories.ShadowSoul>())
            {
                Vector2 direction = player.DirectionTo(target.Center);
                float speed = 15f;
                Projectile newProj = Main.projectile[Projectile.NewProjectile(player.Center, direction * speed, ModContent.ProjectileType<Items.Equipable.Accessories.ShadowSoul>(), (int)(projectile.damage * 0.65f), 2f, projectile.owner)];
                newProj.melee = projectile.melee;
                newProj.ranged = projectile.ranged;
                newProj.magic = projectile.magic;

            }
        }

        public void OnSpawn(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            List<int> bannedProjectiles = new List<int>
            {
                { ProjectileID.LastPrism },
                { ProjectileID.LastPrismLaser},
                { ProjectileID.ChargedBlasterCannon},
                { ProjectileID.ChargedBlasterLaser}
            };

            if (modPlayer.PrismalCore && projectile.magic && Main.rand.NextFloat() <= 0.2f && !fromSplit && !bannedProjectiles.Contains(projectile.type))
            {
                float rotation = MathHelper.ToRadians(20);

                Projectile otherProj = Main.projectile[Projectile.NewProjectile(projectile.Center, projectile.velocity.RotatedBy(rotation), projectile.type, projectile.damage, projectile.knockBack, projectile.owner)];
                modProjectile(otherProj).fromSplit = true;
                //otherProj.ai = projectile.ai;

                otherProj = Main.projectile[Projectile.NewProjectile(projectile.Center, projectile.velocity.RotatedBy(-rotation), projectile.type, projectile.damage, projectile.knockBack, projectile.owner)];
                modProjectile(otherProj).fromSplit = true;
                //otherProj.ai = projectile.ai;
            }
        }

        int hellfireCooldown = 120;
        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (modPlayer.IncendiusArmorBonus && projectile.minion)
            {
                hellfireCooldown--;
                if (hellfireCooldown <= 0 && player.HeldItem.summon)
                {
                    float speed = 15f;
                    hellfireCooldown = 120;
                    Vector2 velocity = projectile.DirectionTo(Main.MouseWorld) * speed;
                    Projectile.NewProjectile(projectile.Center, velocity, ModContent.ProjectileType<Items.Equipable.Armor.HellFire>(), (int)(18 * projectile.minionSlots), 0.01f, projectile.owner);
                }
            }

            if (Start)
            {
                Start = false;
                OnSpawn(projectile);
            }

            if (projectile.friendly)
            {
                if (player.HasBuff(mod.BuffType("HuntersMark")) && projectile.ranged)
                {
                    NPC targetNPC = Main.npc[0];
                    float Distance = 375; //max distance away
                    bool Targeted = false;
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                        {
                            targetNPC = Main.npc[i];
                            Distance = Main.npc[i].Distance(projectile.Center);
                            Targeted = true;
                        }
                    }
                    if (Targeted)
                    {
                        //HOME IN
                        float speed = .35f;
                        Vector2 move = targetNPC.Center - projectile.Center;
                        float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                        move *= speed / magnitude;
                        projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(targetNPC.Center).ToRotation(), MathHelper.ToRadians(2.5f * (projectile.velocity.Length() / 20))).ToRotationVector2() * projectile.velocity.Length();
                    }
                }
            }

            if (Shadowflame)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, 27);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
                dust.scale = 2f;
            }
        }
    }
}
