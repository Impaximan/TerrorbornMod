using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Ranged.Thrown
{
    class DesertClotBomb : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Explodes into homing antlion blood");
        }
        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 0, 20);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shootSpeed = 22;
            Item.shoot = ModContent.ProjectileType<DesertClotBombProj>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(111)
                .AddIngredient(ModContent.ItemType<Materials.TarOfHunger>(), 6)
                .AddIngredient(ItemID.AntlionMandible)
                .AddIngredient(ItemID.Grenade, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    class DesertClotBombProj : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/Thrown/DesertClotBomb";
        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            damage = (int)(damage * 0.75f);
        }
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 22;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X);
            Projectile.velocity.Y += 0.18f;
        }
        public override void Kill(int timeLeft)
        {
            SoundExtensions.PlaySoundOld(SoundID.NPCDeath1, Projectile.Center);
            SoundExtensions.PlaySoundOld(SoundID.Item14, Projectile.Center);
            for (int i = 0; i < Main.rand.Next(3, 6); i++)
            {
                float Speed = Main.rand.Next(7, 10);
                Vector2 ProjectileSpeed = MathHelper.ToRadians(Main.rand.Next(361)).ToRotationVector2() * Speed;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, ProjectileSpeed, ModContent.ProjectileType<AntlionBlood>(), (int)(Projectile.damage * 0.75f), 0, Projectile.owner);
            }
        }
    }
    class AntlionBlood : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.hostile = false;
            Projectile.hide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 30;
        }

        public override string Texture => "TerrorbornMod/placeholder";
        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 115, 0f, 0f, 100, Color.Red, 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Projectile.velocity;
            int targetPlayer = Player.FindClosest(Projectile.position, Projectile.width, Projectile.height);
            NPC targetNPC = Main.npc[0];
            float Distance = 375; //max distance away
            bool Targeted = false;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Distance(Projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                {
                    targetNPC = Main.npc[i];
                    Distance = Main.npc[i].Distance(Projectile.Center);
                    Targeted = true;
                }
            }
            if (Targeted)
            {
                //HOME IN
                float speed = .3f;
                Vector2 move = targetNPC.Center - Projectile.Center;
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                move *= speed / magnitude;
                Projectile.velocity += move;
                Projectile.velocity *= 0.98f;
            }
        }
    }
}
