open util/ordering[Maze]
open  util/integer

sig Player {pcode: Int}

sig Door {dcode: Int, destination: one Room }

sig Room {rcode: Int, doors: set Door,  occupants: set Player}

one sig Maze {rooms: set Room,  start, goal: one Room}

fact MazeContainsAllRooms { first.rooms = Room } 

-- The start room is not the goal room
fact StartNotGoal {first.start != first.goal}

-- Verifies that goal is in the non-reflexive transitive closure of room connections of start
fact StartConnectedToEnd {Room  in first.start.*(doors.destination) }

-- Only accepts mazes with 9 doors
---- is this the correct way???
fact DoorQuantity { #Door = 9 }

-- Every room is accessible
fact AccessibleRooms { Room in first.start.*(doors.destination)}

-- No two rooms have the same door
fact NoSharedDoors { all r: Room {  all o: (Room-r) | #(o.doors & r.doors) = 0 } }

-- No door leads to a dead end
fact NoCulDeSac { no r: Room | #r.doors  = 0 }

-- No door leads back to its own room
fact NoReturn { no r: Room | r in r.doors.destination  }

--fact Dcodes1 { all d: Door | d.dcode >=  1 }
--fact Dcodes2 { all d: Door | d.dcode <=  9 }

-- No two doors have the same number
fact UniqueDoors { all d1: Door {  all d2: (Door-d1) | d1.dcode != d2.dcode }    }

-- No two rooms  have the same number
fact UniqueRoomCodes { all r1: Room {  all r2: (Room-r1) | r1.rcode != r2.rcode }    }


-- Every door comes from somewhere
fact DoorsHaveOrigins { all d: Door { one r: Room | d in r.doors }    }


-- There are only 9 players
---- is this the correct way???
fact NumPlayers { #Player = 9}

--fact Pcodes { all p: Player | p.pcode >= 1 and p.pcode <=9 }

-- No two players have the same number
fact UniquePlayerCodes { all p1: Player {  all p2: (Player-p1) | p1.pcode != p2.pcode }    }

-- Every player starts at the beginning
fact PlayersStartAtBeginning { first.start.occupants =  Player}

-- Every player exists only in one place at a time
fact PlayersAreNotOmnipresent { all p: Player { one r: Room | p in r.occupants  }   }

-- Door-opening Rule : Digital Root
fun DigitalRoot [ids : set Int] : Int { rem[sum[ids], 9] = 0 => 9 else rem[sum[ids], 9]   }

/* At most one item to move from 'from' to 'to' 
pred crossRiver [from, from', to, to': set Object] {
  one x: from | {
    from' = from - x - Farmer - from'.eats
    to' = to + x + Farmer
  }
}*/

pred TraverseDoor [from, from': set Room] {
	some p: from.occupants | {
		one d: from.doors {
			( DigitalRoot[p.pcode] = d.dcode) => {
				d.destination.occupants = d.destination.occupants + p
				from'.occupants = from.occupants - p
			}
		}
	}	
}


fact Transition {
	all m: Maze, m': m.next {
		Player & first.goal.occupants != Player => {
			one r: m.rooms { one nr: m'.rooms  |  r.rcode = nr.rcode =>  TraverseDoor[r, nr] }
		}
	}
}

assert impossible { Room not in first.start.*(doors.destination)  }
check impossible for 9

--assert Valid { last.goal.occupants != Player }
--run { last.goal.occupants = Player }  for 9


