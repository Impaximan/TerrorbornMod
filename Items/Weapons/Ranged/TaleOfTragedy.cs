using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.Projectiles;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class TaleOfTragedy : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires a deathray that launches many arrows at nearby enemeis");
        }

        public override void SetDefaults()
        {
            item.damage = 60;
            item.ranged = true;
            item.width = 32;
            item.height = 54;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 2;
            item.rare = 12;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = ProjectileID.PurificationPowder;
            item.shootSpeed = 20f;
            item.useAmmo = AmmoID.Arrow;
            item.value = Item.sellPrice(0, 35, 0, 0);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2f, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position, new Vector2(speedX, speedY).ToRotation().ToRotationVector2(), ModContent.ProjectileType<TragicBeam>(), damage, knockBack, player.whoAmI);

            for (float i = 0f; i < 1000f; i += 80)
            {
                Vector2 arrowPos = position + new Vector2(speedX, speedY).ToRotation().ToRotationVector2() * i;

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
                    int proj = Projectile.NewProjectile(arrowPos, targetNPC.DirectionFrom(arrowPos) * new Vector2(speedX, speedY).Length(), type, damage / 3, knockBack, player.whoAmI);
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
            projectile.width = 10;
            projectile.height = 10;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.hide = false;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.timeLeft = timeLeft;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.arrow = true;
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
            deathrayWidth -= 1f / (float)timeLeft;
            projectile.velocity.Normalize();
        }
    }
}
