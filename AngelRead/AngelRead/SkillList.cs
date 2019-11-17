

namespace AngelRead
{
    public class SkillList
    {
        public int CastTime(string nskill)
        {

            string[] stringarray = nskill.Split(' ');
            string skill = "";
            int last = stringarray.Length - 1;
            if (stringarray[last] == "I" || stringarray[last] == "II" || stringarray[last] == "III" || stringarray[last] == "IV" || stringarray[last] == "V" || stringarray[last] == "VI" || stringarray[last] == "VII")
            {
                for (int i = 0; i < last; i++)
                {
                    if (skill != "") skill = skill + " ";
                    skill = skill + stringarray[i];
                }
            }
            else skill = nskill;

            
            //Ranger
            if (skill == "Aiming") return 1120;
            if (skill == "Strong Shots") return 1000;
            if (skill == "Entangling Shot") return 1400;
            if (skill == "Stunning Shot") return 1120;
            if (skill == "Rupture Arrow") return 1240;
            if (skill == "Swift Shot") return 1260;
            if (skill == "Arrow Strike") return 1220;
            if (skill == "Spiral Arrow") return 1000;
            if (skill == "Poison Arrow") return 1200;

            //Templar
            if (skill == "Provoke" || skill == "Taunt") return 600;
            if (skill == "Ferocious Strike") return 1000;
            if (skill == "Shield Bash") return 1000;
            if (skill == "Divine Chastisement") return 700;
            if (skill == "Chastisement of Darkness") return 700;
            if (skill == "Empyrean Armor") return 600;
            if (skill == "Provoking Shield Counter") return 700;
            if (skill == "Robust Blow") return 1000;
            if (skill == "Rage") return 1000;
            if (skill == "Weakening Severe Blow") return 1000;
            if (skill == "Provoking Severe Blow") return 1000;
            if (skill == "Shining Slash") return 1000;
            if (skill == "Wrath Strike") return 1000;
            if (skill == "Shield Counter") return 700;
            if (skill == "Holy Shield") return 700;
            if (skill == "Divine Fury") return 600;
            if (skill == "Inescapable Judgment") return 1000;
            if (skill == "Steel Wall Defense") return 700;
            if (skill == "Avenging Blow") return 700;
            if (skill == "Charge") return 600;
            if (skill == "Provoking Roar") return 600;
            if (skill == "Divine Blow") return 1200;
            if (skill == "Dazing Severe Blow") return 1000;
            if (skill == "Divine Slash" || skill == "Punishment") return 1000;
            if (skill == "Incite Rage") return 1000;
            if (skill == "Face Smash") return 700;
            if (skill == "Hand of Healing") return 700;
            if (skill == "Blunting Severe Blow") return 1000;
            if (skill == "Break Power") return 600;
            if (skill == "Unwavering Devotion") return 600;
            if (skill == "Bodyguard") return 600;
            if (skill == "Judgment") return 600;
            if (skill == "Divine Grasp") return 2000;
            if (skill == "Iron Skin") return 600;
            if (skill == "Siegebreaker") return 600;
            if (skill == "Slash Artery") return 700;
            if (skill == "Shield Retribution") return 700;
            if (skill == "Power of Restoration") return 700;
            if (skill == "Aether Armor") return 600;
            if (skill == "Prayer of Victory") return 700;
            if (skill == "Great Resuscitation" || skill == "Prayer of Resilience") return 700;
            if (skill == "Divine Justice") return 600;
            if (skill == "Magic Smash") return 700;
            if (skill == "Ancestral Holy Punishment") return 700;
            if (skill == "Ancestral Righteous Punishment") return 700;
            if (skill == "Prayer of Freedom") return 600;
            if (skill == "Terrible Howl") return 700;
            if (skill == "Righteous Punishment") return 700;
            if (skill == "Nezekan's Shield") return 700;
            if (skill == "Zikel's Shield") return 700;
            if (skill == "Holy Punishment") return 700;
            if (skill == "Punishment of Light") return 600;
            if (skill == "Punishment of Darkness") return 600;
            if (skill == "Shield of Faith") return 700;
            if (skill == "Empyrean Fury") return 700;

            //Gladiator
            if (skill == "Provoke" || skill == "Taunt") return 600;
            if (skill == "Ferocious Strike") return 800;
            if (skill == "Seismic Wave") return 1300;
            if (skill == "Explosion of Rage") return 1000;
            if (skill == "Wrathful Strike") return 1100;
            if (skill == "Aion's Strength") return 700;
            if (skill == "Robust Blow") return 700;
            if (skill == "Weakening Severe Blow") return 1100;
            if (skill == "Charge") return 700;
            if (skill == "Rage") return 600;
            if (skill == "Rupture") return 1000;
            if (skill == "Cleave") return 900;
            if (skill == "Improved Stamina") return 700;
            if (skill == "Aerial Lockdown") return 1200;
            if (skill == "Crippling Cut") return 700;
            if (skill == "Body Smash") return 1100;
            if (skill == "Severe Weakening Blow") return 1100;
            if (skill == "Defense Preparation") return 700;
            if (skill == "Crashing Blow") return 1200;
            if (skill == "Seismic Billow") return 1300;
            if (skill == "Shock Wave") return 1300;
            if (skill == "Ankle Snare") return 700;
            if (skill == "Vengeful Strike") return 700;
            if (skill == "Lockdown") return 1100;
            if (skill == "Reckless Strike") return 1500;
            if (skill == "Howl") return 700;
            if (skill == "Dauntless Spirit") return 600;
            if (skill == "Stamina Recovery") return 700;
            if (skill == "Pressure Wave") return 600;
            if (skill == "Wall of Steel") return 700;
            if (skill == "Unwavering Devotion") return 700;
            if (skill == "Wrathful Wave") return 600;
            if (skill == "Great Cleave") return 900;
            if (skill == "Righteous Cleave") return 900;
            if (skill == "Force Cleave") return 900;
            if (skill == "Earthquake Wave") return 600;
            if (skill == "Draining Blow") return 700;
            if (skill == "Precision Cut") return 1700;
            if (skill == "Assault Posture") return 600;
            if (skill == "Technical Counter") return 600;
            if (skill == "Sharp Strike") return 600;
            if (skill == "Strengthen Wings") return 600;
            if (skill == "Slaughter") return 700;
            if (skill == "Severe Demolishing Blow") return 2000;
            if (skill == "Severing Strike") return 600;
            if (skill == "Shattering Wave") return 600;
            if (skill == "Piercing Rupture") return 600;
            if (skill == "Blessing of Nezekan") return 600;
            if (skill == "Zikel's Threat") return 600;
            if (skill == "Force Blast") return 600;
            if (skill == "Piercing Wave") return 600;

            //Chanter
            if (skill == "Healing Light") return 2200;
            if (skill == "Victory Mantra") return 750;
            if (skill == "Meteor Strike") return 700;
            if (skill == "Yustiel's Protection") return 600;
            if (skill == "Marchutan's Protection") return 600;
            if (skill == "Shield Mantra") return 750;
            if (skill == "Word of Revival") return 600;
            if (skill == "Booming Strike") return 800;
            if (skill == "Promise of Earth") return 750;
            if (skill == "Magic Mantra") return 750;
            if (skill == "Hallowed Strike") return 600;
            if (skill == "Promise of Wind") return 750;
            if (skill == "Celerity Mantra") return 750;
            if (skill == "Incandescent Blow") return 1000;
            if (skill == "Healing Conduit") return 600;
            if (skill == "Rage Spell") return 600;
            if (skill == "Revival Mantra") return 750;
            if (skill == "Focused Parry") return 900;
            if (skill == "Booming Smash") return 600;
            if (skill == "Protective Ward") return 1000;
            if (skill == "Clement Mind Mantra") return 750;
            if (skill == "Parrying Strike") return 600;
            if (skill == "Promise of Aether") return 750;
            if (skill == "Binding Word") return 1500;
            if (skill == "Pentacle Shock") return 600;
            if (skill == "Word of Protection") return 600;
            if (skill == "Blessing of Health") return 600;
            if (skill == "Booming Assault") return 600;
            if (skill == "Intensity Mantra") return 750;
            if (skill == "Word of Wind") return 600;
            if (skill == "Word of Life") return 600;
            if (skill == "Resonance Haze") return 600;
            if (skill == "Word of Quickness") return 600;
            if (skill == "Protection Mantra") return 750;
            if (skill == "Tremor") return 600;
            if (skill == "Word of Inspiration") return 600;
            if (skill == "Splash Swing") return 600;
            if (skill == "Inescapable Judgment") return 600;
            if (skill == "Soul Strike") return 600;
            if (skill == "Enhancement Mantra") return 750;
            if (skill == "Recovery Spell") return 600;
            if (skill == "Blessing of Rock") return 600;
            if (skill == "Stamina Restoration") return 600;
            if (skill == "Blessing of Wind") return 600;
            if (skill == "Magic Recovery") return 600;
            if (skill == "Swiftwing") return 600;
            if (skill == "Soul Crush") return 600;
            if (skill == "Stilling Word") return 600;
            if (skill == "Word of Spellstopping") return 600;
            if (skill == "Aetheric Field") return 600;
            if (skill == "Mountain Crash") return 600;
            if (skill == "Invincibility Mantra") return 600;
            if (skill == "Divine Curtain") return 600;
            if (skill == "Curtain of Aether") return 600;

            //Cleric
            if (skill == "Root") return 600;
            if (skill == "Pandaemonium's Protection") return 1000;
            if (skill == "Hallowed Strike") return 1200;
            if (skill == "Penance") return 600;
            if (skill == "Cure Mind") return 600;
            if (skill == "Grace of Empyrean Lord") return 600;
            if (skill == "Rebirth") return 6500;
            if (skill == "Smite") return 2300;
            if (skill == "Healing Light") return 2500;
            if (skill == "Heaven’s Judgment") return 1000;
            if (skill == "Divine Touch") return 1100;
            if (skill == "Thunderbolt") return 1000;
            if (skill == "Light of Rejuvenation") return 800;
            if (skill == "Healing Wind") return 4100;
            if (skill == "Summer Circle") return 1400;
            if (skill == "Winter Circle") return 1400;
            if (skill == "Divine Spark") return 800;
            if (skill == "Dispel") return 600;
            if (skill == "Summon Holy Servant") return 600;
            if (skill == "Radiant Cure") return 3600;

            //Assassin
            if (skill == "Rune Knife") return 1000;
            if (skill == "Rune Slash") return 1100;
            if (skill == "Ambush") return 1000;
            if (skill == "Weakening Blow") return 1100;
            if (skill == "Assassination") return 800;
            if (skill == "Fang Strike") return 1000;
            if (skill == "Beast Kick") return 1000;
            if (skill == "Beast Swipe") return 1000;
            if (skill == "Binding Rune") return 800; 
            if (skill == "Crashing Wind Strike") return 700;
            if (skill == "Blood Rune") return 800;
            if (skill == "Pain Rune") return 800;
            if (skill == "Rune Burst") return 800; 
            if (skill == "Needle Rune") return 800;
            if (skill == "Whirlwind Slash") return 1000;

            
            //Warrior
            if (skill == "Ferocious Strike") return 1000;
            if (skill == "Shield Defense") return 500;
            if (skill == "Robust Blow") return 1000;
            if (skill == "Weakening Severe Blow") return 1000;
            if (skill == "Rage") return 1000;
            if (skill == "Shield Counter") return 800;
            //Priest
            if (skill == "Healing Light") return 2500;
            if (skill == "Smite") return 2300;
            if (skill == "Blessing of Health") return 1000;
            if (skill == "Hallowed Strike") return 900;
            if (skill == "Blessing of Rock") return 600;
            if (skill == "Light of Renewal") return 600;
            if (skill == "Infernal Blaze") return 600;
            if (skill == "Promise of Wind") return 600;
            if (skill == "Light of Resurrection") return 6000;
            
            //Sorc
            if (skill == "Delayed Blast") return 3400;
            if (skill == "Flame Cage") return 1800;
            if (skill == "Flame Harpoon") return 3000;
            if (skill == "Freezing Wind") return 1200;
            if (skill == "Aether's Hold") return 3000;
            if (skill == "Blind Leap") return 600;
            if (skill == "Erosion") return 600;

            //SpiritMaster
            if (skill == "Summon Fire Energy") return 1600;
            if (skill == "Summon Wind Servant") return 1600;
            if (skill == "Sandblaster") return 1600;

            //Mage
            if (skill == "Flame Bolt") return 3000;
            if (skill == "Root") return 600;
            if (skill == "Ice Chain") return 2900;
            if (skill == "Blaze") return 700;
            if (skill == "Erosion") return 1000;
            if (skill == "Frozen Shock") return 1200;
            if (skill == "Stone Skin") return 600;
            if (skill == "Absorb Energy") return 600;
            //Scout
            if (skill == "Swift Edge") return 1100;
            if (skill == "Focused Evasion") return 700;
            if (skill == "Surprise Attack") return 700;
            if (skill == "Counterattack") return 800;
            if (skill == "Hide") return 600;
            if (skill == "Soul Slash") return 1000;
            if (skill == "Devotion") return 700;

            return 600;
        }
        
    }
}