def sales(): # calculates the commission earned based upon the number of sales the user inputs (any number)
    sales_amount = input("how many sales?")
    if int(sales_amount)<=999:
        return(int(sales_amount)*0.03)
    if 2999>= int(sales_amount)>999:
        return(int(sales_amount)*0.035)
    if int(sales_amount)>=3000:
        return(int(sales_amount)*0.045)


def job(): # calculates the base pay based upon the job type the user inputs
    # (input junior, senior,or manager (no quotation marks))
    job_title = input("what is your position?")
    if job_title == "junior":
        return(500)
    if job_title == "senior":
        return(800)
    if job_title == "manager":
        return(1000)

def calculate_pay(job,sales): # calculates the total pay based upon the commission and base pay
    """
    #Doctest does not work even though the entire function works, so right click anywhere else and just click run
    functions project instead of run calculate pay(That is the doc test).

    >>> calculate_pay(500,1000*3.5/100)
    535.0
    >>> calculate_pay(800,1000 *3.5/100)
    835.0
    >>> calculate_pay(1000,1000*3.5/100)
    1035.0

    :param job:
    :param sales:
    :return:
    """
    return(job_type + sales_value)

def display_price(calculate_pay, job, sales): # displays the total pay to the user
    print("Based upon this information,")
    print("You will be payed",calculate_pay,"dollars")


if __name__ == '__main__':
    print("This is the weekly pay calculator.")
    sales_value = sales()
    job_type = job()
    pay = calculate_pay(job, sales)
    display_price(pay, job_type, sales_value)
