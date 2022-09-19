using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Magic.SpellBooks
{
    class BookOfChaos : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires a chaos ball that bounces between enemies" +
                "\nInflicts shadowflame for 2 seconds on critical hits");
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.width = 32;
            Item.height = 38;
            Item.DamageType = DamageClass.Magic; ;
            Item.damage = 27;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.mana = 10;
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<ChaosBall>();
            Item.shootSpeed = 10;
            Item.UseSound = SoundID.Item20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 0.1f;
            Item.autoReuse = true;
        }
    }

    class ChaosBall : ModProjectile
    {
        public override string Texture => "TerrorbornMod/placeholder";

        public override void SetDefaults()
        {
            NPC NPC = new NPC();
            NPC.SetDefaults(NPCID.ChaosBall);
            Projectile.width = NPC.width;
            Projectile.height = NPC.height;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = 6;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic; ;
            Projectile.ignoreWater = true;
            Projectile.hide = false;
            Projectile.timeLeft = 600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        List<NPC> alreadyHit = new List<NPC>();
        public override void AI()
        {
            Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27)];
            dust.noGravity = true;
            dust.velocity = Vector2.Zero;
            dust.scale = 2f;
        }

        public override void Kill(int timeLeft)
        {
            SoundExtensions.PlaySoundOld(SoundID.NPCHit3, Projectile.Center);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            alreadyHit.Add(target);

            bool targeted = false;
            NPC launchTo = Main.npc[0];
            float distance = 2000;

            foreach (NPC NPC in Main.npc)
            {
                if (NPC.active && NPC.Distance(Projectile.Center) < distance && !alreadyHit.Contains(NPC) && Projectile.CanHitWithOwnBody(NPC) && !NPC.friendly && NPC.CanBeChasedBy())
                {
                    distance = NPC.Distance(Projectile.Center);
                    launchTo = NPC;
                    targeted = true;
                }
            }

            if (targeted)
            {
                float speed = Projectile.velocity.Length();
                Vector2 direction = Projectile.DirectionTo(launchTo.Center);
                Projectile.velocity = speed * direction;
            }

            if (crit)
            {
                target.AddBuff(BuffID.ShadowFlame, 60 * 2);
            }
        }
    }
}

