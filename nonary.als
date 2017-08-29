open util/ordering[Maze]

sig Room {doors: set Door,  occupants: set Player}

one sig Maze {rooms: set Room,  start, goal: one Room}

fact StartNotGoal {first.start != first.goal}

-- verifies that goal is in the non-reflexive transitive closure of room connections of start
fact StartConnectedToEnd { first.goal in first.start.^(doors.destination) }

sig Door {dcode: Int, destination: one Room }

sig Player {pcode: Int}

-- Every Player starts at the beginning
fact PlayersStartAtBeginning { first.start.occupants =  Player && first.start.^(doors.destination).occupants = {}  }
