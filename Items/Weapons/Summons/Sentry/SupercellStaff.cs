using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Summons.Sentry
{
    class SupercellStaff : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 18)
                .AddIngredient(ModContent.ItemType<Items.Materials.NoxiousScale>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a storm cell sentry that rapidly zaps nearby enemies");
        }

        public override void SetDefaults()
        {
            Item.mana = 10;
            Item.DamageType = DamageClass.Summon;
            Item.damage = 55;
            Item.width = 54;
            Item.height = 58;
            Item.sentry = true;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 0;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<Supercell>();
            Item.shootSpeed = 10f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                int turretAmount = 0;
                for (int i = 999; i >= 0; i--)
                {
                    if (Main.projectile[i].sentry && Main.projectile[i].active)
                    {
                        turretAmount++;
                        if (turretAmount >= player.maxTurrets)
                        {
                            Main.projectile[i].active = false;
                        }
                    }
                }
                Projectile.NewProjectile(source, new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition, Vector2.Zero, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim(false);
            }
            return base.UseItem(player);
        }
    }

    class Supercell : ModProjectile
    {
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true; //This is necessary for right-click targeting
        }
        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 52;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.sentry = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        void FindFrame(int FrameHeight)
        {
            Projectile.frameCounter--;
            if (Projectile.frameCounter <= 0)
            {
                Projectile.frame++;
                Projectile.frameCounter = 4;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
        }

        int PinWait = 60;
        int PinRoundsLeft = 2;
        public override void AI()
        {
            FindFrame(Projectile.height);
            Projectile.timeLeft = 10;
            bool Targeted = false;
            //Projectile.velocity.Y = 50;
            //Projectile.velocity.X = 0;
            Player player = Main.player[Projectile.owner];
            NPC target = Main.npc[0];
            if (player.HasMinionAttackTargetNPC && Main.npc[player.MinionAttackTargetNPC].Distance(Projectile.Center) < 1500)
            {
                target = Main.npc[player.MinionAttackTargetNPC];
                Targeted = true;
            }
            else
            {
                float Distance = 750;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].Distance(Projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                    {
                        target = Main.npc[i];
                        Distance = Main.npc[i].Distance(Projectile.Center);
                        Targeted = true;
                    }
                }
            }
            if (Targeted)
            {
                PinWait--;
                if (PinWait <= 0)
                {
                    PinWait = 10;
                    SpawnLightning(target.whoAmI);
                }
            }
        }

        public void SpawnLightning(int target)
        {
            Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
            float speed = Main.rand.NextFloat(10f, 25f);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction * speed, ModContent.ProjectileType<Projectiles.SoulLightning>(), Projectile.damage, 0.5f, Projectile.owner);
            Main.projectile[proj].ai[0] = target;
            Main.projectile[proj].DamageType = DamageClass.Summon;
        }
    }
}

