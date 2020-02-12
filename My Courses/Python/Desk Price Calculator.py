def get_drawers():      # store and return number of drawers
    drawer_name = input("how many drawers?")
    return int(drawer_name)


def get_wood_type():        # store and return the type of wood
    name = input("what wood type?")

    if name == "mahogany" or name == "oak" or name == "pine":
        return name


def calculate_price(get_wood_type, get_drawers):   # calculate cost for drawer: base cost of wood + 30 * num of drawers
    if get_wood_type == "mahogany":
        return int(180 + 30 * get_drawers)
    elif get_wood_type == "oak":
        return int(140 + 30 * get_drawers)
    elif get_wood_type == "pine":
        return int(100 + 30 * get_drawers)


def display_price(calculate_price, get_wood_type, get_drawers):     # displays the cost
    print("Here is your price and specs:")
    if get_drawers == 1:
        print("This desk will cost", calculate_price, "dollars for", get_wood_type, "wood and", get_drawers, "drawer")
    elif get_drawers == 0 or get_drawers > 1:
        print("This desk will cost", calculate_price, "dollars for", get_wood_type, "wood and", get_drawers, "drawers")


if __name__ == '__main__':
    print("This is the beginning of the desk assignment")
    drawer_num = get_drawers()      # get number of drawers user wants
    wood_type = get_wood_type()     # get wood type user wants
    price = calculate_price(wood_type, drawer_num)  # figure out the cost
    display_price(price, wood_type, drawer_num)     # print the info on the console
