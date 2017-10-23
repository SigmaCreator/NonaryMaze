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

sig Room {doors : set Door, entrances : set Door}

sig Maze {rooms : set Room, start, goal : one Room, occupies : Player-> one Room}{

	-- The starting room is not the goal room
	start != goal

	-- The starting room has no entrance (How did they get in?)
	#start.entrances = 0

	-- The goal room has no exit (Do they really get out?)
	#goal.doors = 0

	-- Every room is part of the maze
	rooms = Room

	-- No room except the goal leads to a dead end / No cul-de-sac
	no r : rooms - goal | #r.doors = 0

	-- All rooms have at most 3 doors
	all r : rooms { #r.doors <= 3 }

	-- Only one room has one door
	one r : rooms | #r.doors = 1

	-- The 9 door is the only one to lead to the goal room
	all r : rooms { 
		all d : r.doors { 
			d.dcode = Nine => 
				d.destination = goal 
			else
				d.destination in rooms - goal
		}
	}
}

-- Every player starts at the starting room
fact Initialization { first.occupies = Player->first.start }

-- Every door is in a room
fact NoFloatingDoors { all d : Door { one r : Room | d in r.doors }  }

-- No two rooms share a door
fact NoSharedDoors { all r : Room { all s : (Room - r) | #(r.doors & s.doors) = 0 } }

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
-- fact EveryoneLeaves { all r : Room - first.start { all d : r.entrances { DigitalRoot[d.dcode] = Nine } } }

-- Every door is one of its destination entrances
fact NoFakeEntrances {	all r : Room { 
					all d : Door {  
						r in d.destination => 
							d in r.entrances 
						else 
							d not in r.entrances 
		} 
	} 
}


-- Get player location
fun where [ m : Maze , p : Player ] : one Room {
	p.(m.occupies)
}

-- Sum of elements in a set of natural numbers
fun SetSum[nums : set Natural] : lone Natural {
    { n : Natural | #nat/prevs[n] = ( sum x : nums | #nat/prevs[x] ) }
}

-- Nines Out Operation
fun NinesOut [a : Natural] : Natural { 
	lt[a,Nine] =>
		a
	else
		NinesOut[nat/sub[a,Nine]]
}

-- Digital Root
fun DigitalRoot [ids : set Natural] : Natural { 
	NinesOut[SetSum[ids]] = nat/Zero => 
		Nine
	else 
		NinesOut[SetSum[ids]]
}


pred Transition [m, m' : Maze, to : Room, p : Player] {
	m'.occupies = m.occupies ++ (p->to)
}

fun validMove [from, to : Room] : Room->Room {
	to in from.doors.destination => 
		from->to
	else 
		from->from	
}

sig Combo { maze : Maze, people : set Player }{
	#people >= 3
	#people <= 5
	one r : Room {
		all p : people | where[maze,p] = r
		one d : r.doors | DigitalRoot[people.pcode] = d.dcode 
	}
}

fact AllMazesCombo { 
	all m : Maze | m.occupies != Player->m.goal =>
		{ some c : Combo | c.maze = m }
	else
		{ no c : Combo | c.maze = m }							  
}

--fact { Comb.people = combination[Nine,Player]} 

--fun combination [ RED : Natural, players : some Player] : some Player {
--	{ p : players |  DigitalRoot[p.pcode] = RED }
--}


--pred AdmissionExam [m, m' : Maze] {
		
--	some r : Room - m.goal {
--		one d : r.doors {
--			all p : Player {
--				where[m,p] = r
				--Transition[m,m',d.destination,combination[d.dcode,p]]
--			}
--		}
--	}

--}

--fact Movement {
--	all m : Maze - last | one m' : m.next | AdmissionExam[m, m']
--}



--assert Valid { last.goal.occupants != Player }

pred AllStart { first.occupies = Player->first.start }

-- check AllStart {} for 35 Natural, exactly 9 Player, exactly 9 Door, 5 Room, 3 Maze, 1 SM

run { }  for 35 Natural, exactly 9 Player, exactly 9 Door, 5 Room, 5 Maze, 10 Combo
