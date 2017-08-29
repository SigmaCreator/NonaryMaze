open util/ordering[Maze]

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

assert impossible { Room not in first.start.^(doors.destination)  }
check impossible for 15
