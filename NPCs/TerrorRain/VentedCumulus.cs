using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace TerrorbornMod.NPCs.TerrorRain
{
    class VentedCumulus : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
            NPCID.Sets.TrailCacheLength[NPC.type] = 1;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.width = 56;
            NPC.height = 260 / 5;
            NPC.damage = 0;
            NPC.defense = 25;
            NPC.lifeMax = 750;
            NPC.HitSound = SoundID.NPCHit30;
            NPC.DeathSound = SoundID.NPCDeath33;
            NPC.value = 250;
            NPC.knockBackResist = 0.25f;
            NPC.lavaImmune = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,
                new FlavorTextBestiaryInfoElement("This suspicious cloud used to be a regular angry nimbus, but it seems the terror rain has given it a darker form.")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.ThunderShard>(), 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Ranged.Thrown.ThunderGrenade>(), 8));
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = new SpriteEffects();
            if (NPC.direction == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 drawOrigin = new Vector2(NPC.width / 2, NPC.height / 2);
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                Vector2 drawPos = NPC.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY) + new Vector2(0, 4);
                Color color = NPC.GetAlpha(Color.White) * ((float)(NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/TerrorRain/VentedCumulus_Glow"), drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
            }
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter--;
            if (NPC.frameCounter <= 0)
            {
                frame++;
                NPC.frameCounter = 3;
            }
            if (frame >= 5)
            {
                frame = 0;
            }
            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (TerrorbornSystem.terrorRain && Main.raining && spawnInfo.Player.ZoneRain)
            {
                return 0.35f;
            }
            else
            {
                return 0f;
            }
        }

        int ProjectileWait = 5;
        NPC targetNPC;
        public override void AI()
        {
            NPC.TargetClosest(true);
            NPC.spriteDirection = NPC.direction;
            Player player = Main.player[NPC.target];
            if (NPC.ai[0] == 0)
            {
                NPC.velocity *= 0.98f;

                Vector2 target = player.Center + new Vector2(0, -200);
                float speed = 0.3f;
                Vector2 velocityIncrease = speed * NPC.DirectionTo(target);
                NPC.velocity += velocityIncrease;

                NPC.ai[1]++;
                if (NPC.ai[1] > 420)
                {
                    NPC.ai[1] = 0;
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<VentedCumulusRaincloud>(), 75 / 4, 0);

                    float Distance = 3500; //max distance away
                    bool Targeted = false;
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].Distance(NPC.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].type != ModContent.NPCType<FrightcrawlerBody>() && Main.npc[i].type != ModContent.NPCType<FrightcrawlerTail>() && Main.npc[i].active && Main.npc[i].type != NPC.type)
                        {
                            targetNPC = Main.npc[i];
                            Distance = Main.npc[i].Distance(NPC.Center);
                            Targeted = true;
                        }
                    }
                    if (Targeted)
                    {
                        NPC.ai[0] = 1;
                    }
                }
            }
            if (NPC.ai[0] == 1)
            {
                Vector2 target = new Vector2(targetNPC.Center.X, targetNPC.position.Y - 50);
                target.X -= NPC.width / 2;
                target.Y -= NPC.height / 2;

                NPC.velocity = (target - NPC.position) / 10 + targetNPC.velocity;

                ProjectileWait--;
                if (ProjectileWait <= 0)
                {
                    ProjectileWait = 7;
                    Vector2 position = new Vector2(Main.rand.Next((int)NPC.position.X, (int)NPC.position.X + NPC.width), NPC.position.Y + NPC.height);
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), position, new Vector2(0, 10), ModContent.ProjectileType<Projectiles.VentRain>(), 50 / 4, 0);
                }

                TerrorbornNPC globalNPC = TerrorbornNPC.modNPC(targetNPC);
                globalNPC.CumulusEmpowermentTime = 600;

                NPC.ai[1]++;
                if (NPC.ai[1] > 180)
                {
                    NPC.ai[1] = 0;
                    NPC.ai[0] = 0;
                }
            }
        }
    }

    class VentedCumulusRaincloud : ModProjectile
    {
        int trueTimeLeft = 60 * 15;

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 24;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 10000;
            Projectile.tileCollide = false;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        void FindFrame()
        {
            Projectile.frameCounter--;
            if (Projectile.frameCounter <= 0)
            {
                Projectile.frame++;
                Projectile.frameCounter = 4;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture + "_Glow");
            Vector2 position = Projectile.position - Main.screenPosition;
            //position.Y += 4;
            Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, Projectile.width, Projectile.height), new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(0, 0), SpriteEffects.None, 0);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Venom, 60 * 4);
        }

        int ProjectileWait = 5;
        public override void AI()
        {
            FindFrame();

            ProjectileWait--;
            if (ProjectileWait <= 0)
            {
                ProjectileWait = 7;
                Vector2 position = new Vector2(Main.rand.Next((int)Projectile.position.X, (int)Projectile.position.X + Projectile.width), Projectile.position.Y + Projectile.height);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, new Vector2(0, 10), ModContent.ProjectileType<Projectiles.VentRain>(), Projectile.damage, 0);
            }

            if (trueTimeLeft > 0)
            {
                trueTimeLeft--;
            }
            else
            {
                ProjectileWait = 10;
                Projectile.alpha += 255 / 60;
                if (Projectile.alpha >= 255)
                {
                    Projectile.active = false;
                }
            }
        }
    }
}
