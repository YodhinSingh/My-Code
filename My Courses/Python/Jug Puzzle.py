# All variables that are used in doctests are at the bottom

class Jug: # This class creates a jug with a fill and capacity for parameters, and has a spill method
    def __init__(self,fill,capacity):
        """
        Init creates attributes(fill, capacity) and auto sets the fill to max capacity if fill is greater than
        max capacity or less than zero. Returns None.
        (int,int) -> None
        >>> print(Jug_init_test_1)
        Jug(4,8)
        >>> print(Jug_init_test_2)
        Jug(6,6)
        >>> print(Jug_init_test_3)
        Jug(8,8)

        :param fill:
        :param capacity:
        """
        self.fill = fill
        self.capacity = capacity
        temp = 0
        if not(0 <= self.fill <= self.capacity):
            self.fill = self.capacity

    def spill(self,jug_2):
        """
        Spills fill from one jug to another. Takes parameter of other jug, and transfers all of fill from main
        jug into temp. Temp then transfers as much as possible into other jug, and then transfers remaining back into
        main jug. Returns None.
        (object) -> None
        >>> Jug_spill_test_1.spill(Jug_spill_test_1_1)
        >>> print(Jug_spill_test_1_1)
        Jug(6,6)
        >>> Jug_spill_test_2.spill(Jug_spill_test_2_1)
        >>> print(Jug_spill_test_2)
        Jug(7,8)
        >>> Jug_spill_test_3.spill(Jug_spill_test_3_1)
        >>> print(Jug_spill_test_3_1)
        Jug(9,9)

        :param jug_2:
        :return:
        """
        temp = self.fill
        self.fill = 0
        while (jug_2.fill < jug_2.capacity) and (temp > 0):
            jug_2.fill += 1
            temp -= 1
        if (jug_2.fill == jug_2.capacity):
            self.fill += temp

    def __str__(self):
        """
        Returns the string format of the object. Based on the parameters of the jug, it prints the class name and prints
        the fill and capacity in brackets.
        (self) -> string representation of object
        >>> print(Jug(8,8))
        Jug(8,8)
        >>> print(Jug(-1,8))
        Jug(8,8)
        >>> print(Jug(9,8))
        Jug(8,8)

        :return:
        """
        return "Jug({0},{1})".format(self.fill,self.capacity)

class Jug_puzzle: # This class has the 3 jugs used, a counter for the number of steps, uses input to make moves
                  # and checks if the game is completed.
    def __init__(self):
        """
        Creates the 3 jugs needed for the game by using the Jug class, creates a counter and a boolean to see if the
        game is completed by switching to true if it is.
        (None) -> None
        >>> print(My_puzzle.jugs[0])
        Jug(8,8)
        >>> print(My_puzzle.counter)
        0
        >>> print(My_puzzle.solve)
        False

        """
        self.jugs = [Jug(8, 8), Jug(0, 5), Jug(0, 3)]
        self.counter = 0
        self.solve = False

    def make_move(self): # asks input for what Jug to spill in/out of and uses spill method from Jug class to execute.
        out_of = int(input("Spill from Jug: "))
        into = int(input("Into Jug: "))
        if (0 <= out_of <= (len(self.jugs)-1)) and (0 <= into <= (len(self.jugs)-1)):
            self.jugs[out_of].spill(self.jugs[into])
            self.counter += 1
        else:
            print("sorry, there is no such Jug, please re-choose both your Jugs." + "\n")
            Jug_puzzle.make_move(self)

    def check(self):
        """
        This method checks if the fill of the first 2 jugs are 4 each, and if it is then solve boolean becomes true,
        the loop is broken, the game is completed, and it shows the number of steps taken.
        (None) -> string
        >>> My_puzzle.jugs[0].fill = 4
        >>> My_puzzle.jugs[1].fill = 4
        >>> My_puzzle.check()
        Congratulations, you have solved this puzzle in 0 steps.
        >>> My_puzzle.jugs[0].fill = 5
        >>> My_puzzle.jugs[1].fill = 3
        >>> My_puzzle.check()

        >>> My_puzzle.jugs[0].fill = -1
        >>> My_puzzle.jugs[1].fill = 9
        >>> My_puzzle.check()


        :return:
        """
        if ((self.jugs[0].fill == 4) and (self.jugs[1].fill == 4)):
            self.solve = True
            print("Congratulations, you have solved this puzzle in {0} steps.".format(My_puzzle.counter))

    def intro(self): # This just prints the instructions on the screen before the game begins.
        print("A Jug Puzzle consists of three Jugs (numbered 0,1 and 2) with capacities 8,5 and 3 respectively." + "\n"
        "Initially, jug 0 is full, and the other two are empty. You must spill liquid between the jugs until both "+"\n"
        "jugs 0 and 1 contain 4 units of liquid each. When a player spills between jugs, either one jug will " + "\n"
        "be emptied or one jug will be filled, Begin." + "\n")

    def __str__(self):
        """
        Returns the string format of the object. It prints the number of steps that has been taken with how much each
        jug is filled.
        (self) -> string representation of object
        >>> print(Jug_puzzle())
        0 0: (8/8) 1: (0/5) 2: (0/3)
        <BLANKLINE>

        :return:
        """
        return "{0} 0: ({1}/8) 1: ({2}/5) 2: ({3}/3)".format(self.counter, self.jugs[0].fill, self.jugs[1].fill,
                                                             self.jugs[2].fill) + "\n"


My_puzzle = Jug_puzzle()

Jug_init_test_1 = Jug(4,8)
Jug_init_test_2 = Jug(-1,6)
Jug_init_test_3 = Jug(9,8)
Jug_spill_test_1 = Jug(5,8)
Jug_spill_test_1_1 = Jug(2,6)
Jug_spill_test_2 = Jug(7,8)
Jug_spill_test_2_1 = Jug(9,6)
Jug_spill_test_3 = Jug(-1,8)
Jug_spill_test_3_1 = Jug(3,9)


if __name__ == "__main__":
    My_puzzle.intro() # prints the instructions on the screen
    while My_puzzle.solve == False: # while the solve boolean is False, it will continue to loop
        My_puzzle.make_move() # asks user for input for which jugs to spill into and outof
        print(My_puzzle) # this prints the string representation of object, showing the number of moves, and how much
                         # each jug is filled
        My_puzzle.check() # this checks at the end of each loop if fill of jugs 0,1 = 4, if yes then solve boolean =
                          # true, loop is broken, and prints congratulations with the number of steps taken
