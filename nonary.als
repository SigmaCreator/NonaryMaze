--open util/ordering[Maze] as ord
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

sig Door {dcode : Natural, destination : one Room }

sig Room {rcode : Natural, doors : set Door,  occupants : set Player}{
	
}

--sig Maze {rooms : set Room,  start, goal : one Room}{
--	rooms = Room
--	all r : rooms { all s : (rooms - r) | #(r.doors & s.doors) = 0}
--	no r : rooms | r in r.^(doors.destination)
--}

-- The start room is not the goal room
--fact StartNotGoal {first.start != first.goal}

-- Verifies that goal is in the non-reflexive transitive closure of room connections of start
--fact {Room in first.start.*(doors.destination)}
--fact {no r : (Room - first.goal) | #r.doors  = 0}

-- Every room is accessible

-- No door leads to a dead end

-- Only accepts mazes with 9 doors
---- is this the correct way???
--fact DoorQuantity { #Door = 9 }

-- No two rooms have the same door

-- Every player starts at the beginning
--fact PlayersStartAtBeginning { first.start.occupants =  Player}

-- No door leads back to its own room
--fact NoReturn { no r: Room | r in r.doors.destination  }

--Acyclic 

--fact Dcodes1 { all d: Door | d.dcode  >=  1 }
--fact Dcodes2 { all d: Door | d.dcode <=  9 }


-- No two doors have the same number
fact UniqueDoors { all d1: Door {  all d2: (Door-d1) | d1.dcode != d2.dcode }    }

-- Every door comes from somewhere
fact DoorsHaveOrigins { all d: Door { one r: Room | d in r.doors }    }


-- There are only 9 players
---- is this the correct way???
fact NumPlayers { #Player = 9 }

--fact Pcodes { all p: Player | p.pcode >= 1 and p.pcode <=9 }

-- No two players have the same number
fact UniquePlayerCodes { all p1: Player {  all p2: (Player-p1) | p1.pcode != p2.pcode }    }

-- Every player exists only in one place at a time
--fact PlayersAreNotOmnipresent { all p: Player { one r: Room | p in r.occupants  }   }

-- Door-opening Rule : Digital Root
fun DigitalRoot [ids : set Int] : Int { rem[sum[ids], 9] = 0 => 9 else rem[sum[ids], 9]   }


--pred TraverseDoor [from, from': set Room] {
--	some p: from.occupants | {
	--	one d: from.doors {
--			( DigitalRoot[p.pcode] = d.dcode) => {
--				d.destination.occupants = d.destination.occupants + p
--				from'.occupants = from.occupants - p
--			}
--		}
--	}	
--}

--fact Transition {
--	all m: Maze, m': m.next {
--		Player & first.goal.occupants != Player => {
--			one r: m.rooms { one nr: m'.rooms  |  r = nr =>  TraverseDoor[r, nr] }
--		}
--	}
--}

--assert impossible { Room not in first.start.*(doors.destination)  }
--check impossible for 4 Int, 9 Player, 9 Door, 3 Room, 1 Maze

--assert Valid { last.goal.occupants != Player }
--run { last.goal.occupants = Player }  for 9

pred exemplo {}
run exemplo for 11

