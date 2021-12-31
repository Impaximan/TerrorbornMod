using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using System.Collections.Generic;
using System;
using TerrorbornMod.UI.TitleCard;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod
{
    public class TerrorbornNPC : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public float ogKnockbackResist;
        public int soulSplitTime = 0;
        public int soulOrbCooldown = 0;
        public int mindSpearheadTime = 0;
        public bool extraWormSegment = false;
        public int CumulusEmpowermentTime = 0;
        public int SoulEaterTotalDamageTaken;
        public int SoulReaperTotalDamageTaken;

        public int fighter_TargetPlayerCounter = 0;
        public int fighter_StillTime = 0;
        public int fighter_JumpCooldown = 0;

        public List<float> gloveDoT = new List<float>();
        public int gloveTime = 0;

        public string BossTitle = "";
        public string BossSubtitle = "";
        public Color BossTitleColor = Color.White;
        public bool getsTitleCard = false;

        public void ImprovedFighterAI(NPC npc, float maxSpeed, float accelleration, float decelleration, float jumpSpeed, bool faceDirection = true, int jumpCooldown = 0, int stillTimeUntilTurnaround = 120, int wanderTime = 90)
        {
            Player player = Main.player[npc.target];

            if (fighter_TargetPlayerCounter > 0)
            {
                fighter_TargetPlayerCounter--;
            }
            else
            {
                npc.TargetClosest(true);
            }

            if (Math.Abs(npc.velocity.X) < maxSpeed - accelleration)
            {
                fighter_StillTime++;
                if (fighter_StillTime > stillTimeUntilTurnaround)
                {
                    fighter_TargetPlayerCounter = wanderTime;
                    npc.direction *= -1;
                    fighter_StillTime = 0;
                }
            }
            else
            {
                fighter_StillTime = 0;
            }

            if (npc.direction == 1 && npc.velocity.X < maxSpeed)
            {
                npc.velocity.X += accelleration;
            }

            if (npc.direction == -1 && npc.velocity.X > -maxSpeed)
            {
                npc.velocity.X -= accelleration;
            }

            if (npc.velocity.Y == 0)
            {
                if (fighter_JumpCooldown > 0)
                {
                    fighter_JumpCooldown--;
                }
                else if (!Collision.SolidCollision(npc.position + new Vector2(npc.width * npc.direction, npc.height), npc.width, 17) || Collision.SolidCollision(npc.position + new Vector2(npc.width * npc.direction, 0), npc.width, npc.height) || (MathHelper.Distance(player.Center.X, npc.Center.X) < npc.width && player.position.Y + player.width < npc.position.Y))
                {
                    if (player.Center.Y < npc.position.Y + npc.height || Math.Abs(player.Center.X - npc.Center.X) > 150)
                    {
                        npc.velocity.Y -= jumpSpeed;
                        fighter_JumpCooldown = jumpCooldown;
                    }
                }
            }

            if (faceDirection)
            {
                npc.spriteDirection = npc.direction;
            }
        }

        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            TerrorbornPlayer player = TerrorbornPlayer.modPlayer(target);
            if (player.iFrames > 0 || player.VoidBlinkTime > 0)
            {
                return false;
            }
            return base.CanHitPlayer(npc, target, ref cooldownSlot);
        }

        public override bool PreAI(NPC npc)
        {
            Player player = Main.LocalPlayer;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.TimeFreezeTime > 0)
            {
                npc.position -= npc.velocity;
                return false;
            }
            return base.PreAI(npc);
        }

        public override void SetDefaults(NPC npc)
        {
            ogKnockbackResist = npc.knockBackResist;

            if (npc.type == NPCID.KingSlime)
            {
                BossTitle = "King Slime";
                BossSubtitle = "Gelatinous Monarch";
                BossTitleColor = Color.SkyBlue;
            }

            if (npc.type == NPCID.EyeofCthulhu)
            {
                BossTitle = "Eye of Cthulhu";
                BossSubtitle = "Bloodshot Watcher";
                BossTitleColor = new Color(255, 123, 123);
            }

            if (npc.type == NPCID.EaterofWorldsHead && !NPC.AnyNPCs(NPCID.EaterofWorldsBody) && !NPC.AnyNPCs(NPCID.EaterofWorldsTail))
            {
                getsTitleCard = true;
                BossTitle = "Eater of Worlds";
                BossSubtitle = "Tunneler of Decay";
                BossTitleColor = Color.Purple;
            }

            if (npc.type == NPCID.BrainofCthulhu)
            {
                BossTitle = "The Brain of Cthulhu";
                BossSubtitle = "Bloodthirsty Mastermind";
                BossTitleColor = Color.Crimson;
            }

            if (npc.type == NPCID.QueenBee)
            {
                BossTitle = "The Queen Bee";
                BossSubtitle = "Protector of the hive";
                BossTitleColor = Color.Yellow;
            }

            if (npc.type == NPCID.SkeletronHead)
            {
                BossTitle = "Skeletron";
                BossSubtitle = "Accursed Guardian of the Dungeon";
                BossTitleColor = Color.Beige;
            }

            if (npc.type == NPCID.WallofFlesh)
            {
                BossTitle = "The Wall of Flesh";
                BossSubtitle = "The Seal of Terror; Guardian of the Underworld";
                BossTitleColor = Color.Red;
            }

            if (npc.type == NPCID.SkeletronPrime)
            {
                BossTitle = "Skeletron Prime";
                BossSubtitle = "Construct of Fright";
                BossTitleColor = Color.OrangeRed;
            }

            if (npc.type == NPCID.TheDestroyer)
            {
                BossTitle = "The Destroyer";
                BossSubtitle = "Construct of Might";
                BossTitleColor = Color.RoyalBlue;
            }

            if (npc.type == NPCID.Spazmatism || npc.type == NPCID.Retinazer)
            {
                BossTitle = "The Twins";
                BossSubtitle = "Constructs of Sight";
                BossTitleColor = Color.LightGreen;
            }

            if (npc.type == NPCID.Plantera)
            {
                BossTitle = "Plantera";
                BossSubtitle = "Southern Plantkill";
                BossTitleColor = Color.LimeGreen;
            }

            if (npc.type == NPCID.Golem)
            {
                BossTitle = "Golem";
                BossSubtitle = "Protector of the Lihzahrd Tribe";
                BossTitleColor = Color.SaddleBrown;
            }

            if (npc.type == NPCID.CultistBoss)
            {
                BossTitle = "Lunatic Cultist";
                BossSubtitle = "Messenger of Armaggeddon";
                BossTitleColor = Color.Blue;
            }

            if (npc.type == NPCID.MoonLordCore || npc.type == NPCID.MoonLordHand || npc.type == NPCID.MoonLordHead)
            {
                BossTitle = "The Moon Lord";
                BossSubtitle = "Monarch of the Pillars";
                BossTitleColor = Color.PaleTurquoise;
            }

            if (npc.type == NPCID.DD2DarkMageT1)
            {
                getsTitleCard = true;
                BossTitle = "The Dark Mage";
                BossSubtitle = "Commander of the Dead";
            }

            if (npc.type == NPCID.DD2OgreT2)
            {
                getsTitleCard = true;
                BossTitle = "Shrek (not actually)";
                BossSubtitle = "Ogre of the Ages";
            }

            if (npc.type == NPCID.DD2Betsy)
            {
                getsTitleCard = true;
                BossTitle = "Betsy";
                BossSubtitle = "Leader of the Old Ones";
                BossTitleColor = Color.OrangeRed;
            }

            if (npc.type == NPCID.MartianSaucer)
            {
                getsTitleCard = true;
                BossTitle = "Martian Saucer";
                BossSubtitle = "Otherwordly Battleship";
            }

            if (npc.type == NPCID.PirateShip)
            {
                getsTitleCard = true;
                BossTitle = "Flying Dutchman";
                BossSubtitle = "Ghostly Transport";
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.TravellingMerchant)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Equipable.Accessories.HermesFeather>());
                nextSlot++;
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.LiesOfNourishment && npc.life >= npc.lifeMax)
            {
                crit = true;
            }

            if (CumulusEmpowermentTime > 0)
            {
                damage = (int)(damage * 0.5f);
                knockback = 0;
            }

            if (crit)
            {
                damage = (int)(damage * modPlayer.critDamage);
            }

            if (modPlayer.TimeFreezeTime > 0)
            {
                knockback = 0f;
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.LiesOfNourishment && npc.life >= npc.lifeMax)
            {
                crit = true;
            }

            if (CumulusEmpowermentTime > 0)
            {
                damage = (int)(damage * 0.5f);
                knockback = 0;
            }

            if (crit)
            {
                damage = (int)(damage * modPlayer.critDamage);
            }

            if (modPlayer.TimeFreezeTime > 0)
            {
                knockback = 0f;
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (gloveTime > 0)
            {
                gloveTime--;
            }
            else
            {
                gloveDoT.Clear();
            }

            if (gloveDoT.Count > 0)
            {
                float gloveDamage = 1f;
                float gloveDoTTotal = 0f;

                foreach (float dot in gloveDoT)
                {
                    gloveDamage += dot / 10f;
                    gloveDoTTotal += dot;
                }

                damage += (int)gloveDamage;
                npc.lifeRegen -= (int)gloveDoTTotal;
            }
        }

        public override void PostAI(NPC npc)
        {
            if (soulSplitTime > 0)
            {
                soulSplitTime--;
                if (soulSplitTime <= 0)
                {
                    CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), Color.LightCyan, "Soul Regained");
                    Main.PlaySound(SoundID.NPCDeath39, npc.Center);
                }
            }

            if (soulOrbCooldown > 0)
            {
                soulOrbCooldown--;
            }

            if (mindSpearheadTime > 0)
            {
                mindSpearheadTime--;
                if (mindSpearheadTime <= 0)
                {
                    CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), new Color(80, 112, 109), "Spearhead Dropped!", false, true);
                }
            }

            if (CumulusEmpowermentTime > 0)
            {
                CumulusEmpowermentTime--;
                int d = Dust.NewDust(npc.position, npc.width, npc.height, 21);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = npc.velocity;
            }
        }

        public static TerrorbornNPC modNPC(NPC npc)
        {
            return npc.GetGlobalNPC<TerrorbornNPC>();
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.ZoneRain && TerrorbornWorld.terrorRain && maxSpawns != 0) //Checks current maxSpawns specifically so it works with HERO's mod's spawn thingy
            {
                if (Main.dayTime)
                {
                    spawnRate = (int)(spawnRate * 0.35f);
                }
                else
                {
                    spawnRate = (int)(spawnRate * 0.55f);
                }
                //spawnRate = 180;
                maxSpawns = (int)(maxSpawns * 2f);
            }

            if (player.HasBuff(ModContent.BuffType<Buffs.Debuffs.IncendiaryCurse>()))
            {
                spawnRate = (int)(spawnRate * 0.35f);
                maxSpawns = (int)(maxSpawns * 2f);
            }

            //maxSpawns += TerrorbornWorld.wormExtraSegmentCount;
        }

        List<int> terrorRainEnemies = new List<int>()
        {
            { ModContent.NPCType<NPCs.TerrorRain.VentedCumulus>() },
            { ModContent.NPCType<NPCs.TerrorRain.VenomHopper>() },
            { ModContent.NPCType<NPCs.TerrorRain.BrainStinger>() },
            { ModContent.NPCType<NPCs.TerrorRain.DownpourGecko>() },
        };

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            Player player = spawnInfo.player;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (player.ZoneRain && TerrorbornWorld.terrorRain)
            {
                pool.Clear();

                foreach (int i in terrorRainEnemies)
                {
                    float weight = 1f;
                    if (i == ModContent.NPCType<NPCs.TerrorRain.VentedCumulus>())
                    {
                        weight = 0.3f;
                    }
                    if (i == ModContent.NPCType<NPCs.TerrorRain.BrainStinger>())
                    {
                        weight = 0.8f;
                    }
                    //if (i == ModContent.NPCType<NPCs.TerrorRain.FrightcrawlerHead>())
                    //{
                    //    if (NPC.AnyNPCs(ModContent.NPCType<NPCs.TerrorRain.FrightcrawlerHead>()))
                    //    {
                    //        weight = 0f;
                    //    }
                    //    else
                    //    {
                    //        weight = 0.07f;
                    //    }
                    //}
                    if (i == ModContent.NPCType<NPCs.TerrorRain.DownpourGecko>())
                    {
                        weight = 0.6f;
                    }
                    pool.Add(i, weight);
                }
            }

            if (modPlayer.ZoneIncendiary)
            {
                while (pool.ContainsKey(0))
                {
                    pool.Remove(0);
                }
            }
        }

        public bool CheckBountyBiome(Player player)
        {
            if (TerrorbornWorld.CurrentBountyBiome == 0 && player.ZoneUndergroundDesert)
            {
                return true;
            }
            if (TerrorbornWorld.CurrentBountyBiome == 1 && player.ZoneSnow && player.ZoneRockLayerHeight)
            {
                return true;
            }
            if (TerrorbornWorld.CurrentBountyBiome == 2 && player.ZoneUnderworldHeight)
            {
                return true;
            }
            if (TerrorbornWorld.CurrentBountyBiome == 3 && player.ZoneJungle && !player.ZoneRockLayerHeight)
            {
                return true;
            }
            if (TerrorbornWorld.CurrentBountyBiome == 4 && player.ZoneJungle && player.ZoneRockLayerHeight)
            {
                return true;
            }
            if (TerrorbornWorld.CurrentBountyBiome == 5 && (player.ZoneCorrupt || player.ZoneCrimson) && !player.ZoneRockLayerHeight)
            {
                return true;
            }
            if (TerrorbornWorld.CurrentBountyBiome == 6 && player.ZoneSnow)
            {
                return true;
            }
            if (TerrorbornWorld.CurrentBountyBiome == 7 && player.ZoneHoly && !player.ZoneRockLayerHeight)
            {
                return true;
            }
            if (TerrorbornWorld.CurrentBountyBiome == 8 && player.ZoneHoly && player.ZoneRockLayerHeight)
            {
                return true;
            }
            if (TerrorbornWorld.CurrentBountyBiome == 9 && (player.ZoneCorrupt || player.ZoneCrimson) && player.ZoneRockLayerHeight)
            {
                return true;
            }
            return false;
        }

        void SpawnGeyser(int originalDamage, NPC npc, Player player)
        {
            if (Main.rand.NextFloat() > 0.15f && npc.life > 0)
            {
                return;
            }
            for (int i = 0; i < Main.rand.Next(2, 5); i++)
            {
                Vector2 position = npc.Center;
                if (i != 1)
                {
                    position.X += Main.rand.Next(-200, 200);
                }
                while (!WorldUtils.Find(position.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                    {
        new Conditions.IsSolid()
                    }), out _))
                {
                    position.Y++;
                }
                Projectile.NewProjectile(position, new Vector2(0, -20), ModContent.ProjectileType<Items.Equipable.Armor.TideFireFriendly>(), originalDamage / 2, 0f, player.whoAmI);
            }
            Main.PlaySound(SoundID.Item88, npc.Center);
        }

        void SpawnAzuriteShard(int originalDamage, NPC npc, Player player)
        {
            if (Main.rand.NextFloat() > 0.25f)
            {
                return;
            }
            Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
            float speed = Main.rand.Next(15, 25);
            Projectile.NewProjectile(npc.Center, direction * speed, ModContent.ProjectileType<Projectiles.AzuriteShard>(), originalDamage / 2, 0f, player.whoAmI);
            Main.PlaySound(SoundID.Item118, npc.Center);
        }

        public void SinducementExplosion(NPC npc, int damage, bool death = true)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                float speed = Main.rand.NextFloat(10f, 20f);
                int projDamage = damage / 2;
                if (death && !npc.boss)
                {
                    projDamage = npc.lifeMax / 3;
                }
                Projectile.NewProjectile(npc.Center, direction * speed, ModContent.ProjectileType<Projectiles.VeinBurst>(), projDamage, 0.5f, Main.LocalPlayer.whoAmI);
            }

            if (death)
            {
                Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                float speed = Main.rand.NextFloat(10f, 20f);
                int projDamage = damage / 2;
                Projectile.NewProjectile(npc.Center, direction * speed, ModContent.ProjectileType<SinducementSoul>(), projDamage, 0.5f, Main.LocalPlayer.whoAmI);
            }
        }

        public override void HitEffect(NPC npc, int hitDirection, double damage)
        {
            Player player = Main.LocalPlayer;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            fighter_TargetPlayerCounter = 0;

            if (npc.life <= 0 && !npc.SpawnedFromStatue)
            {
                if (modPlayer.SoulEater)
                {
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<TerrorOrb>(), 0, 0, player.whoAmI);
                }
            }

            if (modPlayer.SoulEater && npc.boss)
            {
                SoulEaterTotalDamageTaken += (int)damage;
                while (SoulEaterTotalDamageTaken > npc.lifeMax * 0.075f)
                {
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<TerrorOrb>(), 0, 0, player.whoAmI);
                    SoulEaterTotalDamageTaken -= (int)(npc.lifeMax * 0.075f);
                }
            }

            if (modPlayer.SoulReaperArmorBonus && npc.boss)
            {
                SoulReaperTotalDamageTaken += (int)damage;
                while (SoulReaperTotalDamageTaken > npc.lifeMax * 0.075f)
                {
                    Item.NewItem(npc.Center, ModContent.ItemType<Items.Equipable.Armor.ThunderSoul>());
                    SoulReaperTotalDamageTaken -= (int)(npc.lifeMax * 0.075f);
                }
            }

            if (modPlayer.BanditGlove)
            {
                gloveDoT.Add((float)(damage * 0.05f));
                if (gloveTime == 0)
                {
                    gloveTime = 60 * 5;
                }
            }
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            if (soulSplitTime > 0)
            {
                if (soulOrbCooldown <= 0)
                {
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<SoulOrb>(), 0, 0, player.whoAmI);
                    Main.PlaySound(SoundID.NPCHit36, npc.Center);
                    soulOrbCooldown = 3;
                }
            }

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (item.melee && modPlayer.TidalShellArmorBonus)
            {
                SpawnGeyser(damage, npc, player);
            }

            if (modPlayer.AzuriteBrooch)
            {
                SpawnAzuriteShard(damage, npc, player);
            }

            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.TownNPCs.SkeletonSheriff>()))
            {
                if (npc.life <= 0)
                {
                    if (CheckBountyBiome(player))
                    {
                        modPlayer.CombatPoints += npc.lifeMax;
                    }
                }
            }

            if (item.melee && modPlayer.IncendiaryShield)
            {
                if (Main.rand.Next(101) <= 8 + player.meleeCrit / 2)
                {
                    DustExplosion(npc.Center, 0, 45, 30, DustID.Fire, DustScale: 1f, NoGravity: true);
                    Main.PlaySound(SoundID.Item14, npc.Center);
                    for (int i = 0; i < 200; i++)
                    {
                        NPC target = Main.npc[i];
                        if (!target.friendly && npc.Distance(target.Center) <= 200 + (target.width + target.height) / 2 && !target.dontTakeDamage)
                        {
                            if (target.type == NPCID.TheDestroyerBody)
                            {
                                target.StrikeNPC(damage / 10, 0, 0, Main.rand.Next(101) <= player.meleeCrit);
                            }
                            else
                            {
                                target.StrikeNPC(damage / 2, 0, 0, Main.rand.Next(101) <= player.meleeCrit);
                            }

                            int choice = Main.rand.Next(4);
                            if (choice == 0)
                            {
                                target.AddBuff(BuffID.OnFire, 60 * 5);
                            }
                            if (choice == 1)
                            {
                                target.AddBuff(BuffID.Frostburn, 60 * 5);
                            }
                            if (choice == 2)
                            {
                                target.AddBuff(BuffID.CursedInferno, 60 * 5);
                            }
                            if (choice == 3)
                            {
                                target.AddBuff(BuffID.ShadowFlame, 60 * 5);
                            }
                        }
                    }
                }
            }

            if (modPlayer.HeadHunter)
            {
                if (modPlayer.HeadHunterCritCooldown <= 0)
                {
                    if ((modPlayer.HeadhunterClass == 0 && item.magic) || 
                        (modPlayer.HeadhunterClass == 1 && item.melee) ||
                        (modPlayer.HeadhunterClass == 2 && item.ranged) ||
                        (modPlayer.HeadhunterClass == 3))
                    {
                        modPlayer.HeadHunterCritCooldown = 60;
                        modPlayer.HeadHunterCritBonus++;

                        CombatText.NewText(player.getRect(), Color.Red, modPlayer.HeadHunterCritBonus + "/30", false, true);

                        if (modPlayer.HeadHunterCritBonus >= 30)
                        {
                            player.AddBuff(ModContent.BuffType<Items.Equipable.Armor.HeadhunterFrenzy>(), 60 * 4);
                            modPlayer.HeadHunterCritBonus = 0;

                            int healingAmount = 40;
                            player.HealEffect(healingAmount);
                            player.statLife += healingAmount;
                        }
                    }
                }
            }
        }

        bool start = true;
        public override void AI(NPC npc)
        {
            if (start)
            {
                start = false;
                ogKnockbackResist = npc.knockBackResist;

                if (getsTitleCard || npc.boss)
                {
                    TitleCardUI.bossName = BossTitle;
                    TitleCardUI.bossSubtitle = BossSubtitle;
                    TitleCardUI.titleColor = BossTitleColor;
                    TitleCardUI.titleCardLifetimeCounter = (int)(60 * TerrorbornMod.titleCardDuration);
                }
            }
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(Main.player[projectile.owner]);
            Player player = Main.player[projectile.owner];

            if (projectile.melee && modPlayer.TidalShellArmorBonus)
            {
                SpawnGeyser(damage, npc, player);
            }

            if (modPlayer.PrismalCore && Main.rand.NextFloat() <= 0.15f && projectile.magic)
            {
                npc.StrikeNPC(damage, 0, 0, crit);
            }

            if (npc.life <= 0 && !npc.SpawnedFromStatue)
            {
                if (player.HasBuff(ModContent.BuffType<Buffs.Sinducement>()))
                {
                    SinducementExplosion(npc, (int)damage, true);
                    TerrorbornMod.ScreenShake(2.5f);
                    Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode, npc.Center);
                }
            }

            if (modPlayer.AzuriteBrooch)
            {
                SpawnAzuriteShard(damage, npc, player);
            }

            if (soulSplitTime > 0)
            {
                if (soulOrbCooldown <= 0)
                {
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<SoulOrb>(), 0, 0, player.whoAmI);
                    Main.PlaySound(SoundID.NPCHit36, npc.Center);
                    soulOrbCooldown = 3;
                }
            }

            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.TownNPCs.SkeletonSheriff>()))
            {
                if (npc.life <= 0)
                {
                    if (CheckBountyBiome(Main.player[projectile.owner]))
                    {
                        modPlayer.CombatPoints += npc.lifeMax;
                    }
                }
            }

            if (projectile.melee && modPlayer.IncendiaryShield)
            {
                if (Main.rand.Next(101) <= 8 + player.meleeCrit / 2)
                {
                    DustExplosion(npc.Center, 0, 45, 30, DustID.Fire, DustScale: 1f, NoGravity: true);
                    Main.PlaySound(SoundID.Item14, npc.Center);
                    for (int i = 0; i < 200; i++)
                    {
                        NPC target = Main.npc[i];
                        if (!target.friendly && npc.Distance(target.Center) <= 200 + (target.width + target.height) / 2 && !target.dontTakeDamage)
                        {
                            if (target.type == NPCID.TheDestroyerBody)
                            {
                                target.StrikeNPC(damage / 10, 0, 0, Main.rand.Next(101) <= player.meleeCrit);
                            }
                            else
                            {
                                target.StrikeNPC(damage / 2, 0, 0, Main.rand.Next(101) <= player.meleeCrit);
                            }

                            int choice = Main.rand.Next(4);
                            if (choice == 0)
                            {
                                target.AddBuff(BuffID.OnFire, 60 * 5);
                            }
                            if (choice == 1)
                            {
                                target.AddBuff(BuffID.Frostburn, 60 * 5);
                            }
                            if (choice == 2)
                            {
                                target.AddBuff(BuffID.CursedInferno, 60 * 5);
                            }
                            if (choice == 3)
                            {
                                target.AddBuff(BuffID.ShadowFlame, 60 * 5);
                            }
                        }
                    }
                }
            }



            if (modPlayer.HeadHunter)
            {
                if (modPlayer.HeadHunterCritCooldown <= 0)
                {
                    if ((modPlayer.HeadhunterClass == 0 && projectile.magic) ||
                        (modPlayer.HeadhunterClass == 1 && projectile.melee) ||
                        (modPlayer.HeadhunterClass == 2 && projectile.ranged) ||
                        (modPlayer.HeadhunterClass == 3))
                    {
                        modPlayer.HeadHunterCritCooldown = 60;
                        modPlayer.HeadHunterCritBonus++;

                        CombatText.NewText(player.getRect(), Color.Red, modPlayer.HeadHunterCritBonus + "/30", false, true);

                        if (modPlayer.HeadHunterCritBonus >= 30)
                        {
                            player.AddBuff(ModContent.BuffType<Items.Equipable.Armor.HeadhunterFrenzy>(), 60 * 4);
                            modPlayer.HeadHunterCritBonus = 0;

                            int healingAmount = 40;
                            player.HealEffect(healingAmount);
                            player.statLife += healingAmount;
                        }
                    }
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
        public override void NPCLoot(NPC npc)
        {
            Player player = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            List<int> extraDarkEnergyIDs = new List<int>();
            extraDarkEnergyIDs.Add(NPCID.SkeletronHand);
            extraDarkEnergyIDs.Add(NPCID.GolemFistLeft);
            extraDarkEnergyIDs.Add(NPCID.GolemFistRight);
            extraDarkEnergyIDs.Add(NPCID.Paladin);
            extraDarkEnergyIDs.Add(NPCID.PirateCaptain);
            extraDarkEnergyIDs.Add(NPCID.PirateShip);
            extraDarkEnergyIDs.Add(NPCID.MourningWood);
            extraDarkEnergyIDs.Add(NPCID.Pumpking);
            extraDarkEnergyIDs.Add(NPCID.Everscream);
            extraDarkEnergyIDs.Add(NPCID.SantaNK1);
            extraDarkEnergyIDs.Add(NPCID.IceQueen);
            extraDarkEnergyIDs.Add(NPCID.DD2DarkMageT1);
            extraDarkEnergyIDs.Add(NPCID.DD2DarkMageT3);
            extraDarkEnergyIDs.Add(NPCID.DD2OgreT2);
            extraDarkEnergyIDs.Add(NPCID.DD2OgreT3);
            extraDarkEnergyIDs.Add(NPCID.DD2Betsy);
            extraDarkEnergyIDs.Add(NPCID.BigMimicCorruption);
            extraDarkEnergyIDs.Add(NPCID.BigMimicCrimson);
            extraDarkEnergyIDs.Add(NPCID.BigMimicHallow);
            extraDarkEnergyIDs.Add(NPCID.GoblinSummoner);
            extraDarkEnergyIDs.Add(NPCID.IceGolem);
            extraDarkEnergyIDs.Add(ModContent.NPCType<NPCs.UndyingSpirit>());
            extraDarkEnergyIDs.Add(ModContent.NPCType<NPCs.TerrorRain.FrightcrawlerHead>());

            if (modPlayer.SoulReaperArmorBonus)
            {
                Item.NewItem(npc.Center, ModContent.ItemType<Items.Equipable.Armor.ThunderSoul>());
            }

            if (TerrorbornWorld.terrorRain && player.ZoneRain && Main.rand.NextFloat() <= 0.5f)
            {
                int type = ItemID.DirtBlock;
                switch (Main.rand.Next(3))
                {
                    case 0: type = ItemID.SoulofNight;
                        break;
                    case 1: type = ItemID.SoulofLight;
                        break;
                    case 2: type = ItemID.SoulofFlight;
                        break;
                }
                Item.NewItem(npc.getRect(), type);
            }


            if (NPCID.Sets.ProjectileNPC[npc.type])
            {
                return;
            }


            float ExpertBoost = 1;
            if (Main.expertMode)
            {
                ExpertBoost = 2;
            }

            if (player.ZoneDungeon)
            {
                if (Main.rand.NextFloat() <= 0.01f * ExpertBoost)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.MiscConsumables.StrangeBag>());
                }
            }

            if (npc.type == NPCID.LunarTowerSolar || npc.type == NPCID.LunarTowerNebula || npc.type == NPCID.LunarTowerStardust || npc.type == NPCID.LunarTowerVortex)
            {
                for (int i = 0; i < Main.rand.Next(5, 21); i++)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.FusionFragment>(), 1);
                }
            }

            if (modPlayer.ZoneIncendiary)
            {
                if (Main.rand.NextFloat() <= 0.02f)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.IncendiaryLockbox>());
                }

                if (Main.rand.NextFloat() <= 0.02f && NPC.downedGolemBoss)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.SkullmoundLockbox>());
                }

                if (Main.rand.NextFloat() <= 0.015f * ExpertBoost)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.MiscConsumables.HotMilk>());
                }

                if (Main.rand.NextFloat() <= 0.01f * ExpertBoost)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Vanity.IncendiaryBreastplate>());
                }

                if (Main.rand.NextFloat() <= 0.01f * ExpertBoost)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Vanity.IncendiaryLeggings>());
                }

                if (Main.rand.NextFloat() <= 0.01f * ExpertBoost)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Vanity.IncendiaryVisor>());
                }

                if (Main.rand.NextFloat() <= 0.25f && NPC.downedGolemBoss)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Materials.HellbornEssence>());
                }
            }

            if (npc.type == NPCID.GoblinSorcerer)
            {
                if (Main.rand.NextFloat() <= 0.15f * ExpertBoost)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Weapons.Magic.BookOfChaos>());
                }
            }

            if (npc.type == NPCID.GoblinArcher)
            {
                if (Main.rand.NextFloat() <= 0.08f * ExpertBoost)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Weapons.Ranged.WarBow>());
                }
            }

            if (npc.type == NPCID.TacticalSkeleton || npc.type == NPCID.SkeletonCommando || npc.type == NPCID.SkeletonSniper)
            {
                if (Main.rand.NextFloat() <= 0.35f)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Accessories.TacticalCommlink>());
                }
            }

            if (npc.type == NPCID.EyeofCthulhu)
            {
                Item.NewItem(npc.Center, ModContent.ItemType<Items.PermanentUpgrades.EyeOfTheMenace>());
                Item.NewItem(npc.Center, ModContent.ItemType<Items.Weapons.Summons.Minions.OpticCane>());
            }

            if (npc.type == NPCID.KingSlime)
            {
                Item.NewItem(npc.Center, ModContent.ItemType<Items.Equipable.Accessories.BurstJumps.CompressedGelatin>());
            }

            if (npc.type == NPCID.MoonLordCore)
            {
                modPlayer.TerrorPercent = 0f;
            }

            if (extraDarkEnergyIDs.Contains(npc.type) || npc.boss)
            {
                for (int i = 0; i < Main.rand.Next(3, 5); i++)
                {
                    Item.NewItem(npc.Center, ModContent.ItemType<Items.DarkEnergy>());
                }
            }

            if (npc.type == NPCID.WallofFlesh)
            {
                Item.NewItem(npc.Center, ModContent.ItemType<Items.PermanentUpgrades.DemonicLense>());
            }

            if (npc.type == NPCID.Mothron && Main.rand.NextFloat() <= 0.33f)
            {
                Item.NewItem(npc.Center, ModContent.ItemType<Items.Weapons.Summons.Other.Armagrenade>());
            }

            if (npc.type == NPCID.MartianSaucerCore)
            {
                Item.NewItem(npc.Center, ModContent.ItemType<Items.Equipable.Accessories.Wings.MartianBoosters>());
            }

            if (npc.type == NPCID.SkeletronHead)
            {
                Item.NewItem(npc.Center, ModContent.ItemType<Items.PermanentUpgrades.CoreOfFear>());
            }

            if (npc.type == NPCID.GraniteFlyer || npc.type == NPCID.GraniteGolem)
            {
                if (Main.rand.NextFloat(101) <= 0.45 * ExpertBoost)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.graniteVirusSpark>());
                }
            }

            if (npc.type == NPCID.Antlion || npc.type == NPCID.FlyingAntlion || npc.type == NPCID.WalkingAntlion)
            {
                if (Main.rand.NextFloat() <= 0.01f * ExpertBoost)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Accessories.AntsMandible>());
                }
            }

            if (player.ZoneBeach && Main.rand.NextFloat() <= 0.02f)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.LunarRitual>());
            }

            if (npc.type == NPCID.KingSlime)
            {
                bool spawnGA = !TerrorbornPlayer.modPlayer(Main.player[Main.myPlayer]).unlockedAbilities.Contains(6);
                for (int i = 0; i < 1000; i++)
                {
                    Projectile projectile = Main.projectile[i];
                    if (projectile.active && projectile.type == ModContent.ProjectileType<Abilities.GelatinArmor>())
                    {
                        spawnGA = false;
                    }
                }

                if (spawnGA)
                {
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<Abilities.GelatinArmor>(), 0, 0, Main.myPlayer);
                }
            }

            if (modPlayer.LiesOfNourishment && Main.rand.Next(5) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Heart);
            }

            if (Main.rand.Next(12) == 0 && modPlayer.TerrorPercent < 100 && TerrorbornWorld.obtainedShriekOfHorror)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.DarkEnergy>());
            }

            if (player.HasBuff(ModContent.BuffType<Buffs.Vampirism>()) && npc.Distance(player.Center) <= 350)
            {
                if (Main.rand.NextFloat() <= 0.2f)
                {
                    Item.NewItem(npc.getRect(), ItemID.Heart);
                }

                if (Main.rand.NextFloat() <= 0.2f)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Items.DarkEnergy>());
                }
            }
        }
    }


    class SoulOrb : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.hide = true;
            projectile.timeLeft = 300;
            projectile.damage = 0;
        }

        int Direction = 1;
        int DirectionCounter = 5;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            int type = 132;
            if (modPlayer.SanguineSetBonus) type = 130;
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, type);
            Main.dust[dust].velocity = projectile.velocity;
            if (modPlayer.SanguineSetBonus) Main.dust[dust].velocity = projectile.velocity / 4;
            Main.dust[dust].scale = 1f;
            Main.dust[dust].alpha = 255 / 2;
            Main.dust[dust].noGravity = true;
            Main.dust[dust].color = Color.White;


            int speed = 8;
            if (modPlayer.SanguineSetBonus) speed = 25;
            projectile.velocity = projectile.DirectionTo(player.Center) * speed;
            if (projectile.Distance(player.Center) <= speed)
            {
                int healAmount = 1;
                player.HealEffect(healAmount);
                player.statLife += healAmount;
                projectile.active = false;
            }
        }
    }

    class TerrorOrb : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.hide = true;
            projectile.timeLeft = 300;
            projectile.damage = 0;
        }

        int Direction = 1;
        int DirectionCounter = 5;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            int type = 54;
            int dust = Dust.NewDust(projectile.Center, 0, 0, type);
            Main.dust[dust].velocity = Vector2.Zero;
            Main.dust[dust].scale = 1f;
            Main.dust[dust].alpha = 255 / 2;
            Main.dust[dust].noGravity = true;
            Main.dust[dust].color = Color.White;


            int speed = 25;
            projectile.velocity = projectile.DirectionTo(player.Center) * speed;
            if (projectile.Distance(player.Center) <= speed)
            {
                modPlayer.GainTerror(2f, false);
                projectile.active = false;
            }
        }
    }

    class SinducementSoul : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }

        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            projectile.width = 15;
            projectile.height = 15;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.penetrate = 10;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            projectile.hostile = false;
            projectile.extraUpdates = 2;
            projectile.timeLeft = 120 * (projectile.extraUpdates + 1);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            BezierCurve bezier = new BezierCurve();
            bezier.Controls.Clear();
            foreach (Vector2 pos in projectile.oldPos)
            {
                if (pos != Vector2.Zero && pos != null)
                {
                    bezier.Controls.Add(pos);
                }
            }

            if (bezier.Controls.Count > 1)
            {
                List<Vector2> positions = bezier.GetPoints(15);
                for (int i = 0; i < positions.Count; i++)
                {
                    Vector2 drawPos = positions[i] - Main.screenPosition + projectile.Size / 2;
                    Color color = projectile.GetAlpha(Color.LightGray) * ((float)(positions.Count - i) / (float)positions.Count);
                    TBUtils.Graphics.DrawGlow_1(spriteBatch, drawPos, (int)(25f * ((float)(positions.Count - i) / (float)positions.Count)), color);
                }
            }
            return false;
        }

        public override void AI()
        {
            projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(Main.MouseWorld).ToRotation(), MathHelper.ToRadians(5f * (projectile.velocity.Length() / 20))).ToRotationVector2() * projectile.velocity.Length();
        }
    }
}
