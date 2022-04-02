using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class SightForSoreEyes : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble Wand");
            Tooltip.SetDefault("Rapidly fires bubbles that vary in speed");
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;;
            Item.UseSound = SoundID.Item13;
            Item.mana = 2;
            Item.damage = 8;
            Item.shootSpeed = 10;
            Item.knockBack = 1;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<MagicBubble>();
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 2; i++)
            {
                Projectile.NewProjectile(source, position, new Vector2(velocity.X * Main.rand.NextFloat(1, 2.5f), velocity.Y * Main.rand.NextFloat(1, 2.5f)).RotatedByRandom(MathHelper.ToRadians(5)), type, Item.damage, knockback, Owner: player.whoAmI);
            }
            return false;
        }
    }
    class MagicBubble : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Bubble";
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.timeLeft = 60;
            Projectile.penetrate = 1;
            Projectile.hide = false;
        }
        public override void AI()
        {
            Projectile.velocity *= 0.98f;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += target.defense / 10;
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item54, Projectile.position);
        }
    }
}
