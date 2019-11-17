# AngelSuite -- Treasure trove of applications I made while learning C# way back when.

History:
I started in 2009 making a bot to help play Aion after seeing a bot named NoFap and enjoying it. He wanted to get paid pretty hard core and was slow at updating at the time so I wanted to give it a shot. The funny thing is I knew VB/Java/C but not C#. I also didnt know anything about Reverse Engineering or "AI". This was a good chance to learn this stuff though information on how to do it all was somewhat scarce back then.

I started out writing a program to just read the memory of Aion to get basic info like Health, Mana, Location, Experience. I found a little source code online that did the very basics for reading memory and someone gave offsets to read those values. I later started to turn it slowly into a bot once I got the hang of finding addresses of things and how to send a process input keys (later function injection).

This repo is a mash of all my programs I made for Angelsuite way back in the day. Some of them I honestly may not really recall what they do, but Angelsuite was a suite of different applications to go along side Angelbot that I made. Originaly I had Angelbot 1, and later redid a good portion of it and made it more advanced with Angelbot2.

Some of the applications from memory are:

-AB Heal Prototype was a bot for a healer to follow you around. It was tailored for just that. I later I think put that functionality in Angelbot 2 itself.

-AbilityTreeTraversal was a test program to get all the abilites and skills through different pointers and structs

-AggroList as simple, it showed anytime an NPC or Player targetted you, hence Aggro list. It was useful for Pvp sometimes

-AionAPP and AIONBot, don't really recall those

-Aionbotsource was different versions of Angelbot1, before I knew what SVN/Git was...

-AionMemory was a program someone else originally wrote, but I expanded the crap out of. It later morphed into AngelRead which was my general library that I let anyone write programs towards. Petes Radar eventually used it, others might have
-AngelAP, I think something to do with AP Points..
-AngelbotWaypoints has lots of waypoints I made during that time while I botted around
-AngelPVP I don't remember all the features, but obviously a nice tool for PVP mode. I think it was more like a PVP assist against other players
-AngelRead, not sure what version, I put this in its own git repo with latest dates I saw
-AngelSuite looks like logs and ini files of some of my characters along with the template ini
-AngelWings this was one of my favorite programs. It allowed unlimited flight by changing your Z axis up and down. I used to laugh when I pvped (I played a stealther) and if I was loosing the fight I'd just jump and flight away scot free LOL
-AutoSkill I think just a basic program that pressed buttons for you... maybe?
-BuffMe sounds like a basic buffer program to make sure your buffs were always on, some classes you had so many buffs it was hard to remember and watch the timers
-Healer was perhaps full standalone healer, wonder what the Heal Prototyper was, perhaps the prototype to this?
-HPandDPS This was a simple utility to show you how much dmg you took or gave, and what your DPS was. Aion didnt give these stats
-KProcCheck I don't recall this, maybe this was part of my battle with NCSoft on trying to find my program and ban people
-Launcher was a way to get around the banner looking for process names, it'd randomize the bot exe and execute it
-Loadini don't remember, perhaps tutorial on ini files?
-Offsetscanner was neato, I got tired of Aion patching every few days and breaking my bot, so It would take me 1-2 hours to refind all my offsets based on structures and pointers I was familiar with. This helped automate me finding stuff
-Petes Radar, guy worked with me and I incorporated his Radar eventually, was a cool guy
-Rhyno I don't remember what these were, one of the guys I collaborated with at one point was named Rhyno, but its been 10 years... It may have to do with when I gave the source to the mmoelite group we had going and we built a new bot based on Angelbot for another MMO that came out. I recall getting screwed out of the group eventually but oh wells
-SigScan2 this is an awesome tool. I eventually got really tired of looking for offsets even with a helper tool and had this help auto find the offsets based on signatures of Aion's code. I put this in the program to auto find offsets and a few other tricks. This would occasionally not work for 100% of all the features and I still had to manually update signatures occasionally











