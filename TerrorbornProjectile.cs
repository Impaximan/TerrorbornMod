using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;

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

        public override bool CanHitPlayer(Projectile Projectile, Player target)
        {
            TerrorbornPlayer player = TerrorbornPlayer.modPlayer(target);
            if (player.iFrames > 0 || player.VoidBlinkTime > 0 || (player.TimeFreezeTime > 0 && Projectile.hostile) || player.BlinkDashTime > 0)
            {
                return false;
            }
            return base.CanHitPlayer(Projectile, target);
        }

        public override bool? CanHitNPC(Projectile Projectile, NPC target)
        {
            TerrorbornPlayer player = TerrorbornPlayer.modPlayer(Main.player[Projectile.owner]);
            if (player.TimeFreezeTime > 0 && Projectile.hostile)
            {
                return false;
            }
            return base.CanHitNPC(Projectile, target);
        }

        public override void SetDefaults(Projectile Projectile)
        {
            if (Projectile.type == ProjectileID.JestersArrow || Projectile.type == ProjectileID.UnholyArrow)
            {
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = -1;
            }
            if (Projectile.type == ProjectileID.MeteorShot)
            {
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 15;
            }
        }

        public override bool PreAI(Projectile Projectile)
        {
            Player player = Main.LocalPlayer;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.TimeFreezeTime > 0 && Projectile.hostile)
            {
                Projectile.position -= Projectile.velocity;
                Projectile.timeLeft++;
                return false;
            }
            return base.PreAI(Projectile);
        }

        public static TerrorbornProjectile modProjectile(Projectile Projectile)
        {
            return Projectile.GetGlobalProjectile<TerrorbornProjectile>();
        }

        public override void ModifyHitNPC(Projectile Projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[Projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (player.HeldItem.ModItem != null && player.HeldItem.ModItem.Mod == Mod)
            {
                if (Projectile.type == ProjectileID.IchorDart) damage = (int)(damage * 0.3f);
                if (Projectile.type == ProjectileID.CrystalDart) damage = (int)(damage * 0.5f);
            }
        }

        public override void OnHitNPC(Projectile Projectile, NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (modPlayer.combatTime < 300)
            {
                modPlayer.combatTime = 300;
            }

            if (crit && modPlayer.SangoonBand && target.type != NPCID.TargetDummy)
            {
                if (modPlayer.SangoonBandCooldown <= 0)
                {
                    player.HealEffect(1);
                    player.statLife += 1;
                    modPlayer.SangoonBandCooldown = 20;
                }
            }

            if (player.HeldItem != null && player != null)
            {
                if (modPlayer.PyroclasticShinobiBonus && crit && TerrorbornItem.modItem(player.HeldItem).countAsThrown && Projectile.friendly)
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
                DustExplosion(Projectile.Center, 0, 45, 30, 6, DustScale: 1f, NoGravity: true);
                SoundExtensions.PlaySoundOld(SoundID.Item14, Projectile.Center);
                TerrorbornSystem.ScreenShake(1.5f);
                for (int i = 0; i < 200; i++)
                {
                    NPC NPC = Main.npc[i];
                    if (!NPC.friendly && Projectile.Distance(NPC.Center) <= 200 + (NPC.width + NPC.height) / 2 && !NPC.dontTakeDamage)
                    {
                        if (NPC.type == NPCID.TheDestroyerBody)
                        {
                            NPC.StrikeNPC(damage / 10, 0, 0, Main.rand.Next(101) <= player.GetCritChance(DamageClass.Melee));
                        }
                        else
                        {
                            NPC.StrikeNPC(damage / 10, 0, 0, Main.rand.Next(101) <= player.GetCritChance(DamageClass.Melee));
                        }

                        int choice = Main.rand.Next(4);
                        if (choice == 0)
                        {
                            NPC.AddBuff(BuffID.OnFire, 60 * 5);
                        }
                        if (choice == 1)
                        {
                            NPC.AddBuff(BuffID.Frostburn, 60 * 5);
                        }
                        if (choice == 2)
                        {
                            NPC.AddBuff(BuffID.CursedInferno, 60 * 5);
                        }
                        if (choice == 3)
                        {
                            NPC.AddBuff(BuffID.ShadowFlame, 60 * 5);
                        }
                    }
                }
                superthrow = false;
            }

            if (VeinBurster)
            {
                SoundExtensions.PlaySoundOld(SoundID.NPCDeath21, target.Center);
                for (int i = 0; i < Main.rand.Next(3, 5); i++)
                {
                    Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                    float speed = Main.rand.Next(25, 35);
                    int proj = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, direction * speed, ModContent.ProjectileType<Projectiles.VeinBurst>(), damage, 0f, player.whoAmI);
                    Main.projectile[proj].DamageType = DamageClass.Ranged;
                }
            }

            if (ContaminatedMarine)
            {
                SoundExtensions.PlaySoundOld(SoundID.DD2_ExplosiveTrapExplode, target.Center);
                for (int i = 0; i < Main.rand.Next(7, 9); i++)
                {
                    Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                    float speed = Main.rand.Next(25, 35);
                    Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, direction * speed, ModContent.ProjectileType<Items.Weapons.Restless.NightmareBoilRanged>(), damage, 0f, player.whoAmI);
                }
            }

            if (Shadowflame)
            {
                target.AddBuff(BuffID.ShadowFlame, 60 * 3);
            }

            if (modPlayer.TacticalCommlink && Projectile.DamageType == DamageClass.Ranged && Main.rand.NextFloat() <= .1f)
            {
                Vector2 position = new Vector2(target.Center.X, target.position.Y - 750);
                position.X += Main.rand.Next(-150, 150);
                Vector2 direction = target.DirectionFrom(position);
                float speed = 30f;
                Projectile newProj = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_OnHit(target), position, direction * speed, ProjectileID.RocketI, damage, 2f, Projectile.owner)];
                newProj.localNPCHitCooldown = -1;
                newProj.usesLocalNPCImmunity = true;
                newProj.tileCollide = false;
                newProj.timeLeft = 60 * 3;
            }

            if (modPlayer.ShadowAmulet && Main.rand.NextFloat() <= .35f && Projectile.type != ModContent.ProjectileType<Items.Equipable.Accessories.ShadowSoul>())
            {
                Vector2 direction = player.DirectionTo(target.Center);
                float speed = 15f;
                Projectile newProj = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_OnHit(target), player.Center, direction * speed, ModContent.ProjectileType<Items.Equipable.Accessories.ShadowSoul>(), (int)(Projectile.damage * 0.65f), 2f, Projectile.owner)];
                newProj.DamageType = Projectile.DamageType;
            }

            if (BeatStopper && Projectile.type != ModContent.ProjectileType<Items.Weapons.Ranged.Guns.BeatstopperFireball>())
            {
                SoundExtensions.PlaySoundOld(SoundID.DD2_BallistaTowerShot, player.Center);
                for (int i = 0; i < 2; i++)
                {
                    float speed = Main.rand.NextFloat(12f, 20f);
                    Vector2 velocity = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2() * speed;
                    Projectile.NewProjectile(Projectile.GetSource_OnHit(target), player.Center, velocity, ModContent.ProjectileType<Items.Weapons.Ranged.Guns.BeatstopperFireball>(), damage / 5, 1, player.whoAmI);
                }
            }
        }

        bool superthrow = false;
        public void OnSpawn(Projectile Projectile)
        {

            
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
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
                if (modPlayer.SuperthrowNext && TerrorbornItem.modItem(player.HeldItem).countAsThrown && projectile.friendly)
                {
                    projectile.extraUpdates = (projectile.extraUpdates * 2) + 1;
                    superthrow = true;
                    modPlayer.SuperthrowNext = false;
                }
            }

            if (projectile.type == ProjectileID.Celeb2Rocket || projectile.type == ProjectileID.Celeb2RocketExplosive || projectile.type == ProjectileID.Celeb2RocketExplosiveLarge || projectile.type == ProjectileID.Celeb2RocketLarge)
            {
                projectile.damage = (int)(projectile.damage * 0.65f);
            }
        }

        int hellfireCooldown = 120;
        public override void AI(Projectile Projectile)
        {
            Player player = Main.player[Projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (Start)
            {
                Start = false;
                OnSpawn(Projectile);

                if (player.HeldItem.type == ModContent.ItemType<Items.Weapons.Ranged.Guns.Beatstopper>() && Projectile.Distance(player.Center) <= 100)
                {
                    BeatStopper = true;
                }
            }

            if (Projectile.friendly)
            {
                if (player.HasBuff(ModContent.BuffType<Items.Equipable.Armor.HuntersMark>()) && Projectile.DamageType == DamageClass.Ranged)
                {
                    NPC targetNPC = Main.npc[0];
                    float Distance = 375; //max distance away
                    bool Targeted = false;
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].Distance(Projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                        {
                            targetNPC = Main.npc[i];
                            Distance = Main.npc[i].Distance(Projectile.Center);
                            Targeted = true;
                        }
                    }
                    if (Targeted)
                    {
                        //HOME IN
                        float speed = .35f;
                        Vector2 move = targetNPC.Center - Projectile.Center;
                        float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                        move *= speed / magnitude;
                        Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(targetNPC.Center).ToRotation(), MathHelper.ToRadians(2.5f * (Projectile.velocity.Length() / 20))).ToRotationVector2() * Projectile.velocity.Length();
                    }
                }
            }

            if (superthrow)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, 235);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
                dust.scale = 2f;
            }

            if (Shadowflame)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, 27);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
                dust.scale = 2f;
            }
        }
    }
}
