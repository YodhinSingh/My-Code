import itertools

"""
Problem Description:
Since time immemorial, the citizens of Dmojistan and Pegland have been at war. Now, they have finally signed a truce.
They have decided to participate in a tandem bicycle ride to celebrate the truce. There are N citizens from each
country. They must be assigned to pairs so that each pair contains one person from Dmojistan and one person from Pegland.
Each citizen has a cycling speed. In a pair, the fastest person will always operate the tandem bicycle while the
slower person simply enjoys the ride. In other words, if the members of a pair have speeds a and b, then the bike
speed of the pair is max(a, b). The total speed is the sum of the N individual bike speeds.

For this problem, in each test case, you will be asked to answer one of two questions:
    Question 1: what is the minimum total speed, out of all possible assignments into pairs?
    Question 2: what is the maximum total speed, out of all possible assignments into pairs?

Input Specification:
The first line will contain the type of question you are to solve, which is either 1 or 2.
The second line contains N (1 ≤ N ≤ 100).
The third line contains N space-separated integers: the speeds of the citizens of Dmojistan.
The fourth line contains N space-separated integers: the speeds of the citizens of Pegland.
Each person’s speed will be an integer between 1 and 1 000 000. For 8 of the 15 available marks, questions of type
1 will be asked. For 7 of the 15 available marks, questions of type 2 will be asked.

Output Specification:
Output the maximum or minimum total speed that answers the question asked.

Sample Input
1
1 3
5 1 4
6 2 4
Output for Sample Input 1
12
Explanation for Output for Sample Input 1
There is a unique optimal solution:
Pair the citizen from Dmojistan with speed 5 and the citizen from Pegland with speed 6. Pair the citizen from
Dmojistan with speed 1 and the citizen from Pegland with speed 2. Pair the citizen from Dmojistan with speed 4
and the citizen from Pegland with speed 4.

Sample Input 2
2
3
5 1 4
6 2 4
Output for Sample Input 2
15

Explanation for Output for Sample Input 2
There are multiple possible optimal solutions. Here is one optimal solution:
Pair the citizen from Dmojistan with speed 5 and the citizen from Pegland with speed 2. Pair the citizen from
Dmojistan with speed 1 and the citizen from Pegland with speed 6. Pair the citizen from Dmojistan with speed 4
and the citizen from Pegland with speed 4.

Sample Input 3
2
5
202 177 189 589 102
17 78 1 496 540
Output for Sample Input 3
2016
Explanation for Output for Sample Input 3
There are multiple possible optimal solutions. Here is one optimal solution:
Pair the citizen from Dmojistan with speed 202 and the citizen from Pegland with speed 1.
Pair the citizen from Dmojistan with speed 177 and the citizen from Pegland with speed 540.
Pair the citizen from Dmojistan with speed 189 and the citizen from Pegland with speed 17.
Pair the citizen from Dmojistan with speed 589 and the citizen from Pegland with speed 78.
Pair the citizen from Dmojistan with speed 102 and the citizen from Pegland with speed 496.
This sum yields 202 + 540 + 189 + 589 + 496 = 2016.

"""
s = open("speeds.txt")

def speeds(s):
	x = []
	n = 0
	m = []
	k = []
	w = []
	r = 0
	new_list = []
	final_list = []
	s = open("speeds.txt", "r")
	data = s.readlines()
	s.close()
	s = open("speeds.txt")
	for line in s:
		if line[2:]:
			number = line.split()
			x.append(number)
	y = list(itertools.product(*x))
	if data[0][0] == "2":
		for l in y:
			m.append(max(l))
		for q in m:
			if not(q in k):
				k.append(q)
			n = n + 1
		for e in k:
			k[r] = int(k[r])
			r = r + 1
		while len(w) < int(data[1][0]):
			w.append(int(max(k)))
			k.remove(max(k))
	if data[0][0] == "1":
		combo = x
		int_list = list(itertools.combinations(combo,2))
		for num in int_list:
			if len(int_list):
				f = max(int_list[0])
				if not(f in new_list):
					new_list.append(f)
				combo.remove(int_list[0][0])
				combo.remove(int_list[0][1])
				int_list = list(itertools.combinations(combo,2))
		while r < len(new_list[0]):
			final_list.append(int(new_list[0][r]))
			r = r + 1
		while len(w) < int(data[1][0]):
			w.append(int(min(final_list)))
			final_list.remove(min(final_list))
	print(sum(w))


