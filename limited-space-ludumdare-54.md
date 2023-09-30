= Summary
-Volfied but reveersed, enemys claim spaces. Player has to interrupt enemy to claim that space. When player has not enough space, round ends. At the new round player can spend gained score/gold/coin in shop to buy powerups/weapons. Each weapon/powerup can be used by limited time. Enemies can die but they spawn after 10 seconds. Each round up to 5 min max. Last 1 min rush for enemies, enemies gains boost for speed and shield. Each enemy has different shield timer. Enemies using shield while staying or moving on the edges. If they start to claim the space, shield will run off and be vulnerable. 

= Game Loop

- Player can interrupt enemy claim
- Enemy cant claim little area if player is in it.
-- If player weakened, enemy can claim player's space
-- If player vulnerable, enemy can claim player's space

- Enemy has shield during round for x second.
-- If enemy shield run off when player hit, player can hit enemy
-- If enemy hit player, enemy spawn random location

- Both sides can fire, 4-way shoot
- weapon
- minimap ?
- If enemy claim %60-%80 ?
-- Level 1, enemy must claim %80 of area
-- Level 2, enemy must claim %70 of area
-- Level 3, etc.

vampire survivors exp/upgrade like

= Story
It was a long trip the spaceship "monotros" returned to his planet, "volfied"
but, volfied had already changed into ruins from invasion of aliens
at the time, he cought "sos" from underground he decided to rescue the
people from the aliens.

The aliens were exterminated and the spaceship monotros discovered the one survivor.
They lost all things.. But, they made up their mind to make a fresh start of life
as the new adam & eve of this planet!

= Powerups
- Possible power-ups can increase speed (S), 
- Freeze the enemies for a short time (T), 
- Prevent the shield from counting down for a little while (P), 
- Give a laser to shoot the smaller enemies (L), 
- Kill all the smaller enemies instantly (C) or grant a weapon to kill the boss (a red tomato only found in blocks with the red light).

- ui/ux
- Game Name
- Title Screen
- Game Icon
- 


= Game Difficulity

- Difficulity Factor (Easy, Medium, Hard, Brutal)

- Easy difficulty
-- Enemy speed indicator multiplies by 1
--- If last 1 min current speed indicator multiplies by 1.25
-- Enemy shiled indicator multiplies by 1
-- Enemy power-ups time indicator multiplies by 1
-- Player power-ups time indicator multiplies by 2
-- Player must have %20 unclaimed space

- Medium difficulty
-- Enemy speed indicator multiplies by 1.5
--- If last 1 min current speed indicator multiplies by 1.25
-- Enemy shiled indicator multiplies by 1.5
-- Enemy power-ups time indicator multiplies by 1.5
-- Player power-ups time indicator multiplies by 1.75
-- Player must have %25 unclaimed space

- Hard difficulty
-- Enemy speed indicator multiplies by 2
--- If last 1 min current speed indicator multiplies by 1.50
-- Enemy shiled indicator multiplies by 1.5
-- Enemy power-ups time indicator multiplies by 1.5
-- Player power-ups time indicator multiplies by 1.50
-- Player must have %30 unclaimed space

- Brutal difficulity
-- Enemy speed indicator multiplies by 3
--- If last 1 min current speed indicator multiplies by 2
-- Enemy shiled indicator multiplies by 3
-- Enemy power-ups time indicator multiplies by 1.5
-- Player power-ups time indicator multiplies by 1
-- Player must have %40 unclaimed space