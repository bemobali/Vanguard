# Vanguard
Manages C# Vanguard Unity behavior script changes, additions, and deletions
Vanguard is the main player character for my Udemy Beginning Unity course. The goal of the course is to come up with a beginner-level zombie shoot'em FPS. I decided to apply my software engineering and 2d3d experience in implementing a beginner-level, decent-looking, 1st person and 3rd person player characters.

Enhancements from the original implementation from Unity are:
1. Addition of a 3rd person camera view. Seems like a waste that I downloaded a nice-looking Vanguard character from Mixamo, only to see its left and right hand most of the time.
2. Using kinematic ragdoll unit for the player. At this beginner stage, I use a ragdoll because I can! Later on, I will add battle damages based on the hits from the player's ragdoll's colliders.
3. Up and down movement is an actual spine tilt, not a simple camera tilt. I do this so the user can train the weapon's elevation more realistically. 
4. Vanguard is also a NavMesh agent. Right now this is superfluous, but I need a Unity engine managed collision rule with the terrain. Otherwise my character might end up climbing over tall vertical wall in one step.
5. Zombies will have crude battle damages to the extent allowed by my beginner-level understanding of Unity. Battle damages will be torn limbs and decapipated heads.
6. All Zombies wll be kinematic ragdolls to facilitate the battle damage behaviors.
In this beginner level, the weapons will use raycaster to project the bullet from the muzzle. Without any physically-based ballistics, 
