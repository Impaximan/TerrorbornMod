using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class RollerSkates : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Double tap down while in the air to do a short flip" +
                "\nLanding while in the middle of a flip will boost you forward" +
                "\nDo this repeatedly to achieve ludicrous amounts of speed" +
                "\n'Why do roller skates help you do a flip? IDK just go with it!'");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 50, 0);
        }

        bool heldDown = false;
        int cancleCounterDown = 0;
        int downTaps = 0;

        int dashDelay = 0;

        bool spinning = false;
        bool spinningVisually = false;
        int spinDir = 1;

        int timeSinceFlip = 0;

        public void Dash(Player player)
        {
            if (player.velocity.Y != 0 && !spinning)
            {
                SoundExtensions.PlaySoundOld(SoundID.Item44, player.Center);
                spinDir = Math.Sign(player.velocity.X);
                if (player.velocity.X == 0)
                {
                    spinDir = player.direction;
                }
                if (player.controlRight)
                {
                    spinDir = 1;
                }
                if (player.controlLeft)
                {
                    spinDir = -1;
                }
                spinning = true;
                spinningVisually = true;
                timeSinceFlip = 0;
            }
        }

        public override void UpdateEquip(Player player)
        {
            if (player.dash > 0 || player.HasBuff(BuffID.SolarShield1) || player.HasBuff(BuffID.SolarShield2) || player.HasBuff(BuffID.SolarShield3))
            {
                return;
            }

            if (spinning)
            {
                if (player.velocity.Y == 0)
                {
                    player.velocity.X += spinDir * 7.5f;
                    spinning = false;
                    SoundExtensions.PlaySoundOld(SoundID.Item8, player.Center);
                    TerrorbornSystem.ScreenShake(3f);
                    List<string> textOptions = new List<string>(){
                        { "Nice!" },
                        { "Rad!" },
                        { "Awesome!" },
                        { "Cool!" },
                        { "Woah!" },
                        { "Snazzy!" },
                        { "Huzzah!" }
                    };
                    CombatText.NewText(player.getRect(), Color.RoyalBlue, Main.rand.Next(textOptions), true, false);
                }
            }

            if (spinningVisually)
            {
                player.fullRotation += spinDir * MathHelper.ToRadians(17f);
                player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);

                if (Math.Abs(player.fullRotation) >= MathHelper.ToRadians(360))
                {
                    spinning = false;
                    spinningVisually = false;
                    player.fullRotation = 0f;
                }
            }

            if (dashDelay > 0)
            {
                dashDelay--;
            }

            if (!player.controlDown)
            {
                heldDown = false;
            }
            if (player.controlDown & !heldDown & dashDelay <= 0)
            {
                heldDown = true;
                cancleCounterDown = 20;
                downTaps++;
                if (downTaps >= 2)
                {
                    Dash(player);
                    dashDelay = 30;
                }
            }

            timeSinceFlip++;
            if (timeSinceFlip <= 30)
            {
                player.jumpSpeedBoost += 4f;
            }

            if (cancleCounterDown > 0)
            {
                cancleCounterDown--;
            }
            if (cancleCounterDown <= 0)
            {
                downTaps = 0;
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
}