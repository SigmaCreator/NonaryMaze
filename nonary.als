open util/ordering[Maze] as ord
open util/natural as nat

let Two = nat/add[nat/One, nat/One]
--let Three = nat/add[Two, nat/One]
let Four = nat/add[Two, Two]
let Five  = nat/add[Four, nat/One]
--let Six = nat/add[Five, nat/One]
--let Seven = nat/add[Six, nat/One]
--let Eight = nat/add[Seven, nat/One]
let Nine = nat/add[Four, Five]

sig Player {pcode : Natural}{
	-- Player code is a number in 1~9
	gte[pcode,	nat/One]
	lte[pcode, Nine]
}

sig Door {dcode : Natural, destination : one Room }{
	-- Door code is a number in 1~9
	gte[dcode, nat/One]
	lte[dcode, Nine]
}

sig Room {doors : set Door, occupants : set Player}

sig Maze {rooms : set Room, start, goal : one Room}{

	-- The starting room is not the goal room
	start != goal

	-- The goal room doesn't have doors as it is the end of the maze
	#goal.doors = 0

	-- Every room is part of the maze
	rooms = Room

	-- No room leads to a dead end / No cul-de-sac
	no r : rooms - goal | #r.doors = 0
}



-- Every player starts at the starting room
fact Initialization { first.start.occupants = Player }

-- Every player exists in one place at a time
fact NoQuantumPlayers { all p : Player { one r : Room | p in r.occupants  } }

-- Every door is in a room
fact NoOutsideDoors { all d : Door { one r : Room | d in r.doors }  }




-- No two rooms share a door
fact NoSharedDoors { all r : Room { all s : (Room - r) | #(r.doors & s.doors) = 0}}

-- No room leads back to itself
fact NoReturn { no r : Room | r in r.^(doors.destination) }

-- No two players share the same number
fact UniquePlayerCodes { all p1 : Player { all p2 : (Player-p1) | p1.pcode != p2.pcode } }

-- No two doors share the same number
fact UniqueDoors { all d1: Door {  all d2: (Door-d1) | d1.dcode != d2.dcode } }

-- Every room is accessible
fact AccessibleRooms { Room in first.start.*(doors.destination) }




-- The room with the 9 Door has no other doors
--fact Room9 {
--	all r : Room | { 
--		one d : r.doors | 
--			d.dcode = Nine => 
--				#r.doors = 1
--			else
--				#r.doors != 0
--} }

-- Only the 9 Door leads to the goal room
--fact Door9 { 
--	all d : Door | 
--		d.dcode = Nine => 
--			d.destination = first.goal 
--		else 
--			d.destination in Room - first.goal 
--}

--fact { one d : Door | d.destination = first.goal }

-- Modulo operation : r = a - n * (a/n)
fun Remainder [a : Natural, n : Natural] : Natural { 
	nat/sub[a, nat/mul[n, nat/div[a, n]]] 
}

-- Sum of elements in a set of natural numbers
fun SetSum[nums : set Natural] : lone Natural {
    {n : Natural | #nat/prevs[n] = (sum x : nums | #nat/prevs[x])}
}

-- Digital Root
fun NatDigitalRoot [ids : set Natural] : Natural { 
	Remainder[SetSum[ids], Nine] = nat/Zero => 
		Nine
	else 
		Remainder[SetSum[ids], Nine]
}

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
		first.goal.occupants != Player => {
			one r : m.rooms { one nr : m'.rooms  |  r = nr =>  TraverseDoor[r, nr] }
		}
		m.start = m'.start
		m.goal = m'.goal
	}
}


assert Valid { last.goal.occupants != Player }
run { }  for 11 Natural, exactly 9 Player, exactly 9 Door, 5 Room, 12 Maze
