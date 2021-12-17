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
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HallowedBar, 5);
            recipe.AddIngredient(ModContent.ItemType<Materials.SoulOfPlight>(), 15);
            recipe.AddIngredient(ItemID.Chain, 3);
            recipe.AddIngredient(ItemID.Glass, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hitting enemies has a chance to cause you to fire an extra projectile towards them" +
                "\nThe behavior of this projectile depends on the damage type" +
                "\n8% increased item use speed");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.noMelee = true;
            item.rare = ItemRarityID.Yellow;
            item.value = Item.sellPrice(0, 8, 0, 0);
            item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ShadowAmulet = true;
            modPlayer.allUseSpeed *= 1.08f;
        }
    }

    class ShadowSoul : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            projectile.hostile = false;
            projectile.hide = true;
            projectile.timeLeft = 300;
        }

        bool start = true;
        float maxSpeed;
        int returnWait = 30;
        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 61, 0f, 0f, 100, Color.Lime, 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;

            if (start)
            {
                start = false;


                maxSpeed = projectile.velocity.Length();

                if (projectile.magic)
                {
                    maxSpeed *= 0.65f;
                }

                if (projectile.melee)
                {
                    projectile.penetrate = -1;
                    projectile.timeLeft = 60 * 3;
                    projectile.damage = (int)(projectile.damage * 0.65f);
                }

                if (projectile.ranged)
                {
                    projectile.extraUpdates = 2;
                }
            }

            if (projectile.magic)
            {
                NPC targetNPC = Main.npc[0];
                float Distance = 1000; //max distance away
                bool Targeted = false;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                    {
                        targetNPC = Main.npc[i];
                        Distance = Main.npc[i].Distance(projectile.Center);
                        Targeted = true;
                    }
                }
                if (Targeted)
                {
                    //HOME IN
                    float speed = 1f;
                    Vector2 direction = projectile.DirectionTo(targetNPC.Center);
                    projectile.velocity += speed * direction;
                    projectile.velocity *= 0.98f;

                    if (projectile.velocity.Length() > maxSpeed)
                    {
                        projectile.velocity *= maxSpeed / projectile.velocity.Length();
                    }
                }
            }

            if (projectile.melee)
            {
                if (returnWait > 0)
                {
                    returnWait--;
                }
                else
                {
                    float speed = 1f;
                    Vector2 direction = projectile.DirectionTo(Main.player[projectile.owner].Center);
                    projectile.velocity += speed * direction;
                    projectile.velocity *= 0.98f;

                    if (projectile.velocity.Length() > maxSpeed)
                    {
                        projectile.velocity *= maxSpeed / projectile.velocity.Length();
                    }
                }
            }
        }
    }
}
