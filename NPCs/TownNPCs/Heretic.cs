using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using ReLogic.Content;
using TerrorbornMod.WeaponPossession;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.Bestiary;


namespace TerrorbornMod.NPCs.TownNPCs
{
    [AutoloadHead]
    public class Heretic : ModNPC
    {
        public override string Texture
        {
            get
            {
                return "TerrorbornMod/NPCs/TownNPCs/Heretic";
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("She's a heretic. That's all you need to know.")
            });
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 23;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 10;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 250;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 5;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.HatOffsetY[NPC.type] = 4;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = 1f,
                Direction = 1
            };

            NPC.Happiness
                 .SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Love)
                 .SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Like)
                 .SetNPCAffection(NPCID.Dryad, AffectionLevel.Dislike)
                 .SetNPCAffection(ModContent.NPCType<SkeletonSheriff>(), AffectionLevel.Hate);
        }
        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 45;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.knockBackResist = 0f;
            AnimationType = NPCID.Guide;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life < 1)
            {
                NPC.life = 1;
            }
        }

        public override bool UsesPartyHat()
        {
            return true;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return TerrorbornSystem.downedIncendiaryBoss;
        }
        public override bool CheckConditions(int left, int right, int top, int bottom)
        {
            return true;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>(){
                "Gabrielle"
            };
        }

        public override ITownNPCProfile TownNPCProfile()
        {
            return new HereticProfile();
        }

        List<string> dialogue = new List<string>();

        bool doingDialogue = false;
        int currentOption1 = 0;
        const int optionCount = 3;
        int loreText = 0;
        int loreTextCount = 5;
        bool showingLore = false;
        bool possessingItem = false;
        int soulIndex = 0;
        List<int> soulsAvailable;

        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (doingDialogue)
            {
                button = Language.GetTextValue("Next");
            }
            else
            {
                if (showingLore)
                {
                    button2 = Language.GetTextValue("Cycle Lore");
                    if (loreText == 0)
                    {
                        button = Language.GetTextValue("Nevermind");
                    }
                    if (loreText == 1)
                    {
                        button = Language.GetTextValue("What types of demons are there?");
                    }
                    if (loreText == 2)
                    {
                        button = Language.GetTextValue("Who leads the demons?");
                    }
                    if (loreText == 3)
                    {
                        button = Language.GetTextValue("What happened to the people who built the seal?");
                    }
                    if (loreText == 4)
                    {
                        button = Language.GetTextValue("How do you know so much about demons?");
                    }
                    return;
                }
                if (possessingItem)
                {
                    button2 = Language.GetTextValue("Cycle Available Soul Types");
                    if (soulIndex == 0)
                    {
                        button = Language.GetTextValue("Nevermind");
                        return;
                    }

                    int type = soulsAvailable[soulIndex - 1];

                    if (type == ItemID.SoulofLight)
                    {
                        button = Language.GetTextValue("Soul of Light");
                    }

                    if (type == ItemID.SoulofNight)
                    {
                        button = Language.GetTextValue("Soul of Night");
                    }

                    if (type == ItemID.SoulofFlight)
                    {
                        button = Language.GetTextValue("Soul of Flight");
                    }

                    if (type == ItemID.SoulofFright)
                    {
                        button = Language.GetTextValue("Soul of Fright");
                    }

                    if (type == ItemID.SoulofMight)
                    {
                        button = Language.GetTextValue("Soul of Might");
                    }

                    if (type == ItemID.SoulofSight)
                    {
                        button = Language.GetTextValue("Soul of Sight");
                    }

                    if (type == ModContent.ItemType<Items.Materials.SoulOfPlight>())
                    {
                        button = Language.GetTextValue("Soul of Plight");
                    }

                    return;
                }
                if (currentOption1 == 0)
                {
                    button = Language.GetTextValue("Possess weapons");
                }
                else if (currentOption1 == 1)
                {
                    button = Language.GetTextValue("Bad advice");
                }
                else if (currentOption1 == 2)
                {
                    button = Language.GetTextValue("Talk");
                }
                button2 = Language.GetTextValue("Cycle Options");
            }
        }
        public override void OnChatButtonClicked(bool firstButton, ref bool openShop)
        {
            Player player = Main.player[Main.myPlayer];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (doingDialogue)
            {
                Main.npcChatText = dialogue[0];
                dialogue.RemoveAt(0);
                if (dialogue.Count <= 0)
                {
                    doingDialogue = false;
                    TerrorbornSystem.talkedToHeretic = true;
                }
            }
            else
            {
                if (firstButton)
                {
                    if (showingLore)
                    {
                        if (loreText == 0) //Nevermind
                        {
                            Main.npcChatText = "Get on with it, I'm busy being edgy.";
                            showingLore = false;
                        }
                        if (loreText == 1) //What types of demons are there?
                        {
                            Main.npcChatText = "I don't know about all of them, but there are generic demons, which are the ones you find in the underworld most of the time, arch demons, which are demons with higher ranks, and dread demons, which represent a soul type and punish sinners.";
                        }
                        if (loreText == 2) //Who leads the demons?
                        {
                            Main.npcChatText = "For an extremely long time, nobody. Currently, also nobody. But for a brief period of time when the humans and an Anekronian used the demons' power, presumably in order to create the seal, a new leader was born. I'm not entirely sure what happened to said leader- all I know is that he was once human and was overtaken by greed and envy, seizing a ton of soul power for himself and ascending to 'divinity'. This only resulted in his very being melting away, not able to contain the power he had gained.";
                        }
                        if (loreText == 3) //What happened to the people who built the seal?
                        {
                            Main.npcChatText = "When they (the Orumians) had indirectly created a new leader for the demons, that leader suddenly became... furious with them, for a reason nobody was quite able to understand. He went into a frenzy, killing his former leaders and leaving his former home without order. Anarchy had begun in Orume, and a terrible infection had taken the kingdom over. TL;DR they all died.";
                        }
                        if (loreText == 4) //How do you know so much about demons?
                        {
                            Main.npcChatText = "I was not always a Heretic like this you know. I was corrupted by the islands, and now I'm kinda... addicted to being demonic, persay. In my brief time there the demons who were attempting to possess me actually taught me quite a bit about how they got there. The energy of the seal, when released, had condensed in the sky in multiple ways, carrying many demons with it. The demons just started building a new home for themselves, right then and there.";
                        }
                        return;
                    }
                    if (possessingItem)
                    {
                        if (soulIndex == 0) //Nevermind
                        {
                            Main.npcChatText = "Get on with it, I'm busy being edgy.";
                            possessingItem = false;
                            return;
                        }

                        int type = soulsAvailable[soulIndex - 1];

                        if (player.HeldItem == null)
                        {
                            Main.npcChatText = "Uh, I can't possess nothing. Sorry. (Make sure you're holding the weapon you want to possess.)";
                            possessingItem = false;
                            return;
                        }

                        if (player.HeldItem.IsAir)
                        {
                            Main.npcChatText = "Uh, I can't possess nothing. Sorry. (Make sure you're holding the weapon you want to possess.)";
                            possessingItem = false;
                            return;
                        }

                        Item item = player.HeldItem;
                        PossessedItem pItem = PossessedItem.modItem(player.HeldItem);

                        if (item == null)
                        {
                            Main.npcChatText = "Uh, I can't possess nothing. Sorry. (Make sure you're holding the weapon you want to possess.)";
                            possessingItem = false;
                            return;
                        }

                        if (item.IsAir)
                        {
                            Main.npcChatText = "Uh, I can't possess nothing. Sorry. (Make sure you're holding the weapon you want to possess.)";
                            possessingItem = false;
                            return;
                        }

                        if (item.DamageType == DamageClass.Summon || item.maxStack != 1 || item.damage == 0 || item.accessory)
                        {
                            Main.npcChatText = "Sorry, I can't possess that item!";
                            possessingItem = false;
                            return;
                        }

                        if (type == ItemID.SoulofLight)
                        {
                            pItem.possessType = PossessType.Light;
                        }

                        if (type == ItemID.SoulofNight)
                        {
                            pItem.possessType = PossessType.Night;
                        }

                        if (type == ItemID.SoulofFlight)
                        {
                            pItem.possessType = PossessType.Flight;
                        }

                        if (type == ItemID.SoulofFright)
                        {
                            pItem.possessType = PossessType.Fright;
                        }

                        if (type == ItemID.SoulofMight)
                        {
                            pItem.possessType = PossessType.Might;
                        }

                        if (type == ItemID.SoulofSight)
                        {
                            pItem.possessType = PossessType.Sight;
                        }

                        if (type == ModContent.ItemType<Items.Materials.SoulOfPlight>())
                        {
                            pItem.possessType = PossessType.Plight;
                        }

                        int amountConsumed = 0;
                        foreach (Item inItem in player.inventory)
                        {
                            if (inItem.type == type)
                            {
                                while (inItem.stack > 0 && amountConsumed < 5)
                                {
                                    amountConsumed++;
                                    inItem.stack--;
                                }
                            }
                        }

                        SoundExtensions.PlaySoundOld(SoundID.NPCDeath52, player.Center);

                        possessingItem = false;
                        item.prefix = (byte)0;
                        Main.npcChatText = "Alright! Your " + item.Name + " has been successfully enhanced. Enjoy it... or something... idk.";
                        return;
                    }
                    if (currentOption1 == 0) //Possess
                    {
                        ResetSoulsAvailable(player, 5);
                        if (soulsAvailable.Count == 0)
                        {
                            Main.npcChatText = "You need at least 5 of a soul type to possess your items. Make sure you're carrying them.";
                        }
                        else
                        {
                            possessingItem = true;
                            soulIndex = 0;
                            Main.npcChatText = "Looking to possess your items eh? Hold the weapon you want to possess and choose a soul type. You need at least 5 of a soul type to possess an Item." +
                                "\n\nItems cannot be reforged and possessed at the same time.";
                        }
                    }
                    else if (currentOption1 == 1) //Bad advice
                    {
                        WeightedRandom<string> text = new WeightedRandom<string>();
                        text.Add("Uhhhh... go... kill something. Or somebody. Or maybe commit mass genocide on the goblins.");
                        text.Add("Become a heretic just like me! Commit as many questionable actions as possible to guarentee your #2 (#1 is mine you can't have it) torture chamber in hell!");
                        text.Add("You should play multiplayer... whatever that means.");
                        text.Add("You should kill your self... NOW!", 0.5f);
                        text.Add("When life gives you pineapples, put them on pizza.");
                        text.Add("The birds and the bears, a powerful force.");
                        text.Add("Buy an NFT! It's a worthwhile investment.");
                        text.Add("Subscribe to Water on terratube.");
                        text.Add("Use bouncy dynamite.");
                        text.Add("Skeletron becomes weaker during the day.");
                        player.AddBuff(ModContent.BuffType<Buffs.BadAdvice>(), 3600 * 15);
                        Main.npcChatText = text;

                    }
                    else if (currentOption1 == 2) //Talk
                    {
                        showingLore = true;
                        Main.npcChatText = "Ummm... okay?";
                    }
                }
                else
                {
                    if (showingLore)
                    {
                        loreText++;
                        if (loreText >= loreTextCount)
                        {
                            loreText = 0;
                        }
                        return;
                    }
                    if (possessingItem)
                    {
                        soulIndex++;
                        if (soulIndex > soulsAvailable.Count)
                        {
                            soulIndex = 0;
                        }

                        if (soulIndex == 0)
                        {
                            Main.npcChatText = "Looking to possess your items eh? Hold the weapon you want to possess and choose a soul type. You need at least 5 of a soul type to possess an Item." +
                                "\n\nItems cannot be reforged and possessed at the same time.";
                            return;
                        }

                        int type = soulsAvailable[soulIndex - 1];
                        WeightedRandom<string> firstText = new WeightedRandom<string>();
                        string text = "";

                        if (type == ItemID.SoulofLight)
                        {
                            text = "Critical hits cause an explosion of light";
                        }

                        if (type == ItemID.SoulofNight)
                        {
                            text = "Lifesteals on critical hits" +
                                "\n-15% damage";
                        }

                        if (type == ItemID.SoulofFlight)
                        {
                            text = "Projectiles can travel through walls" +
                                "\n-15% damage" +
                                "\n-25% velocity";
                        }

                        if (type == ItemID.SoulofFright)
                        {
                            text = "Steals terror from enemies" +
                                "\n-10% damage";
                        }

                        if (type == ItemID.SoulofMight)
                        {
                            text = "Projectiles move 2x as fast" +
                                "\n+50% knockback" +
                                "\n+15% damage";
                        }

                        if (type == ItemID.SoulofSight)
                        {
                            text = "Projectiles will home into enemies" +
                                "\n-15% damage" +
                                "\n-35% velocity";
                        }

                        if (type == ModContent.ItemType<Items.Materials.SoulOfPlight>())
                        {
                            text = "Holding this inflicts you with the 'Midnight Flames' debuff" +
                                "\n+50% damage";
                        }

                        firstText.Add("The bonuses/drawbacks this soul type will provide are:", 0.5);
                        firstText.Add("This soul type has the following possession properties:");
                        firstText.Add("The tradeoff this soul type grants goes as follows:", 0.5);
                        firstText.Add("The tradeoff this soul type will provide is:", 0.5);
                        firstText.Add("The bonuses/drawbacks this soul type grants go as follows:", 0.5);
                        firstText.Add("This soul type will grant a weapon the following:");
                        firstText.Add("This soul type changes the following about a weapon:");
                        Main.npcChatText = firstText + "\n" + text;
                        return;
                    }
                    currentOption1++;
                    if (currentOption1 >= optionCount)
                    {
                        currentOption1 = 0;
                    }
                }
            }
        }

        public void ResetSoulsAvailable(Player player, int requiredAmount)
        {
            soulsAvailable = new List<int>();
            int stackLight = 0;
            int stackNight = 0;
            int stackFlight = 0;
            int stackFright = 0;
            int stackMight = 0;
            int stackSight = 0;
            int stackPlight = 0;
            for (int i = 0; i < player.inventory.Length; i++)
            {
                Item item = player.inventory[i];
                if (item.type == ItemID.SoulofLight)
                {
                    stackLight += item.stack;
                }
                if (item.type == ItemID.SoulofNight)
                {
                    stackNight += item.stack;
                }
                if (item.type == ItemID.SoulofFlight)
                {
                    stackFlight += item.stack;
                }
                if (item.type == ItemID.SoulofFright)
                {
                    stackFright += item.stack;
                }
                if (item.type == ItemID.SoulofMight)
                {
                    stackMight += item.stack;
                }
                if (item.type == ItemID.SoulofSight)
                {
                    stackSight += item.stack;
                }
                if (item.type == ModContent.ItemType<Items.Materials.SoulOfPlight>())
                {
                    stackPlight += item.stack;
                }
            }

            if (stackLight >= requiredAmount)
            {
                soulsAvailable.Add(ItemID.SoulofLight);
            }

            if (stackNight >= requiredAmount)
            {
                soulsAvailable.Add(ItemID.SoulofNight);
            }

            if (stackFlight >= requiredAmount)
            {
                soulsAvailable.Add(ItemID.SoulofFlight);
            }

            if (stackFright >= requiredAmount)
            {
                soulsAvailable.Add(ItemID.SoulofFright);
            }

            if (stackMight >= requiredAmount)
            {
                soulsAvailable.Add(ItemID.SoulofMight);
            }

            if (stackSight >= requiredAmount)
            {
                soulsAvailable.Add(ItemID.SoulofSight);
            }

            if (stackPlight >= requiredAmount)
            {
                soulsAvailable.Add(ModContent.ItemType<Items.Materials.SoulOfPlight>());
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/ExclamationPoint");
            Vector2 position = NPC.Center - new Vector2(0, 65);
            if (doingDialogue)
            {
                spriteBatch.Draw(texture, position: position - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
            }
        }

        public override void PostAI()
        {
            base.PostAI();
            Player player = Main.player[Main.myPlayer];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (!TerrorbornSystem.talkedToHeretic)
            {
                doingDialogue = true;
            }

            if (NPC.Distance(player.Center) >= 500)
            {
                showingLore = false;
                possessingItem = false;
                currentOption1 = 0;
                loreText = 0;
                soulIndex = 0;
            }
        }

        public override string GetChat()
        {
            string shownDialogue = "I'm bugged :D";
            if (doingDialogue)
            {
                dialogue.Clear();
                shownDialogue = "HEY, YOU! Do you own this place!? This is nuts. Unspeakable. Faithful. Impossible! Faithful. Faithful.";
                dialogue.Add("I mean, look at this place. I'm noticing an EXTREME lack of heresy and overall sinning. How could somebody so powerful be so non-evil!");
                dialogue.Add("Oh yeah... I suppose I should explain myself. I'm " + NPC.GivenName + " the enthusiastic heretic. I've been working since I was born to get the #1 most painful spot in hell just for bragging rights.");
                dialogue.Add("I saw you fighting the orumian constructor we over there had hexed and realized that you're an incredibly powerful leader capable of gathering many souls. As such, I quickly tracked down your base.");
                dialogue.Add("Trade offer:\nYou get: to gather souls for me and upgrade your weapons. I get: to use them for unspeakable crimes against the light and also to make your weapons more powerful. Deal? Deal. Good.");
                dialogue.Add("No you may not refuse, you're stuck with me now.");
            }
            else
            {
                WeightedRandom<string> chat = new WeightedRandom<string>();
                chat.Add("Is your software running slow? I may have JUST the update that will do the trick! Have you tried: Demon.");
                chat.Add("The only thing the demons back at the Sisyphean Islands fear is you... especially after you annihilated the Hexed Constructor.");
                chat.Add("Do you have any platinum coins I can borrow real quick? I'm attempting to use magic to duplicate them and destroy the economy.");
                chat.Add("Is murder legal here?");
                chat.Add("SANTA ISN'T REAL. Sorry to crush your dreams.");
                chat.Add("I am going to say the s-word. Scratch.");
                chat.Add("Why SLAM does SLAM everything SLAM go SLAM wrong SLAM for SLAM me SLAM!");
                chat.Add("What was that about pineapple on pizza? Sorry, I'm going to have to crucify you.");
                chat.Add("A hot dog is most DEFINITELY a sandwich.");
                shownDialogue = chat;
            }
            return shownDialogue;
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 5;
            knockback = 7f;
            if (Main.hardMode)
            {
                damage = 20;
                knockback = 10f;
            }
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 0;
            randExtraCooldown = 0;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.BallofFire;
            attackDelay = 1;
        }
        public override void TownNPCAttackShoot(ref bool inBetweenShots)
        {
            inBetweenShots = false;
        }
        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 14f;
            gravityCorrection = 0;
        }
    }
    public class HereticProfile : ITownNPCProfile
    {
        public int RollVariation() => 0;
        public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

        public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
        {
            return ModContent.Request<Texture2D>("TerrorbornMod/NPCs/TownNPCs/Heretic");
        }

        public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot("TerrorbornMod/NPCs/TownNPCs/Heretic_Head");
    }
}