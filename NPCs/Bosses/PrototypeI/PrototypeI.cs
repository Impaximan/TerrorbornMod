using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using TerrorbornMod.Projectiles;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Microsoft.Xna.Framework.Audio;

namespace TerrorbornMod.NPCs.Bosses.PrototypeI
{
    [AutoloadBossHead]
    public class PrototypeI : ModNPC
    {
        bool completedDeathAnimation = false;
        const int totalDeathAnimationTime = 60 * 4;
        int deathAnimationTime = 0;
        int deathSpotlightCounter = 0;
        public override void HitEffect(int hitDirection, double damage)
        {
            if (!completedDeathAnimation && NPC.life < 1)
            {
                NPC.life = 1;
                inPhaseTransition = true;
                if (NPC.velocity.Length() > 10f)
                {
                    NPC.velocity = NPC.velocity.ToRotation().ToRotationVector2() * 10f;
                }
                NPC.dontTakeDamage = true;
                NPC.alpha = 0;
                deathAnimationTime = 0;
                TerrorbornMod.SetScreenToPosition(totalDeathAnimationTime, 30, NPC.Center);
                attackRotationSpeed = 0f;
            }
        }

        public void DeathAnimation()
        {
            TerrorbornMod.screenFollowPosition = NPC.Center;
            NPC.velocity *= 0.98f;
            attackRotationSpeed += MathHelper.ToRadians(15f) / totalDeathAnimationTime;
            NPC.rotation += attackRotationSpeed * NPC.spriteDirection;
            TerrorbornPlayer.modPlayer(player).iFrames = 30;
            mirages.Clear();
            lines.Clear();
            deathAnimationTime++;
            deathSpotlightCounter--;
            if (deathSpotlightCounter <= 0)
            {
                deathSpotlightCounter = totalDeathAnimationTime / 6;
                if (deathAnimationTime > totalDeathAnimationTime / 2)
                {
                    deathSpotlightCounter = totalDeathAnimationTime / 20;
                }
                if (deathAnimationTime > totalDeathAnimationTime * 0.75f)
                {
                    deathSpotlightCounter = totalDeathAnimationTime / 40;
                }
                if (deathAnimationTime > totalDeathAnimationTime * 0.9f)
                {
                    deathSpotlightCounter = totalDeathAnimationTime / 80;
                    TerrorbornSystem.ScreenShake(15f);
                }
                deathSpotlights.Add(MathHelper.ToRadians(Main.rand.Next(360)));
                TerrorbornSystem.ScreenShake(10f);
                SoundExtensions.PlaySoundOld(SoundID.Item62, NPC.Center);
            }
            if (deathAnimationTime > totalDeathAnimationTime)
            {
                TerrorbornSystem.positionLightningP1 = 2f;
                ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/PrototypeIExplosion").Value.Play(Main.soundVolume, 0f, 0f);
                TerrorbornSystem.ScreenShake(50f);
                completedDeathAnimation = true;
                NPC.StrikeNPC(500000, 0f, 0, true);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("The atrocities committed during the creation of the infection are highly numerous, however, some of the most notable ones are the four prototypes. This one in particular was the first test of artificial sentience, and though the results were highly destructive, they showed an immense terror-technological advancement for the military of Neokronyx.")
            });
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Furniture.PrototypeITrophy>(), 10));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<Items.TreasureBags.PI_TreasureBag>()));

            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Equipable.Vanity.BossMasks.PrototypeIMask>(), 7));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.PlasmaliumBar>(), 1, 18, 25));
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1,
                ModContent.ItemType<Items.PrototypeI.PlasmaScepter>(),
                ModContent.ItemType<Items.PrototypeI.PlasmoditeShotgun>(),
                ModContent.ItemType<Items.PrototypeI.PlasmaticVortex>()));
        }

        public override void OnKill()
        {
            TerrorbornSystem.downedPrototypeI = true;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (inPhaseTransition || AIPhase == -1)
            {
                return false;
            }
            return NPC.Distance(target.Center) <= 120;
        }

        public override void ModifyHitByProjectile(Projectile Projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Projectile.type == ProjectileID.HallowStar)
            {
                damage /= 4;
            }
        }

        bool showPortal = false;
        Vector2 portalPos = Vector2.Zero;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prototype I");
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 240;
            NPC.height = 240;
            NPC.boss = true;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/DarkMatter");
            NPC.damage = 65;
            NPC.takenDamageMultiplier = 0.85f;
            NPC.lifeMax = 60000;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath56;
            NPC.value = 0f;
            NPC.knockBackResist = 0.00f;
            NPC.buffImmune[BuffID.Ichor] = true;
            NPC.buffImmune[BuffID.CursedInferno] = true;
            NPC.alpha = 255;

            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(NPC);
            modNPC.BossTitle = "Prototype I";
            modNPC.BossSubtitle = "Experiment for the Infection";
            modNPC.BossDefeatTitle = "Supreme Object";
            modNPC.BossTitleColor = new Color(29, 189, 49);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            currentSpeed = -10f;
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 4);
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            damage = (int)(damage * damageMult);
        }

        int coreFrame = 0;
        int coreFrameCounter = 0;

        List<float> deathSpotlights = new List<float>();
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Utils.Graphics.DrawGlow_1(Main.spriteBatch, NPC.Center - Main.screenPosition, 300, NPC.GetAlpha(Color.LimeGreen) * 0.35f);

            for (int i = 0; i < deathSpotlights.Count; i++)
            {
                if (i >= deathSpotlights.Count)
                {
                    break;
                }

                deathSpotlights[i] += MathHelper.ToRadians(1f) * NPC.spriteDirection;
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Effects/Textures/Spotlight1"), NPC.Center - Main.screenPosition + new Vector2(0, 4), null, new Color(151, 255, 178), deathSpotlights[i], new Vector2(100, 0), 1f, SpriteEffects.None, 0f);
            }

            for (int i = 0; i < mirages.Count; i++)
            {
                if (i >= mirages.Count)
                {
                    break;
                }

                SpriteEffects effect = SpriteEffects.None;
                if (NPC.spriteDirection == 1)
                {
                    effect = SpriteEffects.FlipHorizontally;
                }

                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bosses/PrototypeI/PrototypeIMirage");
                Main.spriteBatch.Draw(texture, mirages[i].Item1 - Main.screenPosition + new Vector2(0, 4), new Rectangle(0, 0, NPC.width, NPC.height), mirages[i].Item3, mirages[i].Item2, new Vector2(NPC.width / 2, NPC.height / 2), 1f, effect, 0);
            }

            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        List<Tuple<Vector2, float, float>> rings = new List<Tuple<Vector2, float, float>>();
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Player target = Main.player[NPC.target];
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bosses/PrototypeI/PrototypeI_core");
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
            Color color = NPC.GetAlpha(Color.White);
            //Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition - new Vector2(texture.Width / 2, frameHeight/ 2 - 4), drawColor);
            Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition - new Vector2(texture.Width / 2, frameHeight / 2 - 4), new Rectangle(0, coreFrame * frameHeight, texture.Width, frameHeight), color);
            SpriteEffects effect = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                effect = SpriteEffects.FlipHorizontally;
            }
            texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bosses/PrototypeI/PrototypeI_glow");
            Vector2 position = NPC.Center - Main.screenPosition;
            position.Y += 4;
            Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, NPC.width, NPC.height), new Rectangle(0, 0, NPC.width, NPC.height), color, NPC.rotation, new Vector2(NPC.width / 2, NPC.height / 2), effect, 0);

            for (int i = 0; i < rings.Count; i++)
            {
                if (i >= rings.Count)
                {
                    break;
                }

                Texture2D ringTexture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Effects/Textures/Ring1");
                spriteBatch.Draw(ringTexture, rings[i].Item1 - Main.screenPosition, null, new Color(0, 255, 109) * 0.25f, 0f, ringTexture.Size() / 2, rings[i].Item2 / ringTexture.Width, SpriteEffects.None, 0f);

                rings[i] = new Tuple<Vector2, float, float>(rings[i].Item1, rings[i].Item2 + rings[i].Item3, rings[i].Item3);
                if (rings[i].Item2 >= player.Distance(rings[i].Item1) + Main.screenWidth * 3)
                {
                    rings.RemoveAt(i);
                }
            }

            for (int i = 0; i < lines.Count; i++)
            {
                if (i >= lines.Count)
                {
                    break;
                }

                Terraria.Utils.DrawLine(spriteBatch, lines[i].Item1, lines[i].Item2, lines[i].Item3, lines[i].Item4, lines[i].Item5);
            }
        }

        List<int> NextAttacks = new List<int>();
        bool phaseStart;
        int AIPhase = 0;
        int LastAttack = 0;
        int timeSinceMurder = 0;
        public void DecideNextAttack()
        {
            if (timeSinceMurder > 60f * MathHelper.Lerp(5f, 20f, (float)NPC.life / (float)NPC.lifeMax * 2f) && NPC.life <= NPC.lifeMax / 2)
            {
                AIPhase = -1;
            }
            else
            {
                if (NextAttacks.Count <= 0)
                {
                    WeightedRandom<int> listOfAttacks = new WeightedRandom<int>();
                    listOfAttacks.Add(0);
                    listOfAttacks.Add(1);
                    listOfAttacks.Add(2);
                    listOfAttacks.Add(3);
                    listOfAttacks.Add(4);
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
                AIPhase = NextAttacks[0];
                LastAttack = AIPhase;
                NextAttacks.RemoveAt(0);
            }
            phaseStart = true;
        }

        bool inPhaseTransition = true;
        int phaseTransitionTime = 0;
        int phase = 0;
        bool start = true;
        Player player;

        int attackCounter1 = 0;
        int attackCounter2 = 0;
        int attackCounter3 = 0;
        int attackCounter4 = 0;

        float damageMult = 1f;

        float attackRotationSpeed = 0f;

        float attackRotation1 = 0f;

        Vector2 currentTarget = Vector2.Zero;

        int attackDirection1 = 0;
        int attackDirection2 = 0;

        List<Tuple<Vector2, Vector2, Color, Color, int>> lines = new List<Tuple<Vector2, Vector2, Color, Color, int>>();

        List<Tuple<Vector2, float, Color>> mirages = new List<Tuple<Vector2, float, Color>>();

        int phaseWait = 0;
        public override void AI()
        {
            if (start)
            {
                start = false;
                inPhaseTransition = true;
                phaseTransitionTime = 180;
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                NPC.position = player.Center + new Vector2(0, -400) - NPC.Size / 2;
                NPC.dontTakeDamage = true;
                completedDeathAnimation = false;
            }

            if (player.wet)
            {
                player.velocity.Y -= 0.5f;
            }

            timeSinceMurder++;

            if (inPhaseTransition)
            {
                if (phase == 0)
                {
                    if (phaseTransitionTime > 0)
                    {
                        phaseTransitionTime--;
                        if (phaseTransitionTime % 30 == 29)
                        {
                            rings.Add(new Tuple<Vector2, float, float>(NPC.Center, 0f, 250f));
                            TerrorbornSystem.ScreenShake(5f);
                            ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/PrototypeIBeat").Value.Play(Main.soundVolume, 0f, Main.rand.NextFloat(-0.3f, 0.3f));
                        }
                    }
                    else
                    {
                        int ringCount = 10;
                        float maxRingDistance = NPC.width / 2;
                        for (float i = 0f; i < 1f; i += 1f / ringCount)
                        {
                            rings.Add(new Tuple<Vector2, float, float>(NPC.Center, MathHelper.Lerp(0f, maxRingDistance, i), 400f));
                        }
                        TerrorbornSystem.p1Thunder();
                        NPC.alpha = 0;
                        inPhaseTransition = false;
                        NPC.dontTakeDamage = false;
                        DecideNextAttack();
                        phase++;
                    }
                }
                else
                {
                    DeathAnimation();
                }
            }
            else
            {
                damageMult = MathHelper.Lerp(1.4f, 1f, (float)NPC.life / (float)NPC.lifeMax);
                mirages.Clear();
                lines.Clear();

                if (player.dead || !player.active)
                {
                    NPC.velocity.Y -= 0.5f;
                    if (NPC.Distance(player.Center) > 3000 || AIPhase == -1)
                    {
                        NPC.active = false;
                    }
                    NPC.rotation += MathHelper.ToRadians(10f) * NPC.spriteDirection;
                    return;
                }

                switch (AIPhase)
                {
                    case -1:
                        timeSinceMurder = 0;
                        TheUltimateSuperHyperUltraMurderFastDashOfDoomDeathJusticeLightAndTragedy();
                        break;
                    case 0:
                        AlignedDashes(3f, 26f, 5, 45, (int)MathHelper.Lerp(60, 110, (float)NPC.life / (float)NPC.lifeMax), 90);
                        break;
                    case 1:
                        FocusDeathray((int)MathHelper.Lerp(150, 220, (float)NPC.life / (float)NPC.lifeMax), 150f, 0.075f, (int)MathHelper.Lerp(24f, 37f, (float)NPC.life / (float)NPC.lifeMax), 0.75f, 45f, 90);
                        break;
                    case 2:
                        SuperHyperUltraMurderFastDash(0.5f, 400f, (int)MathHelper.Lerp(70, 100, (float)NPC.life / (float)NPC.lifeMax), (int)MathHelper.Lerp(5, 3, (float)NPC.life / (float)NPC.lifeMax), 30, 2);
                        break;
                    case 3:
                        CloneDashes(15f, 80, 0.5f);
                        break;
                    case 4:
                        TheSpin(3, 2.5f, 30, 15f, 450f, 240);
                        break;
                    default:
                        break;
                }
            }
        }

        int timeBeforeUltidash = 90;
        const float maxAngleOffset = 40f;
        Vector2 ultidashVelocity = Vector2.Zero;

        List<Tuple<Vector2, Vector2, int>> telegraphs = new List<Tuple<Vector2, Vector2, int>>();
        public void TheUltimateSuperHyperUltraMurderFastDashOfDoomDeathJusticeLightAndTragedy()
        {
            if (phaseStart)
            {
                phaseStart = false;
                attackCounter1 = 0;
                attackCounter2 = 0;
                attackCounter3 = 0;
                attackCounter4 = 0;
                NPC.velocity = Vector2.Zero;
                NPC.dontTakeDamage = true;
                NPC.alpha = 255;
                timeBeforeUltidash = (int)MathHelper.Lerp(70f, 90f, (float)NPC.life / (float)NPC.lifeMax * 2f);
                TerrorbornSystem.p1Thunder();
                int ringCount = 10;
                float maxRingDistance = NPC.width / 2;
                for (float i = 0f; i < 1f; i += 1f / ringCount)
                {
                    rings.Add(new Tuple<Vector2, float, float>(NPC.Center, MathHelper.Lerp(0f, maxRingDistance, i), 400f));
                }

                telegraphs.Clear();
                CreateUltiDashTelegraphs();
            }
            else
            {
                NPC.position = player.Center + new Vector2(0, -500) - NPC.Size / 2;

                NPC.rotation += MathHelper.ToRadians(5f);

                for (int i = 0; i < telegraphs.Count; i++)
                {
                    telegraphs[i] = new Tuple<Vector2, Vector2, int>(telegraphs[i].Item1.RotatedBy(MathHelper.ToRadians(maxAngleOffset) * (float)telegraphs[i].Item3 / (float)timeBeforeUltidash, telegraphs[i].Item2) + ultidashVelocity, telegraphs[i].Item2 + ultidashVelocity, telegraphs[i].Item3);
                    mirages.Add(new Tuple<Vector2, float, Color>(telegraphs[i].Item1, NPC.rotation, new Color(151, 255, 178) * 0.5f));
                }

                attackCounter1++;
                if (attackCounter1 == timeBeforeUltidash / 2 && Main.rand.NextFloat() <= 0.33f)
                {
                    for (int i = 0; i < telegraphs.Count; i++)
                    {
                        telegraphs[i] = new Tuple<Vector2, Vector2, int>(telegraphs[i].Item1, telegraphs[i].Item2, -telegraphs[i].Item3);
                    }
                }
                if (attackCounter1 > timeBeforeUltidash)
                {
                    while (telegraphs.Count > 0)
                    {
                        int proj = Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), telegraphs[0].Item1, Vector2.Zero, ModContent.ProjectileType<PrototypeIMirage>(), (int)(100f * damageMult / 4f), 0f);
                        Main.projectile[proj].ai[0] = NPC.spriteDirection;
                        telegraphs.RemoveAt(0);
                    }

                    TerrorbornSystem.p1Thunder();
                    CreateUltiDashTelegraphs();

                    attackCounter1 = 0;
                    attackCounter3++;
                    if (attackCounter3 >= Math.Round(MathHelper.Lerp(5, 4, (float)NPC.life / (float)NPC.lifeMax * 2f)))
                    {
                        NPC.position = player.Center + MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2() * 3000f;
                        NPC.position -= NPC.Size / 2;
                        DecideNextAttack();
                        NPC.dontTakeDamage = false;
                        NPC.alpha = 0;
                    }
                }
            }
        }

        int currentDirection = 1;
        public void CreateUltiDashTelegraphs()
        {
            int amountToSide = 5;
            int amountOnLineSide = 20;

            ultidashVelocity = Vector2.Zero;
            if (NPC.life >= NPC.lifeMax * 0.35f || Main.rand.NextBool())
            {
                attackCounter2++;
            }
            if (attackCounter2 % 2f == 0f)
            {
                if (NPC.life <= NPC.lifeMax * 0.35f && Main.rand.NextBool())
                {
                    if (Main.rand.NextBool())
                    {
                        ultidashVelocity.X = 210f / timeBeforeUltidash;
                    }
                    else
                    {
                        ultidashVelocity.X = 210f / -timeBeforeUltidash;
                    }
                }
                if (Main.rand.NextBool())
                {
                    for (int i = -amountToSide; i <= amountToSide; i++)
                    {
                        currentDirection *= -1;
                        Vector2 position = new Vector2(player.Center.X + 420 * i, player.Center.Y);
                        for (int j = -amountOnLineSide; j <= amountOnLineSide; j++)
                        {
                            telegraphs.Add(new Tuple<Vector2, Vector2, int>(MathHelper.ToRadians(90f).ToRotationVector2() * 220 * j + position, position, currentDirection));
                        }
                    }
                }
                else
                {
                    for (int i = -amountToSide; i <= amountToSide; i++)
                    {
                        currentDirection *= -1;
                        Vector2 position = new Vector2(player.Center.X + 420 * i, player.Center.Y);
                        for (int j = -amountOnLineSide; j <= amountOnLineSide; j++)
                        {
                            telegraphs.Add(new Tuple<Vector2, Vector2, int>(MathHelper.ToRadians(90f).ToRotationVector2().RotatedBy(MathHelper.ToRadians(maxAngleOffset) * -currentDirection) * 220 * j + position, position, currentDirection));
                        }
                    }
                }
            }
            else
            {
                if (NPC.life <= NPC.lifeMax * 0.35f && Main.rand.NextBool())
                {
                    if (Main.rand.NextBool())
                    {
                        ultidashVelocity.Y = 210f / timeBeforeUltidash;
                    }
                    else
                    {
                        ultidashVelocity.Y = 210f / -timeBeforeUltidash;
                    }
                }
                if (Main.rand.NextBool())
                {
                    for (int i = -amountToSide; i <= amountToSide; i++)
                    {
                        currentDirection *= -1;
                        Vector2 position = new Vector2(player.Center.X, player.Center.Y + 420 * i);
                        for (int j = -amountOnLineSide; j <= amountOnLineSide; j++)
                        {
                            telegraphs.Add(new Tuple<Vector2, Vector2, int>(MathHelper.ToRadians(-0f).ToRotationVector2() * 220 * j + position, position, currentDirection));
                        }
                    }
                }
                else
                {
                    for (int i = -amountToSide; i <= amountToSide; i++)
                    {
                        currentDirection *= -1;
                        Vector2 position = new Vector2(player.Center.X, player.Center.Y + 420 * i);
                        for (int j = -amountOnLineSide; j <= amountOnLineSide; j++)
                        {
                            telegraphs.Add(new Tuple<Vector2, Vector2, int>(MathHelper.ToRadians(-0f).ToRotationVector2().RotatedBy(MathHelper.ToRadians(maxAngleOffset) * -currentDirection) * 220 * j + position, position, currentDirection));
                        }
                    }
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
                attackDirection1 = Math.Sign(player.Center.X - NPC.Center.X);
                currentTarget = new Vector2(-500 * attackDirection1, 0);
                attackDirection2 = 1;
                if (Main.rand.NextBool()) attackDirection2 = -1;
            }
            else if (attackCounter1 == 0)
            {
                Vector2 targetPos = player.Center + currentTarget;
                if (NPC.Distance(targetPos) > 1250)
                {
                    NPC.position = targetPos - NPC.Size / 2 + new Vector2(0, 1);
                    TerrorbornSystem.p1Thunder();
                    rings.Add(new Tuple<Vector2, float, float>(NPC.Center, 0f, 250f));
                }
                if (NPC.Distance(targetPos) > aligningSpeed) NPC.velocity += NPC.DirectionTo(targetPos) * aligningSpeed;
                NPC.velocity *= 0.9f;
                NPC.spriteDirection = Math.Sign(player.Center.X - NPC.Center.X);
                NPC.rotation += MathHelper.ToRadians(10) * NPC.spriteDirection;
                attackCounter2++;
                if (attackCounter2 > minimumTimeUntilDash && NPC.Distance(targetPos) <= 240)
                {
                    NPC.velocity = NPC.DirectionTo(player.Center) * dashSpeed;
                    attackCounter1 = 1;
                    attackCounter2 = 0;
                    currentTarget = currentTarget.RotatedBy(MathHelper.ToRadians(180));
                    currentTarget = currentTarget.RotatedBy(MathHelper.ToRadians(45) * attackDirection2);
                }
            }
            else if (attackCounter1 == 1)
            {
                NPC.rotation += MathHelper.ToRadians(25) * NPC.spriteDirection;
                attackCounter2++;
                if (attackCounter2 % timeBetweenProjectiles == timeBetweenProjectiles - 1)
                {
                    int proj = Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaticVision>(), (int)(75f * damageMult / 4f), 0f);
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
                NPC.spriteDirection = Math.Sign(player.Center.X - NPC.Center.X);
                NPC.rotation += MathHelper.ToRadians(5) * NPC.spriteDirection;
                NPC.velocity *= 0.98f;
                attackCounter2++;
                if (attackCounter2 > timeUntilNext)
                {
                    DecideNextAttack();
                }
            }
        }

        public void FocusDeathray(int timeUntilDeathray, float offsetMultiplier, float angleLerpAmount, int timeBetweenProjectiles, float travelSpeed, float startingFocus = 45f, int timeUntilNext = 60)
        {
            if (phaseStart)
            {
                phaseStart = false;
                attackCounter1 = 0;
                attackCounter2 = 0;
                attackCounter3 = 0;
                attackRotation1 = NPC.DirectionTo(player.Center).ToRotation();
            }
            else if (attackCounter2 == 0)
            {
                float focus = MathHelper.Lerp(startingFocus, 0f, (float)attackCounter1 / (float)timeUntilDeathray);

                attackRotation1 = attackRotation1.AngleLerp(NPC.DirectionTo(player.Center + player.velocity * offsetMultiplier).ToRotation(), angleLerpAmount);

                lines.Add(new Tuple<Vector2, Vector2, Color, Color, int>(NPC.Center, NPC.Center + attackRotation1.ToRotationVector2().RotatedBy(MathHelper.ToRadians(focus)) * NPC.Distance(player.Center) * 1.5f, new Color(151, 255, 178), Color.Transparent, 5));
                lines.Add(new Tuple<Vector2, Vector2, Color, Color, int>(NPC.Center, NPC.Center + attackRotation1.ToRotationVector2().RotatedBy(MathHelper.ToRadians(-focus)) * NPC.Distance(player.Center) * 1.5f, new Color(151, 255, 178), Color.Transparent, 5));

                NPC.spriteDirection = Math.Sign(player.Center.X - NPC.Center.X);
                NPC.rotation += MathHelper.ToRadians(5) * NPC.spriteDirection;

                Vector2 targetPos = NPC.DirectionFrom(player.Center) * 130 + player.Center;
                if (NPC.Distance(targetPos) >= 1250 && attackCounter1 == 0)
                {
                    NPC.position = NPC.DirectionFrom(player.Center) * 500 + player.Center - NPC.Size / 2 + new Vector2(0, 1);
                    TerrorbornSystem.p1Thunder();
                    rings.Add(new Tuple<Vector2, float, float>(NPC.Center, 0f, 250f));
                    NPC.velocity -= NPC.DirectionTo(player.Center) * 20f;
                }

                if (NPC.Distance(targetPos) > travelSpeed) NPC.velocity += NPC.DirectionTo(targetPos) * travelSpeed;
                NPC.velocity *= 0.92f;

                attackCounter1++;
                if (attackCounter1 % timeBetweenProjectiles == timeBetweenProjectiles - 1)
                {
                    float speed = 25f;
                    SoundExtensions.PlaySoundOld(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 125, 1, 1);
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, attackRotation1.ToRotationVector2().RotatedBy(MathHelper.ToRadians(focus)) * speed, ModContent.ProjectileType<CursedBeam>(), (int)(50f * damageMult / 4f), 0f);
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, attackRotation1.ToRotationVector2().RotatedBy(MathHelper.ToRadians(-focus)) * speed, ModContent.ProjectileType<CursedBeam>(), (int)(50f * damageMult / 4f), 0f);
                }
                if (attackCounter1 > timeUntilDeathray)
                {
                    attackCounter2++;
                    attackCounter1 = 0;
                    SoundExtensions.PlaySoundOld(SoundID.Zombie104, (int)NPC.Center.X, (int)NPC.Center.Y, 104, 1, 2f);
                }
            }
            else if (attackCounter2 == 1)
            {
                NPC.spriteDirection = Math.Sign(player.Center.X - NPC.Center.X);
                NPC.rotation += MathHelper.ToRadians(10) * NPC.spriteDirection;

                TerrorbornSystem.ScreenShake(2f);
                int proj = Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, attackRotation1.ToRotationVector2(), ModContent.ProjectileType<PlasmaDeathray>(), (int)(75f * damageMult / 4f), 0f);
                Main.projectile[proj].ai[0] = NPC.whoAmI;

                attackRotation1 = attackRotation1.AngleTowards(NPC.DirectionTo(player.Center).ToRotation(), MathHelper.ToRadians(0.5f));

                Vector2 targetPos = NPC.DirectionFrom(player.Center) * 130 + player.Center;

                if (NPC.Distance(targetPos) > travelSpeed) NPC.velocity += NPC.DirectionTo(targetPos) * travelSpeed;
                NPC.velocity *= 0.92f;

                attackCounter1++;
                if (attackCounter1 > 60)
                {
                    attackCounter2++;
                    attackCounter1 = 0;
                }
            }
            else if (attackCounter2 == 2)
            {
                NPC.spriteDirection = Math.Sign(player.Center.X - NPC.Center.X);
                NPC.rotation += MathHelper.ToRadians(5) * NPC.spriteDirection;
                NPC.velocity *= 0.95f;
                attackCounter1++;
                if (attackCounter1 > timeUntilNext)
                {
                    DecideNextAttack();
                }
            }
        }

        public void SuperHyperUltraMurderFastDash(float travelSpeed, float speed, int timeUntilDash, int dashCount, int dashTime = 30, int mirageCountFromPlayer = 5)
        {
            if (phaseStart)
            {
                phaseStart = false;
                attackCounter1 = 0;
                attackCounter2 = 0;
                attackCounter3 = 0;
                attackRotationSpeed = 0f;
            }
            else if (attackCounter3 == 0)
            {
                Vector2 targetPos = player.Center + NPC.DirectionFrom(player.Center) * speed * mirageCountFromPlayer;

                attackRotationSpeed += 30f / (float)timeUntilDash;

                NPC.spriteDirection = Math.Sign(player.Center.X - NPC.Center.X);
                NPC.rotation += MathHelper.ToRadians(attackRotationSpeed) * NPC.spriteDirection;

                if (attackCounter1 == 0)
                {
                    NPC.position = targetPos - NPC.Size / 2 + new Vector2(0, 0.5f);
                    TerrorbornSystem.p1Thunder();
                    rings.Add(new Tuple<Vector2, float, float>(NPC.Center, 0f, 250f));
                }

                if (NPC.Distance(targetPos) > travelSpeed) NPC.velocity += NPC.DirectionTo(targetPos) * travelSpeed;
                NPC.velocity *= 0.92f;

                int count = (int)(mirageCountFromPlayer * 2.5f);
                for (float i = 0; i < 1f; i += 1f / (float)count)
                {
                    int realI = (int)Math.Round(i / (1f / (float)count));
                    if (i != 0f)
                    {
                        mirages.Add(new Tuple<Vector2, float, Color>(NPC.Center + realI * NPC.DirectionTo(player.Center) * speed + NPC.velocity, NPC.rotation, new Color(151, 255, 178) * (1f - i)));
                    }
                }

                attackCounter1++;
                if (attackCounter1 > timeUntilDash)
                {
                    attackCounter3 = 1;
                    attackCounter1 = 0;
                    NPC.velocity = NPC.DirectionTo(player.Center) * speed;
                    rings.Add(new Tuple<Vector2, float, float>(NPC.Center, 0f, 250f));
                    SoundExtensions.PlaySoundOld(SoundID.Item67, NPC.Center);
                    TerrorbornSystem.ScreenShake(5);
                }
            }
            else
            {
                attackCounter1++;
                if (attackCounter1 > dashTime)
                {
                    attackCounter3 = 0;
                    attackCounter1 = 0;
                    attackRotationSpeed = 0f;
                    NPC.velocity = Vector2.Zero;

                    attackCounter2++;
                    if (attackCounter2 >= dashCount)
                    {
                        DecideNextAttack();
                    }
                }
            }
        }

        float currentSpeed = 0f;
        const int cloneY = 0;
        const int cloneX = 1;
        const int cloneXandY = 2;
        List<int> dashClones = new List<int>();
        public void CloneDashes(float dashSpeed, int timeBetweenDashes, float accelleration)
        {
            Vector2 center = NPC.Center;
            if (phaseStart)
            {
                phaseStart = false;
                attackCounter1 = 0;
                attackCounter2 = 0;
                attackCounter3 = 0;
                currentSpeed = 0f;
                dashClones.Clear();
                float distance = 400;
                currentTarget.X = distance;
                if (Main.rand.NextBool()) currentTarget.X *= -1;
                currentTarget.Y = distance;
                if (Main.rand.NextBool()) currentTarget.Y *= -1;
            }
            else if (attackCounter1 == 0)
            {
                Vector2 targetPos = player.Center + currentTarget;
                if (NPC.Distance(targetPos) > 500)
                {
                    NPC.position = targetPos - NPC.Size / 2 + new Vector2(0, 1);
                    TerrorbornSystem.p1Thunder();
                    rings.Add(new Tuple<Vector2, float, float>(NPC.Center, 0f, 250f));
                }
                if (NPC.Distance(targetPos) > 3f) NPC.velocity += NPC.DirectionTo(targetPos) * 3f;
                NPC.velocity *= 0.9f;

                attackCounter2++;
                if (attackCounter2 > 20)
                {
                    attackCounter2 = 0;
                    int addedClone = Main.rand.Next(3);
                    while (dashClones.Contains(addedClone))
                    {
                        addedClone = Main.rand.Next(3);
                    }
                    Vector2 pos = Vector2.Zero;
                    if (addedClone == cloneX)
                    {
                        pos = new Vector2(center.X + (player.Center.X - center.X) * 2, center.Y);
                    }
                    if (addedClone == cloneY)
                    {
                        pos = new Vector2(center.X, center.Y + (player.Center.Y - center.Y) * 2);
                    }
                    if (addedClone == cloneXandY)
                    {
                        pos = new Vector2(center.X + (player.Center.X - center.X) * 2, center.Y + (player.Center.Y - center.Y) * 2);
                    }
                    rings.Add(new Tuple<Vector2, float, float>(pos, 0f, 250f));
                    dashClones.Add(addedClone);
                    if (dashClones.Count == 3)
                    {
                        attackCounter1++;
                    }
                }
            }
            else if (attackCounter1 == 1)
            {
                if (currentSpeed < dashSpeed)
                {
                    currentSpeed += accelleration;
                }
                NPC.velocity = NPC.DirectionTo(player.Center) * currentSpeed;
                attackCounter2++;
                if (attackCounter2 > timeBetweenDashes)
                {
                    attackCounter2 = 0;

                    if (dashClones.Count == 0)
                    {
                        DecideNextAttack();
                    }
                    else
                    {
                        currentSpeed = 0f;
                        if (NPC.Distance(player.Center) <= 350)
                        {
                            currentSpeed = -10f;
                        }
                        if (dashClones[0] == cloneX)
                        {
                            NPC.position = new Vector2(center.X + (player.Center.X - center.X) * 2, center.Y);
                        }
                        if (dashClones[0] == cloneY)
                        {
                            NPC.position = new Vector2(center.X, center.Y + (player.Center.Y - center.Y) * 2);
                        }
                        if (dashClones[0] == cloneXandY)
                        {
                            NPC.position = new Vector2(center.X + (player.Center.X - center.X) * 2, center.Y + (player.Center.Y - center.Y) * 2);
                        }
                        NPC.position -= NPC.Size / 2;

                        //rings.Add(new Tuple<Vector2, float, float>(NPC.Center, 0f, 250f));
                        SoundExtensions.PlaySoundOld(SoundID.Item67, NPC.Center);
                        TerrorbornSystem.ScreenShake(5);

                        dashClones.RemoveAt(0);
                    }
                }
            }

            NPC.spriteDirection = Math.Sign(player.Center.X - NPC.Center.X);
            NPC.rotation += MathHelper.ToRadians(15) * NPC.spriteDirection;

            center = NPC.Center + NPC.velocity;
            if (dashClones.Contains(cloneX))
            {
                mirages.Add(new Tuple<Vector2, float, Color>(new Vector2(center.X + (player.Center.X - center.X) * 2, center.Y), NPC.rotation, new Color(151, 255, 178) * 0.75f));
            }
            if (dashClones.Contains(cloneY))
            {
                mirages.Add(new Tuple<Vector2, float, Color>(new Vector2(center.X, center.Y + (player.Center.Y - center.Y) * 2), NPC.rotation, new Color(151, 255, 178) * 0.75f));
            }
            if (dashClones.Contains(cloneXandY))
            {
                mirages.Add(new Tuple<Vector2, float, Color>(new Vector2(center.X + (player.Center.X - center.X) * 2, center.Y + (player.Center.Y - center.Y) * 2), NPC.rotation, new Color(151, 255, 178) * 0.75f));
            }
        }

        public void TheSpin(int cloneCount, float spinSpeed, int timeBetweenProjectiles, float ProjectileSpeed, float distance, int totalTime)
        {
            if (phaseStart)
            {
                phaseStart = false;
                attackCounter1 = 0;
                attackCounter2 = cloneCount + 1;
                attackDirection1 = 1;
                if (Main.rand.NextBool()) attackDirection1 = -1;
                attackCounter3 = 0;
                attackCounter4 = 0;

                attackRotation1 = MathHelper.ToRadians(Main.rand.Next(360));

                NPC.position = player.Center + attackRotation1.ToRotationVector2() * distance - NPC.Size / 2;
                TerrorbornSystem.p1Thunder();
                rings.Add(new Tuple<Vector2, float, float>(NPC.Center, 0f, 250f));
            }
            else if (attackCounter4 == 0)
            {
                attackRotation1 += MathHelper.ToRadians(spinSpeed) * attackDirection1;

                NPC.spriteDirection = attackDirection1;
                NPC.rotation += MathHelper.ToRadians(spinSpeed * 3) * attackDirection1;

                NPC.position = player.Center + attackRotation1.ToRotationVector2() * distance - NPC.Size / 2;
                List<Vector2> miragePositions = new List<Vector2>();
                for (int i = 0; i < attackCounter2; i++)
                {
                    if (i != 0)
                    {
                        miragePositions.Add(player.Center + attackRotation1.ToRotationVector2().RotatedBy(MathHelper.ToRadians(360f * (float)i / attackCounter2)) * distance);
                    }
                }

                foreach (Vector2 position in miragePositions)
                {
                    mirages.Add(new Tuple<Vector2, float, Color>(position, NPC.rotation, new Color(151, 255, 178) * 0.75f));
                }

                attackCounter1++;
                if (attackCounter1 % timeBetweenProjectiles == timeBetweenProjectiles - 1)
                {
                    if (attackCounter3 > 0)
                    {
                        SoundExtensions.PlaySoundOld(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 125, 1, 1);
                        float speed = ProjectileSpeed;
                        if (attackCounter1 == 1)
                        {
                            speed /= 2;
                        }
                        int proj = Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, NPC.DirectionTo(player.Center) * speed, ModContent.ProjectileType<CursedBeam>(), (int)(50f * damageMult / 4f), 0f);
                        Main.projectile[proj].timeLeft = (int)(NPC.Distance(player.Center) / speed);
                        foreach (Vector2 position in miragePositions)
                        {
                            proj = Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), position, player.DirectionFrom(position) * speed, ModContent.ProjectileType<CursedBeam>(), (int)(50f * damageMult / 4f), 0f);
                            Main.projectile[proj].timeLeft = (int)(NPC.Distance(player.Center) / speed);
                        }
                    }
                    attackCounter3++;
                }
                if (attackCounter1 >= totalTime)
                {
                    foreach (Vector2 position in miragePositions)
                    {
                        rings.Add(new Tuple<Vector2, float, float>(position, 0f, 250f));
                    }

                    attackCounter1 = (int)(NPC.Distance(player.Center) / ProjectileSpeed) + 30;
                    NPC.velocity = player.velocity;
                    attackCounter4++;
                }
            }
            else
            {
                NPC.spriteDirection = attackDirection1;
                NPC.rotation += MathHelper.ToRadians(spinSpeed) * attackDirection1;
                NPC.velocity *= 0.96f;

                attackCounter1--;
                if (attackCounter1 <= 0)
                {
                    DecideNextAttack();
                    NPC.velocity = Vector2.Zero;
                }
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

    class PrototypeIMirage : ModProjectile
    {
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 10);
        }

        int timeLeft = 15;
        public override void SetDefaults()
        {
            Projectile.width = 240;
            Projectile.height = 240;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = timeLeft;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.ai[0] == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Texture), Projectile.Center - Main.screenPosition + new Vector2(0, 4), null, Projectile.GetAlpha(Color.White), Projectile.rotation, Projectile.Size / 2, Projectile.scale, effects, 0f);
            return false;
        }

        public override void AI()
        {
            Projectile.alpha += 255 / timeLeft;
        }

        public override bool CanHitPlayer(Player target)
        {
            return Projectile.Distance(target.Center) <= 170;
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
            Projectile.width = 24;
            Projectile.height = 34;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 360;
        }

        float homingSpeed = 0;
        bool launched = false;

        public override void AI()
        {
            if (homingSpeed == 0)
            {
                homingSpeed = Main.rand.NextFloat(0.5f, 0.8f);
            }

            int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 74, 0f, 0f, 100, Scale: 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Projectile.velocity;

            if (Projectile.ai[0] <= 0)
            {
                if (!launched)
                {
                    Player target = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
                    Vector2 direction = Projectile.DirectionTo(target.Center);
                    Vector2 speed = direction * 30;
                    Projectile.velocity = speed;
                    launched = true;
                }
            }
            else
            {
                Player target = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
                Vector2 direction = Projectile.DirectionTo(target.Center);
                Projectile.rotation = direction.ToRotation() + MathHelper.ToRadians(90);
                if (target.Center.X <= Projectile.Center.X)
                {
                    Projectile.spriteDirection = -1;
                }
                //Vector2 speed = direction * homingSpeed;
                //Projectile.velocity += speed;
                //Projectile.velocity *= 0.98f;
                Projectile.ai[0]--;
            }
        }
    }

    class PlasmaDeathray : Deathray
    {
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 5);
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 100000;
        }

        public override string Texture => "TerrorbornMod/NPCs/Bosses/PrototypeI/PlasmaDeathray";
        public override void SetDefaults()
        {
            Projectile.width = 25;
            Projectile.height = 25;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hide = false;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.timeLeft = 2;
            MoveDistance = 20f;
            RealMaxDistance = 3000f;
            bodyRect = new Rectangle(0, 0, Projectile.width, Projectile.height);
            headRect = new Rectangle(0, 0, Projectile.width, Projectile.height);
            tailRect = new Rectangle(0, 0, Projectile.width, Projectile.height);
        }

        public override Vector2 Position()
        {
            return Main.npc[(int)Projectile.ai[0]].Center;
        }
    }
}