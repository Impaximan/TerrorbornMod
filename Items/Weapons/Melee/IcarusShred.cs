using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TerrorbornMod.Items.Weapons.Melee
{
    public class IcarusShred : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Teleports you to a nearby enemy when used" +
                "\nGives you some of your flight time back on hit");
        }
        public override void SetDefaults()
        {
            item.damage = 72;
            item.melee = true;
            item.width = 70;
            item.height = 66;
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = 1;
            item.knockBack = 4f;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = 16;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float distance = 1000;
            NPC target = Main.npc[0];
            bool targetted = false;
            foreach (NPC npc in Main.npc)
            {
                if (!npc.friendly && npc.active && npc.Distance(player.Center) <= distance && npc.CanBeChasedBy())
                {
                    target = npc;
                    distance = npc.Distance(player.Center);
                    targetted = true;
                }
            }

            if (targetted && distance > 200)
            {
                int direction = 1;
                if (player.Center.X < target.Center.X)
                {
                    direction = -1;
                }
                position = target.Center + new Vector2(target.width / 2 + 50, 0) * direction;

                if (Collision.CanHitLine(player.position, player.width, player.height, target.position + new Vector2(target.width / 2 + 50, 0) * direction, target.width, target.height))
                {
                    player.Teleport(position - player.Size / 2);
                    player.velocity = target.velocity;
                }
            }

            return false;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            player.wingTime += (player.wingTimeMax - player.wingTime) / 5;
        }
    }
}