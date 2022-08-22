using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using TerrorbornMod.Utils;

namespace TerrorbornMod.NPCs
{
    class MaliciousFace : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }
        public override void SetDefaults()
        {
            NPC.width = 56;
            NPC.height = 54;
            NPC.damage = 100;
            NPC.defense = 70;
            NPC.lifeMax = 7000;
            NPC.value *= 2;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.Tink;
            NPC.aiStyle = -1;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("These strange masses of stone are demons set on guarding the underworld. Be careful around them, though, because they might just have to ULTRAKILL you.")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.TorturedEssence>()));
        }

        public override void FindFrame(int frameHeight)
        {
            if (enraged) NPC.frame.Y = frameHeight;
            else NPC.frame.Y = 0;
        }

        List<Vector2> legs = new List<Vector2>();
        List<Tuple<int, Vector2>> movingLegs = new List<Tuple<int, Vector2>>();
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            foreach (Vector2 leg in legs)
            {
                Terraria.Utils.DrawLine(spriteBatch, NPC.Center, leg, Color.White * 0.25f, Color.White * 0.15f, 5);
                Terraria.Utils.DrawLine(spriteBatch, leg, leg.FindGroundUnder(), Color.White * 0.15f, Color.Transparent, 5);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
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
                    legs.Add(new Vector2(NPC.Center.X + Main.rand.NextFloat(-100f, 100f), NPC.Center.Y + Main.rand.NextFloat(-100f, 100f)));
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
                if (NPC.Distance(leg) > 200 && canMove)
                {
                    movingLegs.Add(new Tuple<int, Vector2>(i, leg.RotatedBy(MathHelper.ToRadians(Main.rand.Next(160, 200)), NPC.Center)));
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
            Vector2 groundPosition = NPC.Center.FindGroundUnder();
            if (NPC.life <= NPC.lifeMax / 2 && Main.expertMode)
            {
                enraged = true;
            }
            NPC.spriteDirection = Math.Sign(player.Center.X - NPC.Center.X);
            if (NPC.spriteDirection == 1)
            {
                NPC.rotation = NPC.DirectionTo(player.Center).ToRotation();
            }
            else
            {
                NPC.rotation = NPC.DirectionTo(player.Center).ToRotation() - MathHelper.ToRadians(180);
            }

            if (firing)
            {
                attackCounter1++;
                if (attackCounter1 % 5 == 4)
                {
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, NPC.DirectionTo(player.Center) * 20f, ModContent.ProjectileType<Projectiles.HellbornLaser>(), 80 / 4, 0f);
                    SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
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
                if (attackCounter1 >= MathHelper.Lerp(60, 120, (float)NPC.life / (float)NPC.lifeMax))
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
            int yDirection = Math.Sign(player.Center.Y - NPC.Center.Y);
            int maxGroundDistance = 100;
            if (enraged)
            {
                maxGroundDistance *= 3;
            }
            if (NPC.Distance(groundPosition) > maxGroundDistance)
            {
                yDirection = 1;
            }
            NPC.velocity.Y += speed * yDirection;

            int xDirection = Math.Sign(player.Center.X - NPC.Center.X);
            NPC.velocity.X += speed * xDirection;
            NPC.velocity *= 0.95f;
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