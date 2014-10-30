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


# Game spec
1. world, gamestate manager, scripti manager ja gameobject manager asuu Game:ssa

# Map spec
1. kartoista tulee objecteja maailmaan

2. Tikkaat vois olla

3. Ruutu ei liiku jos x määrä pelaajia yrittää molempiin suuntiin
 * kun vihreä nuoli tulee, taaksepäin ei pääse
 * mappi generoi näkymättömät seinät oikeille paikoille
 
# World spec
1. piirtojärjestys ylhäältä alaspäin, ota size huomioon, algoritmi = null
 
 
# Gamestate spec
1. Cameran zoom ei saa olla koskaan yli 100%, initial jotain 75%
 * zoomaa max 100%, sitä mukaa missä pelaajat on
 * pelaajien distance = suurin x - pienin x
 
# Gameobject spec
 
## Animation spec
  1. Vaihdettavat aseet, spriterillä käden pos jj jne
  
## Sound spec
 1. Äänet kännissä nauhotetaan/huudetaan mikkiin tms, hahmoille jne.
