import datetime


"""
Problem Description Fiona commutes to work each day. If there is no rush-hour traffic, her commute time is 2 hours.
However, there is often rush-hour traffic. Specifically, rush-hour traffic occurs from 07:00 (7am) until 10:00 (10am)
in the morning and 15:00 (3pm) until 19:00 (7pm) in the afternoon. During rush-hour traffic, her speed is reduced
by half.
She leaves either on the hour (at XX:00), 20 minutes past the hour (at XX:20), or 40 minutes past the hour (at XX:40).
Given Fiona’s departure time, at what time does she arrive at work?

Input Specification:
The input will be one line, which contains an expression of the form HH:MM, where HH is one of
the 24 starting hours (00, 01, . . ., 23) and MM is one of the three possible departure minute times (00, 20, 40).

Output Specification:
Output the time of Fiona’s arrival, in the form HH:MM.

Sample Input 1
05:00
Output for Sample Input 1
07:00
Explanation for Output for Sample Input 1
Fiona does not encounter any rush-hour traffic, and leaving at 5am, she arrives at exactly 7am.

Sample Input 2
07:00
Output for Sample Input 2
10:30
Explanation for Output for Sample Input 2
Fiona drives for 3 hours in rush-hour traffic, but only travels as far as she normally would after driving for 1.5
hours. During the final 30 minutes (0.5 hours) she is driving in non-rush-hour traffic.

Sample Input 3
23:20
Output for Sample Input 3
01:20
Explanation for Output for Sample Input 3
Fiona leaves at 11:20pm, and with non-rush-hour traffic, it takes two hours to travel, so she arrives at
1:20am the next day

"""

def ask_time():
    answer = input("Please type a time in 24 hr format (HH:MM) to see how much traffic you will face:")
    return str(answer)

def arrival(t):
	n = 0
	current = datetime.datetime.now().time().strftime("%H:%M")
	t = datetime.datetime(100,1 ,1, int(t[:2]),int(t[3:5]),0)
	b = str(t.time())
	x = t + datetime.timedelta(seconds=0)
	g = str(x.time())
	if (("24" >= b[:2] >= "0") and (b[3:5] == "00" or b[3:5] == "20" or b[3:5] == "40")):
		while ((g < "07:00") and (n < 120)):
			x = x + datetime.timedelta(minutes=1)
			g = str(x.time())
			n = n + 1
		while(("06:59" < g < "10:00") and (n < 120)):
			x = x + datetime.timedelta(minutes=2)
			g = str(x.time())
			n = n + 1
		while (("09:59" < g < "15:00") and (n < 120)):
			x = x + datetime.timedelta(minutes=1)
			g = str(x.time())
			n = n + 1
		while (("14:59" < g < "19:00") and (n < 120)):
			x = x + datetime.timedelta(minutes=2)
			g = str(x.time())
			n = n + 1
		while ((g > "18:59") and (n < 120)):
			x = x + datetime.timedelta(minutes=1)
			g = str(x.time())
			n = n + 1
		while ((g < "07:00") and (n < 120)):
			x = x + datetime.timedelta(minutes=1)
			g = str(x.time())
			n = n + 1
	print(str(x.time())[:5])


if __name__ == "__main__":
    ask = ask_time()
    get_time = arrival(ask)

