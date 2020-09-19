# Vanguard
NOT ANOTHER FPS GAME!!!!
Vanguard is my Beginner-level Unity FPS project based on a Udemy course from Hollistic3D. Vanguard is the main player character for my Udemy Beginning Unity course. The goal of the course is to come up with a beginner-level zombie shoot'em FPS. I decided to apply my software engineering and 2d3d experience in implementing a beginner-level, decent-looking, 1st person view shoot-em game.

Storyline:
A Viking village somehow got swamped by a pack of zombie clones. There is one alpha zombie leading the pack. Vanguard is an armored human soldier unit suitable for fighting in a nuke, chem, and bio-hazaradous environment. A Vanguard unit, in this mission, is primarily armed with the best zombie-killing weapon known to mankind: a semiautomatic Benelli M4 shotgun with slug shells. If you have doubts about the effectiveness of a slug shell, remember that T-800s can blow holes into T-1000s using these doorbreaching shells. 

The powwer-in-charge decided to drop a lone Vanguard unit to investigate and clear the village of the zombie infestation. I mean, they are just zombies, rite? Initial intel indicates that there are fewer than a dozen zombies, so one Vanguard unit should be enough. Due to the remoteness of the village, dropping supplies is impossible. The hero unit is dropped at the main gate entrance south of the viking village in the early morning. Alongside him is a supply crate filled with shotgun slug shells and medkits Thin fog surrounds the village. There may be shotgun shells and first aid supplies scattered throughout the village. Intel does not know how many may be available.

The zombies attack anything non-zombie organic moving creatures.

Once the village has been cleared of the zombies, proceed to the village's north gate for extraction. Light up a green smoke flare to mark the extraction point.

Technicalities:

This is a beginner level Unity FPS project. Therefore, the gameplay experience will be limited to the knowledge level gained from the Udemy course.

Player gameplay experience:
- Player view is from an FPS camera mounted on the Vanguard unit's head.
- Player shoots one shotgun, a Benelli M4 semi-auto.
- Player sees a round targetting mark in the middle of the screen.
- Player's weapon is always raised. (Weapon lowered state is still up in the air. I prefer Battlefield feel with the coarse crosshair and accurate sight mode. But the weapon is always raised in this experience).
- Shotgun can carry 7 shells
- Player can walk using standard awsd keys. This is the default movement mode. Walking has an animation
- Player runs using walk + left shift. Running has an animation.
- Player jumps using spacebar. Spacebar has an animation.
- Player add shotgun shells using 'R' key. Loading has an animation. Player, by-default, does not auto-reload when the tube magazine is empty (leave this optional)
- Player tilt and pan the FOV using the mouse. Up mouse move lowers the vview elevation. Down mouse move raise the view elevation. Left and right mouse move pans the view azimuth in the same direction.
- Player clicks on the left mouse button to fire the weapon.
- Player runs over supplies to restock shells and patch injuries.
- A player dies as a ragdoll with at least a torn limb.

Shotgun experience:
- The M4 holds 7 rounds.
- The M4 ejects a shell per shot to the right side, if the camera can be configured to view the ejected shots
- No rifle butting. I need to figure out the animation and/or inverse kinematics, which is outside of the beginner scope. (Yeah excuses, excuses!)
- No muzzle flash. I never see muzzle flashes out of a shotgun during skeet shooting sessions.

Zombie experience:
- A zombie random walk by default.
- A zombie walks towards the player if it "sees" the player.
- A zombie charges towards the player to initiage its attack.
- A zombie has 2-3 health points.

Boss experience:
- A boss will have a health bar of 100 damage units. The player needs to deplete this health bar to kill the boss.
- A boss can only walk towards the player.
- A boss does not randown walk. It actively seek the player.
- A boss dies as an animation. Ragdoll is not dramatic enough.

Battle experience:
- The shotgun inflicts 1-4 damage points to the enemies.
- A zombie and boss attack is a melee attack. No projectiles.
- Some zombies can lose limbs and head. Player needs only 1 good shot to tear of a head or a limb.
- Shooting off a limb, or head, instantly kills a zombie, otherwise:
- A player needs 2-3 body shots anywhere to kill a zombie
- Zombies die either as an unanimated ragdoll, or using a death animation.
- A dead zombie will sink under the terrain.
- A zombie inflicts 1 damage point to the Vanguard.
- A boss suffers critical damage on a head or a limb shot. Critical damage amount to be tweaked during testing.
- A boss inflicts 3-10 damage points to the player.
- No building, terrain, or trees battle-damage. (Not yet, but I am definitely interested in collapsing some buildings and falling trees off!).

Foley and sound experience:
- Footsteps on all characters.
- Zombie grunt sound
- Boss howl sound
- Shotgun fire on left mouse click.
- Reload sound during reload
- Splat sound on player's battle damages.

Notes:

Future beginner-level enhancements from the original implementation from Unity are:
1. Addition of a 3rd person camera view. Seems like a waste that I downloaded a nice-looking Vanguard character from Mixamo, only to see its left and right hand most of the time.
2. Using kinematic ragdoll unit for the player. At this beginner stage, I use a ragdoll because I can! Later on, I will add battle damages based on the hits from the player's ragdoll's colliders.
3. Up and down movement is an actual spine tilt, not a simple camera tilt. I do this so the user can train the weapon's elevation more realistically. 
4. Vanguard is a NavMesh agent without a goal. So Unity allows kinematic manual control of the agent. Right now this is superfluous, but I need a Unity engine managed collision rule with the terrain. Otherwise my character might end up climbing over tall vertical wall in one step.
5. Zombies will have crude battle damages to the extent allowed by my beginner-level understanding of Unity. Battle damages will be torn limbs and decapipated heads.
6. All Zombies wll be kinematic ragdolls to facilitate the battle damage behaviors.
In this beginner level, the weapons will use raycaster to project the bullet from the muzzle. Without any physically-based ballistics, 

DISCLAIMER: ONLY ONE PERSON IS WORKING ON THE GAME. HE HAS A BEGINNER-LEVEL UNITY SKILL AT THIS STAGE. LEARNING TAKES TIME! FURTHERMORE, DESPITE THE WIDESPREAD AVAILABILIY OF FREE ARTWORKS ON THE INTERNET, THE SAME PERSON HAS TO SELECT SUITABLE MATERIALS FOR THE GAME AND TWEAK THEM AS NECESSARY. THIS PROCESS TAKES TIME! SO PLEASE TONE DOWN YOU EXPECTATIONS WHEN REVIEWING THIS GAME.
