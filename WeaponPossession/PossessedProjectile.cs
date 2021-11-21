using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.World.Generation;
using Terraria.ModLoader;
using Terraria.UI;
using TerrorbornMod;
using Terraria.Map;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.ModLoader.IO;

namespace TerrorbornMod.WeaponPossession
{
    class PossessedProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void SetDefaults(Projectile projectile)
        {

        }

        Item originalItem;
        bool start = true;
        public override bool PreAI(Projectile projectile)
        {
            if (projectile.owner == 255)
            {
                return base.PreAI(projectile);
            }
            if (Main.player[projectile.owner].HeldItem.IsAir)
            {
                return base.PreAI(projectile);
            }
            if (start && Main.player[projectile.owner] != null && Main.player[projectile.owner].HeldItem != null && projectile.friendly && !projectile.hostile && !Main.gameMenu)
            {
                start = false;

                Player player = Main.player[projectile.owner];
                TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
                if (player.HeldItem == null)
                {
                    return base.PreAI(projectile);
                }
                Item item = player.HeldItem;
                if (PossessedItem.modItem(item) == null)
                {
                    return base.PreAI(projectile);
                }
                originalItem = item;
                PossessedItem pItem = PossessedItem.modItem(item);

                if (pItem.possessType == PossessType.Might)
                {
                    projectile.extraUpdates = projectile.extraUpdates * 2 + 1;
                }

                if (pItem.possessType == PossessType.Flight)
                {
                    projectile.tileCollide = false;
                    projectile.velocity *= 0.75f;
                }

                if (pItem.possessType == PossessType.Sight)
                {
                    projectile.velocity *= 0.65f;
                }
            }
            return base.PreAI(projectile);
        }

        public override void AI(Projectile projectile)
        {
            base.AI(projectile);

            if (projectile.owner == 255)
            {
                return;
            }

            if (Main.player[projectile.owner].HeldItem.IsAir)
            {
                return;
            }

            if (Main.player[projectile.owner] != null && projectile.friendly && !projectile.hostile && !Main.gameMenu && !projectile.minion && !projectile.sentry && originalItem != null && Main.player[projectile.owner].HeldItem != null)
            {
                Player player = Main.player[projectile.owner];
                TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
                if (originalItem.IsAir)
                {
                    return;
                }
                Item item = originalItem;
                PossessedItem pItem = PossessedItem.modItem(item);

                if (pItem.possessType == PossessType.Flight)
                {
                    projectile.tileCollide = false;
                    if (projectile.Distance(player.Center) >= Main.screenWidth * 1.5f)
                    {
                        projectile.timeLeft = 0;
                    }
                }

                if (pItem.possessType == PossessType.Sight)
                {
                    NPC targetNPC = Main.npc[0];
                    float Distance = 750; //max distance away
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
                        projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(targetNPC.Center).ToRotation(), MathHelper.ToRadians(1f * (projectile.velocity.Length() / 20))).ToRotationVector2() * projectile.velocity.Length();
                    }
                }
            }
        }
    }
}
