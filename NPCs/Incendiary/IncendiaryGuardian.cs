using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.NPCs.Incendiary
{
    class IncendiaryGuardian : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }
        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 48;
            NPC.height = 42;
            NPC.damage = 45;
            NPC.defense = 21;
            NPC.lifeMax = 750;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = 250;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextFloat() <= 0.15f)
            {
                Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.Weapons.Summons.Sentry.GuardianStaff>());
            }
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = frame * frameHeight;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (frame == 4 && fireCounter > 0)
            {
                int laserCount = 4;
                for (int i = 0; i < laserCount; i++)
                {
                    Utils.DrawLine(spriteBatch, NPC.Center + new Vector2(0, -15), NPC.Center + new Vector2(0, -15) + rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(360 / laserCount) * i) * 100, Color.Red, Color.Transparent, 5);
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (TerrorbornPlayer.modPlayer(spawnInfo.player).ZoneIncendiary && !NPC.downedGolemBoss)
            {
                return SpawnCondition.Sky.Chance * 0.05f;
            }
            else
            {
                return 0f;
            }
        }

        float rotation = 0f;
        int fireCounter = 45;
        int soundCounter = 0;
        public override void AI()
        {
            if (NPC.life < NPC.lifeMax)
            {
                if (frame < 4)
                {
                    NPC.frameCounter--;
                    if (NPC.frameCounter <= 0)
                    {
                        frame++;
                        NPC.frameCounter = 10;
                    }
                }
                else
                {
                    fireCounter--;
                    if (fireCounter <= 0)
                    {
                        soundCounter--;
                        if (soundCounter <= 0)
                        {
                            soundCounter = 10;
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)NPC.position.X, (int)NPC.position.Y, Style: 15, 4, 0);
                        }

                        rotation += MathHelper.ToRadians(1);
                        TerrorbornSystem.ScreenShake(2);
                        int laserCount = 4;
                        for (int i = 0; i < laserCount; i++)
                        {
                            Projectile Projectile = Main.projectile[Projectile.NewProjectile(NPC.Center, rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(360 / laserCount) * i), ModContent.ProjectileType<Projectiles.IncendiaryDeathray>(), 80 / 4, 0)];
                            Projectile.ai[0] = NPC.whoAmI;
                            Projectile.ai[1] = -15;
                        }
                    }
                }
            }
        }
    }
}