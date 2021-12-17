using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace TerrorbornMod.NPCs
{
    class SkeletonBandit : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 20;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.aiStyle = -1;
            npc.width = 20; //44
            npc.height = 48;
            npc.damage = 15;
            npc.defense = 5;
            npc.lifeMax = 120;
            if (Main.hardMode)
            {
                npc.lifeMax = 300;
            }
            npc.HitSound = SoundID.DD2_SkeletonHurt;
            npc.DeathSound = SoundID.DD2_SkeletonDeath;
            npc.value = Item.buyPrice(0, 0, 75, 0);
            npc.knockBackResist = 1f;
            npc.lavaImmune = true;
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextFloat() <= 0.05f)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Equipable.Accessories.BanditGlove>());
            }
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (shooting)
            {
                float direction = Math.Abs(MathHelper.ToDegrees(npc.DirectionTo(Main.LocalPlayer.Center).RotatedBy(-1.57f).ToRotation()));
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

                npc.frame.Y = frame * frameHeight;
                return;
            }

            if (frame >= 5)
            {
                if (npc.velocity.Y == 0)
                {
                    npc.frameCounter--;
                    if (npc.frameCounter <= 0)
                    {
                        frame++;
                        if (hasSeenPlayer)
                        {
                            npc.frameCounter = 2;
                        }
                        else
                        {
                            npc.frameCounter = 3;
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

            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float chanceMultiplier = 1f;

            if (spawnInfo.player.ZoneSnow)
            {
                chanceMultiplier *= 0.3f;
            }

            if (spawnInfo.player.ZoneJungle)
            {
                chanceMultiplier *= 1.5f;
            }

            if (spawnInfo.player.ZoneDesert)
            {
                chanceMultiplier *= 2f;
            }

            if (spawnInfo.player.ZoneOverworldHeight && !Main.dayTime)
            {
                return SpawnCondition.OverworldNightMonster.Chance * 0.015f * chanceMultiplier;
            }

            return SpawnCondition.Cavern.Chance * 0.05f * chanceMultiplier;
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            if (!hasSeenPlayer)
            {
                ambientCounter = Main.rand.Next(360, 1600);

                hasSeenPlayer = true;
                Main.PlaySound(SoundID.Zombie, (int)npc.Center.X, (int)npc.Center.Y, Main.rand.Next(1), 0.5f, 0.5f);

                WeightedRandom<string> voiceLines = new WeightedRandom<string>();

                voiceLines.Add("Who's there!?");
                voiceLines.Add("Wh... where did that come from!?");
                voiceLines.Add("Hello?");
                voiceLines.Add(TerrorbornWorld.SkeletonSheriffName + "... is that you?", 0.25f);

                CombatText.NewText(npc.getRect(), Color.Red, voiceLines, true);
            }
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            ambientCounter = Main.rand.Next(360, 1600);

            hasSeenPlayer = true;
            Main.PlaySound(SoundID.Zombie, (int)npc.Center.X, (int)npc.Center.Y, Main.rand.Next(1), 0.5f, 0.5f);

            WeightedRandom<string> voiceLines = new WeightedRandom<string>();

            voiceLines.Add("Hey, you there!");
            voiceLines.Add("Wait a minute... you're not a skeleton!");
            voiceLines.Add("Uh oh... a human!");
            voiceLines.Add("You can't hide from me!");
            voiceLines.Add("Woohoo, target practice!");
            voiceLines.Add("HEY! You aren't the taco delivery guy!", 0.5f);

            CombatText.NewText(npc.getRect(), Color.Red, voiceLines, true);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = new SpriteEffects();
            if (npc.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 drawOrigin = new Vector2(44 / 2, 48 / 2);
            for (int i = 0; i < npc.oldPos.Length; i++)
            {
                Vector2 drawPos = npc.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(-12, 4) + npc.visualOffset;
                Color color = npc.GetAlpha(Color.White) * ((float)(npc.oldPos.Length - i) / (float)npc.oldPos.Length);
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/NPCs/SkeletonBandit_Glow"), drawPos, npc.frame, color, npc.rotation, drawOrigin, 1f, effects, 0f);
            }
        }

        bool shooting = false;
        bool hasSeenPlayer = false;
        int projectileCounter = 45;
        int ambientCounter = -1;
        public override void AI()
        {
            Player player = Main.LocalPlayer;
            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(npc);

            if (Collision.CanHitLine(npc.position, npc.width, npc.height, player.position, player.width, player.height) && !hasSeenPlayer)
            {
                ambientCounter = Main.rand.Next(360, 1600);

                hasSeenPlayer = true;
                Main.PlaySound(SoundID.Zombie, (int)npc.Center.X, (int)npc.Center.Y, Main.rand.Next(1), 0.5f, 0.5f);

                WeightedRandom<string> voiceLines = new WeightedRandom<string>();

                voiceLines.Add("Hey, you there!");
                voiceLines.Add("Wait a minute... you're not a skeleton!");
                voiceLines.Add("Uh oh... a human!");
                voiceLines.Add("You can't hide from me!");
                voiceLines.Add("Woohoo, target practice!");
                voiceLines.Add("HEY! You aren't the taco delivery guy!", 0.5f);

                CombatText.NewText(npc.getRect(), Color.Red, voiceLines, true);
            }

            shooting = false;
            if (Collision.CanHitLine(npc.position, npc.width, npc.height, player.position, player.width, player.height) && npc.Distance(player.Center) > 120 && npc.Distance(player.Center) < 600)
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
                Main.PlaySound(SoundID.Zombie, (int)npc.Center.X, (int)npc.Center.Y, Main.rand.Next(1), 0.5f, 0.5f);

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
                    voiceLines.Add("Hey, at least I don't have " + TerrorbornWorld.SkeletonSheriffName + " chasing after me anymore!");
                    voiceLines.Add("Some human flesh would be a very nice treat right now.");
                    voiceLines.Add("Is there anybody around?");
                    voiceLines.Add("Cookies for sale! (but only if you're a skeleton)");
                    voiceLines.Add("Y'know, I wonder if " + TerrorbornWorld.SkeletonSheriffName + " was right all along... nah!");
                }
                else
                {
                    if (npc.Distance(player.Center) > 120)
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

                CombatText.NewText(npc.getRect(), Color.Red, voiceLines, true);
            }

            if (shooting)
            {
                npc.velocity.X *= 0.93f;
                npc.direction = 1;
                if (player.Center.X < npc.Center.X)
                {
                    npc.direction = -1;
                }

                projectileCounter--;
                if (projectileCounter <= 0)
                {
                    projectileCounter = 20;
                    float speed = 15f;
                    float recoil = 1f;
                    Vector2 velocity = speed * npc.DirectionTo(player.Center);
                    int damage = 25;
                    if (Main.hardMode)
                    {
                        damage = 70;
                    }
                    Projectile.NewProjectile(npc.Center, velocity, ProjectileID.BulletDeadeye, damage / 4, 0);

                    npc.velocity += new Vector2(npc.spriteDirection * recoil, 0);
                }
            }
            else
            {
                projectileCounter = 45;
                if (frame >= 5)
                {
                    if (hasSeenPlayer)
                    {
                        modNPC.ImprovedFighterAI(npc, 3, 0.2f, 0.95f, 6, false, 0);
                    }
                    else
                    {
                        modNPC.ImprovedFighterAI(npc, 2, 0.1f, 0.95f, 6, false, 0, wanderTime: 180);
                    }
                }
                else
                {
                    npc.frameCounter--;
                    if (npc.frameCounter <= 0)
                    {
                        frame++;
                        npc.frameCounter = 5;
                    }

                    if (frame == 5)
                    {
                        frame++;
                    }
                }
            }

            npc.spriteDirection = npc.direction * -1;
            npc.visualOffset = new Vector2(6 * -npc.spriteDirection, 0);
        }

        //public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        //{
        //    position = new Vector2(npc.Center.X, position.Y);
        //    return base.DrawHealthBar(hbPosition, ref scale, ref position);
        //}
    }
}