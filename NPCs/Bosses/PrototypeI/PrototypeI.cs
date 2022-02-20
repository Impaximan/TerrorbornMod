using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Liquid;
using Terraria.World.Generation;
using Terraria.Utilities;

namespace TerrorbornMod.NPCs.Bosses.PrototypeI
{
    [AutoloadBossHead]
    public class PrototypeI : ModNPC
    {
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void NPCLoot()
        {
            TerrorbornWorld.downedPrototypeI = true;
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Placeable.Furniture.PrototypeITrophy>());
            }
            if (Main.expertMode)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PI_TreasureBag"));
            }
            else
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.PlasmaliumBar>(), Main.rand.Next(18, 25));
                int choice = Main.rand.Next(3);
                if (choice == 0)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Items.PrototypeI.PlasmaScepter>());
                }
                if (choice == 1)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Items.PrototypeI.PlasmoditeShotgun>());
                }
                if (choice == 2)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Items.PrototypeI.PlasmaticVortex>());
                }
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Vanity.BossMasks.PrototypeIMask>());
                }
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return npc.Distance(target.Center) <= 240;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.type == ProjectileID.HallowStar)
            {
                damage /= 4;
            }
        }

        bool showPortal = false;
        Vector2 portalPos = Vector2.Zero;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prototype I");
            NPCID.Sets.MustAlwaysDraw[npc.type] = true;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 240;
            npc.height = 240;
            npc.boss = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/DarkMatter");
            npc.damage = 45;
            npc.defense = 48;
            npc.takenDamageMultiplier = 0.85f;
            npc.lifeMax = 60000;
            npc.HitSound = SoundID.NPCHit42;
            npc.DeathSound = SoundID.NPCDeath56;
            npc.value = 0f;
            npc.knockBackResist = 0.00f;
            npc.buffImmune[BuffID.Ichor] = true;
            npc.buffImmune[BuffID.CursedInferno] = true;
            npc.alpha = 255;

            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(npc);
            modNPC.BossTitle = "Prototype I";
            modNPC.BossSubtitle = "Experiment for the Infection";
            modNPC.BossTitleColor = new Color(29, 189, 49);
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 90000;
            npc.defense = 48;
        }

        int coreFrame = 0;
        int coreFrameCounter = 0;

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            TBUtils.Graphics.DrawGlow_1(spriteBatch, npc.Center - Main.screenPosition, 300, npc.GetAlpha(Color.LimeGreen) * 0.35f);
            return base.PreDraw(spriteBatch, drawColor);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 4);
        }

        List<int> NextAttacks = new List<int>();
        bool phaseStart;
        int AIPhase = 0;
        int LastAttack = 0;
        public void DecideNextAttack()
        {
            //if (NextAttacks.Count <= 0)
            //{
            //    WeightedRandom<int> listOfAttacks = new WeightedRandom<int>();
            //    listOfAttacks.Add(0);
            //    for (int i = 0; i < listOfAttacks.elements.Count; i++)
            //    {
            //        int choice = listOfAttacks.Get();
            //        while (NextAttacks.Contains(choice) || (choice == LastAttack && NextAttacks.Count == 0))
            //        {
            //            choice = listOfAttacks.Get();
            //        }
            //        NextAttacks.Add(choice);
            //    }
            //}
            //AIPhase = NextAttacks[0];
            AIPhase = 0;
            LastAttack = AIPhase;
            //NextAttacks.RemoveAt(0);
            phaseStart = true;
        }

        List<Tuple<Vector2, float, float>> rings = new List<Tuple<Vector2, float, float>>();
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Player target = Main.player[npc.target];
            Texture2D texture = mod.GetTexture("NPCs/Bosses/PrototypeI/PrototypeI_core");
            coreFrameCounter--;
            if (coreFrameCounter <= 0)
            {
                coreFrameCounter = 7;
                coreFrame++;
                if (coreFrame >= 4)
                {
                    coreFrame = 0;
                }
            }
            int frameHeight = texture.Height / 4;
            Color color = npc.GetAlpha(Color.White);
            //Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition - new Vector2(texture.Width / 2, frameHeight/ 2 - 4), drawColor);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition - new Vector2(texture.Width / 2, frameHeight / 2 - 4), new Rectangle(0, coreFrame * frameHeight, texture.Width, frameHeight), color);
            SpriteEffects effect = SpriteEffects.None;
            if (npc.spriteDirection == 1)
            {
                effect = SpriteEffects.FlipHorizontally;
            }
            texture = mod.GetTexture("NPCs/Bosses/PrototypeI/PrototypeI_glow");
            Vector2 position = npc.Center - Main.screenPosition;
            position.Y += 4;
            Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, npc.width, npc.height), new Rectangle(0, 0, npc.width, npc.height), color, npc.rotation, new Vector2(npc.width / 2, npc.height / 2), effect, 0);


            for (int i = 0; i < rings.Count; i++)
            {
                if (i >= rings.Count)
                {
                    break;
                }

                Texture2D ringTexture = ModContent.GetTexture("TerrorbornMod/Effects/Textures/Ring1");
                spriteBatch.Draw(ringTexture, rings[i].Item1 - Main.screenPosition, null, new Color(0, 255, 109) * 0.25f, 0f, ringTexture.Size() / 2, rings[i].Item2 / ringTexture.Width, SpriteEffects.None, 0f);

                rings[i] = new Tuple<Vector2, float, float>(rings[i].Item1, rings[i].Item2 + rings[i].Item3, rings[i].Item3);
                if (rings[i].Item2 >= player.Distance(rings[i].Item1) + Main.screenWidth * 3)
                {
                    rings.RemoveAt(i);
                }
            }
        }

        bool inPhaseTransition = true;
        int phaseTransitionTime = 0;
        int phase = 0;
        float hitboxDistance = 100;
        bool start = true;
        Player player;

        int attackCounter1 = 0;
        int attackCounter2 = 0;
        int attackCounter3 = 0;
        int attackCounter4 = 0;

        Vector2 currentTarget = Vector2.Zero;

        int attackDirection1 = 0;
        int attackDirection2 = 0;

        int phaseWait = 0;
        public override void AI()
        {
            if (start)
            {
                start = false;
                inPhaseTransition = true;
                phaseTransitionTime = 180;
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                npc.position = player.Center + new Vector2(0, -400) - npc.Size / 2;
                npc.dontTakeDamage = true;
            }

            if (player.wet)
            {
                player.velocity.Y -= 0.5f;
            }


            if (inPhaseTransition)
            {
                if (phase == 0)
                {
                    if (phaseTransitionTime > 0)
                    {
                        phaseTransitionTime--;
                        if (phaseTransitionTime % 30 == 29)
                        {
                            rings.Add(new Tuple<Vector2, float, float>(npc.Center, 0f, 250f));
                            TerrorbornMod.ScreenShake(5f);
                            ModContent.GetSound("TerrorbornMod/Sounds/Effects/PrototypeIBeat").Play(Main.soundVolume, 0f, Main.rand.NextFloat(-0.3f, 0.3f));
                        }
                    }
                    else
                    {
                        int ringCount = 10;
                        float maxRingDistance = npc.width / 2;
                        for (float i = 0f; i < 1f; i += 1f / ringCount)
                        {
                            rings.Add(new Tuple<Vector2, float, float>(npc.Center, MathHelper.Lerp(0f, maxRingDistance, i), 400f));
                        }
                        TerrorbornMod.p1Thunder();
                        npc.alpha = 0;
                        inPhaseTransition = false;
                        npc.dontTakeDamage = false;
                        DecideNextAttack();
                    }
                }
            }
            else
            {
                switch (AIPhase)
                {
                    case 0:
                        AlignedDashes(3f, 26f, 5, 45, 90, 90);
                        break;
                    default:
                        break;
                }
            }
        }

        public void AlignedDashes(float aligningSpeed, float dashSpeed, int timeBetweenProjectiles, int dashTime, int minimumTimeUntilDash, int timeUntilNext = 60)
        {
            if (phaseStart)
            {
                phaseStart = false;
                attackCounter1 = 0;
                attackCounter2 = 0;
                attackCounter3 = 0;
                attackDirection1 = Math.Sign(player.Center.X - npc.Center.X);
                currentTarget = new Vector2(-500 * attackDirection1, 0);
                attackDirection2 = 1;
                if (Main.rand.NextBool()) attackDirection2 = -1;
            }
            else if (attackCounter1 == 0)
            {
                Vector2 targetPos = player.Center + currentTarget;
                if (npc.Distance(targetPos) > 1250)
                {
                    npc.position = targetPos - npc.Size / 2 + new Vector2(0, 1);
                    TerrorbornMod.p1Thunder();
                    rings.Add(new Tuple<Vector2, float, float>(npc.Center, 0f, 250f));
                }
                npc.velocity += npc.DirectionTo(targetPos) * aligningSpeed;
                npc.velocity *= 0.9f;
                npc.spriteDirection = Math.Sign(player.Center.X - npc.Center.X);
                npc.rotation += MathHelper.ToRadians(10) * npc.spriteDirection;
                attackCounter2++;
                if (attackCounter2 > minimumTimeUntilDash && npc.Distance(targetPos) <= 240)
                {
                    npc.velocity = npc.DirectionTo(player.Center) * dashSpeed;
                    attackCounter1 = 1;
                    attackCounter2 = 0;
                    currentTarget = currentTarget.RotatedBy(MathHelper.ToRadians(180));
                    currentTarget = currentTarget.RotatedBy(MathHelper.ToRadians(45) * attackDirection2);
                }
            }
            else if (attackCounter1 == 1)
            {
                npc.rotation += MathHelper.ToRadians(25) * npc.spriteDirection;
                attackCounter2++;
                if (attackCounter2 % timeBetweenProjectiles == timeBetweenProjectiles - 1)
                {
                    int proj = Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaticVision>(), 75 / 4, 0f);
                    Main.projectile[proj].ai[0] = dashTime - attackCounter2;
                }
                if (attackCounter2 > dashTime)
                {
                    attackCounter1 = 0;
                    attackCounter2 = 0;
                    attackCounter3++;
                    if (attackCounter3 >= 3)
                    {
                        attackCounter1 = 2;
                    }
                }
            }
            else
            {
                npc.velocity *= 0.98f;
                attackCounter2++;
                if (attackCounter2 > timeUntilNext)
                {
                    DecideNextAttack();
                }
            }
        }

        public void FocusDeathray(int timeUntilDeathray, float offsetMultiplier, float angleLerpAmount, int timeBetweenProjectiles)
        {
            if (phaseStart)
            {
                phaseStart = false;
            }
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
    }

    class PlasmaticVision : ModProjectile
    {
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 3);
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 34;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 360;
        }

        float homingSpeed = 0;
        bool launched = false;

        public override void AI()
        {
            if (homingSpeed == 0)
            {
                homingSpeed = Main.rand.NextFloat(0.5f, 0.8f);
            }

            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 74, 0f, 0f, 100, Scale: 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;

            if (projectile.ai[0] <= 0)
            {
                if (!launched)
                {
                    Player target = Main.player[Player.FindClosest(projectile.position, projectile.width, projectile.height)];
                    Vector2 direction = projectile.DirectionTo(target.Center);
                    Vector2 speed = direction * 30;
                    projectile.velocity = speed;
                    launched = true;
                }
            }
            else
            {
                Player target = Main.player[Player.FindClosest(projectile.position, projectile.width, projectile.height)];
                Vector2 direction = projectile.DirectionTo(target.Center);
                projectile.rotation = direction.ToRotation() + MathHelper.ToRadians(90);
                if (target.Center.X <= projectile.Center.X)
                {
                    projectile.spriteDirection = -1;
                }
                //Vector2 speed = direction * homingSpeed;
                //projectile.velocity += speed;
                //projectile.velocity *= 0.98f;
                projectile.ai[0]--;
            }
        }
    }
}