using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TerrorbornMod.NPCs.TownNPCs
{
    [AutoloadHead]
    public class Cartographer : ModNPC
    {
        public override string Texture
        {
            get
            {
                return "TerrorbornMod/NPCs/TownNPCs/Cartographer";
            }
        }

        public override bool Autoload(ref string name)
        {
            name = "Cartographer";
            return mod.Properties.Autoload;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 25;
            NPCID.Sets.ExtraFramesCount[npc.type] = 10;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 700;
            NPCID.Sets.AttackType[npc.type] = 0;
            NPCID.Sets.AttackTime[npc.type] = 90;
            NPCID.Sets.AttackAverageChance[npc.type] = 30;
            NPCID.Sets.HatOffsetY[npc.type] = 4;
            NPCID.Sets.SavesAndLoads[npc.type] = false;
        }

        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 18;
            npc.height = 40;
            npc.aiStyle = 7;
            npc.damage = 10;
            npc.lifeMax = 250;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
            npc.dontTakeDamage = true;
            npc.rarity = 1;
            animationType = NPCID.Guide;
        }

        public override bool UsesPartyHat()
        {
            return false;
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            Player player = Main.player[Player.FindClosest(npc.Center, 0, 0)];

            int biomeKeyCost = Item.buyPrice(3, 50, 0, 0);

            shop.item[nextSlot].SetDefaults(ItemID.Torch);
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 0, 25);
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.ThrowingKnife);
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 0, 15);
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.WoodenArrow);
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 0, 3);
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.Rope);
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 0, 5);
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.Glowstick);
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 0, 5);
            nextSlot++;

            if (NPC.downedPlantBoss)
            {
                shop.item[nextSlot].SetDefaults(ItemID.SuperHealingPotion);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 9, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.SuperManaPotion);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 3, 0);
                nextSlot++;
            }
            else if (NPC.downedBoss3 || Main.hardMode)
            {
                shop.item[nextSlot].SetDefaults(ItemID.GreaterHealingPotion);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 6, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.GreaterManaPotion);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 2, 0);
                nextSlot++;
            }
            else if (NPC.downedBoss1)
            {
                shop.item[nextSlot].SetDefaults(ItemID.HealingPotion);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 3, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.ManaPotion);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 1, 0);
                nextSlot++;
            }
            else
            {
                shop.item[nextSlot].SetDefaults(ItemID.HealingPotion);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 2, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.ManaPotion);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 0, 50);
                nextSlot++;
            }

            shop.item[nextSlot].SetDefaults(ItemID.SpelunkerPotion);
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 3, 0, 0);
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.MiningPotion);
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 1, 50, 0);
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.HunterPotion);
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 2, 0, 0);
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.TrapsightPotion);
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 1, 50, 0);
            nextSlot++;

            if (player.ZoneRockLayerHeight)
            {
                shop.item[nextSlot].SetDefaults(ItemID.LifeCrystal);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 3, 0, 0);
                nextSlot++;
            }

            if (player.ZoneCorrupt)
            {
                shop.item[nextSlot].SetDefaults(ItemID.RottenChunk);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 0, 25);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.WormTooth);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 0, 50);
                nextSlot++;
                if (NPC.downedBoss1)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.DemoniteOre);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 5, 0);
                    nextSlot++;
                }
                if (Main.hardMode)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.CursedFlame);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 50, 0);
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.SoulofNight);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 50, 0);
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.CorruptionKey);
                    shop.item[nextSlot].shopCustomPrice = biomeKeyCost;
                    nextSlot++;
                }
            }

            if (player.ZoneDungeon)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Bone);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 3, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.GoldenKey);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 2, 0, 0);
                nextSlot++;
            }

            if (player.ZoneCrimson)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Vertebrae);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 0, 25);
                nextSlot++;
                if (NPC.downedBoss1)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.CrimtaneOre);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 5, 0);
                    nextSlot++;
                }
                if (Main.hardMode)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.Ichor);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 50, 0);
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.SoulofNight);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 50, 0);
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.CrimsonKey);
                    shop.item[nextSlot].shopCustomPrice = biomeKeyCost;
                    nextSlot++;
                }
            }

            if (player.ZoneSnow)
            {
                shop.item[nextSlot].SetDefaults(ItemID.FrostDaggerfish);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 0, 25);
                nextSlot++;
                if (Main.hardMode)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.FrostCore);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 4, 0, 0);
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.FrozenKey);
                    shop.item[nextSlot].shopCustomPrice = biomeKeyCost;
                    nextSlot++;
                }
            }

            if (player.ZoneHoly)
            {
                shop.item[nextSlot].SetDefaults(ItemID.PixieDust);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 10, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.SoulofLight);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 50, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.RodofDiscord);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 50, 0, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.HallowedKey);
                shop.item[nextSlot].shopCustomPrice = biomeKeyCost;
                nextSlot++;
            }

            if (player.ZoneDesert)
            {
                shop.item[nextSlot].SetDefaults(ItemID.AntlionMandible);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 0, 25);
                nextSlot++;
            }

            if (player.ZoneJungle)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Stinger);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 2, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.JungleSpores);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 2, 0);
                nextSlot++;
                if (Main.hardMode)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.TurtleShell);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 10, 0, 0);
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.JungleKey);
                    shop.item[nextSlot].shopCustomPrice = biomeKeyCost;
                    nextSlot++;
                }
                if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.LifeFruit);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 8, 0, 0);
                    nextSlot++;
                }
            }

            if (NPC.downedBoss1)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Binoculars);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 1, 0, 0);
                nextSlot++;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.AnyNPCs(npc.type) || spawnInfo.player.ZoneUnderworldHeight || TerrorbornWorld.CartographerSpawnCooldown > 0)
            {
                return 0f;
            }
            if (spawnInfo.player.ZoneRockLayerHeight && !TerrorbornWorld.talkedToCartographer)
            {
                return 0.03f;
            }
            return 0.005f;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return false;
        }

        public override bool CheckConditions(int left, int right, int top, int bottom)
        {
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life < 1)
            {
                npc.life = 1;
            }
        }

        public override string TownNPCName()
        {
            return TerrorbornWorld.CartographerName;
        }

        int currentOption1 = 0;
        const int optionCount = 3;
        int loreText = 0;
        int loreTextCount = 5;
        bool showingLore = false;
        bool hasRevealedMap = false;

        List<string> dialogue = new List<string>()
        {
            { "I don't have dialogue right now..." },
            { "...oh well!" }
        };
        bool doingDialogue = true;

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = ModContent.GetTexture("TerrorbornMod/ExclamationPoint");
            Vector2 position = npc.Center - new Vector2(0, 65);
            if (doingDialogue)
            {
                spriteBatch.Draw(texture, position: position - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
            }
        }

        bool start = true;
        public override void PostAI()
        {
            Player player = Main.player[Player.FindClosest(npc.Center, 0, 0)];
            if (player.Distance(npc.Center) > 300)
            {
                showingLore = false;
                loreText = 0;
                currentOption1 = 0;
            }

            if (player.Distance(npc.Center) > 2000)
            {
                npc.active = false;
            }

            if (start)
            {
                start = false;
                npc.GivenName = TerrorbornWorld.CartographerName;
            }

            if (TerrorbornWorld.talkedToCartographer)
            {
                TerrorbornWorld.CartographerSpawnCooldown = 3600 * 36;
            }
        }

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
                        button = Language.GetTextValue("What are the kinds of places you've visited?");
                    }
                    if (loreText == 2)
                    {
                        button = Language.GetTextValue("How did you become a cartographer?");
                    }
                    if (loreText == 3)
                    {
                        button = Language.GetTextValue("What do you know about the infection?");
                    }
                    if (loreText == 4)
                    {
                        button = Language.GetTextValue("What is the most dangerous place you've mapped out?");
                    }
                    return;
                }
                if (currentOption1 == 0)
                {
                    if (hasRevealedMap)
                    {
                        button = Language.GetTextValue("Reveal Nearby Map (already bought)");
                    }
                    else
                    {
                        button = Language.GetTextValue("Reveal Nearby Map (2 gold)");
                    }
                }
                else if (currentOption1 == 1)
                {
                    button = Language.GetTextValue("Shop");
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
            Player player = Main.LocalPlayer;
            if (doingDialogue)
            {
                Main.npcChatText = dialogue[0];
                dialogue.RemoveAt(0);
                if (dialogue.Count <= 0)
                {
                    doingDialogue = false;
                    TerrorbornWorld.talkedToCartographer = true;
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
                            Main.npcChatText = "How can I help you then?";
                            showingLore = false;
                        }
                        if (loreText == 1) //What kinds of places have you visited?
                        {
                            Main.npcChatText = "As surprising as it might be, I haven't actually visited very much around these parts; that's why I've decided to stick around for a bit. I actually come from another island, closer to where Orume was. The anekronian ruins over there were remarkable, definitely worth visiting at some point.";
                        }
                        if (loreText == 2) //What made you decide to try cartography?
                        {
                            Main.npcChatText = "Well, even when I was younger, I very much enjoyed exploring the unknown. When Orume fell, just as my parents had predicted, well... I had no choice to go off on my own. Eventually other adventurers I came across started asking me how much I knew about the surrounding area, and so it only felt natural to start making income off of it. Got to have some source of money, after all. Especially with all the wandering merchants.";
                        }
                        if (loreText == 3) //What do you know about the infection?
                        {
                            Main.npcChatText = "Well... when I saw it with my own eyes, it was... horrific. You'd be surprised how quickly it can travel in certain cases; it was an incredibly odd experience, come to think of it. It felt like it was... chasing me? It was intelligent, and the infected flora around me seemed to have a mind of its own. Bottom line is, I don't know a lot about it, but it was pretty creepy.";
                        }
                        if (loreText == 4) //What is the most dangerous place you've mapped out?
                        {
                            Main.npcChatText = "The Orumian ruins, without a doubt. Due to its infected state, I rarely got an opportunity to rest or even draw things on the map. The environment itself was agressive and constantly shifting, so at the end of the day, I guess I never finished mapping it out. It was an interesting experience, seeing how such a great city fell so easily.";
                        }
                        return;
                    }
                    if (currentOption1 == 0)
                    {
                        if (hasRevealedMap)
                        {
                            WeightedRandom<string> alreadyRevealedMessages = new WeightedRandom<string>();
                            alreadyRevealedMessages.Add("You do know I can't reveal the same area twice, right?");
                            alreadyRevealedMessages.Add("I can't reveal the same area twice, y'know.");
                            alreadyRevealedMessages.Add("...are you sure you just want to give me more money?");
                            alreadyRevealedMessages.Add("Sorry, I can only tell you so much about any given area.");
                            alreadyRevealedMessages.Add("What do you hope to achieve by asking me to show you the same area twice?");
                            Main.npcChatText = alreadyRevealedMessages;
                        }
                        else
                        {
                            if (player.CanBuyItem(Item.buyPrice(0, 2, 0, 0)))
                            {
                                player.BuyItem(Item.buyPrice(0, 2, 0, 0));
                                TerrorbornUtils.RevealMapAroundPlayer(300, player);
                                Main.PlaySound(SoundID.Coins, (int)npc.Center.X, (int)npc.Center.Y, -1);
                                CombatText.NewText(player.getRect(), Color.LightYellow, "Nearby area revealed!");
                                hasRevealedMap = true;

                                WeightedRandom<string> revealingMessages = new WeightedRandom<string>();
                                revealingMessages.Add("Thank you for buying- I wish you luck on your future endeavors.");
                                revealingMessages.Add("Thanks for purchasing, I hope it helps you out!");
                                revealingMessages.Add("Thank you for purchasing, and have a good day!");
                                Main.npcChatText = revealingMessages;
                            }
                            else
                            {
                                WeightedRandom<string> revealingMessages = new WeightedRandom<string>();
                                revealingMessages.Add("I can't help you for free; I have to get money somehow after all.");
                                revealingMessages.Add("Sorry, but I can't do that unless you have enough money.");
                                revealingMessages.Add("I need money to be able to continue making maps like this, so I'm gonna have to charge you.");
                                Main.npcChatText = revealingMessages;
                            }
                        }
                    }
                    else if (currentOption1 == 1)
                    {
                        openShop = true;
                    }
                    else if (currentOption1 == 2)
                    {
                        showingLore = true;
                        WeightedRandom<string> talkMessages = new WeightedRandom<string>();
                        talkMessages.Add("So... what is it then?");
                        talkMessages.Add("What would you like to talk about?");
                        talkMessages.Add("Well... I suppose I have the time to chat. What is it that you would like to talk about?");
                        Main.npcChatText = talkMessages;
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
                    currentOption1++;
                    if (currentOption1 >= optionCount)
                    {
                        currentOption1 = 0;
                    }
                }
            }
        }

        //public override void SetupShop(Chest shop, ref int nextSlot)
        //{

        //}
        public override string GetChat()
        {
            string shownDialogue = "h";
            if (doingDialogue) //First dialogue
            {
                Player player = Main.player[Player.FindClosest(npc.Center, 0, 0)];
                TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
                if (!TerrorbornWorld.talkedToCartographer)
                {
                    dialogue = new List<string>()
                    {
                        { "Hello there! I don't believe we've talked before..." },
                        { "...my name is " + npc.GivenName + ", and I'm a cartographer, with a magnified interest in the supernatural and ancient areas of this world."},
                        { "If you wish, I can fill out the nearby area of your map to help you with your adventures- for a price, of course." },
                        { "Anyways, how can I help you?" }
                    };
                }
                else if (player.ZoneDungeon)
                {
                    dialogue = new List<string>()
                    {
                        { "I never thought I'd get to explore somewhere as crazy as this place, especially since it used to be cursed..." },
                        { "...since you're probably in a hurry, I'll just cut to the chase: how can I help you?" }
                    };
                }
                else if (player.ZoneHoly)
                {
                    if (player.ZoneRockLayerHeight)
                    {
                        dialogue = new List<string>()
                        {
                            { "The crystals of this area are magnificent! Just look at how they shine, it's awe-inducing." },
                            { "Unfortunately, it's kind of ruined by the monsters. Perhaps I can help you navigate with a map?" }
                        };
                    }
                    else
                    {
                        dialogue = new List<string>()
                        {
                            { "Hello! As it turns out, the unicorns from fairy tails aren't anywhere near as graceful and elegant as they were made to be..." },
                            { "...same goes for the pixies. Perhaps, however, one could harness their power to create powerful gear." },
                            { "Anyways, what can I do for you?" }
                        };
                    }
                }
                else if (player.ZoneCorrupt)
                {
                    if (player.ZoneRockLayerHeight && Main.hardMode)
                    {
                        dialogue = new List<string>()
                        {
                            { "This place is terrible! Everything around here either spits flames or actual spit at you. Either way, it's not fun to be around." },
                            { "If you want something, make it quick; I don't want to stick around for much longer." }
                        };
                    }
                    else
                    {
                        dialogue = new List<string>()
                        {
                            { "This place... the way it spreads reminds me of something else. Perhaps the [c/ff0000:infection]?" },
                            { "Regardless, it's probably best not to dilly dally. How does a map sound?" }
                        };
                    }
                }
                else if (player.ZoneCrimson)
                {
                    if (player.ZoneRockLayerHeight && Main.hardMode)
                    {
                        dialogue = new List<string>()
                        {
                            { "This place is even bloodier here than it is at the surface. And now the monsters spit... whatever that is at you!?" },
                            { "If you want something, make it quick; I don't want to stick around for much longer." }
                        };
                    }
                    else
                    {
                        dialogue = new List<string>()
                        {
                            { "This place... the way it consumes everything reminds me of somewhere else. Perhaps the [c/ff0000:infection]?" },
                            { "Regardless, it's probably best not to dilly dally. How does a map sound?" }
                        };
                    }
                }
                else if (player.ZoneSnow)
                {
                    if (player.ZoneRockLayerHeight)
                    {
                        dialogue = new List<string>()
                        {
                            { "Hello again. It's freezing down here! I like the surface of the tundra, but this is getting to be a bit much." },
                            { "Perhaps you'd like me fill your map?" }
                        };
                    }
                    else
                    {
                        dialogue = new List<string>()
                        {
                            { "Hello once more! Isn't the tundra magnificent? Reminds me of my childhood, in a way." },
                            { "Anyways, how can I be of service to you?" }
                        };
                    }
                }
                else if (player.ZoneJungle)
                {
                    if (player.ZoneRockLayerHeight)
                    {
                        dialogue = new List<string>()
                        {
                            { "I've been stung so much by these bloody bugs! It's extremely hard to draw any maps with all of these stings." },
                            { "Bugs aside, I've actually found some interesting ruins around here. It's a great place for archaeology... or at least it would be, if it wasn't so dangerous." },
                            { "...I'm getting sidetracked. How can I help you?" }
                        };
                    }
                    else
                    {
                        dialogue = new List<string>()
                        {
                            { "This... this is actually a pretty nice place to be, ignoring all the predators. I wonder if there are any hidden civilizations living around here."},
                            { "Alas, that probably isn't important to you. How can I be of service?" }
                        };
                    }
                }
                else if (player.ZoneBeach)
                {
                    dialogue = new List<string>()
                    {
                        { "The beach... as nice and calm as it is, there's not much to explore and map around here."},
                        { "Although, I did see something... strange, hopping around in the water. It looked like a... crab?" },
                        { "I'm probably boring you to death, so uh... how can I help you?" }
                    };
                }
                else if (player.ZoneDesert)
                {
                    dialogue = new List<string>()
                    {
                        { "I've gotta get out of here before the bugs, vultures, armadillos, or high temperatures kill me."},
                        { "Are you not suffering from this heat? Perhaps I'm just not properly adjusted." },
                        { "Anyways, just let me fill in your map or something so we can get this over with." }
                    };
                }
                else if (player.ZoneMeteor)
                {
                    dialogue = new List<string>()
                    {
                        { "Woah, this place is crazy... in more ways than one!"},
                        { "Most importantly, the meteorite here. I heard it fall just recently, and by the looks of it, it's still burning; be careful!" },
                        { "Anyways, is there anything I can do to help you?" }
                    };
                }
                else if (modPlayer.ZoneDeimostone)
                {
                    dialogue = new List<string>()
                    {
                        { "These caves... I never thought I'd get to see one of them, but they're actually decently common."},
                        { "From my knowledge, these things were created after an experiment went wrong underground on this island. The people involved lost their minds... both literally and figuratively." },
                        { "Judging by the amount of [c/5d8981:terror], it's very possible this place had something to do with the [c/ff0000:infection]."},
                        { "Nonetheless, I'm sure you don't care... so how can I help you?" }
                    };
                }
                else if (player.ZoneGlowshroom)
                {
                    dialogue = new List<string>()
                    {
                        { "This place is trippy... I feel like I'm gonna go crazy!"},
                        { "If you're gonna buy something, do it quick: I don't wanna go insane!" }
                    };
                }
                else if (player.ZoneSkyHeight)
                {
                    WeightedRandom<List<string>> dialogues = new WeightedRandom<List<string>>();
                    dialogues.Add(new List<string>()
                    {
                        { "It's so floaty up here... and fun to explore! Especially with all the sky islands. Getting back down might be a problem, though." },
                        { "In speaking of problems, are there any I can fix for you?" }
                    });
                    dialogues.Add(new List<string>()
                    {
                        { "Dang... who would've thought that the sky would be filled with annoying bird people? Certainly not me!" },
                        { "Perhaps a map might help you find some sky islands... would you like to purchase one?" }
                    });
                    dialogue = dialogues;
                }
                else if (player.ZoneRockLayerHeight)
                {
                    WeightedRandom<List<string>> dialogues = new WeightedRandom<List<string>>();
                    dialogues.Add(new List<string>()
                    {
                        { "Hello again. Down here spelunking? I'm personally looking for some interesting ruins." },
                        { "How can I help you?" }
                    });
                    dialogues.Add(new List<string>()
                    {
                        { "Why hello there! Have you been attacked by those giant worms? They're the worst!" },
                        { "You know what isn't the worst? My mapmaking skills! Care to take a look?" }
                    });
                    dialogues.Add(new List<string>()
                    {
                        { "STAY BA- oh, it's just you. I thought you were an undead for a moment, heh." },
                        { "Anyways, how can I be of service?" }
                    });
                    dialogue = dialogues;
                }
                else if (Main.dayTime)
                {
                    WeightedRandom<List<string>> dialogues = new WeightedRandom<List<string>>();
                    dialogues.Add(new List<string>()
                    {
                        { "What a nice day outside... great for exploring! Other than the [c/7d7dff:slimes], of course." },
                        { "How can I help you?" }
                    });
                    dialogues.Add(new List<string>()
                    {
                        { "It's a beautiful day outside... birds are chirping... flowers are blooming..." },
                        { "...days like these are perfect for making maps. Would you like to buy one?" }
                    });
                    dialogues.Add(new List<string>()
                    {
                        { "Ah... what a great place to be. There's not a whole lot going on, but it's a good break from the other, more intense areas of the world." },
                        { "Anyways, how can I be of service?" }
                    });
                    dialogue = dialogues;
                }
                else
                {
                    WeightedRandom<List<string>> dialogues = new WeightedRandom<List<string>>();
                    dialogues.Add(new List<string>()
                    {
                        { "Hello again... looking for fallen stars? I know I am! Their magical properties are absolutely amazing." },
                        { "How can I help you?" }
                    });
                    dialogues.Add(new List<string>()
                    {
                        { "I've always wondered why undead go away during the day time... perhaps their skin is too weak for the sun, or perhaps it's something more." },
                        { "Would you like to know more of the things on your map? Because I can sure help you with that." }
                    });
                    dialogues.Add(new List<string>()
                    {
                        { "AAAH- oh, it's just you. I thought you were an undead for a moment, hahah!" },
                        { "Anyways, how can I be of service?" }
                    });
                    dialogue = dialogues;
                }
                shownDialogue = dialogue[0];
                dialogue.RemoveAt(0);
            }
            else //Other chat stuff
            {
                WeightedRandom<string> chat = new WeightedRandom<string>();
                chat.Add("Mapmaking is a great hobby to have, as an explorer. It gives a very nice purpose and motivation to adventuring.");
                chat.Add("I've always wondered why certain colors of slime are more powerful than others. Perhaps it has something to do with the material they're made out of.");
                if (NPC.downedBoss3)
                {
                    chat.Add("I heard the curse of the dungeon was lifted! Was that your doing, perhaps?");
                }
                else
                {
                    chat.Add("You seem like a powerful warrior... have you ever visited the dungeon? I hear the old man there is cursed- maybe you can fix that for him.");
                }
                chat.Add("What do you think the undead do while they're not attacking others. Why? I dunno... suppose I'm just curious.");
                chat.Add("While exploring, I highly recommend you bring some tools to help you see traps. Why, you ask? Well uh... there are a lot of traps.");
                shownDialogue = chat;
            }
            return shownDialogue;
        }


        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 12;
            knockback = 7f;
            if (Main.hardMode)
            {
                damage = 45;
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
            projType = ProjectileID.ThrowingKnife;
            attackDelay = 5;
        }

        public override void TownNPCAttackShoot(ref bool inBetweenShots)
        {
            inBetweenShots = false;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 7f;
            gravityCorrection = 0;
        }
    }
}
