/* Qwirkle Rules 

	same color different shape
	same shape different color
	foreach play there must exist a word such that every letter in the play is in the word
	foreach play every word made must be legal
	
	Include ability to dump?
*/ 

xy(space(X, Y), X, Y).
xy(play(S, _), X, Y) :- xy(S, X, Y).
x(SorP, X) :- xy(SorP, X, _).
y(SorP, Y) :- xy(SorP, _, Y).
isFarLeft(Src, Dest)   :- x(Src, X1),  x(Dest, X2), X2 < X1 - 1.
isNextLeft(Src, Dest)   :- x(Src, X1),  x(Dest, X2), X2 is X1 - 1.
isFarAbove(Src, Dest)   :- y(Src, Y1),  y(Dest, Y2), Y2 < Y1 - 1.
isNextAbove(Src, Dest)   :- y(Src, Y1),  y(Dest, Y2), Y2 is Y1 - 1.
isAtVertically(Src, Dest)  :- y(Src, Y),  y(Dest, Y).
 
lookLeft(S, [[P | _] | B ], N) :-       %jump right one column
   isFarLeft(S, P),
   !,lookLeft(S, B, N).
   
lookLeft(S, [[P | C] | B], N) :-       %switch to near mode
   isNextLeft(S, P),
   !,lookNearLeft(S, [[P | C] | B], N).
   
lookLeft(S, B, N) :- lookMaybeUp(S, B, N).      %switch to up mode
lookNearLeft(S, [[P | C] | B], N) :-      %jump down one cell
   isFarAbove(S, P),
   !,lookNearLeft(S, [C | B], N).
lookNearLeft(S, [[P | C] | B], N) :-      %jump down one cell
   isNextAbove(S, P),
   !,lookNearLeft(S, [C | B], N).
lookNearLeft(S, [[P | _] | B], [P | N]) :-     %is left neighbor
   isAtVertically(S, P),
   !,lookUp(S, B, N).
lookNearLeft(S, [_ | B], N) :- lookUp(S, B, N).  %correct column, went to far down because there is no left neoghbor
lookMaybeUp(S, [[P | _] | B], N) :-
   isFarLeft(P, S),
   !, lookRight(S, B, N).
   
lookMaybeUp(S, [[P | _] | B], N) :-
   isNextLeft(P, S),
   !, lookRight(S, B, N).
lookMaybeUp(S, B, N) :- lookUp(S, B, N).   
   
lookUp(S, [[P | C] | B], N) :-
   isFarAbove(S, P),
   !, lookUp(S, [C | B], N).
lookUp(S, [[P | C] | B], [P | N]) :-
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
   lookRight(S, B, N).
   
lookCenter(S, [], N) :-
   lookRight(S, [], N).
  
lookDown(S, [[P | _] | B], [P | N]) :-
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
lookNearRight(S, [[P | C] | B], N) :-      %jump down one cell
   isNextAbove(S, P),
   !,lookNearRight(S, [C | B], N).
lookNearRight(S, [[P | _] | _], [P]) :-     %is left neighbor
   isAtVertically(S, P),
   !.
   
b([[play(space(0,4), tile(star,green)),play(space(0,5), tile(square,green)),play(space(0,6), tile(diamond,green))],[play(space(1,2), tile(diamond,orange)),play(space(1,4), tile(star,purple))],[play(space(2,2), tile(star,orange)),play(space(2,3), tile(star,yellow)),play(space(2,4), tile(star,blue)),play(space(2,5), tile(star,purple)),play(space(2,6), tile(star,red))],[play(space(3,0), tile(square,purple)),play(space(3,1), tile(square,blue)),play(space(3,2), tile(square,orange)),play(space(3,3), tile(square,yellow)),play(space(3,6), tile(cross,red)),play(space(3,7), tile(cross,blue))],[play(space(4,1), tile(circle,blue)),play(space(4,3), tile(cross,yellow)),play(space(4,6), tile(cross,red)),play(space(4,7), tile(cross,orange))],[play(space(5,3), tile(clover,yellow)),play(space(5,4), tile(clover,green)),play(space(5,5), tile(clover,orange)),play(space(5,7), tile(cross,red))],[play(space(6,3), tile(circle,yellow))],[play(space(7,3), tile(diamond,yellow))]]).
isGap(S, N) :-
 b(B),
 lookLeft(S, B, N).
 
spaces(Xmin, Xmax, Ymin, Ymax, space(X, Y)) :-
 between(Xmin, Xmax, X),
 between(Ymin, Ymax, Y).
 
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
isLegal(T) :- isSingleShape(T), !, isLegalShape(T), !. 
isLegal(T) :- isSingleColor(T), !, isLegalColor(T), !.
	
isLegalColor([tile(S, _) | Rest]) :- 
		notContainsShape(Rest, S),
		!,
		isLegalColor(Rest).
isLegalColor([tile(S, _)]).
isLegalColor([]).

isLegalShape([tile(_, C) | Rest]) :-
		notContainsColor(Rest, C),
		!,
		isLegalShape(Rest).
isLegalShape([tile(_, C)]).
isLegalShape([]).
		
/* Contains */
notContainsColor([tile(_, C)], Color) :-
		C \= Color.
notContainsColor([tile(_, C) | Rest], Color) :- 
		C \= Color,
		!,
		notContainsColor(Rest, Color).

notContainsShape([tile(S, _)], Shape) :-
		S \= Shape.
notContainsShape([tile(S, _) | Rest], Shape) :- 
		S \= Shape,
		!,
		notContainsShape(Rest, Shape).
		
/* Single Type */
isSingleColor([]).
isSingleColor([tile(_, _)]) :- !.
isSingleColor([tile(_, C1), tile(_, C2)]) :- C1 == C2.
isSingleColor([tile(_, C1), tile(S2, C2) | Rest]) :- C1 == C2, !, isSingleColor([tile(S2,C2) | Rest]).

isSingleShape([]).
isSingleShape([tile(_, _)]) :- !.
isSingleShape([tile(S1, _), tile(S2, _)]) :- S1 == S2.
isSingleShape([tile(S1, _), tile(S2, C2) | Rest]) :- S1 == S2, !, isSingleShape([tile(S2,C2) | Rest]).


