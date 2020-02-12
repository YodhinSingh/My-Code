# 1. Write a for-loop that prints each list from list food on a separate line. (print_list())
# 2. Write a for-loop that prints the second element of each inner list in list food on a separate line.
#    (print_element_two())
# 3. Write a for-loop that prints out the names of each dessert in food. (print_dessert())
# 4. Write a for-loop that computes the sum of all the costs in food. Costs are the third element of the inner lists.
#    (sum_costs())

food = [["Stuffed Mushrooms", "appetizer", 7], ["Salad", "side", 4], ["Pasta", "main", 10],
        ["Carrot Cake", "dessert", 5], ["Cheesecake", "dessert", 6], ["Sprite", "drink", 3]]

def get_problem():
    n = 0
    while n < 2:
        print("Your choices are: 'menu', 'types of meals', 'all desserts', or 'total cost'")
        answer = input("which problem would you like to solve?")
        answer = answer.lower()
        if answer == '':
            print("Sorry, not an option.")
            n = n + 1
        if answer in "'all desserts' 'types of meals' 'menu' 'total cost'":
            if answer == "all desserts":
                print_dessert()
            if answer == "types of meals":
                print_element_two()
            if answer == "menu":
                print_list()
            if answer == "total cost":
                sum_costs()
            other_answer = input("Would you like to choose another question?")
            other_answer.lower()
            if other_answer.lower() == "yes":
                n = n
            else:
                print("Very well.")
                n = 3
        else:
            print("Sorry, not an option.")
            n = n + 1
    print("Program terminated, have a nice day.")

def print_list():
    n = 0
    print("Options are [meal name, meal type, cost]:")
    for x in food:
        print(food[n])
        n = n + 1

def print_element_two():
    n = 0
    print("The type of each meal is:")
    for x in food:
        print(food[n][1])
        n = n + 1

def print_dessert():
    n = 0
    print("The desserts are:")
    for x in food:
        if food[n][1] == "dessert":
            print(food[n][0])
        n = n + 1

def sum_costs():
    n = 0
    new = 0
    for x in food:
        new = new + food[n][2]
        n = n + 1
    print("The total cost of all meal items are:")
    print("$",new)

if __name__ == "__main__":
    find_problem = get_problem()
