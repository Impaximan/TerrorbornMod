using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace TerrorbornMod.Items.Weapons.Melee.Yoyos
{
    public class ThunderousThrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
            Tooltip.SetDefault("Electrocutes nearby enemies");
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.ThunderShard>(), 18)
                .AddIngredient(ModContent.ItemType<Materials.NoxiousScale>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 26;
            Item.height = 40;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.shootSpeed = 17f;
            Item.shoot = ModContent.ProjectileType<ThunderousThrowProjectile>();
            Item.knockBack = 2.5f;
            Item.damage = 40;
            Item.rare = ItemRarityID.Pink;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.sellPrice(silver: 1);
        }

        private static readonly int[] unwantedPrefixes = new int[] { PrefixID.Terrible, PrefixID.Dull, PrefixID.Shameful, PrefixID.Annoying, PrefixID.Broken, PrefixID.Damaged, PrefixID.Shoddy };

        public override bool AllowPrefix(int pre)
        {
            if (Array.IndexOf(unwantedPrefixes, pre) > -1)
            {
                return false;
            }
            return true;
        }
    }

    public class ThunderousThrowProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 12f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 350f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 22f;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 99;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
        }

        int ProjectileWait = 10;
        public override void PostAI()
        {
            NPC target = Main.npc[0];
            bool foundTarget = false;
            float distance = 2000;

            foreach (NPC NPC in Main.npc)
            {
                if (NPC.Distance(Projectile.Center) <= distance && NPC.CanBeChasedBy() && Projectile.CanHitWithOwnBody(NPC) && !NPC.friendly)
                {
                    distance = NPC.Distance(Projectile.Center);
                    target = NPC;
                    foundTarget = true;
                }
            }

            if (foundTarget)
            {
                ProjectileWait--;
                if (ProjectileWait <= 0)
                {
                    ProjectileWait = Main.rand.Next(20, 30);
                    Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                    float speed = Main.rand.NextFloat(10f, 25f);

                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction * speed, ModContent.ProjectileType<Projectiles.SoulLightning>(), Projectile.damage / 2, 0.5f, Projectile.owner);
                    Main.projectile[proj].DamageType = DamageClass.Melee;
                    Main.projectile[proj].ai[0] = target.whoAmI;
                }
            }
        }
    }
}

