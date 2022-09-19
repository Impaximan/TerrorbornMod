using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace TerrorbornMod.NPCs.Minibosses
{
    class DreadAngel : ModNPC
    {
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("A servant of Ephraim, whose true motivations are mysterious. As brutal foes in combat, they won't pull any punches.")
            });
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 20;
            NPCID.Sets.TrailCacheLength[NPC.type] = 1;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 60;
            NPC.height = 90;
            NPC.damage = 75;
            NPC.defense = 70;
            NPC.lifeMax = 35000;
            NPC.HitSound = new Terraria.Audio.SoundStyle("TerrorbornMod/Sounds/Effects/DreadAngelHurt");
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.value = Item.buyPrice(0, 20, 0, 0);
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(NPC);
            modNPC.getsTitleCard = true;
            modNPC.BossTitle = "Dread Angel";
            modNPC.BossSubtitle = "Servant of Ephraim";
            modNPC.BossDefeatTitle = "Greater Angel";
            modNPC.BossTitleColor = Color.Goldenrod;
        }

        public override void OnKill()
        {
            TerrorbornSystem.downedDreadAngel = true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.DreadfulEssence>(),
                minimumDropped: 5,
                maximumDropped: 10));
            npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Weapons.Ranged.Bows.TaleOfTragedy>(),
                ModContent.ItemType<Items.Weapons.Magic.SpellBooks.PhoenixConjuration>()));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (TerrorbornPlayer.modPlayer(spawnInfo.Player).ZoneIncendiary && NPC.downedMoonlord && !NPC.AnyNPCs(ModContent.NPCType<DreadAngel>()))
            {
                return SpawnCondition.Sky.Chance * 0.02f;
            }
            else
            {
                return 0f;
            }
        }

        int frame = 0;
        bool attacking = false;
        bool hasWeapon = true;

        public override void FindFrame(int frameHeight)
        {
            if (!attacking)
            {
                if (hasWeapon)
                {
                    NPC.frameCounter++;
                    if (NPC.frameCounter > 4)
                    {
                        NPC.frameCounter = 0;
                        frame++;
                    }
                    if (frame >= 5)
                    {
                        frame = 0;
                    }
                }
                else
                {
                    NPC.frameCounter++;
                    if (NPC.frameCounter > 4)
                    {
                        NPC.frameCounter = 0;
                        frame++;
                    }
                    if (frame < 14 || frame >= 20)
                    {
                        frame = 14;
                    }
                }
            }
            NPC.frame.Y = frame * frameHeight;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (spawningLaser)
            {
                Utils.Graphics.DrawGlow_1(Main.spriteBatch, NPC.Center - Main.screenPosition, 200, Color.LightPink * 0.5f);
                Terraria.Utils.DrawLine(spriteBatch, laserPosition + new Vector2(0, -3000), laserPosition + new Vector2(0, 3000), Color.LightPink * 0.5f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        bool spawningLaser = false;
        Vector2 laserPosition;
        int AIPhase = 0;
        int attackCounter = 0;
        int aliveCounter = 0;
        bool start = true;
        Player player;
        public override void AI()
        {
            if (start)
            {
                NPC.TargetClosest();
                player = Main.player[NPC.target];
                start = false;
                spawningLaser = true;
                laserPosition = player.Center;
            }

            NPC.spriteDirection = Math.Sign(player.Center.X - NPC.Center.X);

            aliveCounter++;
            if (aliveCounter == 60 * 3)
            {
                WeightedRandom<string> messages = new WeightedRandom<string>();
                messages.Add("BEHOLD! The power of an ANGEL!");
                messages.Add("Prepare thyself, traitor!");
                messages.Add("Though shall repent, incarnate, though shall repent.");
                messages.Add("Thine end has come!");
                messages.Add("Is your software running slow?");
                CombatText.NewText(NPC.getRect(), Color.Gold, messages, true);
            }
            if (aliveCounter % 600 == 599)
            {
                WeightedRandom<string> messages = new WeightedRandom<string>();
                messages.Add("Die, I tell you, DIE!");
                messages.Add("How does though still live!?");
                messages.Add("We remember you, incarnate, we remember you.");
                messages.Add("Thine end has come!");
                messages.Add("May your woes be many!");
                CombatText.NewText(NPC.getRect(), Color.Gold, messages, true);
            }

            if (AIPhase == 0)
            {
                attackCounter++;

                if (attackCounter % 45 == 44)
                {
                    SoundExtensions.PlaySoundOld(SoundID.Item68, laserPosition);
                    TerrorbornSystem.ScreenShake(10);
                    Projectile proj = Main.projectile[Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), laserPosition + new Vector2(0, 3000), new Vector2(0, -1), ModContent.ProjectileType<Incendiary.AngelBeam>(), 120 / 4, 0f)];
                    proj.velocity.Normalize();
                    laserPosition = player.Center + player.velocity * 45;
                }

                spawningLaser = true;
                attacking = false;
                hasWeapon = true;
                float speed = MathHelper.Lerp(0.5f, 0.3f, (float)NPC.life / (float)NPC.lifeMax);

                int yDirection = Math.Sign(player.Center.Y - NPC.Center.Y);
                NPC.velocity.Y += speed * yDirection;

                int xDirection = Math.Sign(player.Center.X - NPC.Center.X);
                NPC.velocity.X += speed * xDirection;
                NPC.spriteDirection = xDirection;

                NPC.velocity *= 0.99f;

                if (attackCounter >= 45 * 5)
                {
                    AIPhase++;
                    spawningLaser = false;
                    attackCounter = 0;
                }
            }
            else if (AIPhase == 1)
            {
                NPC.velocity *= 0.95f;
                attackCounter++;

                if (attackCounter > 120)
                {
                    AIPhase++;
                    attackCounter = 0;
                    attacking = true;
                    frame = 6;
                    NPC.frameCounter = 0;
                }
            }
            else if (AIPhase == 2)
            {
                NPC.velocity *= 0.98f;
                NPC.frameCounter++;
                if (NPC.frameCounter > 4)
                {
                    NPC.frameCounter = 0;
                    frame++;

                    if (frame == 8)
                    {
                        SoundExtensions.PlaySoundOld(SoundID.Item71, NPC.Center);
                        NPC.velocity = NPC.DirectionTo(player.Center) * 15f;
                    }

                    if (frame == 13)
                    {
                        SoundExtensions.PlaySoundOld(SoundID.Item71, NPC.Center);
                        int proj = Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center + new Vector2(20 * NPC.spriteDirection, -40), NPC.velocity * 3, ModContent.ProjectileType<DreadScytheHostile>(), 140 / 4, 0f);
                        Main.projectile[proj].spriteDirection = -NPC.spriteDirection;
                    }

                    if (frame == 14)
                    {
                        AIPhase++;
                        attackCounter = 0;
                        hasWeapon = false;
                        attacking = false;
                    }
                }
            }
            else if ( AIPhase == 3)
            {
                float speed = MathHelper.Lerp(0.5f, 0.3f, (float)NPC.life / (float)NPC.lifeMax);

                int yDirection = Math.Sign(player.Center.Y - NPC.Center.Y);
                NPC.velocity.Y += speed * yDirection;

                int xDirection = Math.Sign(player.Center.X - NPC.Center.X);
                NPC.velocity.X += speed * xDirection;
                NPC.spriteDirection = xDirection;

                NPC.velocity *= 0.99f;

                attackCounter++;

                bool hasScytheOut = false;

                foreach (Projectile Projectile in Main.projectile)
                {
                    if (Projectile.type == ModContent.ProjectileType<DreadScytheHostile>() && Projectile.active)
                    {
                        hasScytheOut = true;
                        break;
                    }
                }

                if (!hasScytheOut)
                {
                    AIPhase = 0;
                    attackCounter = 0;
                }
            }
        }
    }

    class DreadScytheHostile : ModProjectile
    {
        int trueTimeLeft = 60 * 5;

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 74;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 5;
            Projectile.hostile = true;
            Projectile.hide = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 3000;
        }

        public override void AI()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<DreadAngel>()))
            {
                Projectile.active = false;
                return;
            }

            Projectile.rotation += MathHelper.ToRadians(30f) * -Projectile.spriteDirection;

            if (trueTimeLeft > 0)
            {
                Projectile.velocity += Projectile.DirectionTo(Main.LocalPlayer.Center) * 1.4f;
                Projectile.velocity *= 0.97f;
                trueTimeLeft--;
            }
            else
            {
                Projectile.velocity = Projectile.DirectionTo(Main.npc[NPC.FindFirstNPC(ModContent.NPCType<DreadAngel>())].Center) * 30f;
                Projectile.hostile = false;
                if (Projectile.Distance(Main.npc[NPC.FindFirstNPC(ModContent.NPCType<DreadAngel>())].Center) <= 30f)
                {
                    Projectile.active = false;
                }
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Rectangle originalHitbox = hitbox;
            int newSize = 50;
            hitbox.Width = newSize;
            hitbox.Height = newSize;
            hitbox.X = originalHitbox.Center.X - newSize / 2;
            hitbox.Y = originalHitbox.Center.Y - newSize / 2;
            base.ModifyDamageHitbox(ref hitbox);
        }
    }
}