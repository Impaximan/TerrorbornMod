using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class ShadowAmulet : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 5)
                .AddIngredient(ModContent.ItemType<Materials.SoulOfPlight>(), 15)
                .AddIngredient(ItemID.Chain, 3)
                .AddIngredient(ItemID.Glass, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();

        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hitting enemies has a chance to cause you to fire an extra Projectile towards them" +
                "\nThe behavior of this Projectile depends on the damage type" +
                "\n8% increased item use speed");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ShadowAmulet = true;
            player.GetAttackSpeed(DamageClass.Generic) *= 1.08f;
        }
    }

    class ShadowSoul : ModProjectile
    {
        public override string Texture => "TerrorbornMod/placeholder";

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.hostile = false;
            Projectile.hide = true;
            Projectile.timeLeft = 300;
        }

        bool start = true;
        float maxSpeed;
        int returnWait = 30;
        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 61, 0f, 0f, 100, Color.Lime, 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Projectile.velocity;

            if (start)
            {
                start = false;


                maxSpeed = Projectile.velocity.Length();

                if (Projectile.DamageType == DamageClass.Magic)
                {
                    maxSpeed *= 0.65f;
                }

                if (Projectile.DamageType == DamageClass.Melee)
                {
                    Projectile.penetrate = -1;
                    Projectile.timeLeft = 60 * 3;
                    Projectile.damage = (int)(Projectile.damage * 0.65f);
                }

                if (Projectile.DamageType == DamageClass.Ranged)
                {
                    Projectile.extraUpdates = 2;
                }
            }

            if (Projectile.DamageType == DamageClass.Magic)
            {
                NPC targetNPC = Main.npc[0];
                float Distance = 1000; //max distance away
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
                    float speed = 1f;
                    Vector2 direction = Projectile.DirectionTo(targetNPC.Center);
                    Projectile.velocity += speed * direction;
                    Projectile.velocity *= 0.98f;

                    if (Projectile.velocity.Length() > maxSpeed)
                    {
                        Projectile.velocity *= maxSpeed / Projectile.velocity.Length();
                    }
                }
            }

            if (Projectile.DamageType == DamageClass.Melee)
            {
                if (returnWait > 0)
                {
                    returnWait--;
                }
                else
                {
                    float speed = 1f;
                    Vector2 direction = Projectile.DirectionTo(Main.player[Projectile.owner].Center);
                    Projectile.velocity += speed * direction;
                    Projectile.velocity *= 0.98f;

                    if (Projectile.velocity.Length() > maxSpeed)
                    {
                        Projectile.velocity *= maxSpeed / Projectile.velocity.Length();
                    }
                }
            }
        }
    }
}
