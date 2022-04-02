using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class AdamantiteLaserRifle : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AdamantiteBar, 13)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires an adamantite laser that ricochets to nearby enemies on critical hits and bounces off of walls");
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.noMelee = true;
            Item.width = 66;
            Item.height = 34;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.crit = 14;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item33;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AdamantiteLaser>();
            Item.shootSpeed = 10f;
            Item.mana = 4;
            Item.DamageType = DamageClass.Magic;;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = player.Center + (player.DirectionTo(Main.MouseWorld) * 66);
        }
    }

    class AdamantiteLaser : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 12;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.hide = true;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 350;
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustPerfect(Projectile.Center, 60);
            dust.noGravity = true;
            dust.noLight = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < Projectile.localNPCImmunity.Length; i++)
            {
                if (Projectile.localNPCImmunity[i] < 0 || Projectile.localNPCImmunity[i] > 5)
                {
                    Projectile.localNPCImmunity[i] = 5;
                }
            }
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }

        List<NPC> alreadyHit = new List<NPC>();
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!crit)
            {
                return;
            }
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
        }
    }
}

