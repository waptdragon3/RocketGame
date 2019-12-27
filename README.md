# RocketGame
A game where you land a rocket my coding its flight computer

![reg] = register (reg)

#[number] = literal (lit)

$[address] = ram (ram)


Registers:

fuel: gettable (remaining fuel percent)

velx: gettable (velocity in the x direction)

vely: gettable (velocity in the x direction)

angv: gettable (angular velocity)

alt: gettable (altitude above bottom of screen (for now))

mass: gettable (total mass including fuel)

TWR: gettable (the thrust to mass ratio) (fwi i know i need to fix it)

ttl: gettable settable (current throttle)

rot: gettable settable (current rotation / target rotation depending on if getting or setting)

grav: gettable (gravity of the landing target)

dt: gettable (the time since the last frame)


Math: (axb = c) where 'x' is the operation

add [reg|lit|ram] [reg|lit|ram] [reg|ram] 

sub [reg|lit|ram] [reg|lit|ram] [reg|ram] 

mul [reg|lit|ram] [reg|lit|ram] [reg|ram] 

div [reg|lit|ram] [reg|lit|ram] [reg|ram] 

mod [reg|lit|ram] [reg|lit|ram] [reg|ram] 

pow [reg|lit|ram] [reg|lit|ram] [reg|ram] 


Flow Control:

jmp [reg|lit|ram] (jumps to the 0 based line)

jlt [reg|lit|ram] [reg|lit|ram] [reg|lit|ram] (jumps if the first argument is less than the second argument)

jgt [reg|lit|ram] [reg|lit|ram] [reg|lit|ram] (jumps if the first argument is greather than the second argument)

jeq [reg|lit|ram] [reg|lit|ram] [reg|lit|ram] (jumps if the first argument is equal to the second argument)


Misc:

wait [reg|lit|ram] (waits for the specified amount of time) (NEEDED TO NOT FREEZE PROGRAM)

set [reg|lit|ram] [reg|lit|ram] (set the first argument to the value of the second)

out [reg|lit|ram] (prints the value of the argument)

hlt (halts the program)

