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

        bool Start = true;
        int crystalWait = 10;
        public override bool InstancePerEntity => true;

        public bool VeinBurster = false;
        public bool ContaminatedMarine = false;

        public bool Shadowflame = false;

        public bool fromSplit = false;

        public bool BeatStopper = false;

        public bool Intimidated = false;

        public override bool CanHitPlayer(Projectile projectile, Player target)
        {
            TerrorbornPlayer player = TerrorbornPlayer.modPlayer(target);
            if (player.iFrames > 0 || player.VoidBlinkTime > 0 || (player.TimeFreezeTime > 0 && projectile.hostile) || player.BlinkDashTime > 0)
            {
                return false;
            }
            return base.CanHitPlayer(projectile, target);
        }

        public override bool? CanHitNPC(Projectile projectile, NPC target)
        {
            TerrorbornPlayer player = TerrorbornPlayer.modPlayer(Main.player[projectile.owner]);
            if (player.TimeFreezeTime > 0 && projectile.hostile)
            {
                return false;
            }
            return base.CanHitNPC(projectile, target);
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

        public override bool PreAI(Projectile projectile)
        {
            Player player = Main.LocalPlayer;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.TimeFreezeTime > 0 && projectile.hostile)
            {
                projectile.position -= projectile.velocity;
                projectile.timeLeft++;
                return false;
            }
            return base.PreAI(projectile);
        }

        public static TerrorbornProjectile modProjectile(Projectile projectile)
        {
            return projectile.GetGlobalProjectile<TerrorbornProjectile>();
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
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

            if (superthrow)
            {
                superthrow = false;
            }

            if (player.HeldItem != null && player != null)
            {
                if (modPlayer.PyroclasticShinobiBonus && crit && TerrorbornItem.modItem(player.HeldItem).countAsThrown)
                {
                    modPlayer.SuperthrowNext = true;
                }
            }

            List<int> bannedTypes = new List<int>(){
                NPCID.TheDestroyerBody,
                ModContent.NPCType<NPCs.TerrorRain.FrightcrawlerBody>(),
            };

            if (superthrow && !bannedTypes.Contains(target.type))
            {
                DustExplosion(projectile.Center, 0, 45, 30, DustID.Fire, DustScale: 1f, NoGravity: true);
                Main.PlaySound(SoundID.Item14, projectile.Center);
                TerrorbornMod.ScreenShake(1.5f);
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    if (!npc.friendly && projectile.Distance(npc.Center) <= 200 + (npc.width + npc.height) / 2 && !npc.dontTakeDamage)
                    {
                        if (npc.type == NPCID.TheDestroyerBody)
                        {
                            npc.StrikeNPC(damage / 10, 0, 0, Main.rand.Next(101) <= player.meleeCrit);
                        }
                        else
                        {
                            npc.StrikeNPC(damage / 3, 0, 0, Main.rand.Next(101) <= player.meleeCrit);
                        }

                        int choice = Main.rand.Next(4);
                        if (choice == 0)
                        {
                            npc.AddBuff(BuffID.OnFire, 60 * 5);
                        }
                        if (choice == 1)
                        {
                            npc.AddBuff(BuffID.Frostburn, 60 * 5);
                        }
                        if (choice == 2)
                        {
                            npc.AddBuff(BuffID.CursedInferno, 60 * 5);
                        }
                        if (choice == 3)
                        {
                            npc.AddBuff(BuffID.ShadowFlame, 60 * 5);
                        }
                    }
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

            if (BeatStopper && projectile.type != ModContent.ProjectileType<Items.Weapons.Ranged.BeatstopperFireball>())
            {
                Main.PlaySound(SoundID.DD2_BallistaTowerShot, player.Center);
                for (int i = 0; i < 2; i++)
                {
                    float speed = Main.rand.NextFloat(12f, 20f);
                    Vector2 velocity = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2() * speed;
                    Projectile.NewProjectile(player.Center, velocity, ModContent.ProjectileType<Items.Weapons.Ranged.BeatstopperFireball>(), damage / 5, 1, player.whoAmI);
                }
            }
        }

        bool superthrow = false;
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

            if (player.HeldItem != null && player != null)
            {
                if (modPlayer.SuperthrowNext && TerrorbornItem.modItem(player.HeldItem).countAsThrown)
                {
                    projectile.extraUpdates = (projectile.extraUpdates * 2) + 1;
                    superthrow = true;
                    modPlayer.SuperthrowNext = false;
                }
            }
        }

        int hellfireCooldown = 120;
        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (Start)
            {
                Start = false;
                OnSpawn(projectile);

                if (player.HeldItem.type == ModContent.ItemType<Items.Weapons.Ranged.Beatstopper>() && projectile.Distance(player.Center) <= 100)
                {
                    BeatStopper = true;
                }
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

            if (superthrow)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, 235);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
                dust.scale = 2f;
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
