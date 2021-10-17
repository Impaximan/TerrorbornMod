using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class IntimidationAura : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Intimidation Aura");
            Tooltip.SetDefault("Closely dodging hostile enemies and projectiles will grant you terror" +
                "\nGetting hit will cause you to lose a fourth of the terror you have");
        }

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 34;
            item.accessory = true;
            item.rare = 1;
            item.value = Item.sellPrice(0, 4, 0, 0);
        }
        int Cooldown = 60 / 6;
        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.IntimidationAura = true;

            if (Cooldown > 0)
            {
                Cooldown--;
            }

            if (modPlayer.TimeFreezeTime > 0 || modPlayer.VoidBlinkTime > 0)
            {
                return;
            }

            bool CreateDust = false;
            float range = 16 * 6;

            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.friendly && npc.damage > 0 && npc.Distance(player.Center) <= range + (npc.height + npc.width) / 4 && npc.active && Cooldown <= 0)
                {
                    CreateDust = true;
                    modPlayer.GainTerror(2f, false, false, true);
                }
            }

            for (int i = 0; i < Main.projectile.GetUpperBound(0); i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.hostile && projectile.Distance(player.Center) <= range + (projectile.height + projectile.width) / 4 && projectile.active && projectile.damage > 0 && !TerrorbornProjectile.modProjectile(projectile).Intimidated && projectile.timeLeft > 2)
                {
                    CreateDust = true;
                    modPlayer.GainTerror(0.75f, false, false, true);
                    TerrorbornProjectile.modProjectile(projectile).Intimidated = true;
                }
            }

            if (CreateDust)
            {
                Cooldown = 60 / 6;
                Main.PlaySound(SoundID.MaxMana, player.Center);
                DustCircle(player.Center, 180, range, 63, -5, 3f);
            }
        }

        public void DustCircle(Vector2 position, int Dusts, float Radius, int DustType, float DustSpeed, float DustScale = 1f) //Thanks to seraph for this code
        {
            float currentAngle = Main.rand.Next(360);
            for (int i = 0; i < Dusts; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Dusts) * i) + currentAngle));
                direction.X *= Radius;
                direction.Y *= Radius;

                Dust dust = Dust.NewDustPerfect(position + direction, DustType, (direction / Radius) * DustSpeed, 0, default(Color), DustScale);
                dust.noGravity = true;
                dust.noLight = true;
                dust.alpha = 125;
            }
        }
    }
}
