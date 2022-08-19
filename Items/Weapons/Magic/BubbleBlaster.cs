using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Audio;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class BubbleBlaster : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble Launcher");
            Tooltip.SetDefault("Fires a bubble that can be destroyed by other weapons to create an explosion of mini-bubbles" +
                "\nThe longer you take to destroy the main bubble the more damage the mini-bubbles will do");
        }

        public override void SetDefaults()
        {
            Item.damage = 52;
            Item.noMelee = true;
            Item.width = 44;
            Item.height = 18;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.crit = 14;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = new SoundStyle("TerrorbornMod/Sounds/Effects/ShootIteration3");
            Item.autoReuse = false;
            Item.shoot = ProjectileID.Bubble;
            Item.shootSpeed = 10f;
            Item.mana = 15;
            Item.DamageType = DamageClass.Magic;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int npc = NPC.NewNPC(source, (int)position.X, (int)position.Y, ModContent.NPCType<BubbleBlaster_Bubble_Large>());
            Main.npc[npc].velocity = velocity;
            Main.npc[npc].ai[0] = 0.15f;
            Main.npc[npc].damage = damage;
            Main.npc[npc].Center = position;
            return false;
        }
    }

    class BubbleBlaster_Bubble_Large : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
            DisplayName.SetDefault("Bubble");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void OnKill()
        {
            for (float i = 0f; i < 1f; i += 1f / 15f)
            {
                Vector2 velocity = new Vector2(0, -10f);
                velocity = velocity.RotatedBy(MathHelper.ToRadians(360f * i));
                Projectile.NewProjectile(NPC.GetSource_OnHit(NPC), NPC.Center, velocity, ModContent.ProjectileType<BubbleBlaster_Bubble_Small>(), (int)(NPC.damage * damageMult), 0, Main.myPlayer);

            }

            if (Main.rand.NextBool(10))
            {
                Item.NewItem(NPC.GetSource_Loot(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Heart);
            }
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 38;
            NPC.height = 36;
            NPC.HitSound = SoundID.Item54;
            NPC.defense = 0;
            NPC.DeathSound = SoundID.Item54;
            NPC.frame.Width = 388;
            NPC.frame.Height = 254;
            NPC.lifeMax = 10;
            NPC.knockBackResist = 0.4f;
            NPC.dontTakeDamage = true;
            NPC.chaseable = false;
            NPC.aiStyle = -1;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = 10;
        }

        int frame = 0;
        int frameWait = 10;
        public override void FindFrame(int frameHeight)
        {
            frameWait--;
            if (frameWait <= 0)
            {
                frameWait = 10;
                frame++;
                if (frame >= 4)
                {
                    frame = 0;
                }
            }
            NPC.frame.Y = frame * 36;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.type == ModContent.ProjectileType<Items.Equipable.Armor.azuriteShockwave>())
            {
                return false;
            }
            return base.CanBeHitByProjectile(projectile);
        }

        int invincTime = 15;
        float damageMult = 0.5f;
        public override void AI()
        {
            damageMult += 0.5f / 60f;

            if (invincTime <= 0)
            {
                NPC.dontTakeDamage = false;
            }
            else
            {
                invincTime--;
            }

            NPC.TargetClosest(true);

            NPC.velocity.Y -= NPC.ai[0];

            if (NPC.Center.Y < Main.LocalPlayer.Center.Y - 1000)
            {
                NPC.active = false;
            }
        }
    }

    class BubbleBlaster_Bubble_Small : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 70;
            Projectile.penetrate = -1;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.hide = false;
        }

        public override void Kill(int timeLeft)
        {
            SoundExtensions.PlaySoundOld(SoundID.Item54, Projectile.position);
        }
    }
}
