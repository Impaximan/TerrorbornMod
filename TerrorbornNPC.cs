using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using System.Collections.Generic;
using System;
using TerrorbornMod.UI.TitleCard;
using Terraria.GameContent.ItemDropRules;

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

        public float damageResist = 1f;

        public int currentTagDamageAdditive = 0;
        public float currentTagDamageMultiplicative = 1f;
        public int tagTime = 0;

        public void ImprovedFighterAI(NPC NPC, float maxSpeed, float accelleration, float decelleration, float jumpSpeed, bool faceDirection = true, int jumpCooldown = 0, int stillTimeUntilTurnaround = 120, int wanderTime = 90)
        {
            Player player = Main.player[NPC.target];

            if (fighter_TargetPlayerCounter > 0)
            {
                fighter_TargetPlayerCounter--;
            }
            else
            {
                NPC.TargetClosest(true);
            }

            if (Math.Abs(NPC.velocity.X) < maxSpeed - accelleration)
            {
                fighter_StillTime++;
                if (fighter_StillTime > stillTimeUntilTurnaround)
                {
                    fighter_TargetPlayerCounter = wanderTime;
                    NPC.direction *= -1;
                    fighter_StillTime = 0;
                }
            }
            else
            {
                fighter_StillTime = 0;
            }

            if (NPC.direction == 1 && NPC.velocity.X < maxSpeed)
            {
                NPC.velocity.X += accelleration;
            }

            if (NPC.direction == -1 && NPC.velocity.X > -maxSpeed)
            {
                NPC.velocity.X -= accelleration;
            }

            if (NPC.velocity.Y == 0)
            {
                if (fighter_JumpCooldown > 0)
                {
                    fighter_JumpCooldown--;
                }
                else if (!Collision.SolidCollision(NPC.position + new Vector2(NPC.width * NPC.direction, NPC.height), NPC.width, 17) || Collision.SolidCollision(NPC.position + new Vector2(NPC.width * NPC.direction, 0), NPC.width, NPC.height) || (MathHelper.Distance(player.Center.X, NPC.Center.X) < NPC.width && player.position.Y + player.width < NPC.position.Y))
                {
                    if (player.Center.Y < NPC.position.Y + NPC.height || Math.Abs(player.Center.X - NPC.Center.X) > 150)
                    {
                        NPC.velocity.Y -= jumpSpeed;
                        fighter_JumpCooldown = jumpCooldown;
                    }
                }
            }

            if (faceDirection)
            {
                NPC.spriteDirection = NPC.direction;
            }
        }

        public override bool CanHitPlayer(NPC NPC, Player target, ref int cooldownSlot)
        {
            TerrorbornPlayer player = TerrorbornPlayer.modPlayer(target);
            if (player.iFrames > 0 || player.VoidBlinkTime > 0 || player.BlinkDashTime > 0)
            {
                return false;
            }
            return base.CanHitPlayer(NPC, target, ref cooldownSlot);
        }

        public override bool PreAI(NPC NPC)
        {
            Player player = Main.LocalPlayer;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.TimeFreezeTime > 0)
            {
                NPC.position -= NPC.velocity;
                return false;
            }
            return base.PreAI(NPC);
        }

        public override void SetDefaults(NPC NPC)
        {
            ogKnockbackResist = NPC.knockBackResist;
            tagTime = 0;

            if (NPC.type == NPCID.KingSlime)
            {
                BossTitle = "King Slime";
                BossSubtitle = "Gelatinous Monarch";
                BossTitleColor = Color.SkyBlue;
            }

            if (NPC.type == NPCID.EyeofCthulhu)
            {
                BossTitle = "Eye of Cthulhu";
                BossSubtitle = "Bloodshot Watcher";
                BossTitleColor = new Color(255, 123, 123);
            }

            if (NPC.type == NPCID.EaterofWorldsHead && !NPC.AnyNPCs(NPCID.EaterofWorldsBody) && !NPC.AnyNPCs(NPCID.EaterofWorldsTail))
            {
                getsTitleCard = true;
                BossTitle = "Eater of Worlds";
                BossSubtitle = "Tunneler of Decay";
                BossTitleColor = Color.Purple;
            }

            if (NPC.type == NPCID.BrainofCthulhu)
            {
                BossTitle = "The Brain of Cthulhu";
                BossSubtitle = "Bloodthirsty Mastermind";
                BossTitleColor = Color.Crimson;
            }

            if (NPC.type == NPCID.QueenBee)
            {
                BossTitle = "The Queen Bee";
                BossSubtitle = "Protector of the hive";
                BossTitleColor = Color.Yellow;
            }

            if (NPC.type == NPCID.SkeletronHead)
            {
                BossTitle = "Skeletron";
                BossSubtitle = "Accursed Guardian of the Dungeon";
                BossTitleColor = Color.Beige;
            }

            if (NPC.type == NPCID.WallofFlesh)
            {
                BossTitle = "The Wall of Flesh";
                BossSubtitle = "The Seal of Terror; Guardian of the Underworld";
                BossTitleColor = Color.Red;
            }

            if (NPC.type == NPCID.SkeletronPrime)
            {
                BossTitle = "Skeletron Prime";
                BossSubtitle = "Construct of Fright";
                BossTitleColor = Color.OrangeRed;
            }

            if (NPC.type == NPCID.TheDestroyer)
            {
                BossTitle = "The Destroyer";
                BossSubtitle = "Construct of Might";
                BossTitleColor = Color.RoyalBlue;
            }

            if (NPC.type == NPCID.Spazmatism || NPC.type == NPCID.Retinazer)
            {
                BossTitle = "The Twins";
                BossSubtitle = "Constructs of Sight";
                BossTitleColor = Color.LightGreen;
            }

            if (NPC.type == NPCID.Plantera)
            {
                BossTitle = "Plantera";
                BossSubtitle = "Southern Plantkill";
                BossTitleColor = Color.LimeGreen;
            }

            if (NPC.type == NPCID.Golem)
            {
                BossTitle = "Golem";
                BossSubtitle = "Protector of the Lihzahrd Tribe";
                BossTitleColor = Color.SaddleBrown;
            }

            if (NPC.type == NPCID.CultistBoss)
            {
                BossTitle = "Lunatic Cultist";
                BossSubtitle = "Messenger of Armaggeddon";
                BossTitleColor = Color.Blue;
            }

            if (NPC.type == NPCID.MoonLordCore || NPC.type == NPCID.MoonLordHand || NPC.type == NPCID.MoonLordHead)
            {
                BossTitle = "The Moon Lord";
                BossSubtitle = "Monarch of the Pillars";
                BossTitleColor = Color.PaleTurquoise;
            }

            if (NPC.type == NPCID.DD2DarkMageT1)
            {
                getsTitleCard = true;
                BossTitle = "The Dark Mage";
                BossSubtitle = "Commander of the Dead";
            }

            if (NPC.type == NPCID.DD2OgreT2)
            {
                getsTitleCard = true;
                BossTitle = "Shrek (not actually)";
                BossSubtitle = "Ogre of the Ages";
            }

            if (NPC.type == NPCID.DD2Betsy)
            {
                getsTitleCard = true;
                BossTitle = "Betsy";
                BossSubtitle = "Leader of the Old Ones";
                BossTitleColor = Color.OrangeRed;
            }

            if (NPC.type == NPCID.MartianSaucer)
            {
                getsTitleCard = true;
                BossTitle = "Martian Saucer";
                BossSubtitle = "Otherwordly Battleship";
            }

            if (NPC.type == NPCID.PirateShip)
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
            if (type == NPCID.Merchant)
            {
                if (Main.LocalPlayer.HasItem(ModContent.ItemType<Items.Weapons.Ranged.Riveter>()))
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Ranged.Rivet>());
                    nextSlot++;
                }
            }
        }

        public override void ModifyHitByItem(NPC NPC, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.LiesOfNourishment && NPC.life >= NPC.lifeMax)
            {
                crit = true;
            }

            if (CumulusEmpowermentTime > 0)
            {
                damage = (int)(damage * 0.5f);
                knockback = 0;
            }

            if (player.HeldItem != null)
            {
                if (crit && !player.HeldItem.IsAir)
                {
                    damage = (int)(damage * modPlayer.critDamage * TerrorbornItem.modItem(player.HeldItem).critDamageMult);
                }
            }

            if (modPlayer.TimeFreezeTime > 0)
            {
                knockback = 0f;
            }
        }

        public override void ModifyHitByProjectile(NPC NPC, Projectile Projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[Projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.LiesOfNourishment && NPC.life >= NPC.lifeMax)
            {
                crit = true;
            }
            if (CumulusEmpowermentTime > 0)
            {
                damage = (int)(damage * 0.5f);
                knockback = 0;
            }

            if (player.HeldItem != null)
            {
                if (crit && !player.HeldItem.IsAir)
                {
                    damage = (int)(damage * modPlayer.critDamage * TerrorbornItem.modItem(player.HeldItem).critDamageMult);
                }
            }

            if (modPlayer.TimeFreezeTime > 0)
            {
                knockback = 0f;
            }

            if (Projectile.DamageType == DamageClass.Summon)
            {
                damage += currentTagDamageAdditive;
                damage = (int)(damage * currentTagDamageMultiplicative);
            }
        }

        public override void UpdateLifeRegen(NPC NPC, ref int damage)
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
                NPC.lifeRegen -= (int)gloveDoTTotal;
            }

            if (NPC.friendly && gloveDoT.Count > 0)
            {
                gloveDoT.Clear();
            }
        }

        bool start = true;
        public override void PostAI(NPC NPC)
        {
            if (tagTime > 0)
            {
                tagTime--;
                if (tagTime <= 0)
                {
                    currentTagDamageAdditive = 0;
                    currentTagDamageMultiplicative = 1f;
                }
            }

            if (start)
            {
                start = false;
                ogKnockbackResist = NPC.knockBackResist;

                if (getsTitleCard || NPC.boss)
                {
                    TitleCardUI.bossName = BossTitle;
                    TitleCardUI.bossSubtitle = BossSubtitle;
                    TitleCardUI.titleColor = BossTitleColor;
                    TitleCardUI.titleCardLifetimeCounter = (int)(60 * TerrorbornMod.titleCardDuration);
                }
            }

            if (soulSplitTime > 0)
            {
                soulSplitTime--;
                if (soulSplitTime <= 0)
                {
                    CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), Color.LightCyan, "Soul Regained");
                    SoundExtensions.PlaySoundOld(SoundID.NPCDeath39, NPC.Center);
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
                    CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), new Color(80, 112, 109), "Spearhead Dropped!", false, true);
                }
            }

            if (CumulusEmpowermentTime > 0)
            {
                CumulusEmpowermentTime--;
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, 21);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = NPC.velocity;
            }
        }

        public static TerrorbornNPC modNPC(NPC NPC)
        {
            return NPC.GetGlobalNPC<TerrorbornNPC>();
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && Main.npc[i].boss)
                {
                    spawnRate = 0;
                    maxSpawns = 0;
                    return;
                }
            }

            if (player.ZoneRain && TerrorbornSystem.terrorRain && maxSpawns != 0) //Checks current maxSpawns specifically so it works with HERO's mod's spawn thingy
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
            Player player = spawnInfo.Player;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (player.ZoneRain && TerrorbornSystem.terrorRain)
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
            if (TerrorbornSystem.CurrentBountyBiome == 0 && player.ZoneUndergroundDesert)
            {
                return true;
            }
            if (TerrorbornSystem.CurrentBountyBiome == 1 && player.ZoneSnow && player.ZoneRockLayerHeight)
            {
                return true;
            }
            if (TerrorbornSystem.CurrentBountyBiome == 2 && player.ZoneUnderworldHeight)
            {
                return true;
            }
            if (TerrorbornSystem.CurrentBountyBiome == 3 && player.ZoneJungle && !player.ZoneRockLayerHeight)
            {
                return true;
            }
            if (TerrorbornSystem.CurrentBountyBiome == 4 && player.ZoneJungle && player.ZoneRockLayerHeight)
            {
                return true;
            }
            if (TerrorbornSystem.CurrentBountyBiome == 5 && (player.ZoneCorrupt || player.ZoneCrimson) && !player.ZoneRockLayerHeight)
            {
                return true;
            }
            if (TerrorbornSystem.CurrentBountyBiome == 6 && player.ZoneSnow)
            {
                return true;
            }
            if (TerrorbornSystem.CurrentBountyBiome == 7 && player.ZoneHallow && !player.ZoneRockLayerHeight)
            {
                return true;
            }
            if (TerrorbornSystem.CurrentBountyBiome == 8 && player.ZoneHallow && player.ZoneRockLayerHeight)
            {
                return true;
            }
            if (TerrorbornSystem.CurrentBountyBiome == 9 && (player.ZoneCorrupt || player.ZoneCrimson) && player.ZoneRockLayerHeight)
            {
                return true;
            }
            return false;
        }

        void SpawnGeyser(int originalDamage, NPC NPC, Player player)
        {
            if (Main.rand.NextFloat() > 0.15f && NPC.life > 0)
            {
                return;
            }
            for (int i = 0; i < Main.rand.Next(2, 5); i++)
            {
                Vector2 position = NPC.Center;
                if (i != 1)
                {
                    position.X += Main.rand.Next(-200, 200);
                }
                while (!WorldUtils.Find(position.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                    {
        new Terraria.WorldBuilding.Conditions.IsSolid()
                    }), out _))
                {
                    position.Y++;
                }
                if (player.HeldItem != null && !player.HeldItem.IsAir) Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), position, new Vector2(0, -20), ModContent.ProjectileType<Items.Equipable.Armor.TideFireFriendly>(), originalDamage / 2, 0f, player.whoAmI); ;
            }
            SoundExtensions.PlaySoundOld(SoundID.Item88, NPC.Center);
        }

        void SpawnAzuriteShard(int originalDamage, NPC NPC, Player player)
        {
            if (Main.rand.NextFloat() > 0.25f)
            {
                return;
            }
            Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
            float speed = Main.rand.Next(15, 25);
            if (player.HeldItem != null && !player.HeldItem.IsAir) Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), NPC.Center, direction * speed, ModContent.ProjectileType<Projectiles.AzuriteShard>(), originalDamage / 2, 0f, player.whoAmI);
            SoundExtensions.PlaySoundOld(SoundID.Item118, NPC.Center);
        }

        public void SinducementExplosion(NPC NPC, int damage, bool death = true)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                float speed = Main.rand.NextFloat(10f, 20f);
                int projDamage = damage / 2;
                if (death && !NPC.boss)
                {
                    projDamage = NPC.lifeMax / 3;
                }
                if (Main.LocalPlayer.HeldItem != null && !Main.LocalPlayer.HeldItem.IsAir) Projectile.NewProjectile(Main.LocalPlayer.GetSource_ItemUse(Main.LocalPlayer.HeldItem), NPC.Center, direction * speed, ModContent.ProjectileType<Projectiles.VeinBurst>(), projDamage, 0.5f, Main.LocalPlayer.whoAmI);
            }

            if (death)
            {
                Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                float speed = Main.rand.NextFloat(10f, 20f);
                int projDamage = damage / 2;
                if (Main.LocalPlayer.HeldItem != null && !Main.LocalPlayer.HeldItem.IsAir) Projectile.NewProjectile(Main.LocalPlayer.GetSource_ItemUse(Main.LocalPlayer.HeldItem), NPC.Center, direction * speed, ModContent.ProjectileType<SinducementSoul>(), projDamage, 0.5f, Main.LocalPlayer.whoAmI);
            }
        }

        public override void HitEffect(NPC NPC, int hitDirection, double damage)
        {
            Player player = Main.LocalPlayer;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            fighter_TargetPlayerCounter = 0;

            if (NPC.life <= 0 && !NPC.SpawnedFromStatue)
            {
                if (modPlayer.SoulEater)
                {
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<TerrorOrb>(), 0, 0, player.whoAmI);
                }
                if (modPlayer.TorturersTalisman)
                {
                    player.AddBuff(ModContent.BuffType<Buffs.TortureBoost>(), 60 * 10);
                }
            }

            if (modPlayer.SoulEater && NPC.boss)
            {
                SoulEaterTotalDamageTaken += (int)damage;
                while (SoulEaterTotalDamageTaken > NPC.lifeMax * 0.075f)
                {
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<TerrorOrb>(), 0, 0, player.whoAmI);
                    SoulEaterTotalDamageTaken -= (int)(NPC.lifeMax * 0.075f);
                }
            }

            if (modPlayer.SoulReaperArmorBonus && NPC.boss)
            {
                SoulReaperTotalDamageTaken += (int)damage;
                while (SoulReaperTotalDamageTaken > NPC.lifeMax * 0.075f)
                {
                    Item.NewItem(NPC.GetSource_Loot(), NPC.Center, ModContent.ItemType<Items.Equipable.Armor.ThunderSoul>());
                    SoulReaperTotalDamageTaken -= (int)(NPC.lifeMax * 0.075f);
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

        public override void OnHitByItem(NPC NPC, Player player, Item item, int damage, float knockback, bool crit)
        {
            if (soulSplitTime > 0)
            {
                if (soulOrbCooldown <= 0)
                {
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<SoulOrb>(), 0, 0, player.whoAmI);
                    SoundExtensions.PlaySoundOld(SoundID.NPCHit36, NPC.Center);
                    soulOrbCooldown = 3;
                }
            }

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (item.DamageType == DamageClass.Melee && modPlayer.TidalShellArmorBonus)
            {
                SpawnGeyser(damage, NPC, player);
            }

            if (modPlayer.AzuriteBrooch)
            {
                SpawnAzuriteShard(damage, NPC, player);
            }

            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.TownNPCs.SkeletonSheriff>()))
            {
                if (NPC.life <= 0)
                {
                    if (CheckBountyBiome(player))
                    {
                        modPlayer.CombatPoints += NPC.lifeMax;
                    }
                }
            }

            if (item.DamageType == DamageClass.Melee && modPlayer.IncendiaryShield)
            {
                if (Main.rand.Next(101) <= 8 + player.GetCritChance(DamageClass.Melee) / 2)
                {
                    DustExplosion(NPC.Center, 0, 45, 30, 6, DustScale: 1f, NoGravity: true);
                    SoundExtensions.PlaySoundOld(SoundID.Item14, NPC.Center);
                    for (int i = 0; i < 200; i++)
                    {
                        NPC target = Main.npc[i];
                        if (!target.friendly && NPC.Distance(target.Center) <= 200 + (target.width + target.height) / 2 && !target.dontTakeDamage)
                        {
                            if (target.type == NPCID.TheDestroyerBody)
                            {
                                target.StrikeNPC(damage / 10, 0, 0, Main.rand.Next(101) <= player.GetCritChance(DamageClass.Melee));
                            }
                            else
                            {
                                target.StrikeNPC(damage / 2, 0, 0, Main.rand.Next(101) <= player.GetCritChance(DamageClass.Melee));
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
                    if ((modPlayer.HeadhunterClass == 0 && item.DamageType == DamageClass.Magic) || 
                        (modPlayer.HeadhunterClass == 1 && item.DamageType == DamageClass.Melee) ||
                        (modPlayer.HeadhunterClass == 2 && item.DamageType == DamageClass.Ranged) ||
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

        public override void OnHitByProjectile(NPC NPC, Projectile Projectile, int damage, float knockback, bool crit)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(Main.player[Projectile.owner]);
            Player player = Main.player[Projectile.owner];

            if (Projectile.DamageType == DamageClass.Melee && modPlayer.TidalShellArmorBonus)
            {
                SpawnGeyser(damage, NPC, player);
            }


            if (modPlayer.PrismalCore && Main.rand.NextFloat() <= 0.15f && Projectile.DamageType == DamageClass.Magic)
            {
                NPC.StrikeNPC(damage, 0, 0, crit);
            }

            if (NPC.life <= 0 && !NPC.SpawnedFromStatue)
            {
                if (player.HasBuff(ModContent.BuffType<Buffs.Sinducement>()))
                {
                    SinducementExplosion(NPC, (int)damage, true);
                    TerrorbornSystem.ScreenShake(2.5f);
                    SoundExtensions.PlaySoundOld(SoundID.DD2_ExplosiveTrapExplode, NPC.Center);
                }
            }

            if (modPlayer.AzuriteBrooch)
            {
                SpawnAzuriteShard(damage, NPC, player);
            }

            if (soulSplitTime > 0)
            {
                if (soulOrbCooldown <= 0)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<SoulOrb>(), 0, 0, player.whoAmI);
                    SoundExtensions.PlaySoundOld(SoundID.NPCHit36, NPC.Center);
                    soulOrbCooldown = 3;
                }
            }

            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.TownNPCs.SkeletonSheriff>()))
            {
                if (NPC.life <= 0)
                {
                    if (CheckBountyBiome(Main.player[Projectile.owner]))
                    {
                        modPlayer.CombatPoints += NPC.lifeMax;
                    }
                }
            }

            if (Projectile.DamageType == DamageClass.Melee && modPlayer.IncendiaryShield)
            {
                if (Main.rand.Next(101) <= 8 + player.GetCritChance(DamageClass.Melee) / 2)
                {
                    DustExplosion(NPC.Center, 0, 45, 30, 6, DustScale: 1f, NoGravity: true);
                    SoundExtensions.PlaySoundOld(SoundID.Item14, NPC.Center);
                    for (int i = 0; i < 200; i++)
                    {
                        NPC target = Main.npc[i];
                        if (!target.friendly && NPC.Distance(target.Center) <= 200 + (target.width + target.height) / 2 && !target.dontTakeDamage)
                        {
                            if (target.type == NPCID.TheDestroyerBody)
                            {
                                target.StrikeNPC(damage / 10, 0, 0, Main.rand.Next(101) <= player.GetCritChance(DamageClass.Melee));
                            }
                            else
                            {
                                target.StrikeNPC(damage / 2, 0, 0, Main.rand.Next(101) <= player.GetCritChance(DamageClass.Melee));
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
                    if ((modPlayer.HeadhunterClass == 0 && Projectile.DamageType == DamageClass.Magic) ||
                        (modPlayer.HeadhunterClass == 1 && Projectile.DamageType == DamageClass.Melee) ||
                        (modPlayer.HeadhunterClass == 2 && Projectile.DamageType == DamageClass.Ranged) ||
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

        public override void OnKill(NPC NPC)
        {
            Player player = Main.LocalPlayer;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (modPlayer.SoulReaperArmorBonus)
            {
                Item.NewItem(NPC.GetSource_Loot(), NPC.Center, ModContent.ItemType<Items.Equipable.Armor.ThunderSoul>());
            }

            if (TerrorbornSystem.terrorRain && player.ZoneRain && Main.rand.NextFloat() <= 0.5f)
            {
                int type = ItemID.DirtBlock;
                switch (Main.rand.Next(3))
                {
                    case 0:
                        type = ItemID.SoulofNight;
                        break;
                    case 1:
                        type = ItemID.SoulofLight;
                        break;
                    case 2:
                        type = ItemID.SoulofFlight;
                        break;
                }
                Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), type);
            }

            float ExpertBoost = 1;
            if (Main.expertMode)
            {
                ExpertBoost = 2;
            }
            if (Main.masterMode)
            {
                ExpertBoost = 3;
            }

            if (player.ZoneDungeon)
            {
                if (Main.rand.NextFloat() <= 0.01f * ExpertBoost)
                {
                    Item.NewItem(NPC.GetSource_Loot(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.MiscConsumables.StrangeBag>());
                }
            }

            if (modPlayer.ZoneIncendiary)
            {
                if (Main.rand.NextFloat() <= 0.02f)
                {
                    Item.NewItem(NPC.GetSource_Loot(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.IncendiaryLockbox>());
                }

                if (Main.rand.NextFloat() <= 0.02f && NPC.downedGolemBoss)
                {
                    Item.NewItem(NPC.GetSource_Loot(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.SkullmoundLockbox>());
                }

                if (Main.rand.NextFloat() <= 0.015f * ExpertBoost)
                {
                    Item.NewItem(NPC.GetSource_Loot(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.MiscConsumables.HotMilk>());
                }

                if (Main.rand.NextFloat() <= 0.01f * ExpertBoost)
                {
                    Item.NewItem(NPC.GetSource_Loot(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Equipable.Vanity.IncendiaryBreastplate>());
                }

                if (Main.rand.NextFloat() <= 0.01f * ExpertBoost)
                {
                    Item.NewItem(NPC.GetSource_Loot(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Equipable.Vanity.IncendiaryLeggings>());
                }

                if (Main.rand.NextFloat() <= 0.01f * ExpertBoost)
                {
                    Item.NewItem(NPC.GetSource_Loot(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Equipable.Vanity.IncendiaryVisor>());
                }

                if (Main.rand.NextFloat() <= 0.25f && NPC.downedGolemBoss)
                {
                    Item.NewItem(NPC.GetSource_Loot(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Materials.HellbornEssence>());
                }
            }

            if (NPC.type == NPCID.MoonLordCore)
            {
                modPlayer.TerrorPercent = 0f;
            }

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

            if (extraDarkEnergyIDs.Contains(NPC.type) || NPC.boss)
            {
                for (int i = 0; i < Main.rand.Next(3, 5); i++)
                {
                    Item.NewItem(NPC.GetSource_Loot(), NPC.Center, ModContent.ItemType<Items.DarkEnergy>());
                }
            }

            if (Main.rand.Next(12) == 0 && modPlayer.TerrorPercent < 100 && TerrorbornSystem.obtainedShriekOfHorror)
            {
                Item.NewItem(NPC.GetSource_Loot(), NPC.Center, ModContent.ItemType<Items.DarkEnergy>());
            }

            if (modPlayer.LiesOfNourishment && Main.rand.Next(5) == 0)
            {
                Item.NewItem(NPC.GetSource_Loot(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Heart);
            }

            if (player.HasBuff(ModContent.BuffType<Buffs.Vampirism>()) && NPC.Distance(player.Center) <= 350)
            {
                if (Main.rand.NextFloat() <= 0.2f)
                {
                    Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ItemID.Heart);
                }

                if (Main.rand.NextFloat() <= 0.2f)
                {
                    Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<Items.DarkEnergy>());
                }
            }

            if (NPC.type == NPCID.KingSlime)
            {
                bool spawnGA = !TerrorbornPlayer.modPlayer(Main.player[Main.myPlayer]).unlockedAbilities.Contains(6);
                for (int i = 0; i < 1000; i++)
                {
                    Projectile Projectile = Main.projectile[i];
                    if (Projectile.active && Projectile.type == ModContent.ProjectileType<Abilities.GelatinArmor>())
                    {
                        spawnGA = false;
                    }
                }

                if (spawnGA)
                {
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Abilities.GelatinArmor>(), 0, 0, Main.myPlayer);
                }
            }
        }

        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {

        }

        public override void ModifyNPCLoot(NPC NPC, NPCLoot NPCLoot)
        {

            if (NPCID.Sets.ProjectileNPC[NPC.type])
            {
                return;
            }


            if (NPC.type == NPCID.LunarTowerSolar || NPC.type == NPCID.LunarTowerNebula || NPC.type == NPCID.LunarTowerStardust || NPC.type == NPCID.LunarTowerVortex)
            {
                NPCLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.FusionFragment>(), minimumDropped: 5, maximumDropped: 20));
            }


            if (NPC.type == NPCID.GoblinSorcerer)
            {
                NPCLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Magic.BookOfChaos>(), 7));
            }

            if (NPC.type == NPCID.GoblinArcher)
            {
                NPCLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Ranged.WarBow>(), 9));
            }

            if (NPC.type == NPCID.TacticalSkeleton || NPC.type == NPCID.SkeletonCommando || NPC.type == NPCID.SkeletonSniper)
            {
                NPCLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Equipable.Accessories.TacticalCommlink>(), 3));
            }

            if (NPC.type == NPCID.EyeofCthulhu)
            {
                NPCLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.PermanentUpgrades.EyeOfTheMenace>()));
                NPCLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Summons.Minions.OpticCane>()));
                NPCLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.TwilightModeCondition(), ModContent.ItemType<Items.Weapons.Ranged.Riveter>()));
                NPCLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.TwilightModeCondition(), ModContent.ItemType<Items.Weapons.Ranged.Rivet>(), minimumDropped: 175, maximumDropped: 225));
            }

            if (NPC.type == NPCID.KingSlime)
            {
                NPCLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Equipable.Accessories.BurstJumps.CompressedGelatin>()));
            }

            if (NPC.type == NPCID.WallofFlesh)
            {
                NPCLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.PermanentUpgrades.DemonicLense>()));
            }

            if (NPC.type == NPCID.Mothron)
            {
                NPCLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Summons.Other.Armagrenade>(), 3));
            }

            if (NPC.type == NPCID.Vampire || NPC.type == NPCID.VampireBat)
            {
                NPCLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Equipable.Accessories.VampiricPendant>(), 25));
            }

            if (NPC.type == NPCID.MartianSaucerCore)
            {
                NPCLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Equipable.Accessories.Wings.MartianBoosters>()));
            }

            if (NPC.type == NPCID.SkeletronHead)
            {
                NPCLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.PermanentUpgrades.CoreOfFear>()));
            }

            if (NPC.type == NPCID.GraniteFlyer || NPC.type == NPCID.GraniteGolem)
            {
                NPCLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.GraniteVirusSpark>(), 100));
            }

            if (NPC.type == NPCID.Antlion || NPC.type == NPCID.FlyingAntlion || NPC.type == NPCID.WalkingAntlion)
            {
                NPCLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Equipable.Accessories.AntsMandible>(), 50));
            }
        }
    }


    class SoulOrb : ModProjectile
    {
        public override string Texture => "TerrorbornMod/placeholder";
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.hide = true;
            Projectile.timeLeft = 300;
            Projectile.damage = 0;
        }

        int Direction = 1;
        int DirectionCounter = 5;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            int type = 132;
            if (modPlayer.SanguineSetBonus) type = 130;
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, type);
            Main.dust[dust].velocity = Projectile.velocity;
            if (modPlayer.SanguineSetBonus) Main.dust[dust].velocity = Projectile.velocity / 4;
            Main.dust[dust].scale = 1f;
            Main.dust[dust].alpha = 255 / 2;
            Main.dust[dust].noGravity = true;
            Main.dust[dust].color = Color.White;


            int speed = 8;
            if (modPlayer.SanguineSetBonus) speed = 25;
            Projectile.velocity = Projectile.DirectionTo(player.Center) * speed;
            if (Projectile.Distance(player.Center) <= speed)
            {
                int healAmount = 1;
                player.HealEffect(healAmount);
                player.statLife += healAmount;
                Projectile.active = false;
            }
        }
    }

    class TerrorOrb : ModProjectile
    {
        public override string Texture => "TerrorbornMod/placeholder";
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.hide = true;
            Projectile.timeLeft = 300;
            Projectile.damage = 0;
        }

        int Direction = 1;
        int DirectionCounter = 5;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            int type = 54;
            int dust = Dust.NewDust(Projectile.Center, 0, 0, type);
            Main.dust[dust].velocity = Vector2.Zero;
            Main.dust[dust].scale = 1f;
            Main.dust[dust].alpha = 255 / 2;
            Main.dust[dust].noGravity = true;
            Main.dust[dust].color = Color.White;


            int speed = 25;
            Projectile.velocity = Projectile.DirectionTo(player.Center) * speed;
            if (Projectile.Distance(player.Center) <= speed)
            {
                modPlayer.GainTerror(2f, false);
                Projectile.active = false;
            }
        }
    }

    class SinducementSoul : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }

        public override string Texture => "TerrorbornMod/placeholder";
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.hostile = false;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 120 * (Projectile.extraUpdates + 1);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            BezierCurve bezier = new BezierCurve();
            bezier.Controls.Clear();
            foreach (Vector2 pos in Projectile.oldPos)
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
                    Vector2 drawPos = positions[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color = Projectile.GetAlpha(Color.LightGray) * ((float)(positions.Count - i) / (float)positions.Count);
                    TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(25f * ((float)(positions.Count - i) / (float)positions.Count)), color);
                }
            }
            return false;
        }

        public override void AI()
        {
            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(Main.MouseWorld).ToRotation(), MathHelper.ToRadians(5f * (Projectile.velocity.Length() / 20))).ToRotationVector2() * Projectile.velocity.Length();
        }
    }
}
