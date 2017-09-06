open util/ordering[Maze] as ord
open util/natural as nat

let Two = nat/add[nat/One, nat/One]
let Three = nat/add[Two, nat/One]
let Four = nat/add[Three, nat/One]
let Five  = nat/add[Four, nat/One]
let Six = nat/add[Five, nat/One]
let Seven = nat/add[Six, nat/One]
let Eight = nat/add[Seven, nat/One]
let Nine = nat/add[Eight, nat/One]
let Ten = nat/add[Nine, nat/One]

sig SM {ids: set Natural, thing : Natural} {
	all n : ids | lte[n,Nine] and gt[n,nat/Zero]
	#ids >= 3 and #ids <= 5
	thing = DigitalRoot[ids]
}
	
sig Player {pcode : Natural}{
	-- Player code is a number in 1~9
	gte[pcode, nat/One]
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

	all r : rooms { #r.doors <= 3 }

	-- Only one room has one door
	one r : rooms | #r.doors = 1

	-- The digital root of the doors in a room is 9
	-- all r : rooms | NatDigitalRoot[r.doors.dcode] = Nine

	-- The 9 door is the only one to lead to the goal room
	all r : rooms | { all d : r.doors | d.dcode = Nine => 
								d.destination = goal 
							  else
								d.destination in rooms - goal
	}
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

-- The starting room and the goal remain the same through maze configurations
fact SameGoal { all m : Maze | first.start = m.start and first.goal = m.goal }

-- Every door set with the same destination has digital root equals to 9
-- fact EveryoneLeaves {  }

-- Sum of elements in a set of natural numbers
fun SetSum[nums : set Natural] : lone Natural {
    { n : Natural | #nat/prevs[n] = ( sum x : nums | #nat/prevs[x] ) }
}

-- Nines Out Operation
fun NinesOut [a : Natural, n : Natural] : Natural { 
	lt[a,n] =>
		a
	else
		NinesOut[nat/sub[a,Nine],Nine]
}

-- Digital Root
fun DigitalRoot [ids : set Natural] : Natural { 
	NinesOut[SetSum[ids], Nine] = nat/Zero => 
		Nine
	else 
		NinesOut[SetSum[ids], Nine]
}

--pred TraverseDoor [from, to : set Room] {
--	some p : from.occupants | {
--		one d : from.doors {
--			( DigitalRoot[p.pcode] = d.dcode) => {
--				from.occupants = from.occupants - p
--				to.occupants = to.occupants + p
--			}
--		}
--	}	
--}




--fact Transition {
--	all m : Maze, m' : m.next {
--		#(m.goal.occupants) = 0 => {
--			some r : m.rooms { some nr : m'.rooms  |  nr in r.doors.destination =>  nr.occupants = r.occupants }
--		}
--	}
--}


assert Valid { last.goal.occupants != Player }


run { }  for 35 Natural, exactly 9 Player, exactly 9 Door, 5 Room, 12 Maze, exactly 1 SM
