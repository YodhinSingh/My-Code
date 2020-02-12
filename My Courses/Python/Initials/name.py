space = " "

def initials():
    answer = input("Type your full name and press enter")
    first_space = answer.find(space)
    second_space = answer.rfind(space)
    (let(answer[0]))
    if first_space:
        (let(answer[first_space + 1]))
    if not(space in answer):
        return ""
    if not(space in answer[first_space+1:]):
        return ""
    if second_space:
        (let(answer[second_space + 1]))

if __name__ == '__main__':
    print("Lets Start")
    from default import let
    your_initials = initials()
    print("These are your initials, enjoy.")
