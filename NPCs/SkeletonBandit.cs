using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader.Utilities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace TerrorbornMod.NPCs
{
    class SkeletonBandit : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 20;
            NPCID.Sets.TrailCacheLength[NPC.type] = 3;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                new FlavorTextBestiaryInfoElement("An undead that retained its memory and sanity, which chose to use that as an excuse to live an eternity of anarchy. Although these bandits can be seen in a variety of places, it is most common that you find them lingering underground.")
            });
        }

        public override void SetDefaults()
        {
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.aiStyle = -1;
            NPC.width = 20; //44
            NPC.height = 48;
            NPC.damage = 15;
            NPC.defense = 5;
            NPC.lifeMax = 120;
            if (Main.hardMode)
            {
                NPC.lifeMax = 300;
            }
            NPC.HitSound = SoundID.DD2_SkeletonHurt;
            NPC.DeathSound = SoundID.DD2_SkeletonDeath;
            NPC.value = Item.buyPrice(0, 0, 75, 0);
            NPC.knockBackResist = 1f;
            NPC.lavaImmune = true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Equipable.Accessories.BanditGlove>(), 14, 1, 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Equipable.Vanity.BFCap>(), 50, 1, 2));
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (shooting)
            {
                float direction = Math.Abs(MathHelper.ToDegrees(NPC.DirectionTo(Main.LocalPlayer.Center).RotatedBy(-1.57f).ToRotation()));
                if (direction < 22.5f)
                {
                    frame = 4;
                }
                else if (direction < 67.5f)
                {
                    frame = 3;
                }
                else if (direction < 112.5f)
                {
                    frame = 2;
                }
                else if (direction < 157.5f)
                {
                    frame = 1;
                }
                else
                {
                    frame = 0;
                }

                NPC.frame.Y = frame * frameHeight;
                return;
            }

            if (frame >= 5)
            {
                if (NPC.velocity.Y == 0)
                {
                    NPC.frameCounter--;
                    if (NPC.frameCounter <= 0)
                    {
                        frame++;
                        if (hasSeenPlayer)
                        {
                            NPC.frameCounter = 2;
                        }
                        else
                        {
                            NPC.frameCounter = 3;
                        }
                    }
                    if (frame >= 20)
                    {
                        frame = 6;
                    }
                }
                else
                {
                    frame = 5;
                }
            }

            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float chanceMultiplier = 1f;

            if (spawnInfo.Player.ZoneSnow)
            {
                chanceMultiplier *= 0.3f;
            }

            if (spawnInfo.Player.ZoneJungle)
            {
                chanceMultiplier *= 1.5f;
            }

            if (spawnInfo.Player.ZoneDesert)
            {
                chanceMultiplier *= 2f;
            }

            if (spawnInfo.Player.ZoneOverworldHeight && !Main.dayTime)
            {
                return SpawnCondition.OverworldNightMonster.Chance * 0.015f * chanceMultiplier;
            }

            return SpawnCondition.Cavern.Chance * 0.05f * chanceMultiplier;
        }

        public override void OnHitByProjectile(Projectile Projectile, int damage, float knockback, bool crit)
        {
            if (!hasSeenPlayer)
            {
                ambientCounter = Main.rand.Next(360, 1600);

                hasSeenPlayer = true;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Zombie, (int)NPC.Center.X, (int)NPC.Center.Y, Main.rand.Next(1), 0.5f, 0.5f);

                WeightedRandom<string> voiceLines = new WeightedRandom<string>();

                voiceLines.Add("Who's there!?");
                voiceLines.Add("Wh... where did that come from!?");
                voiceLines.Add("Hello?");
                voiceLines.Add(TerrorbornSystem.SkeletonSheriffName + "... is that you?", 0.25f);

                CombatText.NewText(NPC.getRect(), Color.Red, voiceLines, true);
            }
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            ambientCounter = Main.rand.Next(360, 1600);

            hasSeenPlayer = true;
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Zombie, (int)NPC.Center.X, (int)NPC.Center.Y, Main.rand.Next(1), 0.5f, 0.5f);

            WeightedRandom<string> voiceLines = new WeightedRandom<string>();

            voiceLines.Add("Hey, you there!");
            voiceLines.Add("Wait a minute... you're not a skeleton!");
            voiceLines.Add("Uh oh... a human!");
            voiceLines.Add("You can't hide from me!");
            voiceLines.Add("Woohoo, target practice!");
            voiceLines.Add("HEY! You aren't the taco delivery guy!", 0.5f);

            CombatText.NewText(NPC.getRect(), Color.Red, voiceLines, true);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = new SpriteEffects();
            if (NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 drawOrigin = new Vector2(44 / 2, 48 / 2);
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                Vector2 drawPos = NPC.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(-12, 4)/* + visualOffset*/;
                Color color = NPC.GetAlpha(Color.White) * ((float)(NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/SkeletonBandit_Glow"), drawPos, NPC.frame, color, NPC.rotation, drawOrigin, 1f, effects, 0f);
            }
        }

        Vector2 visualOffset = Vector2.Zero;
        bool shooting = false;
        bool hasSeenPlayer = false;
        int ProjectileCounter = 45;
        int ambientCounter = -1;
        public override void AI()
        {
            Player player = Main.LocalPlayer;
            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(NPC);

            if (Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height) && !hasSeenPlayer)
            {
                ambientCounter = Main.rand.Next(360, 1600);

                hasSeenPlayer = true;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Zombie, (int)NPC.Center.X, (int)NPC.Center.Y, Main.rand.Next(1), 0.5f, 0.5f);

                WeightedRandom<string> voiceLines = new WeightedRandom<string>();

                voiceLines.Add("Hey, you there!");
                voiceLines.Add("Wait a minute... you're not a skeleton!");
                voiceLines.Add("Uh oh... a human!");
                voiceLines.Add("You can't hide from me!");
                voiceLines.Add("Woohoo, target practice!");
                voiceLines.Add("HEY! You aren't the taco delivery guy!", 0.5f);

                CombatText.NewText(NPC.getRect(), Color.Red, voiceLines, true);
            }

            shooting = false;
            if (Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height) && NPC.Distance(player.Center) > 120 && NPC.Distance(player.Center) < 600)
            {
                shooting = true;
            }

            if (ambientCounter == -1)
            {
                ambientCounter = Main.rand.Next(360, 1600);
            }

            ambientCounter--;
            if (ambientCounter <= 0)
            {
                ambientCounter = Main.rand.Next(360, 1600);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Zombie, (int)NPC.Center.X, (int)NPC.Center.Y, Main.rand.Next(1), 0.5f, 0.5f);

                WeightedRandom<string> voiceLines = new WeightedRandom<string>();
                if (shooting)
                {
                    voiceLines.Add("BOOM!", 2f);
                    voiceLines.Add("Pew pew pew!", 2f);
                    voiceLines.Add("DIE!", 2f);
                    voiceLines.Add("BAM!", 2f);
                }
                else if (!hasSeenPlayer)
                {
                    voiceLines.Add("*yawn* I'm getting kinda bored....");
                    voiceLines.Add("...I could really use a taco right now.", 0.5f);
                    voiceLines.Add("Hey, at least I don't have " + TerrorbornSystem.SkeletonSheriffName + " chasing after me anymore!");
                    voiceLines.Add("Some human flesh would be a very nice treat right now.");
                    voiceLines.Add("Is there anybody around?");
                    voiceLines.Add("Cookies for sale! (but only if you're a skeleton)");
                    voiceLines.Add("Y'know, I wonder if " + TerrorbornSystem.SkeletonSheriffName + " was right all along... nah!");
                }
                else
                {
                    if (NPC.Distance(player.Center) > 120)
                    {
                        voiceLines.Add("HEY! Get back here!");
                        voiceLines.Add("Get over here!");
                        voiceLines.Add("All of this running around can get kinda tiring...");
                        voiceLines.Add("Come over here and fight me, coward!");
                    }
                    else
                    {
                        voiceLines.Add("I have you now-", 3f);
                    }
                    voiceLines.Add("*aggressive undead noises*");
                }
                voiceLines.Add("Grahh...");

                CombatText.NewText(NPC.getRect(), Color.Red, voiceLines, true);
            }

            if (shooting)
            {
                NPC.velocity.X *= 0.93f;
                NPC.direction = 1;
                if (player.Center.X < NPC.Center.X)
                {
                    NPC.direction = -1;
                }

                ProjectileCounter--;
                if (ProjectileCounter <= 0)
                {
                    ProjectileCounter = 20;
                    float speed = 15f;
                    float recoil = 1f;
                    Vector2 velocity = speed * NPC.DirectionTo(player.Center);
                    int damage = 25;
                    if (Main.hardMode)
                    {
                        damage = 70;
                    }
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, velocity, ProjectileID.BulletDeadeye, damage / 4, 0);

                    NPC.velocity += new Vector2(NPC.spriteDirection * recoil, 0);
                }
            }
            else
            {
                ProjectileCounter = 45;
                if (frame >= 5)
                {
                    if (hasSeenPlayer)
                    {
                        modNPC.ImprovedFighterAI(NPC, 3, 0.2f, 0.95f, 6, false, 0);
                    }
                    else
                    {
                        modNPC.ImprovedFighterAI(NPC, 2, 0.1f, 0.95f, 6, false, 0, wanderTime: 180);
                    }
                }
                else
                {
                    NPC.frameCounter--;
                    if (NPC.frameCounter <= 0)
                    {
                        frame++;
                        NPC.frameCounter = 5;
                    }

                    if (frame == 5)
                    {
                        frame++;
                    }
                }
            }

            NPC.spriteDirection = NPC.direction * -1;
            visualOffset = new Vector2(6 * -NPC.spriteDirection, 0);
        }

        //public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        //{
        //    position = new Vector2(NPC.Center.X, position.Y);
        //    return base.DrawHealthBar(hbPosition, ref scale, ref position);
        //}
    }
}