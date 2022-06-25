using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class ThunderJavelin : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Sticks onto hit enemies, zapping them or other nearby foes" +
                "\nWhile holding this item you gain 10 armor penetration");
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.15f;
            Item.damage = 42;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 58;
            Item.height = 58;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 5, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shootSpeed = 35;
            Item.shoot = ModContent.ProjectileType<ThunderJavelinProjectile>();
        }

        public override void HoldItem(Player player)
        {
            player.GetArmorPenetration(DamageClass.Ranged) += 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe(225)
                .AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 3)
                .AddIngredient(ModContent.ItemType<Items.Materials.NoxiousScale>(), 2)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    class ThunderJavelinProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 74;
            Projectile.height = 74;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 3600;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.extraUpdates = 4;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.arrow = true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 14;
            height = 14;
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!stuck)
            {
                SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
                stuck = true;
                stuckNPC = target;
                offset = target.position - Projectile.position;
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (stuck)
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Rectangle originalHitbox = hitbox;
            int newSize = 14;
            hitbox.Width = newSize;
            hitbox.Height = newSize;
            hitbox.X = originalHitbox.Center.X - newSize / 2;
            hitbox.Y = originalHitbox.Center.Y - newSize / 2;
            base.ModifyDamageHitbox(ref hitbox);
        }

        bool stuck = false;
        NPC stuckNPC;
        int stuckTimeLeft = 180;
        Vector2 offset;
        bool start = true;
        int ProjectileCounter = 0;
        public override void AI()
        {
            if (start)
            {
                start = false;
                Projectile.velocity /= Projectile.extraUpdates + 1;
                stuckTimeLeft *= Projectile.extraUpdates + 1;
                Projectile.timeLeft *= Projectile.extraUpdates + 1;
            }

            if (stuck)
            {
                Projectile.tileCollide = false;
                Projectile.position = stuckNPC.position - offset;
                if (!stuckNPC.active)
                {
                    Projectile.active = false;
                }

                stuckTimeLeft--;
                if (stuckTimeLeft <= 0)
                {
                    Projectile.timeLeft = 1;
                }

                ProjectileCounter--;
                if (ProjectileCounter <= 0)
                {
                    ProjectileCounter = 50 * (Projectile.extraUpdates + 1);

                    NPC target = Main.npc[0];
                    bool foundTarget = false;
                    float distance = 2000;

                    foreach (NPC NPC in Main.npc)
                    {
                        if (NPC.whoAmI != stuckNPC.whoAmI && NPC.Distance(stuckNPC.Center) <= distance && NPC.CanBeChasedBy() && Projectile.CanHitWithOwnBody(NPC) && !NPC.friendly)
                        {
                            distance = NPC.Distance(stuckNPC.Center);
                            target = NPC;
                            foundTarget = true;
                        }
                    }

                    if (!foundTarget)
                    {
                        target = stuckNPC;
                    }


                    Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                    float speed = Main.rand.NextFloat(10f, 25f);

                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction * speed, ModContent.ProjectileType<Projectiles.SoulLightning>(), Projectile.damage / 3, 0.5f, Projectile.owner);
                    Main.projectile[proj].DamageType = DamageClass.Ranged;
                    Main.projectile[proj].ai[0] = target.whoAmI;
                }
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135);
            }
        }
    }
}
