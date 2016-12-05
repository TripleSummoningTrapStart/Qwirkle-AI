/* Qwirkle Rules 

	same color different shape
	same shape different color
	foreach play there must exist a word such that every letter in the play is in the word
	foreach play every word made must be legal
	
	Include ability to dump?
	
	tile(Shape, Color)
	word([tile(S, C) | Rest])
	played([play(X, Y, tile(S, C)) | Rest])
	
	Exammple Board:
	[[play(0,4, tile(star,green)),play(0,5, tile(square,green)),play(0,6, tile(diamond,green))],[play(1,2, tile(diamond,orange)),play(1,4, tile(star,purple))],[play(2,2, tile(star,orange)),play(2,3, tile(star,yellow)),play(2,4, tile(star,blue)),play(2,5, tile(star,purple)),play(2,6, tile(star,red))],[play(3,0, tile(square,purple)),play(3,1, tile(square,blue)),play(3,2, tile(square,orange)),play(3,3, tile(square,yellow)),play(3,6, tile(cross,red)),play(3,7, tile(cross,blue))],[play(4,1, tile(circle,blue)),play(4,3, tile(cross,yellow)),play(4,6, tile(cross,red)),play(4,7, tile(cross,orange))],[play(5,3, tile(clover,yellow)),play(5,4, tile(clover,green)),play(5,5, tile(clover,orange)),play(5,7, tile(cross,red))],[play(6,3, tile(circle,yellow))],[play(7,3, tile(diamond,yellow))]]
*/
/*
*/

%tile(S, C).
%play(X, Y, tile(S, C)).



b1([[play(0,4, tile(star,green)),play(0,5, tile(square,green)),play(0,6, tile(diamond,green))],[play(1,2, tile(diamond,orange)),play(1,4, tile(star,purple))],[play(2,2, tile(star,orange)),play(2,3, tile(star,yellow)),play(2,4, tile(star,blue)),play(2,5, tile(star,purple)),play(2,6, tile(star,red))],[play(3,0, tile(square,purple)),play(3,1, tile(square,blue)),play(3,2, tile(square,orange)),play(3,3, tile(square,yellow)),play(3,6, tile(cross,red)),play(3,7, tile(cross,blue))],[play(4,1, tile(circle,blue)),play(4,3, tile(cross,yellow)),play(4,6, tile(cross,red)),play(4,7, tile(cross,orange))],[play(5,3, tile(clover,yellow)),play(5,4, tile(clover,green)),play(5,5, tile(clover,orange)),play(5,7, tile(cross,red))],[play(6,3, tile(circle,yellow))],[play(7,3, tile(diamond,yellow))]]).



% Returns true if there is a tile directly left gap
isgapLeft(X1, Y, [[play(X2, _, _) | _] | R2], Neighbors) :- % Too far left
	X2 < X1 - 1,
	!,
	isgapLeft(X1, Y, R2, Neighbors).
isgapLeft(X1, Y1, [[play(X2, Y2, _) | R1] | R2], Neighbors) :- % Correct Column, Too high
	X1 is (X2 + 1),
	Y2 < Y1,
	!,
	isgapLeft(X1, Y1, [R1 | R2], Neighbors).
isgapLeft(X1, Y, [[play(X2, Y, T) | _] | R2], [play(X2, Y, T) | Neighbors]) :- % Is left neighbor
	X1 is (X2 + 1),
	!,
	isgapUp(X1, Y, R2, Neighbors).
isgapLeft(X1, Y, [[play(X2, _, _) | _] | R2], Neighbors) :- % Missed left, Same Column
	X1 is (X2 + 1),
	!,
	isgapUp(X1, Y, R2, Neighbors).
isgapLeft(X, Y, [[] | R2], Neighbors) :- % Missed Left, no elements left in column
	!,
	isgapUp(X, Y, R2, Neighbors).
isgapLeft(X, Y, Board, Neighbors) :- % Missed Left
	isgapUp(X, Y, Board, Neighbors).
	

% Returns true if there is a tile directly above the gap
isgapUp(X1, Y1, [[play(X1, Y2, _) | R1] | R2], Neighbors) :- % Same Column, Too far up
	Y2 < Y1 - 1,
	!,
	isgapUp(X1, Y1, [R1 | R2], Neighbors).
isgapUp(X, Y1, [[play(X, Y2, T) | R1] | R2], [play(X, Y2, T) | Neighbors]) :- % Is top neighbor
	Y1 is Y2 + 1, 
	!,
	isgapCenter(X, Y1, [R1 | R2], Neighbors).
isgapUp(X, Y1, [[play(X, Y2, T) | R1] | R2], Neighbors) :- % Missed Top, Correct Column
	Y2 > Y1 - 1, 
	!,
	isgapCenter(X, Y1, [[play(X, Y2, T) | R1] | R2], Neighbors).
isgapUp(X, Y, [[] | R2], Neighbors) :- % Missed Top, Next Column
	!,
	isgapRight(X, Y, R2, Neighbors).
isgapUp(X, Y, Board, Neighbors) :- % Missed Top
	!,
	isgapRight(X, Y, Board, Neighbors).

	
% Returns true if there is a tile directly at the gap
isgapCenter(X1, Y1, [[play(X2, Y2, T) | R1] | R2], Neighbors) :- % Place not taken
	(not(X1 is X2); not(Y1 is Y2)), 
	!,
	isgapDown(X1, Y1, [[play(X2, Y2, T) | R1] | R2], Neighbors).
isgapCenter(X, Y, [[] | R2], Neighbors) :- 
	!,
	isgapRight(X, Y, R2, Neighbors).
isgapCenter(X, Y, Board, Neighbors) :- 
	isgapRight(X, Y, Board, Neighbors).

	
% Returns true if there is a tile directly below the gap
isgapDown(X, Y1, [[play(X, Y2, T) | _] | R2], [play(X, Y2, T) | Neighbors]) :-
	Y1 is Y2 - 1,
	!,
	isgapRight(X, Y1, R2, Neighbors).
isgapDown(X, Y, [_ | R2], Neighbors) :-
	!,
	isgapRight(X, Y, R2, Neighbors).
isgapDown(X, Y, Board, Neighbors) :-
	isgapRight(X, Y, Board, Neighbors).
	
	
% Returns true if there is a tile directly Right the gap
isgapRight(X1, Y1, [[play(X2, Y2, _) | R1] | R2], Neighbors) :-
	X1 is X2 - 1,
	Y2 < Y1, 
	!,
	isgapRight(X1, Y1, [R1 | R2], Neighbors).
isgapRight(X1, Y, [[play(X2, Y, T) | _] | _], [play(X2, Y, T)]) :- 
	X1 is X2 - 1,
	!.
isgapRight(_, _, _, []).




/* Is Legal */ 
isLegal(T) :- isSingleColor(T), isLegalShape(T).
isLegal(T) :- isSingleShape(T), isLegalColor(T).
	
isLegalColor([]).
isLegalColor([tile(S, _) | Rest]) :- 
		not(containsShape(S, Rest)),
		isLegalColor(Rest).
		
isLegalShape([tile(_, C) | Rest]) :-
		not(containsColor(C, Rest)),
		isLegalShape(Rest).
		
/* Contains */
containsColor([tile(_, C) | Rest], Color) :- 
		C is Color,
		containsColor(Rest, Color).

containsShape([tile(S, _) | Rest], Shape) :- 
		S is Shape,
		containsShape(Rest, Shape).
		
/* Single Type */
isSingleColor([]).
isSingleColor([tile]).
isSingleColor([tile(_, C1), tile(_, C2)]) :- C1 is C2.
isSingleColor([tile(_, C1), tile(S2, C2) | Rest]) :- C1 is C2, isSingleColor([tile(S2,C2) | Rest]).

isSingleShape([]).
isSingleShape([tile]).
isSingleShape([tile(S1, _), tile(S2, _)]) :- S1 is S2.
isSingleShape([tile(S1, _), tile(S2, C2) | Rest]) :- S1 is S2, isSingleShape([tile(S2,C2) | Rest]).



/* Variations */
varia(0,_,[]).
varia(N,L,[H|Varia]) :- 
	N>0,
	N1 is N-1,
	delete(H,L,Rest),
	varia(N1,Rest,Varia).
   
delete(X,[X|T],T).
delete(X,[H|T],[H|NT]) :- delete(X,T,NT).
