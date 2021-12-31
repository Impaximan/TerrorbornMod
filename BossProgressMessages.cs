using System.IO;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod
{
    class BossProgressMessages : ModWorld
    {
        public static bool TarMessageSent;
        public static bool TideMessageSent;
        public static bool BloodMoonMessageSent;
        public static bool HardmodeMessagesSent;
        public static bool PostAllMechMessagesSent;
        public static bool PostOneMechMessagesSent;
        public static bool PostPlanteraMessagesSent;
        public static bool PostShadowcrawlerMessagesSent;

        public override void Initialize()
        {
            TarMessageSent = false;
            TideMessageSent = false;
            BloodMoonMessageSent = false;
            HardmodeMessagesSent = false;
            PostAllMechMessagesSent = false;
            PostOneMechMessagesSent = false;
            PostPlanteraMessagesSent = false;
            PostShadowcrawlerMessagesSent = false;
        }
        public override TagCompound Save()
        {
            var messages = new List<string>();
            if (TarMessageSent) messages.Add("tar");
            if (TideMessageSent) messages.Add("tide");
            if (BloodMoonMessageSent) messages.Add("bloodmoon");
            if (HardmodeMessagesSent) messages.Add("hardmode");
            if (PostAllMechMessagesSent) messages.Add("postmech");
            if (PostOneMechMessagesSent) messages.Add("postonemech");
            if (PostPlanteraMessagesSent) messages.Add("postplant");
            if (PostShadowcrawlerMessagesSent) messages.Add("postshadowcrawler");

            return new TagCompound {
                {"messages", messages}
            };

        }
        public override void Load(TagCompound tag)
        {
            var messages = tag.GetList<string>("messages");
            TarMessageSent = messages.Contains("tar");
            TideMessageSent = messages.Contains("tide");
            BloodMoonMessageSent = messages.Contains("bloodmoon");
            PostAllMechMessagesSent = messages.Contains("postmech");
            HardmodeMessagesSent = messages.Contains("hardmode");
            PostOneMechMessagesSent = messages.Contains("postonemech");
            PostPlanteraMessagesSent = messages.Contains("postplant");
            PostShadowcrawlerMessagesSent = messages.Contains("postshadowcrawler");
        }

        public override void PostUpdate()
        {
            if (NPC.downedBoss3 && !TarMessageSent)
            {
                TarMessageSent = true;
                Main.NewText("Tar flows through the sandy caverns", 87, 63, 135);
            }
            if (NPC.downedBoss2 && !TideMessageSent && !TerrorbornWorld.downedTidalTitan)
            {
                TideMessageSent = true;
                Main.NewText("The moon pulls on the ocean ever stronger", 94, 116, 227);
            }

            if (NPC.downedBoss1 && !BloodMoonMessageSent)
            {
                BloodMoonMessageSent = true;
                Main.NewText("The blood moon calls to you...", 238, 57, 57);
            }

            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && !PostAllMechMessagesSent)
            {
                PostAllMechMessagesSent = true;
                Main.NewText("An ancient predator haunts the night", Color.Green);
                Main.NewText("The Skeleton Sheriff has new items in his shop!", Color.Yellow);
            }

            if ((NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3) && !PostOneMechMessagesSent)
            {
                PostOneMechMessagesSent = true;
                Main.NewText("The Skeleton Sheriff has new items in his shop!", Color.Yellow);
            }

            if (Main.hardMode && !HardmodeMessagesSent)
            {
                HardmodeMessagesSent = true;
                Main.NewText("A hellish curse invades the heavens!", 236, 165, 133);
                TerrorbornWorld.GenerateIncendiaryBiome(density: 1.5f);
                Main.NewText("The souls released from the wall begin to condense in the sky...", Color.FromNonPremultiplied(40 * 2, 55 * 2, 70 * 2, 255));
                Main.NewText("The Skeleton Sheriff has new items in his shop!", Color.Yellow);
            }

            if (NPC.downedPlantBoss && !PostPlanteraMessagesSent)
            {
                PostPlanteraMessagesSent = true;
                Main.NewText("The Skeleton Sheriff has new items in his shop!", Color.Yellow);
            }

            if (TerrorbornWorld.downedShadowcrawler && !PostShadowcrawlerMessagesSent)
            {
                PostShadowcrawlerMessagesSent = true;
                Main.NewText("With the predator defeated, Midnight Fruit flourishes throughout the world!", Color.LimeGreen);
            }
        }
    }
}
