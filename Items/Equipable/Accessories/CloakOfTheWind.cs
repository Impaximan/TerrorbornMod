﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;


namespace TerrorbornMod.Items.Equipable.Accessories
{
    class CloakOfTheWind : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Double tap left or right to dash" +
                "\nDashing right as a Projectile or enemy is near you increases your weapon" +
                "\nuse speed by 10% for 3 seconds");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.defense = 5;
        }

        bool heldRight = false;
        bool heldLeft = false;

        int cancleCounterLeft = 0;
        int cancleCounterRight = 0;
        int rightTaps = 0;
        int leftTaps = 0;

        int dashDelay = 0;
        int dustTime = 0;

        public void OnCloseCall(Player player)
        {
            DustExplosion(player.Center, 0, 10, 25, DustID.GoldFlame, NoGravity: true);
            player.AddBuff(ModContent.BuffType<Windspeed>(), 60 * 3);
            CombatText.NewText(player.getRect(), Color.LightYellow, "Close dodge!", true, false);
        }

        public void Dash(int direction, Player player)
        {
            player.velocity.X = 11.5f * direction;
            Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, player.Center);

            Rectangle closeRectangle = player.getRect();
            int extraWidth = 240;
            int extraHeight = 240;
            closeRectangle.Width += extraWidth;
            closeRectangle.Height += extraHeight;
            closeRectangle.X -= extraWidth / 2;
            closeRectangle.Y -= extraHeight / 2;

            bool intersects = false;
            for (int i = 0; i < 200; i++)
            {
                NPC NPC = Main.npc[i];
                if (!NPC.friendly && NPC.damage > 0 && NPC.getRect().Intersects(closeRectangle) && NPC.active)
                {
                    intersects = true;
                }
            }
            for (int i = 0; i < Main.projectile.GetUpperBound(0); i++)
            {
                Projectile Projectile = Main.projectile[i];
                if (Projectile.hostile && Projectile.getRect().Intersects(closeRectangle) && Projectile.active && Projectile.damage > 0)
                {
                    intersects = true;
                }
            }

            if (intersects)
            {
                OnCloseCall(player);
            }
        }

        public override void UpdateEquip(Player player)
        {
            if (player.dash > 0 || player.HasBuff(BuffID.SolarShield1) || player.HasBuff(BuffID.SolarShield2) || player.HasBuff(BuffID.SolarShield3) || player.mount.Active || player.vortexStealthActive)
            {
                return;
            }

            if (dashDelay > 0)
            {
                dashDelay--;
            }

            if (dustTime > 0)
            {
                dustTime--;
                int dust = Dust.NewDust(player.position, player.width, player.height, DustID.GoldFlame, Scale: 1f);
                Main.dust[dust].velocity = player.velocity;
                Main.dust[dust].noGravity = true;
            }

            if (!player.controlRight)
            {
                heldRight = false;
            }
            if (player.controlRight & !heldRight & dashDelay <= 0)
            {
                heldRight = true;
                cancleCounterRight = 15;
                rightTaps++;
                if (rightTaps >= 2)
                {
                    Dash(1, player);
                    dashDelay = 45;
                    dustTime = 30;
                }
            }

            if (!player.controlLeft)
            {
                heldLeft = false;
            }
            if (player.controlLeft & !heldLeft & dashDelay <= 0)
            {
                heldLeft = true;
                cancleCounterLeft = 15;
                leftTaps++;
                if (leftTaps >= 2)
                {
                    Dash(-1, player);
                    dashDelay = 45;
                    dustTime = 30;
                }
            }

            if (cancleCounterRight > 0)
            {
                cancleCounterRight--;
            }
            if (cancleCounterRight <= 0)
            {
                rightTaps = 0;
            }

            if (cancleCounterLeft > 0)
            {
                cancleCounterLeft--;
            }
            if (cancleCounterLeft <= 0)
            {
                leftTaps = 0;
            }
        }
        public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
        {
            float currentAngle = Main.rand.Next(360);

            //if(Main.netMode!=1){
            for (int i = 0; i < Streams; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
                direction.X *= DustSpeed;
                direction.Y *= DustSpeed;

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }
    }

    public class Windspeed : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Windspeed");
            Description.SetDefault("10% increased weapon use speed");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer mPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetAttackSpeed(DamageClass.Generic) *= 1.1f;
        }
    }
}
