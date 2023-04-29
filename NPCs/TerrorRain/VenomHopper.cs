using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace TerrorbornMod.NPCs.TerrorRain
{
    class VenomHopper : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 1;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.aiStyle = -1;
            NPC.width = 38;
            NPC.height = 30;
            NPC.damage = 65;
            NPC.defense = 15;
            NPC.lifeMax = 450;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 250;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (TerrorbornSystem.terrorRain && Main.raining && spawnInfo.Player.ZoneRain)
            {
                return 0.8f;
            }
            else
            {
                return 0f;
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.ThunderShard>(), 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Equipable.Hooks.VenomTongue>(), 65));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,
                new FlavorTextBestiaryInfoElement("Frogge.")
            });
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = new SpriteEffects();
            if (NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 drawOrigin = new Vector2(NPC.width / 2, NPC.height / 2);
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                Vector2 drawPos = NPC.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY) + new Vector2(0, 4);
                Color color = NPC.GetAlpha(Color.White) * ((float)(NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/TerrorRain/VenomHopper_Glow"), drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
            }
        }

        int frame = 0;
        bool attacking = false;

        public override void FindFrame(int frameHeight)
        {
            if (attacking)
            {
                frame = 2;
            }
            else
            {
                if (NPC.velocity.Y == 0)
                {
                    frame = 0;
                }
                else
                {
                    frame = 1;
                }
            }
            NPC.frame.Y = frame * frameHeight;
        }

        public override void AI()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (Main.player[NPC.target].Center.X > NPC.Center.X)
            {
                NPC.spriteDirection = 1;
            }
            else
            {
                NPC.spriteDirection = -1;
            }

            attacking = false;
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.type == ModContent.ProjectileType<VenomTongueTip>() && proj.ai[0] == NPC.whoAmI && proj.active)
                {
                    attacking = true;
                    break;
                }
            }

            if (!attacking)
            {
                if (NPC.velocity.Y == 0)
                {
                    NPC.velocity.X *= 0.90f;
                    NPC.ai[0]--;
                    if (NPC.ai[0] <= 0)
                    {
                        NPC.ai[0] = 60;
                        NPC.ai[1]--;
                        if (NPC.ai[1] <= 0)
                        {
                            NPC.ai[1] = 3;
                            SoundExtensions.PlaySoundOld(SoundID.Frog, (int)NPC.Center.X, (int)NPC.Center.Y, 0, 1.5f, -0.2f);
                            float speed = 20;
                            Vector2 velocity = NPC.DirectionTo(player.Center) * speed;
                            Vector2 positionOffset = new Vector2(NPC.spriteDirection * 10, 0);
                            int proj = Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center + positionOffset, velocity, ModContent.ProjectileType<VenomTongueTip>(), 75 / 4, 0);
                            Main.projectile[proj].ai[0] = NPC.whoAmI;
                            Main.projectile[proj].hostile = true;
                        }
                        else
                        {
                            NPC.velocity.Y = -10;
                            NPC.velocity.X = 5 * NPC.spriteDirection;
                        }
                    }
                }
                else
                {

                }
            }
        }
    }

    class VenomTongueTip : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = false;
        }

        float speed;
        Vector2 originPoint;
        int timeUntilReturn = 30;

        public override void AI()
        {
            NPC NPC = Main.npc[(int)Projectile.ai[0]];
            originPoint = NPC.Center + new Vector2(NPC.spriteDirection * 10, 0);

            Projectile.active = NPC.active;

            if (timeUntilReturn > 0)
            {
                speed = Projectile.velocity.Length();
                timeUntilReturn--;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            }
            else
            {
                Projectile.velocity = Projectile.DirectionTo(originPoint) * speed;
                if (Projectile.Distance(originPoint) <= speed)
                {
                    Projectile.active = false;
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Venom, 60 * 3);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 center = Projectile.Center;
            Vector2 distToProj = originPoint - Projectile.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/TerrorRain/VenomTongueSegment");

            while (distance > texture.Height && !float.IsNaN(distance))
            {
                distToProj.Normalize();
                distToProj *= texture.Height;
                center += distToProj;
                distToProj = originPoint - center;
                distance = distToProj.Length();


                //Draw chain
                Main.spriteBatch.Draw(texture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, texture.Width, texture.Height), Color.White, projRotation,
                    new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }

            Texture2D texture2 = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 position = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture2, new Rectangle((int)position.X, (int)position.Y, texture2.Width, texture2.Height), new Rectangle(0, 0, texture2.Width, texture2.Height), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(texture2.Width / 2, texture2.Height / 2), SpriteEffects.None, 0);
            return false;
        }
    }
}
