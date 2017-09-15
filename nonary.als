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


sig Test { m : Maze , testers : one Player , roomz : one Room } { 
	roomz = where[m,testers]
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

sig Room {doors : set Door}

sig Maze {rooms : set Room, start, goal : one Room, occupies : Player-> one Room}{

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

	-- No quantum players
}

-- Every player starts at the starting room
fact Initialization { first.occupies = Player->first.start }

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
--fact EveryoneLeaves { all r : Door.destination { some d : Door | r = d.destination => 
--							DigitalRoot[d.dcode] = Nine }	

-- Get player location
fun where [ m : Maze , p : Player ] : one Room {
	p.(m.occupies)
}

	


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

pred TraverseDoor [m, m' : Maze, to : Room, p : Player] {
	m'.occupies = m.occupies ++ (p->to) 
}

--fact { all m : Maze - last | { some r : m.rooms | { some r' : m.next.rooms | r = r' } } }

pred AdmissionExam [m, m' : Maze] {
	one r : m.rooms { 
		one nr : r.doors.destination {
			some p : Player {
			 #p >= 2
			-- There must be at least 1 player in the room
			all p' : p { where[m,p'] = r } 
			-- The destination must not be the current room
			r != nr
			-- The next room must be accessible from the current room
			--p.pcode = Two
			-- The players must be at the destination
			TraverseDoor[m,m',nr,p] 
			}
		}
	 }

}

fact Transition {
	all m : Maze, m' : m.next { 
	 AdmissionExam[m, m'] => m'.occupies = m'.occupies  else
			    m'.occupies = m.occupies 
	}
}



--assert Valid { last.goal.occupants != Player }

pred AllStart { first.occupies = Player->first.start }

-- check AllStart {} for 35 Natural, exactly 9 Player, exactly 9 Door, 5 Room, 3 Maze, 1 SM

run { }  for 35 Natural, exactly 9 Player, exactly 9 Door, 5 Room, 2 Maze, 1 Test
