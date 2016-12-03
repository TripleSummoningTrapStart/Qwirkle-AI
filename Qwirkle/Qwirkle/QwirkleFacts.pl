/* Qwirkle Rules 

	same color different shape
	same shape different color
	foreach play there must exist a word such that every letter in the play is in the word
	foreach play every word made must be legal
	
	Include ability to dump?
	
	tile(Shape, Color)
	word([tile(S, C) | Rest])
	played([play(X, Y, tile(S, C)) | Rest])
*/
/*
*/

% Returns true if there is a tile directly left gap
isgapLeft(X1, Y, [[play(X2, _, _) | _] | R2], Neighbors) :-
	X2 < X1 - 1,
	isgapLeft(X1, Y, R2, Neighbors), !.
isgapLeft(X, Y1, [[play(X - 1, Y2, _) | R1] | R2], Neighbors) :-
	Y2 < Y1,
	isgapLeft(X, Y1, [R1 | R2], Neighbors), !.
isgapLeft(X, Y, [[play(X - 1, Y, T) | _] | R2], [play(X - 1, Y, T) | Neighbors]) :-
	isgapUp(X, Y, R2, Neighbors), !.
isgapLeft(X, Y, [[play(X - 1, _, _) | _] | R2], Neighbors) :-
	isgapUp(X, Y, R2, Neighbors), !.
isgapLeft(X, Y, Board, Neighbors) :-
	isgapUp(X, Y, Board, Neighbors).
	
% Returns true if there is a tile directly above the gap
isgapUp(X, Y, [[play(X - 1, _, _) | _] | R2], Neighbors) :-
	isgapUp(X, Y, R2, Neighbors), !.
isgapUp(X, Y1, [[play(X, Y2, _) | R1] | R2], Neighbors) :-
	Y2 < Y1 - 1,
	isgapUp(X, Y1, [R1 | R2], Neighbors), !.
isgapUp(X, Y, [[play(X, Y - 1, T) | R1] | R2], [play(X, Y - 1, T) | Neighbors]) :-
	isgapCenter(X, Y, [R1 | R2], Neighbors), !.
isgapUp(X, Y, Board, Neighbors) :-
	isgapCenter(X, Y, Board, Neighbors).
	
% Returns true if there is a tile directly at the gap
isgapCenter(X1, Y1, [[play(X2, Y2, _) | R1] | R2], Neighbors) :-
	not(X1 == X2, Y1 == Y2),
	isgapDown(X1, Y1, [R1 | R2], Neighbors).
	
% Returns true if there is a tile directly below the gap
isgapDown(X, Y, [[play(X, Y + 1, T) | _] | R2], [play(X, Y + 1, T) | Neighbors]) :-
	isgapRight(X, Y, R2, Neighbors), !.
isgapDown(X, Y, [[play(X, _, _) | _] | R2], Neighbors) :-
	isgapRight(X, Y, R2, Neighbors), !.
isgapDown(X, Y, Board, Neighbors) :-
	isgapRight(X, Y, Board, Neighbors).
	
% Returns true if there is a tile directly Right the gap
isgapRight(X, Y1, [[play(X + 1, Y2, _) | R1] | R2], Neighbors) :-
	Y2 < Y1, 
	isgapRight(X, Y1, [R1 | R2], Neighbors), !.
isgapRight(X, Y, [[play(X + 1, Y, T) | _] | _], [play(X, Y, T)]) :- !.
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
		C == Color,
		containsColor(Rest, Color).

containsShape([tile(S, _) | Rest], Shape) :- 
		S == Shape,
		containsShape(Rest, Shape).
		
/* Single Type */
isSingleColor([]).
isSingleColor([tile]).
isSingleColor([tile(_, C1), tile(_, C2)]) :- C1==C2.
isSingleColor([tile(_, C1), tile(S2, C2) | Rest]) :- C1 == C2, isSingleColor([tile(S2,C2) | Rest]).

isSingleShape([]).
isSingleShape([tile]).
isSingleShape([tile(S1, _), tile(S2, _)]) :- S1==S2.
isSingleShape([tile(S1, _), tile(S2, C2) | Rest]) :- S1 == S2, isSingleShape([tile(S2,C2) | Rest]).

/* Combinations */
combinations().