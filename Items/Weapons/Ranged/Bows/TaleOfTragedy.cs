using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.Projectiles;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Ranged.Bows
{
    class TaleOfTragedy : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires a deathray that launches many arrows at nearby enemeis");
        }

        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 32;
            Item.height = 54;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.rare = ModContent.RarityType<Rarities.Golden>();
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Arrow;
            Item.value = Item.sellPrice(0, 35, 0, 0);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2f, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y).ToRotation().ToRotationVector2(), ModContent.ProjectileType<TragicBeam>(), damage, knockback, player.whoAmI);

            for (float i = 0f; i < 1000f; i += 80)
            {
                Vector2 arrowPos = position + new Vector2(velocity.X, velocity.Y).ToRotation().ToRotationVector2() * i;

                if (!Collision.CanHitLine(player.Center, 1, 1, arrowPos, 1, 1))
                {
                    break;
                }

                NPC targetNPC = Main.npc[0];
                float Distance = 500;
                bool Targeted = false;
                for (int eI = 0; eI < 200; eI++)
                {
                    if (Main.npc[eI].Distance(arrowPos) < Distance && !Main.npc[eI].friendly && Main.npc[eI].CanBeChasedBy())
                    {
                        targetNPC = Main.npc[eI];
                        Distance = Main.npc[eI].Distance(arrowPos);
                        Targeted = true;
                    }
                }
                if (Targeted)
                {
                    int proj = Projectile.NewProjectile(source, arrowPos, targetNPC.DirectionFrom(arrowPos) * new Vector2(velocity.X, velocity.Y).Length(), type, damage / 3, knockback, player.whoAmI);
                    Main.projectile[proj].noDropItem = true;
                }
            }

            return false;
        }
    }

    class TragicBeam : Deathray
    {
        int timeLeft = 10;
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/LightBlast";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.hide = false;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = timeLeft;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.arrow = true;
            MoveDistance = 20f;
            RealMaxDistance = 1000f;
            bodyRect = new Rectangle(0, 0, 10, 10);
            headRect = new Rectangle(0, 0, 10, 10);
            tailRect = new Rectangle(0, 0, 10, 10);
            FollowPosition = false;
            drawColor = Color.LightGoldenrodYellow;
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / timeLeft;
            Projectile.velocity.Normalize();
        }
    }
}
