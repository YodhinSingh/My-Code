

#include <stdio.h>
#include <string.h>
#include <stdlib.h>

int getSum(char *[], int n);
int getDiff (char **, int n);

// Get args from command line. 1st arg is sum or diff, and based on that either add or subtract all other args

int main(int argc, char *argv[]){
	int i;
	printf("There are %d arguments (excluding '%s')\n", argc-1, argv[0]);

	if (!strcmp(argv[1], "sum")){
		for(i = 2; i < argc-1; i++) {
			printf("%d + ", atoi(argv[i]));
		}
		printf("%d\n", atoi(argv[argc-1]));
		printf("= %d\n", getSum(argv, argc));
	}
	else if (!strcmp(argv[1], "diff")){
		for(i = 2; i < argc-1; i++) {
			printf("%d - ", atoi(argv[i]));
		}
		printf("%d\n", atoi(argv[argc-1]));
		printf("= %d\n", getDiff(argv, argc));
	}
	
 
}

int getSum(char * c[], int n){
	int i, sum = 0;
	for (i = 2; i < n; i++){
		sum += atoi(c[i]);
	}
	return sum;
}

int getDiff (char ** c, int n){
	int i, diff = atoi(*(c+2));
	for (i = 3; i < n; i++){
		diff -= atoi(*(c+i));
	}
	return diff;
}