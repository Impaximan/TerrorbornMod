using System.IO;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using Terraria.Audio;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.NPCs
{
    class MaliciousFace : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 2;
        }
        public override void SetDefaults()
        {
            npc.width = 56;
            npc.height = 54;
            npc.damage = 100;
            npc.defense = 70;
            npc.lifeMax = 7000;
            npc.value *= 2;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.HitSound = new LegacySoundStyle(SoundID.Tink, 0);
            npc.aiStyle = -1;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.TorturedEssence>());
        }

        public override void FindFrame(int frameHeight)
        {
            if (enraged) npc.frame.Y = frameHeight;
            else npc.frame.Y = 0;
        }

        List<Vector2> legs = new List<Vector2>();
        List<Tuple<int, Vector2>> movingLegs = new List<Tuple<int, Vector2>>();
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            foreach (Vector2 leg in legs)
            {
                Utils.DrawLine(spriteBatch, npc.Center, leg, Color.White * 0.25f, Color.White * 0.15f, 5);
                Utils.DrawLine(spriteBatch, leg, leg.findGroundUnder(), Color.White * 0.15f, Color.Transparent, 5);
            }
            return base.PreDraw(spriteBatch, drawColor);
        }

        bool enraged = false;
        int attackCounter1 = 0;
        bool firing = false;
        bool start = true;
        public override void AI()
        {
            if (start)
            {
                start = false;
                for (int i = 0; i < 5; i++)
                {
                    legs.Add(new Vector2(npc.Center.X + Main.rand.NextFloat(-100f, 100f), npc.Center.Y + Main.rand.NextFloat(-100f, 100f)));
                }
            }

            for (int i = 0; i < legs.Count; i++)
            {
                Vector2 leg = legs[i];
                bool canMove = true;
                foreach (Tuple<int, Vector2> tuple in movingLegs)
                {
                    if (tuple.Item1 == i)
                    {
                        canMove = false;
                    }
                }
                if (npc.Distance(leg) > 200 && canMove)
                {
                    movingLegs.Add(new Tuple<int, Vector2>(i, leg.RotatedBy(MathHelper.ToRadians(Main.rand.Next(160, 200)), npc.Center)));
                }
            }

            for (int i = 0; i < movingLegs.Count; i++)
            {
                if (i >= movingLegs.Count)
                {
                    break;
                }
                Tuple<int, Vector2> tuple = movingLegs[i];
                legs[tuple.Item1] = Vector2.Lerp(legs[tuple.Item1], tuple.Item2, 0.15f);
                if (Vector2.Distance(legs[tuple.Item1], tuple.Item2) <= 15)
                {
                    movingLegs.Remove(tuple);
                }
            }

            Player player = Main.LocalPlayer;
            Vector2 groundPosition = npc.Center.findGroundUnder();
            if (npc.life <= npc.lifeMax / 2 && Main.expertMode)
            {
                enraged = true;
            }
            npc.spriteDirection = Math.Sign(player.Center.X - npc.Center.X);
            if (npc.spriteDirection == 1)
            {
                npc.rotation = npc.DirectionTo(player.Center).ToRotation();
            }
            else
            {
                npc.rotation = npc.DirectionTo(player.Center).ToRotation() - MathHelper.ToRadians(180);
            }

            if (firing)
            {
                attackCounter1++;
                if (attackCounter1 % 5 == 4)
                {
                    Projectile.NewProjectile(npc.Center, npc.DirectionTo(player.Center) * 20f, ModContent.ProjectileType<Projectiles.HellbornLaser>(), 80 / 4, 0f);
                    Main.PlaySound(SoundID.Item33, npc.Center);
                }
                if (attackCounter1 >= 30)
                {
                    firing = !firing;
                    attackCounter1 = 0;
                }
            }
            else
            {
                attackCounter1++;
                if (attackCounter1 >= MathHelper.Lerp(60, 120, (float)npc.life / (float)npc.lifeMax))
                {
                    firing = !firing;
                    attackCounter1 = 0;
                }
            }

            float speed = 0.15f;
            if (enraged)
            {
                speed *= 2;
            }
            int yDirection = Math.Sign(player.Center.Y - npc.Center.Y);
            int maxGroundDistance = 100;
            if (enraged)
            {
                maxGroundDistance *= 3;
            }
            if (npc.Distance(groundPosition) > maxGroundDistance)
            {
                yDirection = 1;
            }
            npc.velocity.Y += speed * yDirection;

            int xDirection = Math.Sign(player.Center.X - npc.Center.X);
            npc.velocity.X += speed * xDirection;
            npc.velocity *= 0.95f;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!NPC.downedMoonlord)
            {
                return 0f;
            }
            return SpawnCondition.Underworld.Chance * 0.15f;
        }
    }
}