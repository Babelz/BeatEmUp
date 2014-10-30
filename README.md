# Massacre Simulator 3000

Kajaanin ammattikorkeakoulun teknologiapajan Isis ryhmän projektityö.


# Collision spec
1. collider on 50% original korkeudesta, tai vähemmän (riippuu miten törmäillään)

2. portaikossa joko AABB tai kolmio
 * kun pelaaja tms. kävelee sinne, ignore collision
 * kun pelaaja kävelee portaikossa x++ = y--
 * kun pelaaja kävelee portaikossa x-- = y++
 
3. Collision tarvii kolmion (portaisiin ilmeisesti, reunukseen jne)

4. Hyppy?
 * sama homma collisionin kans ku portaiden ignore jne.
 
5. Pelaaja ignoree collisionin NPC:dein kanssa (Group = X) 
 * a.k.a kaikki liikkuvat HAHMOT on Groupissa X
