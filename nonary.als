open util/ordering[Maze]
open  util/integer

sig Room {doors: set Door,  occupants: set Player}

one sig Maze {rooms: set Room,  start, goal: one Room}

fact StartNotGoal {first.start != first.goal}

-- verifies that goal is in the non-reflexive transitive closure of room connections of start
fact StartConnectedToEnd { first.goal in first.start.^(doors.destination) }

-- Only accepts mazes with 9 rooms (besides the starting room)
fact MaxSize { #first.start.^(doors.destination) <= 9}
fact MinSize { #first.start.^(doors.destination) >= 9}

-- imples StartConnectedToEnd
fact AllRoomsAccessible { Room in first.start.*(doors.destination)}

sig Door {dcode: Int, destination: one Room }

fact NoRoomHasShareDoors { all r: Room {  all o: (Room-r) | #(o.doors & r.doors) = 0 } } 
fact NoRoomHasNoExit { no r: Room | #r.doors  = 0 }
fact NoDoorLeadsToItsOrigin { no r: Room | r in r.doors.destination  }

sig Player {pcode: Int}

fact NumPlayers { #Player = 9}

-- Every Player starts at the beginning
fact PlayersStartAtBeginning { first.start.occupants =  Player}

fact PlayersNeedSpaceButNotTwoSpace { all p: Player { one r: Room | p in r.occupants  }   }

fun DigitalRoot [ids : set Int] : Int { rem[sum[ids], 9] = 0 =>9 else rem[sum[ids], 9]   }

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

--fact {
--  all s: State, s': s.next {
--    Farmer in s.near =>
--      crossRiver [s.near, s'.near, s.far, s'.far]
--    else
--      crossRiver [s.far, s'.far, s.near, s'.near]
--  }
--}

fact Transition {
	all m: Maze, m': m.next {
		Player & first.goal.occupants != Player => {
			one r: m.rooms { one nr: m'.rooms | TraverseDoor[r, nr ] }
		}
	}
}


assert impossible { Room not in first.start.^(doors.destination)  }
--check impossible for 15

assert Valid { last.goal.occupants = Player }
run { last.goal.occupants = Player }  for 15


