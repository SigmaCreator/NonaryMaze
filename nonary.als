open util/ordering[Maze] as ord
open  util/natural as nat

let Two = nat/add[nat/One, nat/One]
--let Three = nat/add[Two, nat/One]
let Four = nat/add[Two, Two]
let Five  = nat/add[Four, nat/One]
--let Six = nat/add[Five, nat/One]
--let Seven = nat/add[Six, nat/One]
--let Eight = nat/add[Seven, nat/One]
let Nine = nat/add[Four, Five]

sig Player {pcode : Natural}{
	gte[pcode,nat/One]
	lte[pcode,Nine]
}

sig Door {dcode : Natural, destination : one Room }{
	gte[dcode,nat/One]
	lte[dcode,Nine]
}

sig Room {doors : set Door,  occupants : set Player}

sig Maze {rooms : set Room,  start, goal : one Room}{
	start != goal
	#goal.doors = 0
	rooms = Room
	all r : rooms { all s : (rooms - r) | #(r.doors & s.doors) = 0}
	no r : rooms | r in r.^(doors.destination)
	no r : rooms - goal | #r.doors = 0
}

-- Verifies that goal is in the non-reflexive transitive closure of room connections of start
fact {Room in first.start.*(doors.destination)}

-- Every room is accessible

-- No door leads to a dead end

-- No two rooms have the same door

-- Every player starts at the beginning
fact PlayersStartAtBeginning { first.start.occupants =  Player}

-- No door leads back to its own room

-- No two players have the same number
fact UniquePlayerCodes { all p1: Player { all p2: (Player-p1) | p1.pcode != p2.pcode } }
-- No two doors have the same number
fact UniqueDoors { all d1: Door {  all d2: (Door-d1) | d1.dcode != d2.dcode } }

-- Every door comes from somewhere
fact DoorsHaveOrigins { all d: Door { one r: Room | d in r.doors }  }

-- There are only 9 players
---- is this the correct way??? yes
fact NPlayers { #Player = 9 }
fact NDoors { #Door = 9 }

-- Every player exists only in one place at a time
fact NoQuantumPlayers { all p: Player { one r: Room | p in r.occupants  } }

fact Room9 { all r : Room | { one d : r.doors | d.dcode = Nine => 
																			#r.doors = 1
																		else
																			#r.doors != 0
} }

-- Only one door leads to the goal
fact Door9 { all d : Door | d.dcode = Nine => 
											d.destination = first.goal 
										 else 
											d.destination in Room - first.goal }

fun Remainder [a : Natural, n : Natural] : Natural { nat/sub[a, nat/mul[n, nat/div[a, n]]] }

fun setsum[nums : set Natural] : lone Natural {
    {n : Natural | #nat/prevs[n] = (sum x : nums | #nat/prevs[x])}
}

fun NatDigitalRoot [ids : set Natural] : Natural { 
	Remainder[setsum[ids], Nine] = nat/Zero => 
		Nine
	else 
		Remainder[setsum[ids], Nine]
}

-- Door-opening Rule : Digital Root
-- fun DigitalRoot [ids : set Int] : Int { 
--	rem[sum[ids], 9] = 0 => 
--		9 
--	else 
--		rem[sum[ids], 9]   
--}


pred TraverseDoor [from, from': set Room] {
	some p : from.occupants | {
		one d : from.doors {
 		( NatDigitalRoot[p.pcode] = d.dcode) => {
				d.destination.occupants = d.destination.occupants + p
				from'.occupants = from.occupants - p
			}
		}
	}	
}

fact Transition {
	all m: Maze, m': m.next {
		first.goal.occupants = Player => {
			one r : m.rooms { one nr : m'.rooms  |  r = nr =>  TraverseDoor[r, nr] }
		}
		m.start = m'.start
		m.goal = m'.goal
	}
}

--assert impossible { Room not in first.start.*(doors.destination)  }
--check impossible for 11 Natural, 9 Player, 9 Door, 3 Room, 1 Maze

assert Valid { last.goal.occupants != Player }
run { }  for 11 Natural, 9 Player, 9 Door, 5 Room, 12 Maze
