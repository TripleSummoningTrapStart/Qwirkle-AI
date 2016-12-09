/* Qwirkle Rules 

	same color different shape
	same shape different color
	foreach play there must exist a word such that every letter in the play is in the word
	foreach play every word made must be legal
*/ 

xy(space(X, Y), X, Y).
xy(play(S, _), X, Y) :- xy(S, X, Y).

x(SorP, X) :- xy(SorP, X, _).
y(SorP, Y) :- xy(SorP, _, Y).

isFarLeft(Src, Dest)			:-	x(Src, X1),		x(Dest, X2),	X2 < X1 - 1.
isNextLeft(Src, Dest)			:-	x(Src, X1),		x(Dest, X2),	X2 is X1 - 1.

isFarAbove(Src, Dest)			:-	y(Src, Y1),		y(Dest, Y2),	Y2 < Y1 - 1.
isNextAbove(Src, Dest)			:-	y(Src, Y1),		y(Dest, Y2),	Y2 is Y1 - 1.

isAtVertically(Src, Dest)		:-	y(Src, Y),		y(Dest, Y).
	
lookLeft(S, [[P | _] | B ], N) :-							%jump right one column
			isFarLeft(S, P),
			!,lookLeft(S, B, N).
			
lookLeft(S, [[P | C] | B], N) :-							%switch to near mode
			isNextLeft(S, P),
			!,lookNearLeft(S, [[P | C] | B], N).
			
lookLeft(S, B, N) :- lookMaybeUp(S, B, N).						%switch to up mode

lookNearLeft(S, [[P | C] | B], N) :-						%jump down one cell
			isFarAbove(S, P),
			!,lookNearLeft(S, [C | B], N).

lookNearLeft(S, [[P | C] | B], N) :-						%jump down one cell
			isNextAbove(S, P),
			!,lookNearLeft(S, [C | B], N).

lookNearLeft(S, [[P | _] | B], [tuple(left, P) | N]) :-					%is left neighbor
			isAtVertically(S, P),
			!,lookUp(S, B, N).

lookNearLeft(S, [_ | B], N) :- lookUp(S, B, N).		%correct column, went to far down because there is no left neoghbor

lookMaybeUp(S, [[P | C] | B], N) :-
			isFarLeft(P, S),
			!, lookRight(S, [[P | C] | B], N).
			
lookMaybeUp(S, [[P | C] | B], N) :-
			isNextLeft(P, S),
			!, lookRight(S, [[P | C] | B], N).

lookMaybeUp(S, B, N) :- lookUp(S, B, N).			
			
lookUp(S, [[P | C] | B], N) :-
			isFarAbove(S, P),
			!, lookUp(S, [C | B], N).

lookUp(S, [[P | C] | B], [tuple(up, P) | N]) :-
			isNextAbove(S, P),
			!, lookCenter(S, [C | B], N).

lookUp(S, B, N) :- lookCenter(S, B, N).


lookCenter(S, [[P | C] | B], N) :-
			isFarAbove(P, S),
			!, lookDown(S, [[P | C] | B], N).
		
lookCenter(S, [[P | C] | B], N) :-
			isNextAbove(P, S),
			!, lookDown(S, [[P | C] | B], N).
			
lookCenter(S, [[] | B], N) :-
			!, lookRight(S, B, N).
			
lookCenter(S, [], N) :-
			lookRight(S, [], N).
		
lookDown(S,	[[P | _] | B], [tuple(down, P) | N]) :-
			isNextAbove(P, S),
			!, lookRight(S, B, N).

lookDown(S, [_ | B], N) :-
			lookRight(S, B, N).

lookRight(S, [[P | C] | B], N) :-
			isNextLeft(P, S),
			!, lookNearRight(S, [[P | C] | B], N).

lookRight(_, _, []).			
			 
lookNearRight(S, [[P | C] | B], N) :-
			isFarAbove(S, P),
			!,lookNearRight(S, [C | B], N).

lookNearRight(S, [[P | C] | B], N) :-						%jump down one cell
			isNextAbove(S, P),
			!,lookNearRight(S, [C | B], N).

lookNearRight(S, [[P | _] | _], [tuple(right , P)]) :-					%is left neighbor
			isAtVertically(S, P),
			!.

lookNearRight(_,_,[]).
			
b([
[play(space(1,11), tile(cross,yellow))],
[play(space(2,11), tile(cross,orange)),play(space(2,15), tile(circle,yellow))],
[play(space(3,11), tile(cross,purple)),play(space(3,13), tile(clover,purple)),play(space(3,15), tile(circle,blue))],
[play(space(4,11), tile(cross,green)),play(space(4,13), tile(clover,green)),play(space(4,15), tile(circle,green))],
[play(space(5,10), tile(diamond,red)),play(space(5,11), tile(cross,red)),play(space(5,12), tile(star,red)),play(space(5,13), tile(clover,red)),play(space(5,15), tile(circle,red)),play(space(5,16), tile(star,red)),play(space(5,17), tile(cross,red))],[play(space(6,10), tile(diamond,blue)),play(space(6,12), tile(star,orange)),play(space(6,13), tile(clover,orange)),play(space(6,14), tile(square,orange)),play(space(6,15), tile(circle,orange)),play(space(6,17), tile(cross,green))],[play(space(7,11), tile(diamond,blue)),play(space(7,12), tile(star,blue)),play(space(7,14), tile(square,green))],[play(space(8,12), tile(star,green)),play(space(8,13), tile(cross,green)),play(space(8,17), tile(diamond,blue))],[play(space(9,13), tile(cross,orange)),play(space(9,14), tile(clover,orange)),play(space(9,15), tile(circle,orange)),play(space(9,16), tile(square,orange)),play(space(9,17), tile(diamond,orange))],[play(space(10,16), tile(square,green))],[play(space(11,16), tile(square,red)),play(space(11,17), tile(clover,red))]]).

isGap(S, B, N) :-
	lookLeft(S, B, N).
	
%is up down and don't clash
%is left right and don't clash
	
conflicts(N) :-
	get(N, up, play(_, Up)),
	get(N, down, play(_, Down)),
	not(singleShape(Up, Down)),
	not(singleColor(Up, Down)).

conflicts(N) :-
	get(N, left, play(_, Left)),
	get(N, right, play(_, Right)),
	not(singleShape(Left, Right)),
	not(singleColor(Left, Right)).

	
singleColor(tile(S1, C), tile(S2, C)) :-
	not(S1 == S2).


singleShape(tile(S, C1), tile(S, C2)) :-
	not(C1 == C2).


	
get([tuple(K, V) | _], K, V) :- !.
get([_ | D], K, V) :- get(D, K, V).	
	
spaces(Xmin, Xmax, Ymin, Ymax, space(X, Y)) :-
	between(Xmin, Xmax, X),
	between(Ymin, Ymax, Y).
	
fuckingGapPred(S, B, N) :-
	spaces(0,15,0,29,S),
	isGap(S, B, N),
	length(N, L),
	L > 0,
	not(conflicts(N)).
	
lfgp(S,N) :-
	b(B),
	fuckingGapPred(S, B, N).
	
slfgp(tuple(S, N)) :- lfgp(S, N).

% findAll(tuple(S, N), fuckingGapPred(S, "your fucking literal here", N), L).
 
/* Create hand play permutations 
getPlays(Hand, Board, Plays) :-
		forall(fuckingGapPred(S, Board, N), permuteGaps(Hand, Board, S, N, Plays)),		 % find gaps and insert permutations iteratively into gaps and check validity
 
permuteGaps(Hand, Board, S, N, Plays) :- 
		forall()
		
checkPlay()
*/
% Donnoe <------------------------------------------------------------------------
% Use: comb(InList, Output).
comb(InList,Out) :-
    splitSet(InList,_,SubList),
    SubList = [_|_],     /* disallow empty list */
    permute(SubList,Out).

splitSet([ ],[ ],[ ]).
splitSet([H|T],[H|L],R) :-
    splitSet(T,L,R).
splitSet([H|T],L,[H|R]) :-
    splitSet(T,L,R).

permute([ ],[ ]) :- !.
permute(L,[X|R]) :-
    omit(X,L,M),
    permute(M,R).

omit(H,[H|T],T).
omit(X,[H|L],[H|R]) :-
    omit(X,L,R).

 
/* Is Legal */ 
isLegal(T, Result) :- isSingleColumn(T), !, isLegalTile(T, Result), !.
isLegal(T, Result) :- isSingleRow(T), !, isLegalTile(T, Result), !. 
isLegalTile(T, Result) :- isSingleShape(T), !, isLegalShape(T, Result), !.
isLegalTile(T, Result) :- isSingleColor(T), !, isLegalColor(T, Result), !.

	
isLegalColor([play(space(_,_), tile(S, _)) | Rest], Result) :- 
		notContainsShape(Rest, S),
		!,
		isLegalColor(Rest, Result).
isLegalColor([play(space(_,_), tile(S, _))], true).
isLegalColor([], true).

isLegalShape([play(space(_,_), tile(_, C)) | Rest], Result) :-
		notContainsColor(Rest, C),
		!,
		isLegalShape(Rest, Result).
isLegalShape([play(space(_,_), tile(_, C))], true).
isLegalShape([], true).

		
/* Contains */
notContainsColor([play(space(_,_), tile(_, C))], Color) :-
		C \= Color.
notContainsColor([play(space(_,_), tile(_, C)) | Rest], Color) :- 
		C \= Color,
		!,
		notContainsColor(Rest, Color).

notContainsShape([play(space(_,_), tile(S, _))], Shape) :-
		S \= Shape.
notContainsShape([play(space(_,_), tile(S, _)) | Rest], Shape) :- 
		S \= Shape,
		!,
		notContainsShape(Rest, Shape).
		
/* Single Type */
isSingleColor([]).
isSingleColor([play(space(_,_), tile(_, _))]) :- !.
isSingleColor([play(space(_,_), tile(_, C1)), play(space(_,_), tile(_, C2))]) :- C1 == C2.
isSingleColor([play(space(_,_), tile(_, C1)), play(space(_,_), tile(S2, C2)) | Rest]) :- C1 == C2, !, isSingleColor([play(space(_,_), tile(S2,C2)) | Rest]).

isSingleShape([]).
isSingleShape([play(space(_,_), tile(_, _))]) :- !.
isSingleShape([play(space(_,_),tile(S1, _)), play(space(_,_),tile(S2, _))]) :- S1 == S2.
isSingleShape([play(space(_,_),tile(S1, _)), play(space(_,_),tile(S2, C2)) | Rest]) :- S1 == S2, !, isSingleShape([play(space(_,_),tile(S2,C2)) | Rest]).

isSingleRow([]).
isSingleRow([play(space(X1,_), tile(_, _))]) :- !.
isSingleRow([play(space(X1,_),tile(_, _)), play(space(X2,_),tile(_, _))]) :- X1 == X2.
isSingleRow([play(space(X1,_),tile(_, _)), play(space(X2,Y2),tile(_, _)) | Rest]) :- X1 == X2, !, isSingleRow([play(space(X2,Y2),tile(_,_)) | Rest]).

isSingleColumn([]).
isSingleColumn([play(space(_,Y1), tile(_, _))]) :- !.
isSingleColumn([play(space(_,Y1),tile(_, _)), play(space(_,Y2),tile(_, _))]) :- Y1 == Y2.
isSingleColumn([play(space(_,Y1),tile(_, _)), play(space(X2,Y2),tile(_, _)) | Rest]) :- Y1 == Y2, !, isSingleColumn([play(space(X2,Y2),tile(_,_)) | Rest]).
